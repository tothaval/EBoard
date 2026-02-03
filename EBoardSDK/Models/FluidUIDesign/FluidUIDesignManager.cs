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
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System.Windows.Media;

/// <summary>
/// This class is tasked with manipulation of <see cref="IFluidUIDesignModel"/> instances
/// and has getter and setter methods that get values or apply changes. It requires an
/// instance of a <see cref="FluidUIBaseViewModel"/> and will manipulate the font model
/// within its <see cref="IFluidUIContext"/>.
/// </summary>
internal class FluidUIDesignManager : IFluidUIManager
{
    private FluidUIBaseViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIDesignManager"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    internal FluidUIDesignManager(FluidUIBaseViewModel viewModel)
    {
        this.viewModel = viewModel;
    }

    /// <summary>
    /// Resets FluidUI-Design properties to initial values
    /// and updates EboardFluidUIBaseViewModel.
    /// </summary>
    /// <param name="calledByIFluidUIContextManager"></param>
    public void Reset(bool calledByIFluidUIContextManager = false)
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

            if (!calledByIFluidUIContextManager)
            {
                this.viewModel.SetFluidUIByUser(this.viewModel.FluidUI);
            }

            this.viewModel.UpdateDesign();
        }
    }

    internal Brush GetBrush(BrushTargets brushTargets)
    {
        Brush brush = FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;

        brush = this.GetFluidUIBrush(brushTargets);

        return brush;
    }

    internal string GetImagePath(BrushTargets brushTargets)
    {
        string path = string.Empty;

        if (this.viewModel.FluidUI.Design == null)
        {
            return string.Empty;
        }

        switch (brushTargets)
        {
            case BrushTargets.Background:
                path = this.viewModel.FluidUI.Design.BackgroundColor.ImagePath;
                break;
            case BrushTargets.Foreground:
                path = this.viewModel.FluidUI.Design.ForegroundColor.ImagePath;
                break;
            case BrushTargets.Border:
                path = this.viewModel.FluidUI.Design.BorderColor.ImagePath;
                break;
            case BrushTargets.Highlight:
                path = this.viewModel.FluidUI.Design.HighlightColor.ImagePath;
                break;
            case BrushTargets.SelectionFallback:
                path = this.viewModel.FluidUI.Design.SelectionFallbackColor.ImagePath;
                break;
            default:
                break;
        }

        return path;
    }

    internal double GetOpacity()
    {
        return this.viewModel.FluidUI.Design?.Opacity ?? FluidUIDesignDefaultPropertyFactory.DefaultOpacity;
    }

    internal void ResetBrush(BrushTargets brushTargets)
    {
        this.ResetImagePath(brushTargets);

        Brush brush = FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;

        switch (brushTargets)
        {
            case BrushTargets.Background:
                brush = FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;
                break;
            case BrushTargets.Border:
                brush = FluidUIDesignDefaultPropertyFactory.BorderDefaultSolidColorBrush;
                break;
            case BrushTargets.Foreground:
                brush = FluidUIDesignDefaultPropertyFactory.ForegroundDefaultSolidColorBrush;
                break;
            case BrushTargets.Highlight:
                brush = FluidUIDesignDefaultPropertyFactory.HighlightDefaultSolidColorBrush;
                break;
            default:
                break;
        }

        this.SetBrush(brush, brushTargets);
    }

    internal void SetBrush(Brush brush, BrushTargets brushTarget)
    {
        this.AssignBrush(brushTarget, brush);

        this.viewModel.UpdateDesign();
    }

    internal void SetOpacity(double value)
    {
        if (this.viewModel.FluidUI.Design != null)
        {
            this.viewModel.FluidUI.Design.Opacity = value;

            this.viewModel.UpdateDesign();
        }
    }

    internal Brush SetUserChosenImagePath(BrushTargets brushTargets)
    {
        string path = string.Empty;

        if (this.viewModel.FluidUI.Design != null)
        {
            switch (brushTargets)
            {
                case BrushTargets.Background:
                    path = new SharedMethod_UI().UserSelectImage(this.viewModel.FluidUI.Design.BackgroundColor.ImagePath);
                    break;
                case BrushTargets.Border:
                    path = new SharedMethod_UI().UserSelectImage(this.viewModel.FluidUI.Design.BorderColor.ImagePath);
                    break;
                case BrushTargets.Foreground:
                    path = new SharedMethod_UI().UserSelectImage(this.viewModel.FluidUI.Design.ForegroundColor.ImagePath);
                    break;
                case BrushTargets.Highlight:
                    path = new SharedMethod_UI().UserSelectImage(this.viewModel.FluidUI.Design.HighlightColor.ImagePath);
                    break;
                default:
                    break;
            }
        }

        return FluidUIDesignDefaultPropertyFactory.GetImageBrush(path);
    }

    internal void SwitchBorderToBorder()
    {
        if (this.viewModel.FluidUI.Design != null)
        {
            this.AssignBrush(BrushTargets.Border, this.GetFluidUIBrush(BrushTargets.SelectionFallback));

            this.viewModel.UpdateDesign();
        }
    }

    internal void SwitchBorderToHighlight()
    {
        if (this.viewModel.FluidUI.Design != null)
        {
            this.AssignBrush(BrushTargets.SelectionFallback, this.GetFluidUIBrush(BrushTargets.Border));
            this.AssignBrush(BrushTargets.Border, this.GetFluidUIBrush(BrushTargets.Highlight));

            this.viewModel.UpdateDesign();
        }
    }

    private void AssignBrush(BrushTargets brushTargets, Brush brush)
    {
        if (this.viewModel.FluidUI.Design == null)
        {
            return;
        }

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
            case BrushTargets.SelectionFallback:
                this.viewModel.FluidUI.Design.SelectionFallbackBrush = brush;
                break;
            default:
                break;
        }
    }

    private Brush GetFluidUIBrush(BrushTargets brushTargets)
    {
        Brush brush = FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;

        if (this.viewModel.FluidUI.Design == null)
        {
            return brush;
        }

        switch (brushTargets)
        {
            case BrushTargets.Background:
                brush = this.viewModel.FluidUI.Design.Background;
                break;
            case BrushTargets.Foreground:
                brush = this.viewModel.FluidUI.Design.Foreground;
                break;
            case BrushTargets.Border:
                brush = this.viewModel.FluidUI.Design.Border;
                break;
            case BrushTargets.Highlight:
                brush = this.viewModel.FluidUI.Design.Highlight;
                break;
            case BrushTargets.SelectionFallback:
                brush = this.viewModel.FluidUI.Design.SelectionFallbackBrush;
                break;
            default:
                break;
        }

        return brush;
    }

    private void ResetImagePath(BrushTargets brushTargets)
    {
        if (this.viewModel.FluidUI.Design == null)
        {
            return;
        }

        switch (brushTargets)
        {
            case BrushTargets.Background:
                this.viewModel.FluidUI.Design.BackgroundColor.ImagePath = string.Empty;
                break;
            case BrushTargets.Border:
                this.viewModel.FluidUI.Design.BorderColor.ImagePath = string.Empty;
                break;
            case BrushTargets.Foreground:
                this.viewModel.FluidUI.Design.ForegroundColor.ImagePath = string.Empty;
                break;
            case BrushTargets.Highlight:
                this.viewModel.FluidUI.Design.HighlightColor.ImagePath = string.Empty;
                break;
            default:
                break;
        }
    }
}

// EOF