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
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using EBoardSDK.Views;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// TODO: refactoring to further reduce code towards the necessary minimum and nothing more.
///
/// update minium(minus?) and maximum values for x, y and z upon valuechange in eboardviewmodel
///
/// </summary>
public partial class ElementViewModel : EboardFluidUIBaseViewModel, IElementSelection
{
    private EBoardViewModel eBoardViewModel;

    private string eID;

    private ElementView elementView;

    [ObservableProperty]
    private bool isSelected = false;

    private ArrangeSelectedElements arrangeSelectedElements;

    //public ElementViewModel()
    //    : base()
    //{
    //    this.fluidUIMenuViewModel = new FluidUIMenuViewModel(this, eBoardViewModel: this.eBoardViewModel, fluidUIContextHasStand: true);

    //    this.arrangeSelectedElements = new ArrangeSelectedElements();

    //    if (this.EID == null || this.EID.Equals("-1"))
    //    {
    //        DateTime dateTime = DateTime.Now;

    //        this.eID = $"Element_{dateTime.Ticks}";
    //        this.OnPropertyChanged(nameof(this.EID));
    //    }

    //    this.FontSizeValue = (int)this.FluidUI.Font.FontSize;

    //    this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
    //    this.OnPropertyChanged(nameof(this.FluidUI));
    //}

    public ElementViewModel(EBoardViewModel eBoardViewModel)
        : base()
    {
        this.eBoardViewModel = eBoardViewModel;

        this.fluidUIMenuViewModel = new FluidUIMenuViewModel(this, eBoardViewModel: this.eBoardViewModel, fluidUIContextHasStand: true);

        this.arrangeSelectedElements = new ArrangeSelectedElements();

        if (this.eID == null || this.eID.Equals("-1"))
        {
            DateTime dateTime = DateTime.Now;

            this.eID = $"Element_{dateTime.Ticks}";
            this.OnPropertyChanged(nameof(this.EID));
        }

        var helper = new SharedMethod_UI();
        helper.SetupTitleAndText(
            this.FluidUI.DataBlock,
            "Element",
            $"elements can be changed, moved and selected\n\nthis element contains the {this.Plugin?.PluginName} plugin\n\nyou can change this description");

        this.FontSizeValue = (int)this.FluidUI.Font.FontSize;

        this.SetElementSizeDisplayValue();

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    public ElementViewModel(EBoardViewModel eBoardViewModel, ElementConfig elementConfig)
        : base()
    {
        this.eBoardViewModel = eBoardViewModel;

        elementConfig?.ElementContext?.Design?.LoadBrushesFromColorData();

        this.SetFluidUI(elementConfig.ElementContext);

        var helper = new SharedMethod_UI();
        helper.SetupTitleAndText(
            this.FluidUI.DataBlock,
            "Element",
            $"elements can be changed, moved and selected\n\nthis element contains the {this.Plugin?.PluginName} plugin\n\nyou can change this description");

        this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, eBoardViewModel: this.eBoardViewModel, fluidUIContextHasStand: true));

        this.arrangeSelectedElements = new ArrangeSelectedElements();

        this.FontSizeValue = (int)this.FluidUI.Font.FontSize;

        this.ApplyData(elementConfig);

        if (this.eID == null || this.eID.Equals("-1"))
        {
            DateTime dateTime = DateTime.Now;

            this.eID = $"Element_{dateTime.Ticks}";
            this.OnPropertyChanged(nameof(this.EID));
        }

        this.SetElementSizeDisplayValue();

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    public string EID => this.eID;

    public EBoardViewModel EBoardViewModel => this.eBoardViewModel;

    public ElementView ElementView => this.elementView;

    public IPlugin Plugin { get; set; }

    public void ApplyData(ElementConfig elementConfig)
    {
        elementConfig.SetEBoardAndElementViewModel(this.eBoardViewModel, this);

        this.eID = elementConfig.EID;
    }

    public void ApplyRotationToGroupSelectedElement(int rotationValueDelta)
    {
        this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.IsRotating = true;

        this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.RotationAngleValue += rotationValueDelta;
    }

    public void BeginMovement(ElementViewModel elementViewModel)
    {
        this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = this.ElementView.X;
        this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = this.ElementView.Y;

        this.FluidUI.Stand.Position = new Point(this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition, this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition);
    }

    public void MoveXY(ElementViewModel elementViewModel, Point deltaPosition)
    {
        if (this.elementView != null)
        {
            double x, y;

            x = this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition - deltaPosition.X;
            y = this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition - deltaPosition.Y;

            this.FluidUI.Stand.Position = new Point(x, y);

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

    [RelayCommand]
    public void Select()
    {
        this.IsSelected = !this.IsSelected;

        this.SelectionChange(this.IsSelected);
    }

    public bool SelectionChange(bool isSelected)
    {
        if (isSelected)
        {
            this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.SwitchBorderToHighlight();

            this.OnPropertyChanged(nameof(this.FluidUI));
            this.OnPropertyChanged(nameof(this.Plugin));

            return true;
        }

        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.SwitchBorderToBorder();

        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.Plugin));

        return false;
    }

    public void SetView(ElementView elementView)
    {
        this.elementView = elementView;
    }

    public void StopMovement()
    {
        this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = Canvas.GetLeft(this.elementView.VisualParent);
        this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = Canvas.GetTop(this.elementView.VisualParent);

        this.FluidUI.Stand.Position = new Point(this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition, this.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition);
    }

    public override void TriggerRedraw()
    {
        if (this.ElementView != null)
        {
            this.ElementView.X = this.FluidUI.Stand.Position.X;
            this.ElementView.Y = this.FluidUI.Stand.Position.Y;
            this.ElementView.Z = this.FluidUI.Stand.Z;

            this.ElementView.SetPlacement();
        }
    }

    public void WasLastActive()
    {
        this.eBoardViewModel?.MoveLastClickedElement(this);
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

    [RelayCommand]
    private void DeleteElement(object s)
    {
        this.eBoardViewModel?.RemoveElement(this);
    }

    [RelayCommand]
    private void ArrangeGroupAsLine()
    {
        this.arrangeSelectedElements.ArrangeGroupAsLine(this.EBoardViewModel);
    }

    [RelayCommand]
    private void ArrangeGroupAsSquare()
    {
        this.arrangeSelectedElements.ArrangeGroupAsSquare(this.EBoardViewModel);
    }

    [RelayCommand]
    private void ArrangeGroupAsRandomMatrix10x10()
    {
        this.arrangeSelectedElements.ArrangeGroupAsRandomMatrix10x10(this.EBoardViewModel);
    }
}

// EOF