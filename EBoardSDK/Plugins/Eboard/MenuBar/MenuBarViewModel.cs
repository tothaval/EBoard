// <copyright file="MenuBarViewModel.cs" company=".">
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
/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  MenuBar
 *
 *  view model for MainWindowMenuBarView, which has prototype element instantiation
 *  button and a prototype shape menu and a button to switch on or off the EBoardBrowserView
 */
namespace EBoardSDK.Plugins.Eboard.MenuBar;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.Interfaces.FluidUIFont;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using Serilog;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

public partial class MenuBarViewModel : EboardPluginBaseViewModel
{
    private readonly string pluginName = "MenuBar";
    private readonly string pluginHeader = "MenuBar";

    private SDKPluginManager? sDKPluginManager;

    private IFluidUIDesignModel? screenBrushManagement;
    private IFluidUIFontModel? screenFontManagement;

    [ObservableProperty]
    private bool eBoardBrowserSwitch = false;

    [ObservableProperty]
    private bool invokeOnly = false;

    [ObservableProperty]
    private bool screenControlSwitch = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuBarViewModel"/> class.
    /// </summary>
    public MenuBarViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);

        this.SetMenuItemViewModel(new MenuBarMenuItemViewModel(this));
        this.SetMenuItem(new MenuBarMenuItem(this.MenuItemViewModel!));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuBarViewModel"/> class.
    /// </summary>
    /// <param name="nonElementPlugin"></param>
    /// <param name="mainViewModel"></param>
    public MenuBarViewModel(bool nonElementPlugin, MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        this.instantiatedAsElement = !nonElementPlugin;

        this.Setup();

        this.OnPropertyChanged(nameof(this.InstantiatedAsElement));
        this.OnPropertyChanged(nameof(this.MainViewModel));
    }

    public bool IsActiveScreenNotNull => this.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard != null;

    public List<PluginRepresentationItem> FoundAddons { get; private set; } = [];

    public List<PluginRepresentationItem> FoundElements { get; private set; } = [];

    public List<PluginRepresentationItem> FoundShapes { get; private set; } = [];

    public List<PluginRepresentationItem> FoundAreas { get; private set; } = [];

    public List<PluginRepresentationItem> FoundTools { get; private set; } = [];

    public List<PluginRepresentationItem> FoundPlugins { get; private set; } = [];

    public List<PluginRepresentationItem> FoundSystem { get; private set; } = [];

    public List<PluginRepresentationItem> FoundEboard { get; private set; } = [];

    public IFluidUIDesignModel? ScreenBrushManagement => this.screenBrushManagement;

    public IFluidUIFontModel? ScreenFontManagement => this.screenFontManagement;

    public NavigationContextViewModel NavigationContextViewModel => this.MainViewModel.NavigationContextViewModel;

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
    public override Type? PluginModelType => typeof(MenuBarModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(MenuBarViewModel);

    public override bool InstantiatedAsElement => this.instantiatedAsElement;

    public override void Dispose()
    {
        base.Dispose();

        if (this.ScreenBrushManagement != null)
        {
            this.ScreenBrushManagement.PropertyChangedEvent -= this.ScreenBrushManagement_PropertyChangedEvent;
        }

        if (this.ScreenFontManagement != null)
        {
            this.ScreenFontManagement.PropertyChangedEvent -= this.ScreenFontManagement_PropertyChangedEvent;
        }

        this.mainViewModel.NavigationContextViewModel.EboardBrowserViewModel.PropertyChanged -= this.EBoardBrowserViewModel_PropertyChanged;
    }

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            this.SetMainViewModel(this.ElementViewModel.ScreenViewModel.MainViewModel);

            this.screenBrushManagement = this.ElementViewModel.ScreenViewModel.FluidUI.Design;
            this.screenFontManagement = this.ElementViewModel.ScreenViewModel.FluidUI.Font;

            this.OnPropertyChanged(nameof(this.ScreenBrushManagement));
            this.OnPropertyChanged(nameof(this.ScreenFontManagement));
        }

        this.Setup();
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<MenuBarModel>(path);

            if (data != null)
            {
                this.ApplyModel(data);

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(LinkModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new MenuBarModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is MenuBarModel menuBarModel)
        {
            this.ApplyModel(menuBarModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<MenuBarModel>(json!);

            if (jsonParsed != null)
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

    public override void PrepareCopy()
    {
        var model = new MenuBarModel(this);

        this.SetModel(model);
    }

    internal void SearchForPlugins()
    {
        if (this.sDKPluginManager == null)
        {
            this.sDKPluginManager = new SDKPluginManager();
        }

        this.FoundPlugins = this.sDKPluginManager.FoundPlugins;
        this.FoundAddons = this.sDKPluginManager.FoundAddons;
        this.FoundElements = this.sDKPluginManager.FoundElements;
        this.FoundShapes = this.sDKPluginManager.FoundShapes;
        this.FoundAreas = this.sDKPluginManager.FoundAreas;

        this.FoundSystem = this.sDKPluginManager.FoundSystem;
        this.FoundEboard = this.sDKPluginManager.FoundEboard;

        var pluginItems = new List<PluginRepresentationItem>();

        foreach (var item in this.sDKPluginManager.FoundTools)
        {
            pluginItems.Add(new PluginRepresentationItem()
            {
                PluginHeader = item.PluginHeader,
                PluginLogo = item.PluginLogo,
                PluginName = item.PluginName,
            });
        }

        this.FoundTools = pluginItems;

        this.OnPropertyChanged(nameof(this.FoundPlugins));
        this.OnPropertyChanged(nameof(this.FoundAddons));
        this.OnPropertyChanged(nameof(this.FoundElements));
        this.OnPropertyChanged(nameof(this.FoundShapes));
        this.OnPropertyChanged(nameof(this.FoundAreas));
        this.OnPropertyChanged(nameof(this.FoundTools));
    }

    internal void InstallPlugin(PluginRepresentationItem eBoardElementPluginBaseViewModel)
    {
        this.sDKPluginManager?.InstallPlugin(eBoardElementPluginBaseViewModel);

        this.SearchForPlugins();
    }

    internal void UninstallPlugin(PluginRepresentationItem eBoardElementPluginBaseViewModel)
    {
        this.sDKPluginManager?.UninstallPlugin(eBoardElementPluginBaseViewModel);

        this.SearchForPlugins();
    }

    internal void Update()
    {
        this.OnPropertyChanged(nameof(this.ScreenBrushManagement));
        this.OnPropertyChanged(nameof(this.ScreenFontManagement));
    }

    /// <inheritdoc/>
    protected override void Setup()
    {
        if (this.mainViewModel == null)
        {
            return;
        }

        this.screenBrushManagement = this.mainViewModel.FluidUI.Design!;
        this.screenFontManagement = this.mainViewModel.FluidUI.Font!;

        this.EBoardBrowserSwitch = this.mainViewModel.EBoardConfig.EBoardBrowserSwitch;
        this.ScreenControlSwitch = this.mainViewModel.EBoardConfig.ScreenControlSwitch;

        this.mainViewModel.NavigationContextViewModel.EboardBrowserViewModel.PropertyChanged += this.EBoardBrowserViewModel_PropertyChanged;

        this.SearchForPlugins();

        this.Update();
    }

    private void ApplyModel(MenuBarModel menuBarModel)
    {
        this.InvokeOnly = menuBarModel.InvokeOnly;
    }

    private async Task InvokeObjectAsPluginAsync(object s)
    {
        if (s is PluginRepresentationItem)
        {
            var pluginitem = s as PluginRepresentationItem;
            if (pluginitem != null && this.mainViewModel != null)
            {
                if (this.sDKPluginManager == null)
                {
                    this.sDKPluginManager = new SDKPluginManager();
                }

                var plugin = await this.sDKPluginManager.InvokePluginByName(pluginitem.PluginName ?? string.Empty);

                var selectedEboard = this.mainViewModel.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard;

                if (selectedEboard != null)
                {
                    this.sDKPluginManager.InvokePluginOnEboard(plugin, selectedEboard);
                }
            }
        }
    }

    private void EBoardBrowserViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (this.mainViewModel != null && this.mainViewModel.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard != null)
        {
            this.screenBrushManagement = this.mainViewModel.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard.FluidUI.Design!;
            this.screenFontManagement = this.mainViewModel.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard.FluidUI.Font!;

            if (this.ScreenBrushManagement != null)
            {
                this.ScreenBrushManagement.PropertyChangedEvent += this.ScreenBrushManagement_PropertyChangedEvent;
            }

            if (this.ScreenFontManagement != null)
            {
                this.ScreenFontManagement.PropertyChangedEvent += this.ScreenFontManagement_PropertyChangedEvent;
            }

            this.Update();

            this.OnPropertyChanged(nameof(this.ScreenBrushManagement));
            this.OnPropertyChanged(nameof(this.ScreenFontManagement));
        }

        this.mainViewModel?.LogOutBar?.Update();

        this.OnPropertyChanged(nameof(this.IsActiveScreenNotNull));
    }

    private void ScreenFontManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.ScreenFontManagement));
    }

    private void ScreenBrushManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.ScreenBrushManagement));
    }

    partial void OnEBoardBrowserSwitchChanged(bool value)
    {
        if (value && this.ScreenControlSwitch)
        {
            this.ScreenControlSwitch = false;
        }

        if (this.mainViewModel != null && this.InstantiatedAsElement)
        {
            this.mainViewModel.MenuBar.EBoardBrowserSwitch = value;
        }
    }

    partial void OnScreenControlSwitchChanged(bool value)
    {
        if (value && this.EBoardBrowserSwitch)
        {
            this.EBoardBrowserSwitch = false;
        }

        if (this.mainViewModel != null && this.InstantiatedAsElement)
        {
            this.mainViewModel.MenuBar.ScreenControlSwitch = value;
        }
    }

    [RelayCommand]
    private async Task InvokePlugin(object s)
    {
        await this.InvokeObjectAsPluginAsync(s);
    }
}

// EOF