// <copyright file="SolidBrushSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.BrushSetup.SolidBrushSetup;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System.Windows.Media;

public partial class SolidBrushSetupViewModel : ObservableObject, IDisposable
{
    private readonly FluidUIBaseViewModel viewModel;
    private readonly BrushTargets brushTargets;

    private Action okAction;

    [ObservableProperty]
    private Brush colorBrush = FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;

    [ObservableProperty]
    private string colorStringValue = string.Empty;

    [ObservableProperty]
    private Color colorValue;

    [ObservableProperty]
    private byte greyscaleValue = 0;

    [ObservableProperty]
    private byte redValue = 0;

    [ObservableProperty]
    private byte greenValue = 0;

    [ObservableProperty]
    private byte blueValue = 0;

    [ObservableProperty]
    private byte alphaValue = 255;

    /// <summary>
    /// Initializes a new instance of the <see cref="SolidBrushSetupViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="brushTargets"></param>
    /// <param name="okAction"></param>
    public SolidBrushSetupViewModel(FluidUIBaseViewModel viewModel, BrushTargets brushTargets, Action okAction)
    {
        this.viewModel = viewModel;
        this.okAction = okAction;
        this.brushTargets = brushTargets;

        _ = this.SetColorBrushToFluidUIDesignValue();

        this.Refresh();
    }

    public FluidUIBaseViewModel ViewModel => this.viewModel;

    private Color ActualColor => Color.FromArgb(this.AlphaValue, this.RedValue, this.GreenValue, this.BlueValue);

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    public void Refresh()
    {
        this.OnPropertyChanged(nameof(this.ColorBrush));
        this.OnPropertyChanged(nameof(this.ColorStringValue));
        this.OnPropertyChanged(nameof(this.ColorValue));
    }

    internal void SetColorValue(Color color)
    {
        this.ColorBrush = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(color);
        this.ColorStringValue = color.ToString();
        this.ColorValue = color;

        this.RedValue = color.R;
        this.GreenValue = color.G;
        this.BlueValue = color.B;

        this.AlphaValue = color.A;
    }

    private void UpdateSliderPositions()
    {
        if (this.ColorBrush != null)
        {
            if (this.ColorBrush.GetType().Equals(typeof(SolidColorBrush)))
            {
                var c = ((SolidColorBrush)this.ColorBrush).Color;

                this.AlphaValue = c.A;

                var cmid = ((int)c.R + (int)c.G + (int)c.B) / 3;

                this.GreyscaleValue = (byte)cmid;

                this.RedValue = c.R;
                this.GreenValue = c.G;
                this.BlueValue = c.B;
            }
        }
    }

    private Brush SetColorBrushToFluidUIDesignValue()
    {
        Brush? brush;

        switch (this.brushTargets)
        {
            case BrushTargets.Background:
                brush = this.ViewModel.FluidUI.Design?.Background ?? FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;
                break;
            case BrushTargets.Border:
                brush = this.ViewModel.FluidUI.Design?.Border ?? FluidUIDesignDefaultPropertyFactory.BorderDefaultSolidColorBrush;
                break;
            case BrushTargets.Foreground:
                brush = this.ViewModel.FluidUI.Design?.Foreground ?? FluidUIDesignDefaultPropertyFactory.ForegroundDefaultSolidColorBrush;
                break;
            case BrushTargets.Highlight:
                brush = this.ViewModel.FluidUI.Design?.Highlight ?? FluidUIDesignDefaultPropertyFactory.HighlightDefaultSolidColorBrush;
                break;
            default:
                brush = FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;
                break;
        }

        if (this.ColorBrush != brush)
        {
            this.ColorBrush = brush;

            this.UpdateSliderPositions();
        }

        return brush;
    }

    partial void OnColorValueChanged(Color value)
    {
        if (value != this.ColorValue)
        {
            this.UpdateSliderPositions();
        }
    }

    partial void OnGreyscaleValueChanged(byte value)
    {
        this.RedValue = value;
        this.GreenValue = value;
        this.BlueValue = value;

        this.SetColorValue(this.ActualColor);
    }

    partial void OnRedValueChanged(byte value)
    {
        this.SetColorValue(this.ActualColor);
    }

    partial void OnGreenValueChanged(byte value)
    {
        this.SetColorValue(this.ActualColor);
    }

    partial void OnBlueValueChanged(byte value)
    {
        this.SetColorValue(this.ActualColor);
    }

    partial void OnAlphaValueChanged(byte value)
    {
        this.SetColorValue(this.ActualColor);
    }

    [RelayCommand]
    private void Ok()
    {
        this.okAction?.Invoke();
    }

    [RelayCommand]
    private void Reset()
    {
        this.SetColorBrushToFluidUIDesignValue();
    }
}

// EOF