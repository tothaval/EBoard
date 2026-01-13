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
/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  ElementViewModel
 *
 *  view model for ElementView, which offers some basic dragmove functionality,
 *  element placement properties within EBoardView canvas and basic content management
 */
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.Plugins.Tools.Coordinates;
using EBoardSDK.Views;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// TODO: refactoring to further reduce code towards the necessary minimum and nothing more.
///
/// update minium(minus?) and maximum values for x, y and z upon valuechange in eboardviewmodel.
///
/// </summary>
public partial class ElementViewModel : EboardFluidUIBaseViewModel, IElementSelection
{
    private EBoardViewModel eBoardViewModel;

    private string eID;

    private ElementView elementView;

    [ObservableProperty]
    private bool menuIsOpening = true;

    [ObservableProperty]
    private bool isSelected = false;

    [ObservableProperty]
    private IPlugin plugin;

    /// <summary>
    /// Initializes a new instance of the <see cref="ElementViewModel"/> class.
    /// </summary>
    /// <param name="eBoardViewModel"></param>
    public ElementViewModel(EBoardViewModel eBoardViewModel)
        : base()
    {
        this.eBoardViewModel = eBoardViewModel;

        this.CreateFluidUIMenuViewModel();

        var manager = new FluidUISizeManager(this);
        manager.Reset();

        if (this.eID == null || this.eID.Equals("-1"))
        {
            DateTime dateTime = DateTime.Now;

            this.eID = $"Element_{dateTime.Ticks}";
            this.OnPropertyChanged(nameof(this.EID));
        }

        this.SetSize();

        this.TriggerRedraw();

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ElementViewModel"/> class.
    /// </summary>
    /// <param name="eBoardViewModel"></param>
    /// <param name="elementConfig"></param>
    public ElementViewModel(EBoardViewModel eBoardViewModel, ElementConfig elementConfig)
        : base()
    {
        this.eBoardViewModel = eBoardViewModel;

        elementConfig?.ElementContext?.Design?.LoadBrushesFromColorData();

        if (elementConfig != null)
        {
            this.SetFluidUI(elementConfig.ElementContext);
            this.ApplyData(elementConfig);
        }

        this.CreateFluidUIMenuViewModel();

        if (this.eID == null || this.eID.Equals("-1"))
        {
            DateTime dateTime = DateTime.Now;

            this.eID = $"Element_{dateTime.Ticks}";
            this.OnPropertyChanged(nameof(this.EID));
        }

        this.SetSize();

        this.TriggerRedraw();

        this.OnPropertyChanged(nameof(this.FluidUI.DataBlock.Title));
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    public string EID => this.eID;

    public EBoardViewModel EBoardViewModel => this.eBoardViewModel;

    public ElementView ElementView => this.elementView;

    public void ApplyData(ElementConfig elementConfig)
    {
        elementConfig.SetEBoardAndElementViewModel(this.eBoardViewModel, this);

        this.eID = elementConfig.EID;
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

    public override void Dispose()
    {
        base.Dispose();

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

        x = this.ElementView.X;
        y = this.ElementView.Y;
        z = this.ElementView.Z;

        manager.SetPosition(x, y);
        manager.SetZ(z);

        this.FluidUIMenuViewModel?.FluidUIStandSetupViewModel?.ApplyFluidUIStandValues();
    }

    public void WasLastActive()
    {
        this.eBoardViewModel?.MoveLastClickedElementToEndOfList(this);
    }

    internal override void CreateFluidUIMenuViewModel()
    {
        this.fluidUIMenuViewModel?.Dispose();

        if (this.fluidUIMenuViewModel == null)
        {
            this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, eBoardViewModel: this.eBoardViewModel, fluidUIContextHasStand: true));
        }

        this.FluidUIMenuViewModel.CreateViewModels();
    }

    internal override void TriggerRedraw()
    {
        if (this.ElementView != null)
        {
            this.ElementView.SetPlacement();
        }
    }

    internal override void UpdateStand()
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
        this.eBoardViewModel?.ChangeSelection_CornerRadius(this, cornerRadius);
    }

    private void ChangeSelection_HeightValue(int heightValue)
    {
        this.eBoardViewModel?.ChangeSelection_Height(this, heightValue);
    }

    private void ChangeSelection_RotationAngleValue(int rotationValueDelta)
    {
        this.eBoardViewModel?.ChangeSelection_RotationAngle(this, rotationValueDelta);
    }

    private void ChangeSelection_WidthValue(int widthValue)
    {
        this.eBoardViewModel?.ChangeSelection_WidthValue(this, widthValue);
    }

    private void ChangeSelection_ZIndexValue(int zIndexValue)
    {
        this.eBoardViewModel?.ChangeSelection_ZIndex(this, zIndexValue);
    }

    partial void OnPluginChanged(IPlugin value)
    {
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    private void DeleteElement(object s)
    {
        this.eBoardViewModel?.RemoveElement(this);
    }

    [RelayCommand]
    private void Select()
    {
        this.SelectElement();
    }
}

// EOF