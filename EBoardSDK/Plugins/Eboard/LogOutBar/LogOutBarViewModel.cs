// <copyright file="LogOutBarViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Eboard.LogOutBar;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Eboard.About;
using EBoardSDK.Plugins.Eboard.Manual;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using Serilog;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

public partial class LogOutBarViewModel : EboardPluginBaseViewModel
{
    private readonly string pluginName = "LogOutBar";
    private readonly string pluginHeader = "LogOutBar";

    private AboutViewModel? aboutViewModel;
    private ManualViewModel? manualViewModel;

    [ObservableProperty]
    private bool aboutClicked = false;

    [ObservableProperty]
    private bool exitOnly = false;

    [ObservableProperty]
    private bool manualClicked = false;

    [ObservableProperty]
    private string txtAbout = "About";

    [ObservableProperty]
    private string txtExitEboard = "Off";

    [ObservableProperty]
    private string txtManual = "Manual";

    [ObservableProperty]
    private string txtShutDownMachine = "Shutdown";

    /// <summary>
    /// Initializes a new instance of the <see cref="LogOutBarViewModel"/> class.
    /// </summary>
    public LogOutBarViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);

        this.SetMenuItemViewModel(new LogOutBarMenuItemViewModel(this));
        this.SetMenuItem(new LogOutBarMenuItem(this.MenuItemViewModel!));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogOutBarViewModel"/> class.
    /// </summary>
    /// <param name="nonElementPlugin"></param>
    /// <param name="mainViewModel"></param>
    public LogOutBarViewModel(bool nonElementPlugin, MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        this.instantiatedAsElement = !nonElementPlugin;

        this.Setup();

        this.OnPropertyChanged(nameof(this.InstantiatedAsElement));
        this.OnPropertyChanged(nameof(this.MainViewModel));
    }

    public AboutViewModel? AboutViewModel => this.aboutViewModel;

    public ManualViewModel? ManualViewModel => this.manualViewModel;

    public MainViewModel? MainViewModel => this.mainViewModel;

    public bool ScreenExists => this.MainViewModel?.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard != null;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Eboard;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new ();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new ();

    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(LogOutBarModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(LogOutBarViewModel);

    /// <inheritdoc/>
    public override bool InstantiatedAsElement => this.instantiatedAsElement;

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            this.SetMainViewModel(this.ElementViewModel.ScreenViewModel.MainViewModel);
        }

        this.Setup();
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<LogOutBarModel>(path);

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
        var model = new LogOutBarModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is LogOutBarModel logOutBarModel)
        {
            this.ApplyModel(logOutBarModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<LogOutBarModel>(json!);

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
        var model = new LogOutBarModel(this);

        this.SetModel(model);
    }

    internal void Update()
    {
        if (this.ScreenExists)
        {
            this.AboutClicked = false;
            this.ManualClicked = false;
        }

        this.OnPropertyChanged(nameof(this.ScreenExists));
        this.OnPropertyChanged(nameof(this.MainViewModel));
    }

    /// <inheritdoc/>
    protected override void Setup()
    {
        if (this.mainViewModel == null)
        {
            return;
        }
    }

    private void ApplyModel(LogOutBarModel logOutBarModel)
    {
        this.ExitOnly = logOutBarModel.ExitOnly;
    }

    partial void OnAboutClickedChanged(bool value)
    {
        if (value)
        {
            this.ManualClicked = false;

            this.aboutViewModel = new AboutViewModel();
            this.aboutViewModel.SetFluidUI(this.MainViewModel.FluidUI);
            this.aboutViewModel.RefreshInitialization();
        }
        else
        {
            this.aboutViewModel = null;
        }

        this.OnPropertyChanged(nameof(this.AboutViewModel));
    }

    partial void OnManualClickedChanged(bool value)
    {
        if (value)
        {
            this.AboutClicked = false;

            this.manualViewModel = new ManualViewModel();
            this.manualViewModel.SetFluidUI(this.MainViewModel.FluidUI);
            this.manualViewModel.RefreshInitialization();
        }
        else
        {
            this.aboutViewModel = null;
        }

        this.OnPropertyChanged(nameof(this.ManualViewModel));
    }

    [RelayCommand]
    private void Close()
    {
        new SharedMethod_UI().CloseApplication();
    }

    [RelayCommand]
    private void InvokeOrShowAbout()
    {
        if (this.ScreenExists && this.MainViewModel?.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard != null)
        {
            var pluginManager = new SDKPluginManager();
            pluginManager.InvokePluginOnEboard(new AboutViewModel(), this.MainViewModel.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard);

            return;
        }

        this.AboutClicked = !this.AboutClicked;
    }

    [RelayCommand]
    private void InvokeOrShowManual()
    {
        if (this.ScreenExists && this.MainViewModel?.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard != null)
        {
            var pluginManager = new SDKPluginManager();
            pluginManager.InvokePluginOnEboard(new ManualViewModel(), this.MainViewModel.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard);

            return;
        }

        this.ManualClicked = !this.ManualClicked;
    }

    [RelayCommand]
    private void ShutDown()
    {
        var runner = this.mainViewModel.GetRunnerInstance();

        var saveResult = runner.SaveEboard().Result;

        new SharedMethod_UI().ShutDownMachine();
    }
}

// EOF