// <copyright file="ScreenViewModel.cs" company=".">
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
using EBoardSDK.Plugins.Eboard.Coordinates;
using EBoardSDK.Plugins.Eboard.MenuBar;
using EBoardSDK.Plugins.Eboard.ScreenChanger;
using EBoardSDK.Plugins.Eboard.ScreenControl;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

public partial class ScreenViewModel : FluidUIBaseViewModel, IEboardIdentity
{
    private readonly FluidUIContextAreas contextArea = FluidUIContextAreas.Screen;

    private readonly MainViewModel mainViewModel;
    private readonly EboardScreen loadedFluidUIConfiguration;

    private EBoardView eBoardView;

    [ObservableProperty]
    private bool menuIsOpening = true;

    [ObservableProperty]
    private ElementViewModel lastClickedElement;

    [ObservableProperty]
    private Point mousePosition;

    [ObservableProperty]
    private ObservableCollection<ElementViewModel> elements = new ObservableCollection<ElementViewModel>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenViewModel"/> class.
    /// </summary>
    /// <param name="mainViewModel"></param>
    /// <param name="eboardscreenconfig"></param>
    public ScreenViewModel(MainViewModel mainViewModel, EboardScreen eboardscreenconfig)
        : base()
    {
        this.mainViewModel = mainViewModel;
        this.loadedFluidUIConfiguration = eboardscreenconfig;

        this.Setup();
    }

    public override FluidUIContextAreas ContextArea => this.contextArea;

    public IList<InstantiationPolicy>? InstantiationPolicies => [
        InstantiationPolicy.Unique,
        InstantiationPolicy.Global,
        InstantiationPolicy.OnePerScreen,
        InstantiationPolicy.DefaultScreenTypesOnly,

        // InstantiationPolicy.Unconstrained,
        // InstantiationPolicy.ValueNotSet
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

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();
    }

    internal MainViewModel MainViewModel => this.mainViewModel;

    internal MenuBarViewModel GetWindowMenuBarViewModel()
    {
        return this.mainViewModel.MenuBar;
    }

    internal void AddElement(ElementViewModel elementViewModel)
    {
        if (!this.Elements.Contains(elementViewModel))
        {
            elementViewModel.Plugin?.RefreshInitialization();
            this.Elements.Add(elementViewModel);

            this.MainViewModel.WriteToMessageStrip(
                $"{elementViewModel.Plugin?.Name ?? "unkown"} plugin invoked on screen {new FluidUIDataBlockManager(this).GetTitle()}, " +
                $"plugin instantiation policy: {elementViewModel.Plugin?.ScreenInstantiationConstraints?.InstantiationPolicy}");
        }

        this.OnPropertyChanged(nameof(this.Elements));
    }

    internal async void ApplyFluidUIConfigOnSelectedElements(FluidUISelectionViewModel configuration)
    {
        foreach (var item in this.GetSelectedElements())
        {
            var manager = new FluidUIContextManager(item);

            var copy = manager.GetCustomFluidUIConfiguration(configuration);

            item.SetFluidUIByUser(copy);
        }
    }

    internal override void BecomesActive()
    {
        this.CreateFluidUIMenuViewModel();

        //foreach (var item in this.Elements)
        //{
        //    item.ScreenBecomesActive();
        //}
    }

    internal override void BecomesInactive()
    {
        base.BecomesInactive();

        this.fluidUIMenuViewModel = null;

        //foreach (var item in this.Elements)
        //{
        //    item.ScreenBecomesInactive();
        //}
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
        string question = MainViewModel.TxtRemoveElementQuestion;
        string title = MainViewModel.TxtRemoveElementTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        var selection = this.GetSelectedElements();
        var count = selection.Count;

        foreach (ElementViewModel item in selection)
        {
            var removeItem = this.DetermineElementCanBeselected(selectionTargets, titleString, item, pluginName);

            if (removeItem)
            {
                this.MainViewModel.WriteToMessageStrip($"deleting {item.EID} containing {item.Plugin?.Name ?? "unknown"} plugin");

                item.Dispose();
                this.Elements.Remove(item);
            }
        }

        this.MainViewModel.WriteToMessageStrip($"{selection.Count} {(selection.Count == 1 ? "plugin" : "plugins")} deleted");
    }

    internal void CopyMoveSelectedElements()
    {
        foreach (var item in this.GetSelectedElements())
        {
            if (this.MainViewModel.DeepCopyElementToElementCopyList(item, moveCopy: true))
            {
                this.Elements.Remove(item);
            }
        }
    }

    internal void CopySelectedElements()
    {
        foreach (var item in this.GetSelectedElements())
        {
            this.MainViewModel.DeepCopyElementToElementCopyList(item, moveCopy: false);
        }
    }

    internal override void CreateFluidUIMenuViewModel()
    {
        if (this.fluidUIMenuViewModel == null)
        {
            this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, Enums.FluidUIStandSettings.ScreenContextArea));
        }
    }

    internal void DeselectElements()
    {
        var selection = this.GetSelectedElements();

        if (selection.Count == 0)
        {
            return;
        }

        foreach (ElementViewModel item in selection)
        {
            item.SelectElement();
        }

        this.MainViewModel.WriteToMessageStrip($"{selection.Count} {(selection.Count == 1 ? "plugin" : "plugins")} deselected");
    }

    internal void DuplicateSelectedElements()
    {
        var selection = this.GetSelectedElements();

        if (selection.Count == 0)
        {
            return;
        }

        foreach (ElementViewModel item in this.GetSelectedElements())
        {
            item.Duplicate();
        }

        this.MainViewModel.WriteToMessageStrip($"{selection.Count} {(selection.Count == 1 ? "plugin" : "plugins")} duplicated");
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
        string cutEBID = string.Empty;

        if (this.loadedFluidUIConfiguration.EBID.StartsWith("Eboard_"))
        {
            cutEBID = this.loadedFluidUIConfiguration.EBID.Replace("Eboard_", string.Empty);
        }

        if (this.loadedFluidUIConfiguration.EBID.StartsWith("EBoard_"))
        {
            cutEBID = this.loadedFluidUIConfiguration.EBID.Replace("EBoard_", string.Empty);
        }

        if (this.loadedFluidUIConfiguration.EBID.StartsWith("Screen_"))
        {
            cutEBID = this.loadedFluidUIConfiguration.EBID.Replace("Screen_", string.Empty);
        }

        long ticks = long.Parse(cutEBID);

        DateTime dateTime = new DateTime(ticks);

        return dateTime;
    }

    internal int GetPluginCategoryCount(PluginCategories pluginCategory)
    {
        var count = this.Elements.Where(x => x?.Plugin?.Category == pluginCategory)?.ToList().Count;

        if (count == null)
        {
            return 0;
        }

        return (int)count;
    }

    internal List<PluginRepresentationItem> GetPlugins()
    {
        return this.mainViewModel.MenuBar.FoundPlugins;
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

    internal async void LoadFluidUIConfigOnSelectedElements(FluidUISelectionViewModel configuration)
    {
        var manager = new FluidUIContextManager(this);
        var deepCopyManager = new FluidUIDeepCopyManager();

        var fluiduiconfig = await manager.GetFluidUIConfigurationFromFile(configuration);

        if (fluiduiconfig != null)
        {
            foreach (var item in this.GetSelectedElements())
            {
                var copy = deepCopyManager.DeepCopyIFluidUIContext(fluiduiconfig);

                item.SetFluidUIByUser(copy);
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
        string question = MainViewModel.TxtRemoveElementQuestion;
        string title = MainViewModel.TxtRemoveElementTitle;

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

            this.MainViewModel.WriteToMessageStrip($"deleting {elementViewModel.EID} containing {elementViewModel.Plugin?.Name ?? "unknown"} plugin");

            elementViewModel.Dispose();

            this.Elements.Remove(elementViewModel);
        }

        this.OnPropertyChanged(nameof(this.Elements));
    }

    internal void ResetSelectedFluidUI(FluidUISelectionViewModel configuration)
    {
        foreach (var item in this.GetSelectedElements())
        {
            IFluidUIManager? manager = null;

            if (configuration.All.Selected)
            {
                manager = new FluidUIContextManager(item);

                manager?.Reset();

                continue;
            }
            else
            {
                if (configuration.DataBlock.Selected)
                {
                    manager = new FluidUIDataBlockManager(item);
                    manager.Reset();
                }

                if (configuration.Design.Selected)
                {
                    manager = new FluidUIDesignManager(item);
                    manager.Reset();
                }

                if (configuration.Font.Selected)
                {
                    manager = new FluidUIFontManager(item);
                    manager.Reset();
                }

                if (configuration.Size.Selected)
                {
                    manager = new FluidUISizeManager(item);
                    manager.Reset();
                }

                if (configuration.Stand.Selected)
                {
                    manager = new FluidUIStandManager(item);
                    manager.Reset();
                }
            }
        }
    }

    internal void SelectSelectionTargets(SelectionTargets selectionTargets, string titleString, string? pluginName = null)
    {
        var counter = 0;

        foreach (var item in this.Elements)
        {
            var selectItem = this.DetermineElementCanBeselected(selectionTargets, titleString, item, pluginName);

            if (selectItem && !item.IsSelected)
            {
                item.SelectElement();
                counter++;
            }
        }

        this.MainViewModel.WriteToMessageStrip($"{counter} {(counter == 1 ? "plugin" : "plugins")} selected");
    }

    /// <inheritdoc/>
    internal override void Setup()
    {
        this.SetFluidUI(this.loadedFluidUIConfiguration.EBoardScreenContext);

        this.CreateFluidUIMenuViewModel();

        this.OnPropertyChanged(nameof(this.Elements));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
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

    internal override void UpdateDesign(bool updateFluidUI = true)
    {
        base.UpdateDesign();

        this.mainViewModel.MenuBar.Update();
    }

    internal override void UpdateFont(bool updateFluidUI = true)
    {
        base.UpdateFont();

        this.mainViewModel.MenuBar.Update();
    }

    internal override void UpdateStand(bool updateFluidUI = true)
    {
        base.UpdateStand();

        this.UpdateElementsZIndexProperties();
    }

    internal void InstallPlugin(PluginRepresentationItem eBoardElementPluginBaseViewModel)
    {
        this.mainViewModel?.MenuBar.InstallPlugin(eBoardElementPluginBaseViewModel);
    }

    /// <summary>
    /// TODO copy plugin model as well or null object
    /// create new instance of iplugin viewmodel.
    /// </summary>
    internal void PasteToScreen()
    {
        var count = this.MainViewModel.ElementCopyList.Count;

        foreach (var item in this.MainViewModel.ElementCopyList)
        {
            if (item.ElementConfig != null)
            {
                var ev = ElementFactory.GetElementViewModel(this, item.ElementConfig);

                if (item.ElementConfig.Plugin == null)
                {
                    ev.Plugin = new SDKPluginManager().InvokePluginByName(item.ElementConfig.PluginName).Result;
                }
                else
                {
                    ev.Plugin = item.ElementConfig.Plugin;
                }

                if (ev.Plugin != null)
                {
                    ev.Plugin.SetElementViewModel(ev);

                    if (ev.Plugin.PluginViewModelType.BaseType == typeof(EboardPluginBaseViewModel))
                    {
                        var eboardPlugin = ev.Plugin as EboardPluginBaseViewModel;

                        eboardPlugin?.SetMainViewModel(this.MainViewModel);
                    }

                    ev.Plugin.RefreshInitialization();

                    if (item.PluginModel != null)
                    {
                        ev.Plugin.InsertModel(item.PluginModel);
                    }
                }

                this.AddElement(ev);
            }
        }

        this.MainViewModel.WriteToMessageStrip($"{count} {(count == 1 ? "plugin" : "plugins")} pasted to screen {new FluidUIDataBlockManager(this).GetTitle()}");

        this.MainViewModel.ElementsPastedToScreen();
    }

    internal void UninstallPlugin(PluginRepresentationItem eBoardElementPluginBaseViewModel)
    {
        this.mainViewModel?.MenuBar.UninstallPlugin(eBoardElementPluginBaseViewModel);
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
                if (item.Plugin.Name.Equals(pluginName))
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
                    if (item.Plugin.Name.Equals(pluginName))
                    {
                        return true;
                    }
                }
                else
                {
                    if (item.Plugin.Category.ToString().Equals(adaptedString))
                    {
                        return true;
                    }
                }

                return false;
            default:
                return false;
        }
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

    private void ScreenBorder_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.MousePosition = e.GetPosition(this.eBoardView.ScreenBorder);
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
        this.MainViewModel?.NavigationContextViewModel.RefreshSelectedEboardData();
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
    private void CreateNavigation()
    {
        var navPlugin = new ScreenChangerViewModel();

        var manager = new SDKPluginManager();
        manager.InvokePluginOnEboard(new ScreenChangerViewModel(), this, this.MousePosition);
    }

    [RelayCommand]
    private void DeleteEboard()
    {
        this.MainViewModel?.NavigationContextViewModel?.RemoveActiveScreen();
    }

    [RelayCommand]
    private void DeselectEboard()
    {
        this.MainViewModel?.NavigationContextViewModel?.DeselectEboard();
    }

    [RelayCommand]
    private async void InvokePlugin(object? parameter)
    {
        string? commandParameter = parameter as string;

        // TODO refactor into SDKPluginManager and use it instead,
        // this code is used by a variety of context menus or buttons.
        if (!string.IsNullOrWhiteSpace(commandParameter))
        {
            var manager = new SDKPluginManager();
            var plugin = await manager.InvokePluginByName(commandParameter);

            manager.InvokePluginOnEboard(plugin, this, this.MousePosition);
        }
    }

    [RelayCommand]
    private void LeftClick()
    {
        this.DeselectElements();
    }

    [RelayCommand]
    private void Paste()
    {
        this.PasteToScreen();
    }

    [RelayCommand]
    private void SwitchToEboard(object? parameter)
    {
        string? commandParameter = parameter as string;

        if (!string.IsNullOrWhiteSpace(commandParameter))
        {
            this.MainViewModel.NavigationContextViewModel?.SwitchToEboard(commandParameter);
        }
    }
}

// EOF