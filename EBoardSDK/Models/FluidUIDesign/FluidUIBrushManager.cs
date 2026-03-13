// <copyright file="FluidUIBrushManager.cs" company=".">
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

using EBoardSDK.Interfaces;
using EBoardSDK.Utilities.Factories;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;

public class FluidUIBrushManager : IFluidUIManager
{
    private readonly FluidUIBrushModel? brushModel;

    public FluidUIBrushManager(FluidUIBrushModel brushModel)
    {
        this.brushModel = brushModel;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<Brush?> GetBrush()
    {
        if (this.brushModel == null)
        {
            return null;
        }

        switch (this.brushModel.BrushTypeEnum)
        {
            case Enums.BrushTypes.SolidColorBrush:
                SolidColorBrush solidColorBrush;

                if (this.brushModel.GradientColors.Count > 0)
                {
                    var modelColor = this.brushModel.GradientColors[0];

                    var color = System.Windows.Media.Color.FromArgb(modelColor.A, modelColor.R, modelColor.G, modelColor.B);

                    solidColorBrush = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(color);
                }
                else
                {
                    solidColorBrush = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Colors.White);
                }

                this.ParseBrushToModel(solidColorBrush);
                return solidColorBrush;

            case Enums.BrushTypes.LinearGradientBrush:
                LinearGradientBrush? linearGradientBrush = this.BuildLinearGradientBrush();

                if (linearGradientBrush != null)
                {
                    this.ParseBrushToModel(linearGradientBrush);
                }

                return linearGradientBrush;

            case Enums.BrushTypes.RadialGradientBrush:
                RadialGradientBrush? radialGradientBrush = this.BuildRadialGradientBrush();

                if (radialGradientBrush != null)
                {
                    this.ParseBrushToModel(radialGradientBrush);
                }

                return radialGradientBrush;

            case Enums.BrushTypes.ImageBrush:
                Brush brush = FluidUIDesignDefaultPropertyFactory.CreateImageBrush(this.brushModel.ImagePath);

                this.ParseBrushToModel(brush);

                if (this.brushModel.BrushTypeEnum == Enums.BrushTypes.SolidColorBrush)
                {
                    return brush as SolidColorBrush;
                }

                return brush as ImageBrush;

            case Enums.BrushTypes.VisualBrush:
                // TODO implement VisulaBrush logic
                // examples can't be saved or loaded,
                // therefore a simple default brush
                // is returned
                var vb = new VisualBrush();

                var grid = new Grid()
                {
                    Background = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Colors.Gainsboro),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                };

                grid.Children.Add(new System.Windows.Shapes.Path()
                {
                    Stroke = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Colors.Gold),
                    StrokeThickness = 5,
                    Margin = new System.Windows.Thickness(15),
                    Data = Geometry.Parse(
                    "M 10 100 " +
                    "C 190 10 40 4 10 40 " +
                    "C 10 0.10 40 30 40 60 " +
                    "C 40 4 40 4 22 3.7" +
                    "S 117 22 15 9"),
                });

                grid.Children.Add(new TextBlock()
                {
                    Text = "\nVisual Brush fallback brush\n\nvisual brush permanency not yet supported\n\n",
                    TextAlignment = System.Windows.TextAlignment.Center,
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    Foreground = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Colors.RoyalBlue),
                });

                vb.Visual = grid;

                this.ParseBrushToModel(vb);

                return vb;

            case Enums.BrushTypes.Brush:
            default:
                return FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;
        }
    }

    public void Reset(bool calledByIFluidUIContextManager = false)
    {
        this.ParseBrushToModel(FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Colors.White));
    }

    internal void ParseBrushToModel(System.Windows.Media.Brush brush)
    {
        if (brush == null || this.brushModel == null)
        {
            return;
        }

        this.brushModel.BrushTypeName = brush.GetType().Name;

        this.brushModel.GradientColors.Clear();
        this.brushModel.GradientPoints.Clear();
        this.brushModel.GradientStops.Clear();

        switch (this.brushModel.BrushTypeName)
        {
            case "SolidColorBrush":
                this.ProcessSolidColorBrush((SolidColorBrush)brush);
                break;
            case "LinearGradientBrush":
                this.ProcessLinearGradientBrush((LinearGradientBrush)brush);
                break;
            case "RadialGradientBrush":
                this.ProcessRadialGradientBrush((RadialGradientBrush)brush);
                break;
            case "ImageBrush":
                this.ProcessImageBrush((ImageBrush)brush);
                break;
            case "VisualBrush":
                this.ProcessVisualBrush((VisualBrush)brush);
                break;
            default:
                break;
        }
    }

    private LinearGradientBrush? BuildLinearGradientBrush()
    {
        if (this.brushModel == null)
        {
            return null;
        }

        LinearGradientBrush linearGradientBrush = new LinearGradientBrush();

        if (this.brushModel.GradientPoints.Count == 2)
        {
            var startPoint = this.brushModel.GradientPoints[0];
            var endPoint = this.brushModel.GradientPoints[1];

            linearGradientBrush.StartPoint = new System.Windows.Point(startPoint.X, startPoint.Y);
            linearGradientBrush.EndPoint = new System.Windows.Point(endPoint.X, endPoint.Y);
        }

        if (this.brushModel.GradientColors.Count == this.brushModel.GradientStops.Count)
        {
            for (int i = 0; i < this.brushModel.GradientColors.Count; i++)
            {
                var modelColor = this.brushModel.GradientColors[i];

                var color = System.Windows.Media.Color.FromArgb(modelColor.A, modelColor.R, modelColor.G, modelColor.B);

                var stop = this.brushModel.GradientStops[i];

                linearGradientBrush.GradientStops.Add(new GradientStop(color, stop));
            }
        }

        return linearGradientBrush;
    }

    private RadialGradientBrush? BuildRadialGradientBrush()
    {
        if (this.brushModel == null)
        {
            return null;
        }

        RadialGradientBrush radialGradientBrush = new RadialGradientBrush();

        if (this.brushModel.GradientPoints.Count == 2)
        {
            var startPoint = this.brushModel.GradientPoints[0];
            var endPoint = this.brushModel.GradientPoints[1];

            radialGradientBrush.Center = new System.Windows.Point(startPoint.X, startPoint.Y);
            radialGradientBrush.GradientOrigin = new System.Windows.Point(endPoint.X, endPoint.Y);
        }

        if (this.brushModel.GradientColors.Count == this.brushModel.GradientStops.Count)
        {
            for (int i = 0; i < this.brushModel.GradientColors.Count; i++)
            {
                var modelColor = this.brushModel.GradientColors[i];

                var color = System.Windows.Media.Color.FromArgb(modelColor.A, modelColor.R, modelColor.G, modelColor.B);

                var stop = this.brushModel.GradientStops[i];

                radialGradientBrush.GradientStops.Add(new GradientStop(color, stop));
            }
        }

        return radialGradientBrush;
    }

    private void ProcessImageBrush(ImageBrush brush)
    {
        if (brush == null || this.brushModel == null)
        {
            return;
        }

        var path = ((BitmapImage)brush.ImageSource).UriSource.AbsoluteUri;

        this.brushModel.ImagePath = path;
        this.brushModel.BrushTypeEnum = Enums.BrushTypes.ImageBrush;
    }

    private void ProcessLinearGradientBrush(LinearGradientBrush brush)
    {
        if (brush == null || this.brushModel == null)
        {
            return;
        }

        this.brushModel.GradientPoints.Add(new System.Windows.Point(brush.StartPoint.X, brush.StartPoint.Y));
        this.brushModel.GradientPoints.Add(new System.Windows.Point(brush.EndPoint.X, brush.EndPoint.Y));

        foreach (GradientStop item in brush.GradientStops)
        {
            this.brushModel.GradientStops.Add(item.Offset);
            this.brushModel.GradientColors.Add(item.Color);
        }

        this.brushModel.BrushTypeEnum = Enums.BrushTypes.LinearGradientBrush;
    }

    private void ProcessRadialGradientBrush(RadialGradientBrush brush)
    {
        if (brush == null || this.brushModel == null)
        {
            return;
        }

        this.brushModel.GradientPoints.Add(new System.Windows.Point(brush.Center.X, brush.Center.Y));
        this.brushModel.GradientPoints.Add(new System.Windows.Point(brush.GradientOrigin.X, brush.GradientOrigin.Y));

        foreach (GradientStop item in brush.GradientStops)
        {
            this.brushModel.GradientStops.Add(item.Offset);
            this.brushModel.GradientColors.Add(item.Color);
        }

        this.brushModel.BrushTypeEnum = Enums.BrushTypes.RadialGradientBrush;
    }

    private void ProcessSolidColorBrush(SolidColorBrush brush)
    {
        if (brush == null || this.brushModel == null)
        {
            return;
        }

        var color = Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);

        this.brushModel.Color = color;
        this.brushModel.GradientColors.Add(color);
        this.brushModel.GradientStops.Add(0.0);

        this.brushModel.BrushTypeEnum = Enums.BrushTypes.SolidColorBrush;
    }

    private void ProcessVisualBrush(VisualBrush brush)
    {
        if (brush == null || this.brushModel == null)
        {
            return;
        }

        // TODO implement VisualBrush logic
        this.brushModel.BrushTypeEnum = Enums.BrushTypes.VisualBrush;
    }
}

// EOF