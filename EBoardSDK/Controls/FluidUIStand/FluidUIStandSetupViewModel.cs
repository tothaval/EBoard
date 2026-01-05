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
using EBoardSDK.Interfaces.FluidUIStand;
using EBoardSDK.ViewModels;
using System;
using System.Windows;
using System.Windows.Media;

public partial class FluidUIStandSetupViewModel : ObservableObject, IFluidUIStandSetup
{
    private EboardFluidUIBaseViewModel viewModel;
    private EBoardViewModel? eBoardViewModel;

    private bool fluidUIContextHasStand = false;

    [ObservableProperty]
    private int rotationAngleValue;

    [ObservableProperty]
    private RotateTransform rotateTransformValue;

    [ObservableProperty]
    private Point transformOriginPoint;

    [ObservableProperty]
    private int xMaximumValue;

    [ObservableProperty]
    private double xPosition;

    [ObservableProperty]
    private int xSliderValue;

    [ObservableProperty]
    private int yMaximumValue;

    [ObservableProperty]
    private double yPosition;

    [ObservableProperty]
    private int ySliderValue;

    [ObservableProperty]
    private int zIndexValue = 0;

    [ObservableProperty]
    private int zMaximumValue;

    [ObservableProperty]
    private int zMinimumValue;

    [ObservableProperty]
    private bool isRotating = false;

    public FluidUIStandSetupViewModel()
    {
        this.XMaximumValue = 2048;
        this.YMaximumValue = 1024;

        this.ZIndexValue = this.ViewModel.FluidUI.Stand.Z;
        this.RotationAngleValue = (int)this.ViewModel.FluidUI.Stand.Angle;

        this.CalibrateZSliderValues(100);

        this.XPosition = (int)this.ViewModel.FluidUI.Stand.Position.X;
        this.YPosition = (int)this.ViewModel.FluidUI.Stand.Position.Y;

        this.ApplyRotationAngleValue(this.RotationAngleValue);
    }

    public FluidUIStandSetupViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel, EBoardViewModel? eBoardViewModel = null, bool fluidUIContextHasStand = false)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.eBoardViewModel = eBoardViewModel;
        this.fluidUIContextHasStand = fluidUIContextHasStand;

        this.ZIndexValue = this.ViewModel.FluidUI.Stand.Z;
        this.RotationAngleValue = (int)this.ViewModel.FluidUI.Stand.Angle;

        if (this.eBoardViewModel != null)
        {
            this.XMaximumValue = (int)this.eBoardViewModel.FluidUI.Size.Width;
            this.YMaximumValue = (int)this.eBoardViewModel.FluidUI.Size.Height;
            this.CalibrateZSliderValues(this.eBoardViewModel.EBoardDepth);
        }
        else
        {
            this.XMaximumValue = 2048;
            this.YMaximumValue = 1024;
            this.CalibrateZSliderValues(100);
        }

        this.XPosition = (int)this.ViewModel.FluidUI.Stand.Position.X;
        this.YPosition = (int)this.ViewModel.FluidUI.Stand.Position.Y;

        this.ApplyRotationAngleValue(this.RotationAngleValue);

        if (fluidUIContextHasStand)
        {
            this.fluidUIContextHasStand = fluidUIContextHasStand;
        }

        this.OnPropertyChanged(nameof(this.FluidUIContextHasStand));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public event Action PropertyChangedEvent;

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public bool FluidUIContextHasStand => this.fluidUIContextHasStand;

    public int ApplyRotationAngleValue(int rotationAngleValue)
    {
        this.UpdateRotation(rotationAngleValue);

        this.RotationAngleValue = rotationAngleValue;

        return this.RotationAngleValue;
    }

    public int ApplyRotationAngleValueByMouseWheel(int delta)
    {
        if (delta < 0 && this.RotationAngleValue > -180)
        {
            this.RotationAngleValue--;
        }

        if (delta > 0 && this.RotationAngleValue < 180)
        {
            this.RotationAngleValue++;
        }

        this.UpdateRotation(this.RotationAngleValue);

        return this.RotationAngleValue;
    }

    public void ApplyZIndexValue(int zIndexValue)
    {
        this.ZIndexValue = zIndexValue;

        this.ViewModel.FluidUI.Stand.Z = zIndexValue;

        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Stand.Z));
        this.OnPropertyChanged(nameof(this.ZIndexValue));
    }

    public int ApplyZIndexValueByMouseWheel(int delta)
    {
        if (delta < 0 && this.ZMinimumValue < this.ZIndexValue)
        {
            this.ZIndexValue--;
        }

        if (delta > 0 && this.ZMaximumValue > this.ZIndexValue)
        {
            this.ZIndexValue++;
        }

        this.ViewModel.FluidUI.Stand.Z = this.ZIndexValue;

        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Stand.Z));
        this.OnPropertyChanged(nameof(this.ZIndexValue));

        return this.ZIndexValue;
    }

    public void CalibrateZSliderValues(int eboardDepth)
    {
        if (eboardDepth >= 0)
        {
            this.ZMinimumValue = 0;
            this.ZMaximumValue = eboardDepth;

            if (eboardDepth == 0)
            {
                this.ZMaximumValue = 1;
            }
        }
        else if (eboardDepth < 0)
        {
            this.ZMinimumValue = eboardDepth;
            this.ZMaximumValue = 0;
        }

        this.OnPropertyChanged(nameof(this.ZMinimumValue));
        this.OnPropertyChanged(nameof(this.ZMaximumValue));
    }

    public void Dispose()
    {
    }

    public void SetInitialValues()
    {
    }

    partial void OnRotationAngleValueChanging(int oldValue, int newValue)
    {
        if (oldValue != newValue && !this.isRotating)
        {
            this.isRotating = true;

            //ChangeSelection_RotationAngleValue(-(oldValue - newValue));

            return;
        }

        if (oldValue != newValue)
        {
            this.UpdateRotation(newValue);

            return;
        }
    }

    partial void OnRotateTransformValueChanged(RotateTransform value)
    {
        this.ViewModel.FluidUI.Stand.Angle = RotationAngleValue;

        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Stand.Angle));
        this.OnPropertyChanged(nameof(this.ViewModel));
        this.OnPropertyChanged(nameof(this.RotationAngleValue));
    }

    partial void OnXPositionChanged(double value)
    {
        if (XSliderValue != (int)value)
        {
            XSliderValue = (int)value;
        }

        this.ViewModel.FluidUI.Stand.Position = new Point(value, this.ViewModel.FluidUI.Stand.Position.Y);

        this.ViewModel.TriggerRedraw();
    }

    partial void OnXSliderValueChanged(int value)
    {
        if (XPosition != value)
        {
            XPosition = value;
        }
    }

    partial void OnYPositionChanged(double value)
    {
        if (YSliderValue != (int)value)
        {
            YSliderValue = (int)value;
        }

        this.ViewModel.FluidUI.Stand.Position = new Point(this.ViewModel.FluidUI.Stand.Position.X, value);

        this.ViewModel.TriggerRedraw();
    }

    partial void OnYSliderValueChanged(int value)
    {
        if (YPosition != value)
        {
            YPosition = value;
        }
    }

    partial void OnZIndexValueChanged(int value)
    {
        this.ViewModel.FluidUI.Stand.Z = value;

        //ChangeSelection_ZIndexValue(value);

        this.ViewModel.TriggerRedraw();
    }

    private void UpdateRotation(int rotationAngle)
    {
        this.RotateTransformValue = new RotateTransform(rotationAngle * -1);

        this.TransformOriginPoint = new Point(0.5, 0.5);

        this.IsRotating = false;

        this.ViewModel.TriggerRedraw();

        this.OnPropertyChanged(nameof(this.RotationAngleValue));
        this.OnPropertyChanged(nameof(this.TransformOriginPoint));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }
}

// EOF