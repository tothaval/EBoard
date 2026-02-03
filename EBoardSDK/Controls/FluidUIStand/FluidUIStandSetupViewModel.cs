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
using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUIStand;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.ViewModels;
using System;
using System.Windows;

public partial class FluidUIStandSetupViewModel : ObservableObject, IFluidUIStandSetup
{
    private FluidUIBaseViewModel viewModel;
    private ScreenViewModel? eBoardViewModel;
    private FluidUIStandManager fluidUIStandManager;

    private FluidUIStandSettings fluidUIStandSettings = FluidUIStandSettings.NoStandContextArea;

    [ObservableProperty]
    private int rotationAngleValue;

    [ObservableProperty]
    private double skewAngleX = 0.0;

    [ObservableProperty]
    private double skewAngleY = 0.0;

    [ObservableProperty]
    private double skewCenterX;

    [ObservableProperty]
    private double skewCenterY;

    [ObservableProperty]
    private Point skewCenterPoint;

    [ObservableProperty]
    private Point transformOriginPoint;

    [ObservableProperty]
    private double xTransformOrigin;

    [ObservableProperty]
    private double yTransformOrigin;

    [ObservableProperty]
    private int xMaximumValue;

    [ObservableProperty]
    private int xMinimumValue;

    [ObservableProperty]
    private int xPosition;

    [ObservableProperty]
    private int yMaximumValue;

    [ObservableProperty]
    private int yMinimumValue;

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
    public FluidUIStandSetupViewModel(FluidUIBaseViewModel eboardFluidUIBaseViewModel, FluidUIStandSettings fluidUIStandSettings, ScreenViewModel? eBoardViewModel = null)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.eBoardViewModel = eBoardViewModel;
        this.fluidUIStandManager = new FluidUIStandManager(this.ViewModel);

        this.fluidUIStandSettings = fluidUIStandSettings;

        this.ApplyFluidUIStandValues();

        this.SetupSliderValues();

        this.OnPropertyChanged(nameof(this.ElementContextArea));
        this.OnPropertyChanged(nameof(this.ScreenContextArea));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    /// <inheritdoc/>
    public event Action? PropertyChangedEvent;

    public FluidUIBaseViewModel ViewModel => this.viewModel;

    public bool ElementContextArea => this.fluidUIStandSettings == FluidUIStandSettings.ElementContextArea;

    public bool ScreenContextArea => this.fluidUIStandSettings == FluidUIStandSettings.ScreenContextArea;

    public void ApplyFluidUIStandValues()
    {
        this.RotationAngleValue = (int)this.fluidUIStandManager.GetAngle();
        this.TransformOriginPoint = this.fluidUIStandManager.GetTransformOriginPoint();
        this.XTransformOrigin = this.TransformOriginPoint.X;
        this.YTransformOrigin = this.TransformOriginPoint.Y;

        this.XMaximumValue = this.fluidUIStandManager.GetXmaximum();
        this.XMinimumValue = this.fluidUIStandManager.GetXminimum();
        this.XPosition = (int)this.fluidUIStandManager.GetPosition().X;

        this.YMaximumValue = this.fluidUIStandManager.GetYmaximum();
        this.YMinimumValue = this.fluidUIStandManager.GetYminimum();
        this.YPosition = (int)this.fluidUIStandManager.GetPosition().Y;

        this.ZIndexValue = this.fluidUIStandManager.GetZ();
        this.ZMaximumValue = this.fluidUIStandManager.GetZmaximum();
        this.ZMinimumValue = this.fluidUIStandManager.GetZminimum();

        this.SkewAngleX = this.fluidUIStandManager.GetSkewAngleX();
        this.SkewAngleY = this.fluidUIStandManager.GetSkewAngleY();

        this.SkewCenterX = this.fluidUIStandManager.GetSkewCenterPoint().X;
        this.SkewCenterY = this.fluidUIStandManager.GetSkewCenterPoint().Y;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    public void Reset()
    {
        this.fluidUIStandManager.Reset();

        this.ApplyFluidUIStandValues();

        this.SetupSliderValues();
    }

    /// <inheritdoc/>
    public void SetInitialValues()
    {
    }

    /// <inheritdoc/>
    public void UpdateValues()
    {
        this.ApplyFluidUIStandValues();
    }

    private void SetupSliderValues()
    {
        if (this.eBoardViewModel != null)
        {
            var manager = new FluidUIStandManager(this.eBoardViewModel);

            this.XMaximumValue = manager.GetXmaximum();
            this.XMinimumValue = manager.GetXminimum();

            this.YMaximumValue = manager.GetYmaximum();
            this.YMinimumValue = manager.GetYminimum();

            this.ZMaximumValue = manager.GetZmaximum();
            this.ZMinimumValue = manager.GetZminimum();
        }
        else
        {
            this.XMaximumValue = 2048;
            this.XMinimumValue = -10;

            this.YMaximumValue = 1024;
            this.YMinimumValue = -10;

            this.ZMaximumValue = 100;
            this.ZMinimumValue = -10;
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

            return;
        }
    }

    partial void OnSkewAngleXChanged(double value)
    {
        this.fluidUIStandManager.SetSkewAngleX(value);
    }

    partial void OnSkewAngleYChanged(double value)
    {
        this.fluidUIStandManager.SetSkewAngleY(value);
    }

    partial void OnSkewCenterXChanged(double value)
    {
        this.SkewCenterPoint = new Point(value, this.fluidUIStandManager.GetSkewCenterPoint().Y);

        this.fluidUIStandManager.SetSkewCenterPoint(this.SkewCenterPoint);
    }

    partial void OnSkewCenterYChanged(double value)
    {
        this.SkewCenterPoint = new Point(this.fluidUIStandManager.GetSkewCenterPoint().X, value);

        this.fluidUIStandManager.SetSkewCenterPoint(this.SkewCenterPoint);
    }

    partial void OnTransformOriginPointChanged(Point value)
    {
        this.fluidUIStandManager.SetTransformOriginPoint(value);
    }

    partial void OnXPositionChanged(int value)
    {
        this.fluidUIStandManager.SetX(value);
    }

    partial void OnYPositionChanged(int value)
    {
        this.fluidUIStandManager.SetY(value);
    }

    partial void OnXTransformOriginChanged(double value)
    {
        this.TransformOriginPoint = new Point(value, this.TransformOriginPoint.Y);
    }

    partial void OnYTransformOriginChanged(double value)
    {
        this.TransformOriginPoint = new Point(this.TransformOriginPoint.X, value);
    }

    partial void OnZIndexValueChanged(int value)
    {
        this.fluidUIStandManager.SetZ(value);
    }

    partial void OnXMaximumValueChanged(int value)
    {
        this.fluidUIStandManager.SetXmax(value);
    }

    partial void OnXMinimumValueChanged(int value)
    {
        this.fluidUIStandManager.SetXmin(value);
    }

    partial void OnYMaximumValueChanged(int value)
    {
        this.fluidUIStandManager.SetYmax(value);
    }

    partial void OnYMinimumValueChanged(int value)
    {
        this.fluidUIStandManager.SetYmin(value);
    }

    partial void OnZMaximumValueChanged(int value)
    {
        this.fluidUIStandManager.SetZmaximum(value);
    }

    partial void OnZMinimumValueChanged(int value)
    {
        this.fluidUIStandManager.SetZminimum(value);
    }

    [RelayCommand]
    private void ResetStand()
    {
        this.Reset();
    }
}

// EOF