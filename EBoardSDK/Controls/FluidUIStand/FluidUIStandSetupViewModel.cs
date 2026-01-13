// <copyright file="FluidUIStandSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.FluidUIStand;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Interfaces.FluidUIStand;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.ViewModels;
using System;
using System.Windows;
using System.Windows.Media;

public partial class FluidUIStandSetupViewModel : ObservableObject, IFluidUIStandSetup
{
    private EboardFluidUIBaseViewModel viewModel;
    private EboardFluidUIBaseViewModel outerViewModel;
    private EBoardViewModel? eBoardViewModel;
    private FluidUIStandManager fluidUIStandManager;

    private bool fluidUIContextHasStand = false;

    [ObservableProperty]
    private int rotationAngleValue;

    [ObservableProperty]
    private RotateTransform rotateTransformValue;

    [ObservableProperty]
    private Point transformOriginPoint;

    [ObservableProperty]
    private double xTransformOrigin;

    [ObservableProperty]
    private double yTransformOrigin;

    [ObservableProperty]
    private int xMaximumValue;

    [ObservableProperty]
    private int xPosition;

    [ObservableProperty]
    private int yMaximumValue;

    [ObservableProperty]
    private int yPosition;

    [ObservableProperty]
    private int zIndexValue = 0;

    [ObservableProperty]
    private int zMaximumValue;

    [ObservableProperty]
    private int zMinimumValue;

    [ObservableProperty]
    private bool isRotating = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIStandSetupViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    /// <param name="eBoardViewModel"></param>
    /// <param name="fluidUIContextHasStand"></param>
    public FluidUIStandSetupViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel, EBoardViewModel? eBoardViewModel = null, bool fluidUIContextHasStand = false)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.eBoardViewModel = eBoardViewModel;
        this.fluidUIStandManager = new FluidUIStandManager(this.ViewModel);

        this.fluidUIContextHasStand = fluidUIContextHasStand;

        this.ApplyFluidUIStandValues();

        this.SetupSliderValues();

        if (fluidUIContextHasStand)
        {
            this.fluidUIContextHasStand = fluidUIContextHasStand;
        }

        this.OnPropertyChanged(nameof(this.FluidUIContextHasStand));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIStandSetupViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    /// <param name="outerViewModel"></param>
    /// <param name="eBoardViewModel"></param>
    /// <param name="fluidUIContextHasStand"></param>
    public FluidUIStandSetupViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel, EboardFluidUIBaseViewModel outerViewModel, EBoardViewModel? eBoardViewModel = null, bool fluidUIContextHasStand = false)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.outerViewModel = outerViewModel;
        this.eBoardViewModel = eBoardViewModel;
        this.fluidUIStandManager = new FluidUIStandManager(this.outerViewModel);

        this.fluidUIContextHasStand = fluidUIContextHasStand;

        this.ApplyFluidUIStandValues();

        this.SetupSliderValues();

        if (fluidUIContextHasStand)
        {
            this.fluidUIContextHasStand = fluidUIContextHasStand;
        }

        this.OnPropertyChanged(nameof(this.FluidUIContextHasStand));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public event Action? PropertyChangedEvent;

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public bool FluidUIContextHasStand => this.fluidUIContextHasStand;

    public void ApplyFluidUIStandValues()
    {
        this.XPosition = (int)this.fluidUIStandManager.GetPosition().X;
        this.YPosition = (int)this.fluidUIStandManager.GetPosition().Y;
        this.ZIndexValue = this.fluidUIStandManager.GetZ();
        this.RotationAngleValue = (int)this.fluidUIStandManager.GetAngle();
        this.TransformOriginPoint = this.fluidUIStandManager.GetTransformOriginPoint();
        this.XTransformOrigin = this.TransformOriginPoint.X;
        this.YTransformOrigin = this.TransformOriginPoint.Y;

        if (this.outerViewModel != null)
        {
            this.ViewModel.FluidUI.Stand = this.outerViewModel.FluidUI.Stand;
            //this.ViewModel.UpdateStand();
        }
    }

    public void CalibrateZSliderValues(int eboardDepth)
    {
        if (eboardDepth >= 0)
        {
            this.fluidUIStandManager.SetZmaximum(eboardDepth);
            this.fluidUIStandManager.SetZminimum(0);

            this.ZMaximumValue = this.fluidUIStandManager.GetZmaximum();
            this.ZMinimumValue = this.fluidUIStandManager.GetZminimum();

            if (eboardDepth == 0)
            {

                this.fluidUIStandManager.SetZminimum(-1);
                this.fluidUIStandManager.SetZmaximum(1);

                this.ZMaximumValue = this.fluidUIStandManager.GetZmaximum();
                this.ZMinimumValue = this.fluidUIStandManager.GetZminimum();
            }
        }
        else if (eboardDepth < 0)
        {
            this.fluidUIStandManager.SetZmaximum(0);
            this.fluidUIStandManager.SetZminimum(eboardDepth);

            this.ZMaximumValue = this.fluidUIStandManager.GetZmaximum();
            this.ZMinimumValue = this.fluidUIStandManager.GetZminimum();
        }

        this.OnPropertyChanged(nameof(this.ZMaximumValue));
        this.OnPropertyChanged(nameof(this.ZMinimumValue));
    }

    public void Dispose()
    {
    }

    public void Reset()
    {
        this.fluidUIStandManager.Reset();

        this.ApplyFluidUIStandValues();

        this.SetupSliderValues();
    }

    public void SetInitialValues()
    {
    }

    private void SetupSliderValues()
    {
        if (this.eBoardViewModel != null)
        {
            var manager = new FluidUISizeManager(this.eBoardViewModel);

            this.XMaximumValue = manager.GetWidth();
            this.YMaximumValue = manager.GetHeight();

            if (this.eBoardViewModel.FluidUI.Stand != null)
            {
                this.CalibrateZSliderValues(this.eBoardViewModel.FluidUI.Stand.Zmaximum);
            }
        }
        else
        {
            this.XMaximumValue = 2048;
            this.YMaximumValue = 1024;

            this.CalibrateZSliderValues(100);
        }
    }

    private void UpdateRotation()
    {
        this.IsRotating = false;
    }

    partial void OnRotationAngleValueChanging(int oldValue, int newValue)
    {
        if (oldValue != newValue && !this.isRotating)
        {
            this.isRotating = true;

            return;
        }

        if (oldValue != newValue)
        {
            this.UpdateRotation();
            this.fluidUIStandManager.SetAngle(newValue);

            if (this.outerViewModel != null)
            {
                this.ViewModel.FluidUI.Stand = outerViewModel.FluidUI.Stand;
                //this.ViewModel.UpdateStand();
            }

            return;
        }
    }

    partial void OnTransformOriginPointChanged(Point value)
    {
        this.fluidUIStandManager.SetTransformOriginPoint(value);

        if (this.outerViewModel != null)
        {
            this.ViewModel.FluidUI.Stand = outerViewModel.FluidUI.Stand;
            //this.ViewModel.UpdateStand();
        }
    }

    partial void OnXPositionChanged(int value)
    {
        this.fluidUIStandManager.SetX(value);

        if (this.outerViewModel != null)
        {
            this.ViewModel.FluidUI.Stand = outerViewModel.FluidUI.Stand;
            //this.ViewModel.UpdateStand();
        }
    }

    partial void OnYPositionChanged(int value)
    {
        this.fluidUIStandManager.SetY(value);

        if (this.outerViewModel != null)
        {
            this.ViewModel.FluidUI.Stand = outerViewModel.FluidUI.Stand;
            //this.ViewModel.UpdateStand();
        }
    }

    partial void OnXTransformOriginChanged(double value)
    {
        this.TransformOriginPoint = new Point(value, this.TransformOriginPoint.Y);

        if (this.outerViewModel != null)
        {
            this.ViewModel.FluidUI.Stand = outerViewModel.FluidUI.Stand;
            //this.ViewModel.UpdateStand();
        }
    }

    partial void OnYTransformOriginChanged(double value)
    {
        this.TransformOriginPoint = new Point(this.TransformOriginPoint.X, value);

        if (this.outerViewModel != null)
        {
            this.ViewModel.FluidUI.Stand = outerViewModel.FluidUI.Stand;
            //this.ViewModel.UpdateStand();
        }
    }

    partial void OnZIndexValueChanged(int value)
    {
        this.fluidUIStandManager.SetZ(value);

        if (this.outerViewModel != null)
        {
            this.ViewModel.FluidUI.Stand = outerViewModel.FluidUI.Stand;
            //this.ViewModel.UpdateStand();
        }
    }

    [RelayCommand]
    private void ResetStand()
    {
        this.Reset();
    }
}

// EOF