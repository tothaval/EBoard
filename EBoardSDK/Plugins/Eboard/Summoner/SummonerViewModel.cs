// <copyright file="SummonerViewModel.cs" company=".">
// Stephan Kammel
// </copyright>
/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
namespace EBoardSDK.Plugins.Eboard.Summoner;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

public partial class SummonerViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "Summoner Element";
    private readonly string pluginName = "Summoner";

    private object? summoneeModel = null;

    private PluginSelectionViewModel? pluginSelectionViewModel;

    [ObservableProperty]
    private bool pluginTypeSelected = false;

    [ObservableProperty]
    private PluginRepresentationItem? selectedPlugin = null;

    [ObservableProperty]
    private string userCommandString = ">";

    [ObservableProperty]
    private IPlugin? summonee;

    [ObservableProperty]
    private List<PluginRepresentationItem> plugins = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="SummonerViewModel"/> class.
    /// </summary>
    public SummonerViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);

        this.SetMenuItemViewModel(new SummonerMenuItemViewModel(this));
        this.SetMenuItem(new SummonerMenuItem(this.MenuItemViewModel!));

        this.OnPropertyChanged(nameof(this.ScreenInstantiationConstraints));
    }

    public PluginSelectionViewModel? PluginSelectionViewModel => this.pluginSelectionViewModel;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Eboard;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new();

    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(SummonerModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(SummonerViewModel);

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();

        if (this.PluginSelectionViewModel != null)
        {
            this.PluginSelectionViewModel.PropertyChanged -= this.PluginSelectionViewModel_PropertyChanged;
        }
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        var summonerModel = model as SummonerModel;

        if (summonerModel != null)
        {
            this.ApplyModel(summonerModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<SummonerModel>(json!);

            if (jsonParsed != null && jsonParsed.Plugin != null)
            {
                this.ApplyModel(jsonParsed);
            }
        }
        catch (JsonException jsonEx)
        {
            Log.Error(jsonEx.Message);

            throw;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);

            throw;
        }
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<SummonerModel>(path);

            if (data != null)
            {
                if (data.Plugin != null && data.PluginTypeSelected)
                {
                    this.InsertModel(data);
                }

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(SummonerModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new SummonerModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    public override void PrepareCopy()
    {
        var model = new SummonerModel(this);

        this.SetModel(model);
    }

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        var pluginManager = new SDKPluginManager();

        this.Plugins = pluginManager.FoundPlugins;

        this.pluginSelectionViewModel = new PluginSelectionViewModel(this.FluidUI, null, singleSelectionTarget: false);

        this.pluginSelectionViewModel.PropertyChanged += this.PluginSelectionViewModel_PropertyChanged;
        this.OnPropertyChanged(nameof(this.PluginSelectionViewModel));
    }

    protected async virtual Task ExecuteCommandAsync()
    {
        string command = this.UserCommandString.Substring(1);

        if (this.UserCommandString.StartsWith(">") && this.ElementViewModel != null)
        {
            var plugin = await new SDKPluginManager().InvokePluginByName(command);

            if (plugin == null)
            {
                this.Summonee = new StandardTextViewModel() { Title = "plugin not found", Text = $"unknown '{command}' called" };

                return;
            }

            var screen = this.ElementViewModel.ScreenViewModel;
            var elementViewModel = ElementFactory.GetElementViewModel(screen);

            var fluiduicopy = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(this.ElementViewModel.FluidUI);
            elementViewModel.SetFluidUI(fluiduicopy);

            plugin.SetElementViewModel(elementViewModel);
            plugin.SetContainerIsArea();

            if (this.summoneeModel != null)
            {
                plugin.InsertModel(this.summoneeModel);
            }

            plugin.RefreshInitialization();

            this.Summonee = plugin;
            this.PluginTypeSelected = true;

            if (this.SelectedPlugin == null || this.SelectedPlugin.PluginName != plugin.Name)
            {
                this.SelectedPlugin = new PluginRepresentationItem()
                {
                    PluginName = plugin.Name,
                    PluginHeader = plugin.Header,
                    PluginCategory = plugin.Category,
                    PluginLogo = plugin.Logo,
                    ScreenConstraints = plugin.ScreenInstantiationConstraints,
                    PluginMainViewModelType = plugin.PluginViewModelType,
                };
            }
        }
    }

    protected void SetPluginSelectionViewModel(PluginSelectionViewModel pluginSelectionViewModel)
    {
        this.pluginSelectionViewModel = pluginSelectionViewModel;

        this.OnPropertyChanged(nameof(this.PluginSelectionViewModel));
    }

    private void ApplyModel(SummonerModel summonerModel)
    {
        this.summoneeModel = summonerModel.SummoneeModel;
        this.PluginTypeSelected = summonerModel.PluginTypeSelected;
        this.SelectedPlugin = summonerModel.Plugin;
    }

    private void PluginSelectionViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (this.PluginSelectionViewModel != null)
        {
            this.SelectedPlugin = this.PluginSelectionViewModel.SelectedPlugin;
        }
    }

    partial void OnSelectedPluginChanged(PluginRepresentationItem? value)
    {
        this.UserCommandString = $">{value?.PluginName}";

        this.ExecuteCommandString();
    }

    [RelayCommand]
    private void DeleteElement(object s)
    {
        this.Summonee = null;
        this.summoneeModel = null;
    }

    /// <summary>
    /// until a real command architecture is implemented, this serves as a mockup solution
    ///
    /// a real command architecture should be done in a separate class or using an api that
    /// handles all validations etc.
    ///
    /// validation could also use onerrorinfo with community toolkit, but i need to look into that first.
    /// </summary>
    [RelayCommand]
    private void ExecuteCommandString()
    {
        _ = this.ExecuteCommandAsync();
    }
}

// EOF