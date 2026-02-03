// <copyright file="FluidUISizeManager.cs" company=".">
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
namespace EBoardSDK.Models.FluidUISize;

using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Interfaces.FluidUISize;
using EBoardSDK.SharedMethods;
using EBoardSDK.ViewModels;
using System.Windows;

/// <summary>
/// This class is tasked with manipulation of <see cref="IFluidUISizeModel"/> instances
/// and has getter and setter methods that get values or apply changes. It requires an
/// instance of a <see cref="FluidUIBaseViewModel"/> and will manipulate the font model
/// within its <see cref="IFluidUIContext"/>.
/// </summary>
internal class FluidUISizeManager : IFluidUIManager
{
    private FluidUIBaseViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUISizeManager"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    internal FluidUISizeManager(FluidUIBaseViewModel viewModel)
    {
        this.viewModel = viewModel;
    }

    /// <summary>
    /// Resets FluidUI-Size properties to initial values
    /// and updates EboardFluidUIBaseViewModel.
    /// </summary>
    /// <param name="calledByIFluidUIContextManager"></param>
    public void Reset(bool calledByIFluidUIContextManager = false)
    {
        if (this.viewModel.FluidUI.Size != null)
        {
            this.viewModel.FluidUI.Size.BorderThickness = new Thickness(2, 2, 2, 2);
            this.viewModel.FluidUI.Size.CornerRadius = new CornerRadius(5, 5, 5, 5);
            this.viewModel.FluidUI.Size.Margin = new Thickness(5);
            this.viewModel.FluidUI.Size.Padding = new Thickness(5);

            this.Reset_FluidUISizeWidthAndHeight();
            this.ResetScale();

            if (!calledByIFluidUIContextManager)
            {
                this.viewModel.SetFluidUIByUser(this.viewModel.FluidUI);
            }
        }

        this.viewModel.UpdateSize();
    }

    internal void Apply_FluidUISizeQuadValue(QuadValue<int> quadValue, BorderTargets borderTargets)
    {
        if (quadValue == null)
        {
            return;
        }

        switch (borderTargets)
        {
            case BorderTargets.CornerRadius:
                this.SetCornerRadius(new CornerRadius(quadValue.Value1, quadValue.Value2, quadValue.Value3, quadValue.Value4));
                break;
            case BorderTargets.Margin:
                this.SetMargin(new Thickness(quadValue.Value1, quadValue.Value2, quadValue.Value3, quadValue.Value4));
                break;
            case BorderTargets.Padding:
                this.SetPadding(new Thickness(quadValue.Value1, quadValue.Value2, quadValue.Value3, quadValue.Value4));
                break;
            case BorderTargets.Thickness:
                this.SetBorderThickness(new Thickness(quadValue.Value1, quadValue.Value2, quadValue.Value3, quadValue.Value4));
                break;
            default:
                break;
        }
    }

    internal CornerRadius GetCornerRadius()
    {
        return this.viewModel.FluidUI.Size?.CornerRadius ?? new(5.0);
    }

    internal int GetHeight()
    {
        int checkedHeight = -1;

        if (this.viewModel.FluidUI.Size != null)
        {
            checkedHeight = new SharedMethod_UI().TransformDoubleNaNToInt(this.viewModel.FluidUI.Size.Height);
        }

        return checkedHeight;
    }

    internal Thickness GetMargin()
    {
        return this.viewModel.FluidUI.Size?.Margin ?? new(5.0);
    }

    internal Thickness GetPadding()
    {
        return this.viewModel.FluidUI.Size?.Padding ?? new(5.0);
    }

    internal Thickness GetBorderThickness()
    {
        return this.viewModel.FluidUI.Size?.BorderThickness ?? new(5.0);
    }

    internal double GetScaleX()
    {
        return this.viewModel.FluidUI.Size?.ScaleX ?? 1.0;
    }

    internal double GetScaleY()
    {
        return this.viewModel.FluidUI.Size?.ScaleY ?? 1.0;
    }

    internal int GetWidth()
    {
        int checkedWidth = -1;

        if (this.viewModel.FluidUI.Size != null)
        {
            checkedWidth = new SharedMethod_UI().TransformDoubleNaNToInt(this.viewModel.FluidUI.Size.Width);
        }

        return checkedWidth;
    }

    internal void ResetFluidUISizeTarget(BorderTargets borderTargets)
    {
        switch (borderTargets)
        {
            case BorderTargets.CornerRadius:
                this.SetCornerRadius(new CornerRadius(0));
                break;
            case BorderTargets.Margin:
                this.SetMargin(new Thickness(0));
                break;
            case BorderTargets.Padding:
                this.SetPadding(new Thickness(0));
                break;
            case BorderTargets.Thickness:
                this.SetBorderThickness(new Thickness(0));
                break;
            default:
                break;
        }

        this.viewModel.UpdateSize();
    }

    internal void Reset_FluidUISizeWidthAndHeight()
    {
        this.SetWidth(double.NaN);
        this.SetHeight(double.NaN);
    }

    internal void ResetScale()
    {
        this.SetScaleX(1.0);
        this.SetScaleY(1.0);
    }

    internal void SetCornerRadius(CornerRadius cornerRadius)
    {
        if (this.viewModel.FluidUI.Size != null)
        {
            this.viewModel.FluidUI.Size.CornerRadius = cornerRadius;

            this.viewModel.UpdateSize();
        }
    }

    internal void SetHeight(double height)
    {
        if (this.viewModel.FluidUI.Size != null)
        {
            this.viewModel.FluidUI.Size.Height = height;

            this.viewModel.UpdateSize();
        }
    }

    internal void SetMargin(Thickness margin)
    {
        if (this.viewModel.FluidUI.Size != null)
        {
            this.viewModel.FluidUI.Size.Margin = margin;

            this.viewModel.UpdateSize();
        }
    }

    internal void SetPadding(Thickness padding)
    {
        if (this.viewModel.FluidUI.Size != null)
        {
            this.viewModel.FluidUI.Size.Padding = padding;

            this.viewModel.UpdateSize();
        }
    }

    internal void SetBorderThickness(Thickness borderThickness)
    {
        if (this.viewModel.FluidUI.Size != null)
        {
            this.viewModel.FluidUI.Size.BorderThickness = borderThickness;

            this.viewModel.UpdateSize();
        }
    }

    internal void SetScaleX(double x)
    {
        if (this.viewModel.FluidUI.Size != null && this.GetScaleX() != x)
        {
            this.viewModel.FluidUI.Size.ScaleX = x;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetScaleY(double y)
    {
        if (this.viewModel.FluidUI.Size != null && this.GetScaleY() != y)
        {
            this.viewModel.FluidUI.Size.ScaleY = y;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetWidth(double width)
    {
        if (this.viewModel.FluidUI.Size != null)
        {
            this.viewModel.FluidUI.Size.Width = width;

            this.viewModel.UpdateSize();
        }
    }
}

// EOF