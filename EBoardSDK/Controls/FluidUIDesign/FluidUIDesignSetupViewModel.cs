// <copyright file="FluidUIDesignSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.FluidUIDesign;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.BrushSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.ViewModels;
using System;
using System.Windows.Media;

public partial class FluidUIDesignSetupViewModel : ObservableObject, IFluidUIDesignSetup
{
    private FluidUIBaseViewModel viewModel;
    private FluidUIDesignManager fluidUIDesignManager;

    private BrushSetupViewModel? backgroundBrushSetupViewModel;
    private BrushSetupViewModel? foregroundBrushSetupViewModel;
    private BrushSetupViewModel? borderBrushSetupViewModel;
    private BrushSetupViewModel? highlightBrushSetupViewModel;

    [ObservableProperty]
    private double opacityValue = 1.0;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIDesignSetupViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    public FluidUIDesignSetupViewModel(FluidUIBaseViewModel eboardFluidUIBaseViewModel)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.fluidUIDesignManager = new FluidUIDesignManager(this.ViewModel);

        this.ApplyModel();

        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public event Action? PropertyChangedEvent;

    public FluidUIBaseViewModel ViewModel => this.viewModel;

    public BrushSetupViewModel? BackgroundBS => this.backgroundBrushSetupViewModel;

    public BrushSetupViewModel? ForegroundBS => this.foregroundBrushSetupViewModel;

    public BrushSetupViewModel? BorderBS => this.borderBrushSetupViewModel;

    public BrushSetupViewModel? HighlightBS => this.highlightBrushSetupViewModel;

    public bool ApplyBrush(Brush brush, BrushTargets brushTargets)
    {
        try
        {
            this.fluidUIDesignManager.SetBrush(brush, brushTargets);

            if (brush.GetType() == typeof(ImageBrush))
            {
                switch (brushTargets)
                {
                    case BrushTargets.Background:
                        if (this.BackgroundBS != null)
                        {
                            this.ChangeColorStringValue(this.BackgroundBS, brush);
                        }

                        break;
                    case BrushTargets.Foreground:
                        if (this.ForegroundBS != null)
                        {
                            this.ChangeColorStringValue(this.ForegroundBS, brush);
                        }

                        break;
                    case BrushTargets.Border:
                        if (this.BorderBS != null)
                        {
                            this.ChangeColorStringValue(this.BorderBS, brush);
                        }

                        break;
                    case BrushTargets.Highlight:
                        if (this.HighlightBS != null)
                        {
                            this.ChangeColorStringValue(this.HighlightBS, brush);
                        }

                        break;
                    default:
                        break;
                }
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void Dispose()
    {
    }

    public void SetInitialValues()
    {
    }

    /// <inheritdoc/>
    public void UpdateValues()
    {
        this.ApplyModel();
    }

    private void ApplyModel()
    {
        this.backgroundBrushSetupViewModel = new BrushSetupViewModel(this.ViewModel, Enums.BrushTargets.Background, this.SetColorValueAsBackground);

        this.foregroundBrushSetupViewModel = new BrushSetupViewModel(this.ViewModel, Enums.BrushTargets.Foreground, this.SetColorValueAsForeground);

        this.borderBrushSetupViewModel = new BrushSetupViewModel(this.ViewModel, Enums.BrushTargets.Border, this.SetColorValueAsBorder);

        this.highlightBrushSetupViewModel = new BrushSetupViewModel(this.ViewModel, Enums.BrushTargets.Highlight, this.SetColorValueAsHighlight);

        this.OpacityValue = this.fluidUIDesignManager.GetOpacity();

        this.OnPropertyChanged(nameof(this.BackgroundBS));

        this.OnPropertyChanged(nameof(this.ForegroundBS));

        this.OnPropertyChanged(nameof(this.BorderBS));

        this.OnPropertyChanged(nameof(this.HighlightBS));
    }

    private void ChangeColorStringValue(BrushSetupViewModel brushSetupViewModel, Brush brush)
    {
        if (brush.GetType() == typeof(ImageBrush))
        {
            brushSetupViewModel.SolidBrush.ColorStringValue = "image";
        }

        if (brush.GetType() == typeof(LinearGradientBrush))
        {
            brushSetupViewModel.SolidBrush.ColorStringValue = "linear";
        }

        if (brush.GetType() == typeof(RadialGradientBrush))
        {
            brushSetupViewModel.SolidBrush.ColorStringValue = "radial";
        }

        if (brush.GetType() == typeof(VisualBrush))
        {
            brushSetupViewModel.SolidBrush.ColorStringValue = "visual";
        }
    }

    partial void OnOpacityValueChanged(double value)
    {
        this.fluidUIDesignManager.SetOpacity(value);
    }

    [RelayCommand]
    private void SetColorValueAsBackground()
    {
        this.ApplyBrush(this.BackgroundBS!.SolidBrush.ColorBrush, BrushTargets.Background);
    }

    [RelayCommand]
    private void SetColorValueAsForeground()
    {
        this.ApplyBrush(this.ForegroundBS!.SolidBrush.ColorBrush, BrushTargets.Foreground);
    }

    [RelayCommand]
    private void SetColorValueAsBorder()
    {
        this.ApplyBrush(this.BorderBS!.SolidBrush.ColorBrush, BrushTargets.Border);
    }

    [RelayCommand]
    private void SetColorValueAsHighlight()
    {
        this.ApplyBrush(this.HighlightBS!.SolidBrush.ColorBrush, BrushTargets.Highlight);
    }
}

// EOF