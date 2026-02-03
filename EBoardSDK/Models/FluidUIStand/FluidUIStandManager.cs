// <copyright file="FluidUIStandManager.cs" company=".">
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
namespace EBoardSDK.Models.FluidUIStand;

using EBoardSDK.Interfaces;
using EBoardSDK.Interfaces.FluidUIStand;
using EBoardSDK.ViewModels;
using System.Windows;

/// <summary>
/// This class is tasked with manipulation of <see cref="IFluidUIStandSetup"/> instances
/// and has getter and setter methods that get values or apply changes. It requires an
/// instance of a <see cref="FluidUIBaseViewModel"/> and will manipulate the font model
/// within its <see cref="IFluidUIContext"/>.
/// </summary>
internal class FluidUIStandManager : IFluidUIManager
{
    private FluidUIBaseViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIStandManager"/> class.
    /// </summary>
    /// <param name="viewModel">The inserted viewmodel of EboardFluidUIBaseViewModel class
    /// will be used as target for any changes to its FluidUI property.</param>
    internal FluidUIStandManager(FluidUIBaseViewModel viewModel)
    {
        this.viewModel = viewModel;
    }

    /// <summary>
    /// Resets FluidUI-Stand properties to initial values
    /// and updates EboardFluidUIBaseViewModel.
    /// </summary>
    /// <param name="calledByIFluidUIContextManager"></param>
    public void Reset(bool calledByIFluidUIContextManager = false)
    {
        this.viewModel.FluidUI.Stand?.SetInitialValues();

        if (!calledByIFluidUIContextManager)
        {
            this.viewModel.SetFluidUIByUser(this.viewModel.FluidUI);
        }

        this.viewModel.UpdateStand();
    }

    internal void ApplyRotationAngleValueByMouseWheel(int delta)
    {
        if (this.viewModel.FluidUI.Stand != null && delta < 0 && this.viewModel.FluidUI.Stand.Angle > -180)
        {
            this.viewModel.FluidUI.Stand.Angle--;
        }

        if (this.viewModel.FluidUI.Stand != null && delta > 0 && this.viewModel.FluidUI.Stand.Angle < 180)
        {
            this.viewModel.FluidUI.Stand.Angle++;
        }

        this.viewModel.UpdateStand();
    }

    internal void ApplyZIndexValueByMouseWheel(int delta)
    {
        if (this.viewModel.FluidUI.Stand != null && delta < 0 && this.GetZ() > this.GetZminimum())
        {
            this.viewModel.FluidUI.Stand.Z--;
        }

        if (this.viewModel.FluidUI.Stand != null && delta > 0 && this.GetZmaximum() > this.GetZ())
        {
            this.viewModel.FluidUI.Stand.Z++;
        }

        this.viewModel.UpdateStand();
    }

    internal double GetAngle()
    {
        return this.viewModel.FluidUI.Stand?.Angle ?? 0.0;
    }

    internal Point GetPosition()
    {
        return this.viewModel.FluidUI.Stand?.Position ?? new(25.0, 25.0);
    }

    internal Point GetSkewCenterPoint()
    {
        return this.viewModel.FluidUI.Stand?.SkewCenterPoint ?? new(0.5, 0.5);
    }

    internal double GetSkewAngleX()
    {
        return this.viewModel.FluidUI.Stand?.SkewAngleX ?? 0.0;
    }

    internal double GetSkewAngleY()
    {
        return this.viewModel.FluidUI.Stand?.SkewAngleY ?? 0.0;
    }

    internal Point GetTransformOriginPoint()
    {
        return this.viewModel.FluidUI.Stand?.TransformOriginPoint ?? new(25.0, 25.0);
    }

    internal int GetXmaximum()
    {
        return this.viewModel.FluidUI.Stand?.Xmax ?? 1280;
    }

    internal int GetXminimum()
    {
        return this.viewModel.FluidUI.Stand?.Xmin ?? 0;
    }

    internal int GetYmaximum()
    {
        return this.viewModel.FluidUI.Stand?.Ymax ?? 640;
    }

    internal int GetYminimum()
    {
        return this.viewModel.FluidUI.Stand?.Ymin ?? 0;
    }

    internal int GetZ()
    {
        return this.viewModel.FluidUI.Stand?.Z ?? 0;
    }

    internal int GetZmaximum()
    {
        return this.viewModel.FluidUI.Stand?.Zmaximum ?? 100;
    }

    internal int GetZminimum()
    {
        return this.viewModel.FluidUI.Stand?.Zminimum ?? 0;
    }

    internal void SetAngle(double a)
    {
        if (this.viewModel.FluidUI.Stand != null && this.GetAngle() != a)
        {
            this.viewModel.FluidUI.Stand.Angle = a;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetPosition(Point p)
    {
        if (this.viewModel.FluidUI.Stand != null && this.GetPosition() != p)
        {
            this.viewModel.FluidUI.Stand.Position = p;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetPosition(double x, double y)
    {
        this.SetPosition(new Point(x, y));
    }

    internal void SetSkewCenterPoint(Point skewp)
    {
        if (this.viewModel.FluidUI.Stand != null && this.GetSkewCenterPoint() != skewp)
        {
            this.viewModel.FluidUI.Stand.SkewCenterPoint = skewp;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetSkewAngleX(double skewx)
    {
        if (this.viewModel.FluidUI.Stand != null && this.GetSkewAngleX() != skewx)
        {
            this.viewModel.FluidUI.Stand.SkewAngleX = skewx;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetSkewAngleY(double skewy)
    {
        if (this.viewModel.FluidUI.Stand != null && this.GetSkewAngleY() != skewy)
        {
            this.viewModel.FluidUI.Stand.SkewAngleY = skewy;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetTransformOriginPoint(Point p)
    {
        if (this.viewModel.FluidUI.Stand != null && this.viewModel.FluidUI.Stand.TransformOriginPoint != p)
        {
            this.viewModel.FluidUI.Stand.TransformOriginPoint = p;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetY(double y)
    {
        if (this.viewModel.FluidUI.Stand != null && this.viewModel.FluidUI.Stand.Position.Y != y)
        {
            this.SetPosition(new Point(this.viewModel.FluidUI.Stand.Position.X, y));
        }
    }

    internal void SetXmax(int xMax)
    {
        if (this.viewModel.FluidUI.Stand != null && this.viewModel.FluidUI.Stand.Xmax != xMax)
        {
            this.viewModel.FluidUI.Stand.Xmax = xMax;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetXmin(int xMin)
    {
        if (this.viewModel.FluidUI.Stand != null && this.viewModel.FluidUI.Stand.Xmin != xMin)
        {
            this.viewModel.FluidUI.Stand.Xmin = xMin;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetX(double x)
    {
        if (this.viewModel.FluidUI.Stand != null && this.viewModel.FluidUI.Stand.Position.X != x)
        {
            this.SetPosition(new Point(x, this.viewModel.FluidUI.Stand.Position.Y));
        }
    }

    internal void SetYmax(int yMax)
    {
        if (this.viewModel.FluidUI.Stand != null && this.viewModel.FluidUI.Stand.Ymax != yMax)
        {
            this.viewModel.FluidUI.Stand.Ymax = yMax;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetYmin(int yMin)
    {
        if (this.viewModel.FluidUI.Stand != null && this.viewModel.FluidUI.Stand.Ymin != yMin)
        {
            this.viewModel.FluidUI.Stand.Ymin = yMin;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetZ(int z)
    {
        if (this.viewModel.FluidUI.Stand != null && this.viewModel.FluidUI.Stand.Z != z)
        {
            this.viewModel.FluidUI.Stand.Z = z;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetZmaximum(int z)
    {
        if (this.viewModel.FluidUI.Stand != null && this.viewModel.FluidUI.Stand.Zmaximum != z)
        {
            this.viewModel.FluidUI.Stand.Zmaximum = z;

            this.viewModel.UpdateStand();
        }
    }

    internal void SetZminimum(int z)
    {
        if (this.viewModel.FluidUI.Stand != null && this.viewModel.FluidUI.Stand.Zminimum != z)
        {
            this.viewModel.FluidUI.Stand.Zminimum = z;

            this.viewModel.UpdateStand();
        }
    }
}

// EOF