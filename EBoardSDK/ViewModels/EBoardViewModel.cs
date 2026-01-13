// <copyright file="EBoardViewModel.cs" company=".">
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
 *  ViewModel
 *
 *  view model class for EBoardView
 *
 *  it is basically a canvas within a frame and some properties, that can be edited,
 *  stored and loaded
 */
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Helper;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Interfaces.ScreenIntegration;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Models.FluidUIFont;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Tools.Coordinates;
using EBoardSDK.SharedMethods;
using EBoardSDK.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

public partial class EBoardViewModel : EboardFluidUIBaseViewModel, IEboardIdentity
{
    private readonly MainViewModel mainViewModel;
    private readonly EboardScreen loadedFluidUIConfiguration;

    private readonly string txtRemoveAllElementsQuestion = "Delete all elements?";
    private readonly string txtRemoveEboardQuestion = "Delete this screen?";
    private readonly string txtRemoveEboardTitle = "Delete screen(s) confirmation";
    private readonly string txtRemoveElementQuestion = "Delete selected element(s)?";
    private readonly string txtRemoveElementTitle = "Delete Element(s) confirmation";

    private EBoardView eBoardView;

    private ScreenControlViewModel? screenControlViewModel;

    [ObservableProperty]
    private bool menuIsOpening = true;

    [ObservableProperty]
    private ElementViewModel lastClickedElement;

    [ObservableProperty]
    private Point mousePosition;

    [ObservableProperty]
    private ObservableCollection<ElementViewModel> elements = new ObservableCollection<ElementViewModel>();

    /// <summary>
    /// Initializes a new instance of the <see cref="EBoardViewModel"/> class.
    /// </summary>
    /// <param name="mainViewModel"></param>
    /// <param name="eboardscreenconfig"></param>
    public EBoardViewModel(MainViewModel mainViewModel, EboardScreen eboardscreenconfig)
        : base()
    {
        this.mainViewModel = mainViewModel;
        this.loadedFluidUIConfiguration = eboardscreenconfig;

        this.Setup();
    }

    public IList<ElementInstantiationPolicy>? InstantiationPolicies => [
        ElementInstantiationPolicy.Unique,
        ElementInstantiationPolicy.Global,
        ElementInstantiationPolicy.OnePerScreen,
        ElementInstantiationPolicy.DefaultScreenTypesOnly,

        // ElementInstantiationPolicy.Unconstrained,
        // ElementInstantiationPolicy.ValueNotSet
    ];

    public IList<EboardScreenType>? ScreenTypes => [
        EboardScreenType.EBoardDefault,
        EboardScreenType.EboardSDKDefault,
        ];

    /// <summary>
    /// Gets eBoard ID, created upon first creation,
    /// built using $"EBoard_{DateTime().Ticks}".
    /// </summary>
    public string EBID => this.loadedFluidUIConfiguration.EBID;

    public ScreenControlViewModel? ScreenControlViewModel => this.screenControlViewModel;

    public override void Dispose()
    {
        base.Dispose();

        this.screenControlViewModel = null;
    }

    internal MainViewModel MainViewModel => this.mainViewModel;

    internal MainWindowMenuBarViewModel GetWindowMenuBarViewModel()
    {
        return this.mainViewModel.MainWindowMenuBarVM;
    }

    internal void AddElement(ElementViewModel elementViewModel)
    {
        if (!this.Elements.Contains(elementViewModel))
        {
            this.Elements.Add(elementViewModel);
        }

        this.OnPropertyChanged(nameof(this.Elements));
    }

    internal override void BecomesActive()
    {
        this.Setup();
    }

    internal override void BecomesInactive()
    {
        base.BecomesInactive();

        this.fluidUIMenuViewModel = null;
        this.screenControlViewModel = null;
    }

    internal void BeginElementSelectionMovement(ElementViewModel elementViewModel)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false)
            {
                continue;
            }

            if (!item.Equals(elementViewModel))
            {
                item.BeginMovement(elementViewModel);
            }
        }
    }

    internal void ChangeSelection_CornerRadius(ElementViewModel elementViewModel, QuadValue<int> cornerRadius)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            if (item.IsSelected)
            {
                var manager = new FluidUISizeManager(this);
                manager.Apply_FluidUISizeQuadValue(cornerRadius, BorderTargets.CornerRadius);
            }
        }
    }

    internal void ChangeSelection_BackgroundBrush(ElementViewModel elementViewModel, Brush brush)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            if (item.IsSelected)
            {
                item.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.ApplyBrush(brush, BrushTargets.Background);
            }
        }
    }

    internal void ChangeSelection_Height(ElementViewModel elementViewModel, int heightValue)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        var manager = new FluidUISizeManager(this);

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            if (item.IsSelected)
            {
                manager.SetHeight(heightValue);
            }
        }
    }

    internal void ChangeSelection_RotationAngle(ElementViewModel elementViewModel, int rotationValueDelta)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            item.ApplyRotationToGroupSelectedElement(rotationValueDelta);
        }
    }

    internal void ChangeSelection_WidthValue(ElementViewModel elementViewModel, int widthValue)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        var manager = new FluidUISizeManager(this);

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            manager.SetWidth(widthValue);
        }
    }

    internal void ChangeSelection_ZIndex(ElementViewModel elementViewModel, int zIndexValue)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            var manager = new FluidUIStandManager(item);

            manager.SetZ(zIndexValue);
        }
    }

    internal void ClearSelectionTargets(SelectionTargets selectionTargets, string titleString = "title", string? pluginName = null)
    {
        string question = this.txtRemoveElementQuestion;
        string title = this.txtRemoveElementTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        foreach (ElementViewModel item in this.GetSelectedElements())
        {
            var removeItem = this.DetermineElementCanBeselected(selectionTargets, titleString, item, pluginName);

            if (removeItem)
            {
                this.Elements.Remove(item);
            }
        }
    }

    internal override void CreateFluidUIMenuViewModel()
    {
        if (this.fluidUIMenuViewModel == null)
        {
            this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, fluidUIContextHasStand: false));
        }
    }

    internal void DeselectElements()
    {
        foreach (ElementViewModel item in this.GetSelectedElements())
        {
            item.SelectElement();
        }
    }

    internal List<CoordinatesViewModel> GetCoordinates()
    {
        var coordinatesPlugins = new List<CoordinatesViewModel>();

        foreach (ElementViewModel item in this.Elements)
        {
            if (item.Plugin != null)
            {
                if (item.Plugin.GetType() == typeof(CoordinatesViewModel))
                {
                    var coords = item.Plugin as CoordinatesViewModel;

                    if (coords != null)
                    {
                        coordinatesPlugins.Add(coords);
                    }
                }
            }
        }

        return coordinatesPlugins;
    }

    internal DateTime GetCreatedDate()
    {
        string cutEBID = this.loadedFluidUIConfiguration.EBID.Replace("EBoard_", string.Empty);

        long ticks = long.Parse(cutEBID);

        DateTime dateTime = new DateTime(ticks);

        return dateTime;
    }

    internal int GetPluginCategoryCount(PluginCategories pluginCategory)
    {
        var count = this.Elements.Where(x => x?.Plugin?.PluginCategory == pluginCategory)?.ToList().Count;

        if (count == null)
        {
            return 0;
        }

        return (int)count;
    }

    internal List<PluginRepresentationItem> GetPlugins()
    {
        return this.mainViewModel.MainWindowMenuBarVM.FoundPlugins;
    }

    internal int GetTotalCount()
    {
        return this.Elements.Count;
    }

    internal List<ElementViewModel> GetSelectedElements()
    {
        var selectedElements = new List<ElementViewModel>();

        foreach (ElementViewModel item in this.Elements)
        {
            if (item.IsSelected)
            {
                selectedElements.Add(item);
            }
        }

        return selectedElements;
    }
    private void ProcessConfigurationTarget(IFluidUIContext fluiduiconfig, ConfigurationTargets configurationTarget)
    {
        if (configurationTarget != ConfigurationTargets.All)
        {
            if (configurationTarget != ConfigurationTargets.DataBlock)
            {
                fluiduiconfig.DataBlock = null;
            }

            if (configurationTarget != ConfigurationTargets.Design)
            {
                fluiduiconfig.Design = null;
            }

            if (configurationTarget != ConfigurationTargets.Font)
            {
                fluiduiconfig.Font = null;
            }

            if (configurationTarget != ConfigurationTargets.Size)
            {
                fluiduiconfig.Size = null;
            }

            if (configurationTarget != ConfigurationTargets.Stand)
            {
                fluiduiconfig.Stand = null;
            }
        }
    }
    internal async void LoadFluidUIConfigOnSelectedElements(ConfigurationTargets configurationTargets)
    {
        var folder = "eboard/fluidui/";
        var helper = new SharedMethod_UI();

        var filename = await helper.GetFileToLoad(folder, $"files (*.{helper.FluidUIConfigurationFileExtension})|*.{helper.FluidUIConfigurationFileExtension}");

        if (filename != null && Loader.FileExists(filename))
        {
            var fluiduiconfig = await Loader.LoadJsonFile<FluidUIContext>(filename);

            if (fluiduiconfig != null)
            {
                this.ProcessConfigurationTarget(fluiduiconfig, this.ScreenControlViewModel.FluidUISelectionViewModel.ConfigurationTarget);

                fluiduiconfig.Design?.LoadBrushesFromColorData();

                var deepcopy = new FluidUIDeepCopyManager();

                foreach (var item in this.GetSelectedElements())
                {
                    item.SetFluidUI(deepcopy.DeepCopyIFluidUIContext(fluiduiconfig));
                }
            }
        }
    }

    internal void MoveElementSelection(ElementViewModel elementViewModel, Point newPosition)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.GetSelectedElements())
        {
            if (item.EID != null && elementViewModel.EID != null)
            {
                if (item.EID.Equals(elementViewModel.EID))
                {
                    continue;
                }
            }

            item.MoveXY(elementViewModel, newPosition);
        }
    }

    internal void MoveLastClickedElementToEndOfList(ElementViewModel elementViewModel)
    {
        this.LastClickedElement = elementViewModel;

        if (this.Elements.Count > 1)
        {
            this.Elements.Move(this.Elements.IndexOf(this.LastClickedElement), this.Elements.Count - 1);
        }
    }

    internal void RemoveElement(ElementViewModel elementViewModel)
    {
        string question = this.txtRemoveElementQuestion;
        string title = this.txtRemoveElementTitle;

        if (elementViewModel.IsSelected)
        {
            this.ClearSelectionTargets(SelectionTargets.All);
        }
        else
        {
            MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            elementViewModel.Dispose();

            this.Elements.Remove(elementViewModel);
        }

        this.OnPropertyChanged(nameof(this.Elements));
    }

    internal void ResetSelectedFluidUI(ConfigurationTargets configurationTargets)
    {
        foreach (var item in this.GetSelectedElements())
        {
            IFluidUIManager? manager = null;

            switch (configurationTargets)
            {
                case ConfigurationTargets.All:
                    manager = new FluidUIContextManager(item);
                    break;
                case ConfigurationTargets.DataBlock:
                    manager = new FluidUIDataBlockManager(item);
                    break;
                case ConfigurationTargets.Design:
                    manager = new FluidUIDesignManager(item);
                    break;
                case ConfigurationTargets.Font:
                    manager = new FluidUIFontManager(item);
                    break;
                case ConfigurationTargets.Size:
                    manager = new FluidUISizeManager(item);
                    break;
                case ConfigurationTargets.Stand:
                    manager = new FluidUIStandManager(item);
                    break;
                default:
                    break;
            }

            manager?.Reset();
        }
    }

    internal void SelectSelectionTargets(SelectionTargets selectionTargets, string titleString, string? pluginName = null)
    {
        foreach (var item in this.Elements)
        {
            var selectItem = this.DetermineElementCanBeselected(selectionTargets, titleString, item, pluginName);

            if (selectItem && !item.IsSelected)
            {
                item.SelectElement();
            }
        }
    }

    internal override void Setup()
    {
        this.loadedFluidUIConfiguration.EBoardScreenContext.Design?.LoadBrushesFromColorData();
        this.SetFluidUI(this.loadedFluidUIConfiguration.EBoardScreenContext);

        this.CreateFluidUIMenuViewModel();

        this.screenControlViewModel = new ScreenControlViewModel(this);

        this.OnPropertyChanged(nameof(this.Elements));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.ScreenControlViewModel));
    }

    internal void SetView(EBoardView eBoardView)
    {
        this.eBoardView = eBoardView;

        this.eBoardView.ScreenBorder.MouseMove += this.ScreenBorder_MouseMove;
    }

    internal void StopElementSelectionMovement(ElementViewModel elementViewModel)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.GetSelectedElements())
        {
            if (!item.EID.Equals(elementViewModel.EID))
            {
                item.StopMovement();
            }
        }
    }

    internal override void UpdateDesign()
    {
        base.UpdateDesign();

        this.mainViewModel.MainWindowMenuBarVM.Update();
    }

    internal override void UpdateFont()
    {
        base.UpdateFont();

        this.mainViewModel.MainWindowMenuBarVM.Update();
    }

    internal override void UpdateStand()
    {
        base.UpdateStand();

        this.UpdateElementsZIndexProperties();
    }

    internal void InstallPlugin(PluginRepresentationItem eBoardElementPluginBaseViewModel)
    {
        this.mainViewModel?.MainWindowMenuBarVM.InstallPlugin(eBoardElementPluginBaseViewModel);
    }

    internal void UninstallPlugin(PluginRepresentationItem eBoardElementPluginBaseViewModel)
    {
        this.mainViewModel?.MainWindowMenuBarVM.UninstallPlugin(eBoardElementPluginBaseViewModel);
    }

    internal override void UpdateFluidUI()
    {
        base.UpdateFluidUI();
    }

    private bool DetermineElementCanBeselected(SelectionTargets selectionTargets, string titleString, ElementViewModel item, string? pluginName = null)
    {
        switch (selectionTargets)
        {
            case SelectionTargets.All:
                return true;
            case SelectionTargets.Specific:
                if (item.Plugin.PluginName.Equals(pluginName))
                {
                    return true;
                }

                return false;
            case SelectionTargets.Title:
                if (item.FluidUI.DataBlock != null)
                {
                    if (item.FluidUI.DataBlock.Title.Equals(titleString))
                    {
                        return true;
                    }
                }

                return false;
            case SelectionTargets.Addons:
            case SelectionTargets.Elements:
            case SelectionTargets.Shapes:
            case SelectionTargets.Areas:
            case SelectionTargets.Tools:
                var selectedTarget = selectionTargets.ToString();
                var adaptedString = selectedTarget.Remove(selectedTarget.Length - 1);

                if (pluginName != null)
                {
                    if (item.Plugin.PluginName.Equals(pluginName))
                    {
                        return true;
                    }
                }
                else
                {
                    if (item.Plugin.PluginCategory.ToString().Equals(adaptedString))
                    {
                        return true;
                    }
                }

                return false;
            default:
                return false;
        }
    }

    private void ScreenBorder_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.MousePosition = e.GetPosition(this.eBoardView.ScreenBorder);
    }

    private void SwitchToNextEboard()
    {
        for (int i = 0; i < this.mainViewModel.EBoardBrowserViewModel.EBoards.Count; i++)
        {
            if (this.mainViewModel.EBoardBrowserViewModel.EBoards[i] == this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard)
            {
                if (i + 1 < this.mainViewModel.EBoardBrowserViewModel.EBoards.Count)
                {
                    this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard = this.mainViewModel.EBoardBrowserViewModel.EBoards[i + 1];

                    break;
                }
            }
        }
    }

    private void SwitchToPrevEboard()
    {
        for (int i = 0; i < this.mainViewModel.EBoardBrowserViewModel.EBoards.Count; i++)
        {
            if (this.mainViewModel.EBoardBrowserViewModel.EBoards[i] == this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard)
            {
                if (i - 1 >= 0)
                {
                    this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard = this.mainViewModel.EBoardBrowserViewModel.EBoards[i - 1];

                    break;
                }
            }
        }
    }

    private void UpdateElementsZIndexProperties()
    {
        if (this.Elements != null && this.Elements.Count > 0 && this.FluidUI.Stand != null)
        {
            foreach (ElementViewModel item in this.Elements)
            {
                var manager = new FluidUIStandManager(item);

                manager.SetZmaximum(this.FluidUI.Stand.Zmaximum);
            }
        }
    }

    partial void OnElementsChanged(ObservableCollection<ElementViewModel>? oldValue, ObservableCollection<ElementViewModel> newValue)
    {
        this.MainViewModel?.EBoardBrowserViewModel.RefreshSelectedEboardData();
    }

    partial void OnMousePositionChanged(Point value)
    {
        foreach (var item in this.GetCoordinates())
        {
            item.ChangeMouseCoords(value);
        }
    }

    partial void OnMenuIsOpeningChanging(bool value)
    {
        if (value)
        {
            this.CreateFluidUIMenuViewModel();
        }
        else
        {
            this.DeleteFluidUIMenuViewModel();
        }
    }

    [RelayCommand]
    private void DeleteEBoard()
    {
        this.mainViewModel?.EBoardBrowserViewModel?.RemoveEBoard(this);
    }

    [RelayCommand]
    private void LeftClick()
    {
        this.DeselectElements();
    }

    [RelayCommand]
    public void SwitchToEboard(object? parameter)
    {
        string? commandParameter = parameter as string;

        if (this.mainViewModel != null && this.mainViewModel.EBoardBrowserViewModel.EBoards.Count > 1)
        {
            switch (commandParameter)
            {
                case "First":
                    this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard = this.mainViewModel.EBoardBrowserViewModel.EBoards.First();
                    break;
                case "Prev":
                    this.SwitchToPrevEboard();
                    break;

                case "Next":
                    this.SwitchToNextEboard();
                    break;

                case "Last":
                    this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard = this.mainViewModel.EBoardBrowserViewModel.EBoards.Last();
                    break;

                default:
                    break;
            }
        }
    }
}

// EOF