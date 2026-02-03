// <copyright file="MainViewModel.cs" company=".">
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
 *  MainViewModel
 *
 *  view model for MainWindow
 */
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Eboard.LogOutBar;
using EBoardSDK.Plugins.Eboard.MenuBar;
using EBoardSDK.Plugins.Eboard.ScreenChanger;
using EBoardSDK.Plugins.Eboard.ScreenControl;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;
using System.Windows.Threading;

public partial class MainViewModel : FluidUIBaseViewModel
{
    private readonly FluidUIContextAreas contextArea = FluidUIContextAreas.Eboard;

    private readonly DispatcherTimer messageStripDurationTimer = new();

    private readonly Runner runner;

    private EBoardSDK.Models.EboardConfig eboardConfig;

    private ObservableCollection<PluginCopy> elementCopyList = new();

    private IFluidUIContext fluidUIContextCopy = SDKDataManager.DefaultFluidUIContext;

    private bool messageStripMessageActive = false;

    private ScreenControlViewModel screenControlViewModel;

    [ObservableProperty]
    private LogOutBarViewModel logOutBar;

    [ObservableProperty]
    private bool mainControlsVisible = true;

    [ObservableProperty]
    private MenuBarViewModel menuBar;

    [ObservableProperty]
    private string messageStripMessage = string.Empty;

    [ObservableProperty]
    private NavigationContextViewModel navigationContextViewModel;

    [ObservableProperty]
    private ScreenChangerViewModel screenChanger;

    [ObservableProperty]
    private bool showCopyListItems = true;

    [ObservableProperty]
    private bool showMessageStrip = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    /// <param name="eboardConfig"></param>
    /// <param name="screenData"></param>
    /// <param name="runner"></param>
    public MainViewModel(EBoardSDK.Models.EboardConfig eboardConfig, Runner runner)
        : base()
    {
        this.runner = runner;

        this.messageStripDurationTimer.Interval = TimeSpan.FromSeconds(5);
        this.messageStripDurationTimer.Tick += this.MessageStripDurationTimer_Tick;

        if (eboardConfig == null)
        {
            eboardConfig = new EboardConfig();
        }

        this.eboardConfig = eboardConfig;

        this.SetFluidUI(this.eboardConfig.EBoardContext);

        var manager = new FluidUIDataBlockManager(this);

        manager.SetupTitleAndText("Eboard", "the eboard main window\n\nyou can change this description");

        this.CreateFluidUIMenuViewModel();
    }

    public override FluidUIContextAreas ContextArea => this.contextArea;

    public Task Initialize(List<EboardScreen> screenData)
    {
        this.NavigationContextViewModel = new NavigationContextViewModel(this);
        this.NavigationContextViewModel.EboardBrowserViewModel.PropertyChanged += this.EboardBrowserViewModel_PropertyChanged;

        this.MenuBar = new MenuBarViewModel(nonElementPlugin: true, this);

        this.LogOutBar = new LogOutBarViewModel(nonElementPlugin: true, this);
        this.LogOutBar.SetMainViewModel(this);

        this.ScreenChanger = new ScreenChangerViewModel(nonElementPlugin: true, this);
        this.ScreenChanger.SetMainViewModel(this);

        this.screenControlViewModel = new ScreenControlViewModel();
        this.ScreenControlViewModel.SetMainViewModel(this);

        foreach (var item in this.eboardConfig.ElementCopyList)
        {
            this.elementCopyList.Add(item);
        }

        screenData.Select(x => x).ToList().ForEach(
            (Action<EboardScreen>)(escreen =>
            {
                var eBoardViewModel = EboardScreenFactory.GetScreenViewModel(this, escreen);

                if (this.eboardConfig.EBoardIndex > this.eboardConfig.EBoardCount)
                {
                    this.eboardConfig.EBoardIndex = 0;
                }

                if (escreen.ID == this.eboardConfig.EBoardIndex)
                {
                    this.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard = eBoardViewModel;
                }

                this.NavigationContextViewModel.AddScreenViewModel(eBoardViewModel);
            }));

        _ = this.EBoardConfigInitialization(this.eboardConfig).Result;

        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));

        this.OnPropertyChanged(nameof(this.ElementCopyList));
        this.OnPropertyChanged(nameof(this.LogOutBar));
        this.OnPropertyChanged(nameof(this.MenuBar));
        this.OnPropertyChanged(nameof(this.NavigationContextViewModel));
        this.OnPropertyChanged(nameof(this.ScreenControlViewModel));

        return Task.CompletedTask;
    }

    public bool MessageStripMessageActive => this.messageStripMessageActive;

    public bool CopyListCountGreaterZero => this.ElementCopyList.Count > 0;

    public ObservableCollection<PluginCopy> ElementCopyList => this.elementCopyList;

    public IFluidUIContext FluidUIContextCopy => this.fluidUIContextCopy;

    public ScreenControlViewModel ScreenControlViewModel => this.screenControlViewModel;

    internal static string TxtRemoveAllEboardScreensQuestion => "Delete all screens?";

    internal static string TxtRemoveEboardSelectionQuestion => "Delete selected screen(s)?";

    internal static string TxtRemoveEboardsTitle => "Delete screen(s) confirmation";

    internal static string TxtRemoveAllElementsQuestion => "Delete all elements?";

    internal static string TxtRemoveEboardQuestion => "Delete this screen?";

    internal static string TxtRemoveEboardTitle => "Delete screen(s) confirmation";

    internal static string TxtRemoveElementQuestion => "Delete selected elementViewModel(s)?";

    internal static string TxtRemoveElementTitle => "Delete Element(s) confirmation";

    internal EboardConfig EBoardConfig => this.eboardConfig;

    internal override void CreateFluidUIMenuViewModel()
    {
        if (this.fluidUIMenuViewModel == null)
        {
            this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, Enums.FluidUIStandSettings.NoStandContextArea));
        }
    }

    internal bool DeepCopyElementToElementCopyList(ElementViewModel elementViewModel, bool moveCopy)
    {
        if (elementViewModel.Plugin == null || elementViewModel.Plugin.ScreenInstantiationConstraints == null)
        {
            return false;
        }

        if (elementViewModel.Plugin.ScreenInstantiationConstraints.CopyConstraints == Enums.CopyConstraints.Denied
            || elementViewModel.Plugin.ScreenInstantiationConstraints.CopyConstraints == Enums.CopyConstraints.ValueNotSet
            || elementViewModel.Plugin.ScreenInstantiationConstraints.InstantiationPolicy == Enums.InstantiationPolicy.ValueNotSet
            || elementViewModel.Plugin.ScreenInstantiationConstraints.InstantiationPolicy == Enums.InstantiationPolicy.OnePerScreen
            || elementViewModel.Plugin.ScreenInstantiationConstraints.InstantiationPolicy == Enums.InstantiationPolicy.Unique
            || elementViewModel.Plugin.ScreenInstantiationConstraints.InstantiationPolicy == Enums.InstantiationPolicy.Global)
        {
            var message = $"{elementViewModel.Plugin.Name}: plugin settings forbid action, " +
                $"copy constraints: {elementViewModel.Plugin.ScreenInstantiationConstraints.CopyConstraints}, " +
                $"instantiation policy: {elementViewModel.Plugin.ScreenInstantiationConstraints.InstantiationPolicy}";

            this.WriteToMessageStrip(message);

            return false;
        }

        var wasSelected = false;

        if (elementViewModel.IsSelected)
        {
            wasSelected = true;
            elementViewModel.SelectElement();
        }

        PluginCopy data = new(new ElementConfig(), null);
        var manager = new SDKPluginManager();
        var instance = manager.InvokePluginByName(elementViewModel.Plugin.Name).Result;
        IFluidUIContext fluidUIContext;

        if (instance != null)
        {
            switch (elementViewModel.Plugin.ScreenInstantiationConstraints.CopyConstraints)
            {
                case Enums.CopyConstraints.FullCopy:
                    elementViewModel.PrepareCopy();
                    fluidUIContext = this.DeepCopyFluidUI(elementViewModel);

                    data.ElementConfig = this.GetCopyElementConfig(elementViewModel, instance, fluidUIContext, moveCopy);

                    data.PluginModel = elementViewModel.Plugin.PluginModel;
                    break;

                case Enums.CopyConstraints.OnlyFluidUI:
                    fluidUIContext = this.DeepCopyFluidUI(elementViewModel);

                    data.ElementConfig = this.GetCopyElementConfig(elementViewModel, instance, fluidUIContext, moveCopy);
                    data.PluginModel = null;
                    break;

                case Enums.CopyConstraints.OnlyModel:
                    elementViewModel.PrepareCopy();
                    fluidUIContext = new FluidUIContext();

                    data.ElementConfig = this.GetCopyElementConfig(elementViewModel, instance, fluidUIContext, true);
                    data.PluginModel = elementViewModel.Plugin.PluginModel;
                    break;
                default:
                    break;
            }

            if (!this.elementCopyList.Contains(data))
            {
                this.elementCopyList.Add(data);

                var message = $"{elementViewModel.Plugin.Name}: was added to element copy list";

                this.WriteToMessageStrip(message);

                this.OnPropertyChanged(nameof(this.ElementCopyList));
                this.OnPropertyChanged(nameof(this.CopyListCountGreaterZero));
            }
        }

        // to prevent issues with highlight brush as borderbrush
        if (wasSelected)
        {
            elementViewModel.SelectElement();
        }

        return true;
    }

    internal void DeselectElements()
    {
        if (this.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard != null)
        {
            this.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard.DeselectElements();
        }
    }

    internal void ElementsPastedToScreen()
    {
        this.elementCopyList.Clear();
        this.OnPropertyChanged(nameof(this.ElementCopyList));
        this.OnPropertyChanged(nameof(this.CopyListCountGreaterZero));
    }

    internal EBoardSDK.Models.EboardConfig GetEboardConfig()
    {
        this.eboardConfig.EBoardIndex = this.NavigationContextViewModel.EboardBrowserViewModel.CurrentSelectionID;
        this.eboardConfig.EBoardCount = this.NavigationContextViewModel.EboardCount;
        this.eboardConfig.EBoardBrowserSwitch = this.MenuBar.EBoardBrowserSwitch;
        this.eboardConfig.ScreenControlSwitch = this.MenuBar.ScreenControlSwitch;

        this.eboardConfig.EBoardContext = (FluidUIContext)this.FluidUI;
        this.eboardConfig.FluidUIContextCopy = (FluidUIContext)this.FluidUIContextCopy;

        this.eboardConfig.EBoardBrowserViewContext = (FluidUIContext)this.NavigationContextViewModel.FluidUI;

        this.eboardConfig.ElementCopyList = this.ElementCopyList.ToList();

        return this.eboardConfig;
    }

    internal Runner GetRunnerInstance()
    {
        return this.runner;
    }

    internal List<EBoardSDK.Models.EboardScreen> GetScreenData()
    {
        var screenDataList = this.NavigationContextViewModel.GetScreenData();

        return screenDataList;
    }

    internal void LoadIFluidUIContextCopy(IFluidUIContext fluidUIContext)
    {
        this.fluidUIContextCopy = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(fluidUIContext);

        this.OnPropertyChanged(nameof(this.FluidUIContextCopy));
    }

    internal void TriggerVisibilityChange()
    {
        this.MainControlsVisibilityChange();
    }

    internal void WriteToMessageStrip(string message)
    {
        this.MessageStripMessage = $"{message}\n{this.MessageStripMessage}";

        this.messageStripMessageActive = true;
        this.OnPropertyChanged(nameof(this.MessageStripMessageActive));

        this.messageStripDurationTimer.Start();
    }

    private IFluidUIContext DeepCopyFluidUI(ElementViewModel elementViewModel)
    {
        var deepCopyFluidUI = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(elementViewModel.FluidUI);

        return deepCopyFluidUI;
    }

    private void EboardBrowserViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.ScreenControlViewModel.RefreshInitialization();
    }

    private Task<EboardConfig?> EBoardConfigInitialization(EboardConfig eboardConfig)
    {
        if (eboardConfig != null)
        {
            if (eboardConfig.EBoardIndex > 0 && eboardConfig.EBoardIndex <= this.NavigationContextViewModel.EboardCount)
            {
                this.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard = this.NavigationContextViewModel.Eboards[eboardConfig.EBoardIndex - 1];
            }

            this.MenuBar.EBoardBrowserSwitch = eboardConfig.EBoardBrowserSwitch;

            if (eboardConfig.EBoardContext == null)
            {
                eboardConfig.EBoardContext = new FluidUIContext();
                eboardConfig.EBoardContext.SetInitialValues();
            }

            if (eboardConfig.EBoardBrowserViewContext == null)
            {
                eboardConfig.EBoardBrowserViewContext = new FluidUIContext();
                eboardConfig.EBoardBrowserViewContext.SetInitialValues();
            }

            this.LoadIFluidUIContextCopy(this.eboardConfig.FluidUIContextCopy);
        }

        return Task.FromResult(eboardConfig);
    }

    private ElementConfig GetCopyElementConfig(ElementViewModel elementViewModel, IPlugin plugin, IFluidUIContext fluidUIContext, bool moveCopy)
    {
        var elementConfig = new ElementConfig()
        {
            EID = moveCopy ? elementViewModel.EID : new EboardIdFactory().GetElementId(),
            ID = elementViewModel.ScreenViewModel.Elements.IndexOf(elementViewModel),
            PluginName = plugin.Name,
            Plugin = plugin,
            PluginType = plugin.GetType()?.FullName!,
            AssemblyName = plugin.GetType()?.AssemblyQualifiedName!,

            ElementContext = (FluidUIContext)fluidUIContext,
        };

        return elementConfig;
    }

    private void MainControlsVisibilityChange()
    {
        if (this.MainControlsVisible)
        {
            this.MainControlsVisible = false;

            this.WriteToMessageStrip($"main controls hidden");
            this.OnPropertyChanged(nameof(this.MainControlsVisible));
            return;
        }

        this.MainControlsVisible = true;

        this.WriteToMessageStrip($"main controls visible");
        this.OnPropertyChanged(nameof(this.MainControlsVisible));
    }

    private void MessageStripDurationTimer_Tick(object? sender, EventArgs e)
    {
        this.messageStripMessageActive = false;
        this.OnPropertyChanged(nameof(this.MessageStripMessageActive));

        this.messageStripDurationTimer.Stop();

        this.MessageStripMessage = string.Empty;
    }

    private async Task SetNewEboardDataSavePath()
    {
        var sdKManager = new SDKDataManager();
        await sdKManager.SaveNewEBoardDataPath(new SharedMethod_UI().SetSaveDirectory());

        return;
    }

    [RelayCommand]
    private void Close()
    {
        new SharedMethod_UI().CloseApplication();
    }

    [RelayCommand]
    private void ResetFluidUIDefault()
    {
        this.fluidUIContextCopy = SDKDataManager.DefaultFluidUIContext;
        this.OnPropertyChanged(nameof(this.FluidUIContextCopy));
    }

    [RelayCommand]
    private void SetSavePath()
    {
        this.SetNewEboardDataSavePath().RunSynchronously();
    }

    [RelayCommand]
    private void SwitchMenuVisibility()
    {
        this.MainControlsVisibilityChange();
    }

    [RelayCommand]
    private void SwitchToEboard(object? parameter)
    {
        string? commandParameter = parameter as string;

        if (!string.IsNullOrWhiteSpace(commandParameter))
        {
            this.NavigationContextViewModel?.SwitchToEboard(commandParameter);
        }
    }
}

// EOF