/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ColorDataSet 
 * 
 *  serializable helper class to store and retrieve color and brush related data to
 *  or from hard drive storage, to allow for saving and loading of complex brushes
 *  like LinearGradientBrush, RadialGradientBrush, VisualBrush
 */
using EBoardSDK.SharedMethods;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EBoardSDK.Models.DataSets;

[Serializable]
public class ColorDataSet
{
    /// <summary>
    /// Gets or sets can be used to store the brushtype, this is used to rebuild the
    /// brush data on load
    /// </summary>
    public string BrushType { get; set; }

    /// <summary>
    /// Gets or sets gradientColor[0]
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    /// Gets or sets in case the brush is an image, the path to the image can be stored here
    /// </summary>
    public string ImagePath { get; set; }

    /// <summary>
    /// Gets or sets can be used to store gradient color strings, they will
    /// be matched with GradientStops list
    /// </summary>
    public ObservableCollection<Color> GradientColors { get; set; }

    /// <summary>
    /// Gets or sets can be used to store gradient stop points, they will
    /// be matched with GradientColors list
    /// </summary>
    public ObservableCollection<double> GradientStops { get; set; }

    /// <summary>
    /// Gets or sets can be used to store gradient points for radialgradientbrush and lineargradientbrush
    /// </summary>
    public ObservableCollection<Point> GradientPoints { get; set; }

    public ColorDataSet()
    {
    }

    public ColorDataSet(Brush brush)
    {
        this.ImagePath = string.Empty;

        this.GradientColors = new ObservableCollection<Color>();

        this.GradientStops = new ObservableCollection<double>();

        this.GradientPoints = new ObservableCollection<Point>();

        this.ProcessBrush(brush);
    }

    private LinearGradientBrush BuildLinearGradientBrush()
    {
        LinearGradientBrush linearGradientBrush = new LinearGradientBrush();

        if (this.GradientPoints.Count == 2)
        {
            linearGradientBrush.StartPoint = this.GradientPoints[0];
            linearGradientBrush.EndPoint = this.GradientPoints[1];
        }

        if (this.GradientColors.Count == this.GradientStops.Count)
        {
            for (int i = 0; i < this.GradientColors.Count; i++)
            {
                linearGradientBrush.GradientStops.Add(
                    new GradientStop(
                        this.GradientColors[i],
                        this.GradientStops[i])
                    );
            }
        }

        return linearGradientBrush;
    }

    private RadialGradientBrush BuildRadialGradientBrush()
    {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush();

        if (this.GradientPoints.Count == 2)
        {
            radialGradientBrush.Center = this.GradientPoints[0];
            radialGradientBrush.GradientOrigin = this.GradientPoints[1];
        }

        if (this.GradientColors.Count == this.GradientStops.Count)
        {
            for (int i = 0; i < this.GradientColors.Count; i++)
            {
                radialGradientBrush.GradientStops.Add(
                    new GradientStop(
                        this.GradientColors[i],
                        this.GradientStops[i])
                    );
            }
        }

        return radialGradientBrush;
    }

    public async Task<Brush> GetBrush()
    {
        if (this.BrushType.Equals("SolidColorBrush"))
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush(this.GradientColors[0]);

            await Task.CompletedTask;

            return solidColorBrush;
        }

        if (this.BrushType.Equals("LinearGradientBrush"))
        {
            LinearGradientBrush linearGradientBrush = this.BuildLinearGradientBrush();

            await Task.CompletedTask;

            return linearGradientBrush;
        }

        if (this.BrushType.Equals("RadialGradientBrush"))
        {
            RadialGradientBrush radialGradientBrush = this.BuildRadialGradientBrush();

            await Task.CompletedTask;

            return radialGradientBrush;
        }

        if (this.BrushType.Equals("ImageBrush"))
        {
            Brush brush = new ImageBrush();

            brush = new SharedMethod_UI().ChangeBackgroundToImage(brush, this.ImagePath);

            if (brush.GetType().Name.Equals("SolidColorBrush"))
            {
                this.BrushType = "SolidColorBrush";

                this.Color = ((SolidColorBrush)brush).Color;

                this.GradientColors.Clear();

                this.GradientColors.Add(this.Color);

                await Task.CompletedTask;

                return brush as SolidColorBrush;
            }

            await Task.CompletedTask;

            return brush as ImageBrush;
        }

        if (this.BrushType.Equals("VisualBrush"))
        {
            return new VisualBrush();
        }

        return new SolidColorBrush();
    }

    private void ProcessBrush(Brush brush)
    {
        this.BrushType = brush.GetType().Name;

        this.GradientColors.Clear();
        this.GradientPoints.Clear();
        this.GradientStops.Clear();

        if (this.BrushType.Equals("SolidColorBrush"))
        {
            this.ProcessSolidColorBrush((SolidColorBrush)brush);
        }

        if (this.BrushType.Equals("LinearGradientBrush"))
        {
            this.ProcessLinearGradientBrush((LinearGradientBrush)brush);
        }

        if (this.BrushType.Equals("RadialGradientBrush"))
        {
            this.ProcessRadialGradientBrush((RadialGradientBrush)brush);
        }

        if (this.BrushType.Equals("ImageBrush"))
        {
            this.ProcessImageBrush((ImageBrush)brush);
        }

        if (this.BrushType.Equals("VisualBrush"))
        {
        }
    }

    private void ProcessImageBrush(ImageBrush brush)
    {
        this.ImagePath = ((BitmapImage)brush.ImageSource).UriSource.AbsoluteUri;
    }

    private void ProcessLinearGradientBrush(LinearGradientBrush brush)
    {
        this.GradientPoints.Add(brush.StartPoint);
        this.GradientPoints.Add(brush.EndPoint);

        foreach (GradientStop item in brush.GradientStops)
        {
            this.GradientStops.Add(item.Offset);
            this.GradientColors.Add(item.Color);
        }
    }

    private void ProcessRadialGradientBrush(RadialGradientBrush brush)
    {
        this.GradientPoints.Add(brush.Center);
        this.GradientPoints.Add(brush.GradientOrigin);

        foreach (GradientStop item in brush.GradientStops)
        {
            this.GradientStops.Add(item.Offset);
            this.GradientColors.Add(item.Color);
        }
    }

    private void ProcessSolidColorBrush(SolidColorBrush brush)
    {
        this.Color = brush.Color;
        this.GradientColors.Add(brush.Color);
        this.GradientStops.Add(0.0);
    }
}
// EOF