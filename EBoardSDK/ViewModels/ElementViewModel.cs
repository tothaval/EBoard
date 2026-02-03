// <copyright file="ElementViewModel.cs" company=".">
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
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.Plugins.Eboard.Coordinates;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.Views;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// ElementViewModel is the FluidUI Element Context Area of the eboard prototype.
///
/// Its job is to contain a derivate of a PluginBaseViewModel, that offers functions
/// to the user. Its implementation offers a way to display a menuitem for the plugin.
///
/// -> Elements can be instantiated and manipulated within other FluidUI context areas
///    depending on the Plugins ScreenInstantiationPolicy and other factors(partly implemented)
/// -> Elements can be transformed in a variety of ways to the users wishes using FluidUI
/// -> Elements show a ToolTip with FluidUI-DataBlock properties and Screen coordinates
/// -> Elements have a right-click context menu with Plugin menu, Element menu and FluidUI menu
/// -> Elements can be selected via left-click and dragged (x & y axis movement) via left-click
///    pressed and mousemove
/// -> Z level movement can be done via mousewheel rotation
/// -> Rotation angle change can be done via ctrl-mousewheel rotation
/// -> Groups of elements can be manipulated together (broken/implementation disabled)
///
/// Be advised:
/// View code behind is used.
/// Date of entry: 2026|01|20.
/// </summary>
public partial class ElementViewModel : FluidUIBaseViewModel, IElementSelection
{
    private readonly FluidUIContextAreas contextArea = FluidUIContextAreas.Element;

    private ScreenViewModel screenViewModel;

    private string eID;

    private ElementView? elementView;

    [ObservableProperty]
    private bool menuIsOpening = true;

    [ObservableProperty]
    private bool isSelected = false;

    [ObservableProperty]
    private int duplicationCount = 1;

    [ObservableProperty]
    private IPlugin? plugin;

    [ObservableProperty]
    private ElementConfig? elementConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="ElementViewModel"/> class.
    /// </summary>
    /// <param name="eBoardViewModel">Desired is the FluidUI screen context area containing the element context area instance.</param>
    public ElementViewModel(ScreenViewModel eBoardViewModel)
        : base()
    {
        this.screenViewModel = eBoardViewModel;

        this.CreateFluidUIMenuViewModel();

        var manager = new FluidUISizeManager(this);
        manager.Reset();

        if (string.IsNullOrWhiteSpace(this.eID) || this.eID.Equals("-1"))
        {
            this.eID = new EboardIdFactory().GetElementId();
            this.OnPropertyChanged(nameof(this.EID));
        }

        this.TriggerRedraw();

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ElementViewModel"/> class.
    /// </summary>
    /// <param name="eBoardViewModel">Desired is the FluidUI screen context area containing the element context area instance.</param>
    /// <param name="elementConfig">Desired is the serialization model class of the element instance.</param>
    public ElementViewModel(ScreenViewModel eBoardViewModel, ElementConfig elementConfig)
        : base()
    {
        this.screenViewModel = eBoardViewModel;

        if (elementConfig != null)
        {
            this.SetFluidUI(elementConfig.ElementContext);
            this.ApplyData(elementConfig);
        }

        this.CreateFluidUIMenuViewModel();

        if (string.IsNullOrWhiteSpace(this.eID) || this.eID.Equals("-1"))
        {
            this.eID = new EboardIdFactory().GetElementId();
            this.OnPropertyChanged(nameof(this.EID));
        }

        this.TriggerRedraw();

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    public override FluidUIContextAreas ContextArea => this.contextArea;

    /// <summary>
    /// Gets the element ID.
    ///
    /// This string property is used in serialization for the filename
    /// and for comparisons between ElementViewModel instances. It
    /// consists of a prefix and DateTime.Ticks number, that is retrieved
    /// on ElementViewModel instantiation.
    ///
    /// Be advised:
    /// It was not tested if situations can occur where EIDs of many
    /// Elements created in parallel equal each other. This could lead
    /// to problems filtering and saving the ElementViewModel instance.
    /// Date of entry: 2026|01|20.
    /// </summary>
    public string EID => this.eID;

    /// <summary>
    /// Gets the FluidUI screen context area that contains the
    /// ElementViewModel instance.
    /// </summary>
    public ScreenViewModel ScreenViewModel => this.screenViewModel;

    /// <summary>
    /// Gets the underlying View for this ViewModel. Given incomplete
    /// knowledge of WPF XAML and some requirements for ElementViewModel,
    /// code behind must be used on some occations. This became evident
    /// through the development of this FluidUI context area, but does
    /// not necessarily mean that it is the best way.
    ///
    /// This property provides an ok way to access the view if necessary.
    /// It should be avoided using it.
    /// </summary>
    public ElementView? ElementView => this.elementView;

    public bool CopyAllowed { get; set; }

    public void ApplyData(ElementConfig elementConfig)
    {
        this.ElementConfig = elementConfig;

        this.eID = this.ElementConfig.EID;
    }

    public void ApplyRotationToGroupSelectedElement(int rotationValueDelta)
    {
        var manager = new FluidUIStandManager(this);

        var nextangle = manager.GetAngle() + rotationValueDelta;

        manager.SetAngle(nextangle);
    }

    public void BeginMovement(ElementViewModel elementViewModel)
    {
        if (this.ElementView != null)
        {
            var manager = new FluidUIStandManager(this);

            manager.SetPosition(this.ElementView.Position);
        }
    }

    //public void ScreenBecomesActive()
    //{
    //    this.CreateFluidUIMenuViewModel();
    //}

    //public void ScreenBecomesInactive()
    //{
    //    this.FluidUIMenuViewModel?.Dispose();
    //    this.fluidUIMenuViewModel = null;
    //}

    public override void Dispose()
    {
        base.Dispose();

        this.FluidUI?.Dispose();
        this.FluidUIMenuViewModel?.Dispose();
        this.fluidUIMenuViewModel = null;

        this.Plugin?.Dispose();
    }

    public void MoveXY(ElementViewModel elementViewModel, Point deltaPosition)
    {
        if (this.ElementView != null)
        {
            var manager = new FluidUIStandManager(this);

            double x, y;

            x = manager.GetPosition().X - deltaPosition.X;
            y = manager.GetPosition().Y - deltaPosition.Y;

            manager.SetPosition(x, y);

            Canvas.SetLeft(this.ElementView.VisualParent, x);
            Canvas.SetTop(this.ElementView.VisualParent, y);
        }
    }

    public void Redraw()
    {
        if (this.Plugin == null)
        {
            return;
        }

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    public void SelectElement()
    {
        this.IsSelected = !this.IsSelected;

        this.SelectionChange(this.IsSelected);
    }

    public bool SelectionChange(bool isSelected)
    {
        var manager = new FluidUIDesignManager(this);

        if (isSelected)
        {
            manager.SwitchBorderToHighlight();

            return true;
        }

        manager.SwitchBorderToBorder();

        return false;
    }

    public void SetView(ElementView elementView)
    {
        this.elementView = elementView;

        if (this.Plugin != null)
        {
            this.Plugin.ViewWasSet();
        }
    }

    // trigggered for selection movement endpoint
    public void StopMovement()
    {
        var manager = new FluidUIStandManager(this);

        double x, y;
        int z;

        if (this.ElementView != null)
        {
            x = this.ElementView.X;
            y = this.ElementView.Y;
            z = this.ElementView.Z;

            manager.SetPosition(x, y);
            manager.SetZ(z);
        }

        this.FluidUIMenuViewModel?.FluidUIStandSetupViewModel?.SetupViewModel.ApplyFluidUIStandValues();
    }

    public void WasLastActive()
    {
        this.screenViewModel?.MoveLastClickedElementToEndOfList(this);
    }

    internal override void CreateFluidUIMenuViewModel()
    {
        this.fluidUIMenuViewModel?.Dispose();

        if (this.fluidUIMenuViewModel == null)
        {
            this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, Enums.FluidUIStandSettings.ElementContextArea, screenViewModel: this.screenViewModel));
        }

        this.FluidUIMenuViewModel.CreateViewModels();
    }

    internal void Duplicate()
    {
        this.Plugin?.Duplicate();
    }

    internal void PrepareCopy()
    {
        this.Plugin?.PrepareCopy();
    }

    internal override void TriggerRedraw()
    {
        if (this.ElementView != null)
        {
            this.ElementView.SetPlacement();
        }
    }

    internal override void UpdateStand(bool updateFluidUI = true)
    {
        base.UpdateStand();

        if (this.Plugin != null)
        {
            var coords = this.Plugin as CoordinatesViewModel;

            if (coords != null)
            {
                coords.ChangeXYCoord();
            }
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

    private void ChangeSelection_CornerRadiusValue(QuadValue<int> cornerRadius)
    {
        this.screenViewModel?.ChangeSelection_CornerRadius(this, cornerRadius);
    }

    private void ChangeSelection_HeightValue(int heightValue)
    {
        this.screenViewModel?.ChangeSelection_Height(this, heightValue);
    }

    private void ChangeSelection_RotationAngleValue(int rotationValueDelta)
    {
        this.screenViewModel?.ChangeSelection_RotationAngle(this, rotationValueDelta);
    }

    private void ChangeSelection_WidthValue(int widthValue)
    {
        this.screenViewModel?.ChangeSelection_WidthValue(this, widthValue);
    }

    private void ChangeSelection_ZIndexValue(int zIndexValue)
    {
        this.screenViewModel?.ChangeSelection_ZIndex(this, zIndexValue);
    }

    partial void OnPluginChanged(IPlugin? value)
    {
        if (value != null && value.ScreenInstantiationConstraints != null)
        {
            this.CopyAllowed = true;

            if (value.ScreenInstantiationConstraints.CopyConstraints == Enums.CopyConstraints.Denied
                || value.ScreenInstantiationConstraints.CopyConstraints == Enums.CopyConstraints.ValueNotSet)
            {
                this.CopyAllowed = false;
                return;
            }

            if (value.ScreenInstantiationConstraints.InstantiationPolicy == Enums.InstantiationPolicy.OnePerScreen
                || value.ScreenInstantiationConstraints.InstantiationPolicy == Enums.InstantiationPolicy.Global
                || value.ScreenInstantiationConstraints.InstantiationPolicy == Enums.InstantiationPolicy.Unique
                || value.ScreenInstantiationConstraints.InstantiationPolicy == Enums.InstantiationPolicy.ValueNotSet)
            {
                this.CopyAllowed = false;
                return;
            }
        }

        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.CopyAllowed));
    }

    private void DeleteThisElement()
    {
        this.screenViewModel?.RemoveElement(this);
    }

    [RelayCommand]
    private void Copy()
    {
        this.screenViewModel?.MainViewModel.DeepCopyElementToElementCopyList(this, moveCopy: false);
        // TODO implement, write new elementconfig instance into mainwindow copylist
    }

    [RelayCommand]
    private void Delete(object s)
    {
        this.DeleteThisElement();
    }

    [RelayCommand]
    private void DuplicateNTimes()
    {
        this.Duplicate();
    }

    [RelayCommand]
    private void Move()
    {
        if (this.ScreenViewModel.MainViewModel.DeepCopyElementToElementCopyList(this, moveCopy: true))
        {
            this.DeleteThisElement();
        }
    }

    [RelayCommand]
    private void Save()
    {
        // TODO implement, write elementconfig to designated location
    }

    [RelayCommand]
    private void Select()
    {
        this.SelectElement();
    }
}

// EOF