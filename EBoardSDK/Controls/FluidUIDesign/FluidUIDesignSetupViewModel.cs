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
using EBoardSDK.Controls.BrushSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.SharedMethods;
using EBoardSDK.ViewModels;
using System;
using System.Windows.Media;

public class FluidUIDesignSetupViewModel : ObservableObject, IFluidUIDesignSetup
{
    private EboardFluidUIBaseViewModel viewModel;

    private BrushSetupViewModel backgroundBrushSetupViewModel;
    private BrushSetupViewModel foregroundBrushSetupViewModel;
    private BrushSetupViewModel borderBrushSetupViewModel;
    private BrushSetupViewModel highlightBrushSetupViewModel;

    public FluidUIDesignSetupViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel)
    {
        this.viewModel = eboardFluidUIBaseViewModel;

        this.backgroundBrushSetupViewModel = new BrushSetupViewModel(eboardFluidUIBaseViewModel, Enums.BrushTargets.Background, eboardFluidUIBaseViewModel.SetColorValueAsBackground);

        this.foregroundBrushSetupViewModel = new BrushSetupViewModel(eboardFluidUIBaseViewModel, Enums.BrushTargets.Foreground, eboardFluidUIBaseViewModel.SetColorValueAsForeground);

        this.borderBrushSetupViewModel = new BrushSetupViewModel(eboardFluidUIBaseViewModel, Enums.BrushTargets.Border, eboardFluidUIBaseViewModel.SetColorValueAsBorder);

        this.highlightBrushSetupViewModel = new BrushSetupViewModel(eboardFluidUIBaseViewModel, Enums.BrushTargets.Highlight, eboardFluidUIBaseViewModel.SetColorValueAsHighlight);

        this.OnPropertyChanged(nameof(this.ViewModel));

        this.OnPropertyChanged(nameof(this.BackgroundBS));

        this.OnPropertyChanged(nameof(this.ForegroundBS));

        this.OnPropertyChanged(nameof(this.BorderBS));

        this.OnPropertyChanged(nameof(this.HighlightBS));
    }

    public event Action PropertyChangedEvent;

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public BrushSetupViewModel BackgroundBS => this.backgroundBrushSetupViewModel;

    public BrushSetupViewModel ForegroundBS => this.foregroundBrushSetupViewModel;

    public BrushSetupViewModel BorderBS => this.borderBrushSetupViewModel;

    public BrushSetupViewModel HighlightBS => this.highlightBrushSetupViewModel;

    public void Apply_FluidUIDesignBrushTargetToImage(BrushTargets brushTargets, string path)
    {
        this.SetImageToBrushTarget(brushTargets, path);
    }

    public bool Apply_FluidUIDesignBrush(Brush brush, BrushTargets brushTargets)
    {
        try
        {
            switch (brushTargets)
            {
                case BrushTargets.Background:
                    this.ViewModel.FluidUI.Design.Background = brush;
                    if (brush.GetType() == typeof(ImageBrush) && this.BackgroundBS != null)
                    {
                        this.BackgroundBS.SolidBrush.ColorStringValue = "imagebrush";
                    }

                    break;
                case BrushTargets.Border:
                    this.ViewModel.FluidUI.Design.Border = brush;

                    if (brush.GetType() == typeof(ImageBrush) && this.BorderBS != null)
                    {
                        this.BorderBS.SolidBrush.ColorStringValue = "imagebrush";
                    }

                    break;
                case BrushTargets.Foreground:
                    this.ViewModel.FluidUI.Design.Foreground = brush;

                    if (brush.GetType() == typeof(ImageBrush) && this.ForegroundBS != null)
                    {
                        this.ForegroundBS.SolidBrush.ColorStringValue = "imagebrush";
                    }

                    break;
                case BrushTargets.Highlight:
                    this.ViewModel.FluidUI.Design.Highlight = brush;

                    if (brush.GetType() == typeof(ImageBrush) && this.HighlightBS != null)
                    {
                        this.HighlightBS.SolidBrush.ColorStringValue = "imagebrush";
                    }

                    break;
                default:
                    break;
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

    public void Reset_FluidUIDesignBrush(BrushTargets brushTargets)
    {
        switch (brushTargets)
        {
            case BrushTargets.Background:
                this.ViewModel.FluidUI.Design.ImagePath = string.Empty;

                this.Apply_FluidUIDesignBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, BrushTargets.Background);
                break;
            case BrushTargets.Border:
                this.ViewModel.FluidUI.Design.ImageBorderPath = string.Empty;
                this.Apply_FluidUIDesignBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, BrushTargets.Border);
                break;
            case BrushTargets.Foreground:
                this.ViewModel.FluidUI.Design.ImageForegroundPath = string.Empty;

                this.Apply_FluidUIDesignBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, BrushTargets.Foreground);
                break;
            case BrushTargets.Highlight:
                this.ViewModel.FluidUI.Design.ImageHighlightPath = string.Empty;

                this.Apply_FluidUIDesignBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, BrushTargets.Highlight);
                break;
            default:
                break;
        }
    }

    public void SetInitialValues()
    {
        //this.Background = new SolidColorBrush(Color.FromArgb(205, 0, 0, 0));
        //this.Border = new SolidColorBrush(Colors.Goldenrod);
        //this.Foreground = new SolidColorBrush(Colors.DarkGoldenrod);
        //this.Highlight = new SolidColorBrush(Colors.YellowGreen);

        //this.ImagePath = string.Empty;
        //this.ImageForegroundPath = string.Empty;
        //this.ImageBorderPath = string.Empty;
        //this.ImageHighlightPath = string.Empty;
    }

    public void SetUserChosenImagePath(BrushTargets brushTargets)
    {
        switch (brushTargets)
        {
            case BrushTargets.Background:
                this.Apply_FluidUIDesignBrushTargetToImage(brushTargets, new SharedMethod_UI().UserSelectImage(this.ViewModel.FluidUI.Design.ImagePath));
                break;
            case BrushTargets.Border:
                this.Apply_FluidUIDesignBrushTargetToImage(brushTargets, new SharedMethod_UI().UserSelectImage(this.ViewModel.FluidUI.Design.ImageBorderPath));
                break;
            case BrushTargets.Foreground:
                this.Apply_FluidUIDesignBrushTargetToImage(brushTargets, new SharedMethod_UI().UserSelectImage(this.ViewModel.FluidUI.Design.ImageForegroundPath));
                break;
            case BrushTargets.Highlight:
                this.Apply_FluidUIDesignBrushTargetToImage(brushTargets, new SharedMethod_UI().UserSelectImage(this.ViewModel.FluidUI.Design.ImageHighlightPath));
                break;
            default:
                break;
        }
    }

    private bool SetImageToBrushTarget(BrushTargets brushTargets, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        Brush brush = new ImageBrush();

        switch (brushTargets)
        {
            case BrushTargets.Background:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.ViewModel.FluidUI.Design.Background, path);
                break;
            case BrushTargets.Border:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.ViewModel.FluidUI.Design.Border, path);
                break;
            case BrushTargets.Foreground:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.ViewModel.FluidUI.Design.Foreground, path);
                break;
            case BrushTargets.Highlight:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.ViewModel.FluidUI.Design.Highlight, path);
                break;
            default:
                break;
        }

        return (bool)this.Apply_FluidUIDesignBrush(brush, brushTargets);
    }

    public void SwitchBorderToBorder()
    {
        this.ViewModel.FluidUI.Design.Border = this.ViewModel.FluidUI.Design.SelectionFallbackBrush;

        this.PropertyChangedEvent?.Invoke();
    }

    public void SwitchBorderToHighlight()
    {
        this.ViewModel.FluidUI.Design.SelectionFallbackBrush = this.ViewModel.FluidUI.Design.Border;
        this.ViewModel.FluidUI.Design.Border = this.ViewModel.FluidUI.Design.Highlight;

        this.PropertyChangedEvent?.Invoke();
    }
}

// EOF