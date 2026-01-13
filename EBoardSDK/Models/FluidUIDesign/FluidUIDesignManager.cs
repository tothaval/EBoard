// <copyright file="FluidUIDesignManager.cs" company=".">
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
namespace EBoardSDK.Models.FluidUIDesign;

using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.SharedMethods;
using EBoardSDK.ViewModels;
using System.Windows.Media;

internal class FluidUIDesignManager : IFluidUIManager
{
    private EboardFluidUIBaseViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIDesignManager"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    internal FluidUIDesignManager(EboardFluidUIBaseViewModel viewModel)
    {
        this.viewModel = viewModel;
    }

    public void Reset()
    {
        if (this.viewModel.FluidUI.Design != null)
        {
            var selectedElementViewModel = this.viewModel as ElementViewModel;

            if (selectedElementViewModel != null)
            {
                this.viewModel.FluidUI.Design.ResetValuesToInitial(selectedElementViewModel.IsSelected);
            }
            else
            {
                this.viewModel.FluidUI.Design.SetInitialValues();
            }

            this.viewModel.UpdateDesign();
        }
    }

    internal double GetOpacity()
    {
        return this.viewModel.FluidUI.Design?.Opacity ?? 0.0;
    }

    internal void ResetBrush(BrushTargets brushTargets)
    {
        if (this.viewModel.FluidUI.Design != null)
        {
            switch (brushTargets)
            {
                case BrushTargets.Background:
                    this.viewModel.FluidUI.Design.ImagePath = string.Empty;
                    this.SetBrush(new SolidColorBrush(Colors.White), BrushTargets.Background);
                    break;
                case BrushTargets.Border:
                    this.viewModel.FluidUI.Design.ImageBorderPath = string.Empty;
                    this.SetBrush(new SolidColorBrush(Colors.Black), BrushTargets.Border);
                    break;
                case BrushTargets.Foreground:
                    this.viewModel.FluidUI.Design.ImageForegroundPath = string.Empty;
                    this.SetBrush(new SolidColorBrush(Colors.DarkGray), BrushTargets.Foreground);
                    break;
                case BrushTargets.Highlight:
                    this.viewModel.FluidUI.Design.ImageHighlightPath = string.Empty;
                    this.SetBrush(new SolidColorBrush(Colors.DarkGoldenrod), BrushTargets.Highlight);
                    break;
                default:
                    break;
            }

            this.viewModel.UpdateDesign();
        }
    }

    internal void SetBrush(Brush brush, BrushTargets brushTargets)
    {
        if (this.viewModel.FluidUI.Design != null)
        {
            switch (brushTargets)
            {
                case BrushTargets.Background:
                    this.viewModel.FluidUI.Design.Background = brush;
                    break;
                case BrushTargets.Border:
                    this.viewModel.FluidUI.Design.Border = brush;
                    break;
                case BrushTargets.Foreground:
                    this.viewModel.FluidUI.Design.Foreground = brush;
                    break;
                case BrushTargets.Highlight:
                    this.viewModel.FluidUI.Design.Highlight = brush;
                    break;
                default:
                    break;
            }

            this.viewModel.UpdateDesign();
        }
    }

    internal void SetOpacity(double value)
    {
        if (this.viewModel.FluidUI.Design != null)
        {
            this.viewModel.FluidUI.Design.Opacity = value;

            this.viewModel.UpdateDesign();
        }
    }

    internal void SetUserChosenImagePath(BrushTargets brushTargets)
    {
        if (this.viewModel.FluidUI.Design != null)
        {
            switch (brushTargets)
            {
                case BrushTargets.Background:
                    this.SetImageToBrushTarget(brushTargets, new SharedMethod_UI().UserSelectImage(this.viewModel.FluidUI.Design.ImagePath));
                    break;
                case BrushTargets.Border:
                    this.SetImageToBrushTarget(brushTargets, new SharedMethod_UI().UserSelectImage(this.viewModel.FluidUI.Design.ImageBorderPath));
                    break;
                case BrushTargets.Foreground:
                    this.SetImageToBrushTarget(brushTargets, new SharedMethod_UI().UserSelectImage(this.viewModel.FluidUI.Design.ImageForegroundPath));
                    break;
                case BrushTargets.Highlight:
                    this.SetImageToBrushTarget(brushTargets, new SharedMethod_UI().UserSelectImage(this.viewModel.FluidUI.Design.ImageHighlightPath));
                    break;
                default:
                    break;
            }
        }
    }

    internal void SwitchBorderToBorder()
    {
        if (this.viewModel.FluidUI.Design != null)
        {
            this.viewModel.FluidUI.Design.Border = this.viewModel.FluidUI.Design.SelectionFallbackBrush;

            this.viewModel.UpdateDesign();
        }
    }

    internal void SwitchBorderToHighlight()
    {
        if (this.viewModel.FluidUI.Design != null)
        {
            this.viewModel.FluidUI.Design.SelectionFallbackBrush = this.viewModel.FluidUI.Design.Border;
            this.viewModel.FluidUI.Design.Border = this.viewModel.FluidUI.Design.Highlight;

            this.viewModel.UpdateDesign();
        }
    }

    internal void SetImageToBrushTarget(BrushTargets brushTargets, string path)
    {
        if (this.viewModel.FluidUI.Design == null || string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        Brush brush = new ImageBrush();

        switch (brushTargets)
        {
            case BrushTargets.Background:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.viewModel.FluidUI.Design.Background, path);
                break;
            case BrushTargets.Border:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.viewModel.FluidUI.Design.Border, path);
                break;
            case BrushTargets.Foreground:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.viewModel.FluidUI.Design.Foreground, path);
                break;
            case BrushTargets.Highlight:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.viewModel.FluidUI.Design.Highlight, path);
                break;
            default:
                break;
        }

        this.SetBrush(brush, brushTargets);
    }
}

// EOF