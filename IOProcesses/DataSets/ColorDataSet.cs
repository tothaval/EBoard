/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ColorDataSet 
 * 
 *  serializable helper class to store and retrieve color and brush related data to
 *  or from hard drive storage, to allow for saving and loading of complex brushes
 *  like LinearGradientBrush, RadialGradientBrush, VisualBrush
 */
using EBoard.Utilities.SharedMethods;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EBoard.IOProcesses.DataSets;

[Serializable]
public class ColorDataSet
{
    /// <summary>
    /// can be used to store the brushtype, this is used to rebuild the
    /// brush data on load
    /// </summary>
    public string BrushType { get; set; }


    /// <summary>
    /// GradientColor[0]
    /// </summary>
    public Color Color { get; set; }


    /// <summary>
    /// in case the brush is an image, the path to the image can be stored here
    /// </summary>
    public string ImagePath { get; set; }


    /// <summary>
    /// can be used to store gradient color strings, they will
    /// be matched with GradientStops list
    /// </summary>
    public ObservableCollection<Color> GradientColors { get; set; }


    /// <summary>
    /// can be used to store gradient stop points, they will
    /// be matched with GradientColors list
    /// </summary>
    public ObservableCollection<double> GradientStops { get; set; }


    /// <summary>
    /// can be used to store gradient points for radialgradientbrush and lineargradientbrush
    /// </summary>
    public ObservableCollection<Point> GradientPoints { get; set; }


    public ColorDataSet()
    {

    }


    public ColorDataSet(Brush brush)
    {
        ImagePath = string.Empty;

        GradientColors = new ObservableCollection<Color>();

        GradientStops = new ObservableCollection<double>();

        GradientPoints = new ObservableCollection<Point>();

        ProcessBrush(brush);
    }


    private LinearGradientBrush BuildLinearGradientBrush()
    {
        LinearGradientBrush linearGradientBrush = new LinearGradientBrush();

        if (GradientPoints.Count == 2)
        {
            linearGradientBrush.StartPoint = GradientPoints[0];
            linearGradientBrush.EndPoint = GradientPoints[1];
        }

        if (GradientColors.Count == GradientStops.Count)
        {
            for (int i = 0; i < GradientColors.Count; i++)
            {
                linearGradientBrush.GradientStops.Add(
                    new GradientStop(
                        GradientColors[i],
                        GradientStops[i])
                    );
            }
        }

        return linearGradientBrush;
    }


    private RadialGradientBrush BuildRadialGradientBrush()
    {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush();

        if (GradientPoints.Count == 2)
        {
            radialGradientBrush.Center = GradientPoints[0];
            radialGradientBrush.GradientOrigin = GradientPoints[1];
        }

        if (GradientColors.Count == GradientStops.Count)
        {
            for (int i = 0; i < GradientColors.Count; i++)
            {
                radialGradientBrush.GradientStops.Add(
                    new GradientStop(
                        GradientColors[i],
                        GradientStops[i])
                    );
            }
        }

        return radialGradientBrush;
    }


    public async Task<Brush> GetBrush()
    {
        if (BrushType.Equals("SolidColorBrush"))
        {

            SolidColorBrush solidColorBrush = new SolidColorBrush(GradientColors[0]);

            await Task.CompletedTask;

            return solidColorBrush;
        }

        if (BrushType.Equals("LinearGradientBrush"))
        {
            LinearGradientBrush linearGradientBrush = BuildLinearGradientBrush();

            await Task.CompletedTask;

            return linearGradientBrush;
        }

        if (BrushType.Equals("RadialGradientBrush"))
        {
            RadialGradientBrush radialGradientBrush = BuildRadialGradientBrush();

            await Task.CompletedTask;

            return radialGradientBrush;
        }

        if (BrushType.Equals("ImageBrush"))
        {
            Brush brush = new ImageBrush();

            brush = new SharedMethod_UI().ChangeBackgroundToImage(brush, ImagePath);

            if (brush.GetType().Name.Equals("SolidColorBrush"))
            {
                BrushType = "SolidColorBrush";

                Color = ((SolidColorBrush)brush).Color;

                GradientColors.Clear();

                GradientColors.Add(Color);

                await Task.CompletedTask;

                return brush as SolidColorBrush;
            }            

            await Task.CompletedTask;

            return brush as ImageBrush;

        }

        if (BrushType.Equals("VisualBrush"))
        {

            return new VisualBrush();
        }

        return new SolidColorBrush();
    }


    private void ProcessBrush(Brush brush)
    {
        BrushType = brush.GetType().Name;

        GradientColors.Clear();
        GradientPoints.Clear();
        GradientStops.Clear();

        if (BrushType.Equals("SolidColorBrush"))
        {
            ProcessSolidColorBrush((SolidColorBrush)brush);
        }

        if (BrushType.Equals("LinearGradientBrush"))
        {
            ProcessLinearGradientBrush((LinearGradientBrush)brush);
        }

        if (BrushType.Equals("RadialGradientBrush"))
        {
            ProcessRadialGradientBrush((RadialGradientBrush)brush);
        }

        if (BrushType.Equals("ImageBrush"))
        {
            ProcessImageBrush((ImageBrush)brush);
        }

        if (BrushType.Equals("VisualBrush"))
        {

        }
    }


    private void ProcessImageBrush(ImageBrush brush)
    {
        ImagePath = ((BitmapImage)brush.ImageSource).UriSource.AbsoluteUri;
    }


    private void ProcessLinearGradientBrush(LinearGradientBrush brush)
    {
        GradientPoints.Add(brush.StartPoint);
        GradientPoints.Add(brush.EndPoint);

        foreach (GradientStop item in brush.GradientStops)
        {
            GradientStops.Add(item.Offset);
            GradientColors.Add(item.Color);
        }
    }


    private void ProcessRadialGradientBrush(RadialGradientBrush brush)
    {
        GradientPoints.Add(brush.Center);
        GradientPoints.Add(brush.GradientOrigin);

        foreach (GradientStop item in brush.GradientStops)
        {
            GradientStops.Add(item.Offset);
            GradientColors.Add(item.Color);
        }
    }


    private void ProcessSolidColorBrush(SolidColorBrush brush)
    {
        Color = brush.Color;
        GradientColors.Add(brush.Color);
        GradientStops.Add(0.0);
    }

}
// EOF