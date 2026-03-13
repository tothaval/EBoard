// <copyright file="BrushSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.BrushSetup;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.BrushSetup.LinearBrushSetup;
using EBoardSDK.Controls.BrushSetup.RadialBrushSetup;
using EBoardSDK.Controls.BrushSetup.SolidBrushSetup;
using EBoardSDK.Enums;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.ViewModels;
<<<<<<< Updated upstream
=======
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;
using Path = System.Windows.Shapes.Path;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
>>>>>>> Stashed changes

public partial class BrushSetupViewModel : ObservableObject
{
    private BrushTargets brushTargets;

    private readonly EboardFluidUIBaseViewModel viewModel;
    private FluidUIDesignManager fluidUIDesignManager;

    private SolidBrushSetupViewModel solidBrushViewModel;
    private LinearBrushSetupViewModel linearBrushViewModel = new LinearBrushSetupViewModel();
    private RadialBrushSetupViewModel radialBrushViewModel = new RadialBrushSetupViewModel();

    /// <summary>
    /// Initializes a new instance of the <see cref="BrushSetupViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    /// <param name="brushTargets"></param>
    /// <param name="okAction"></param>
    public BrushSetupViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel, BrushTargets brushTargets, Action okAction)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.fluidUIDesignManager = new FluidUIDesignManager(this.ViewModel);

        this.solidBrushViewModel = new SolidBrushSetupViewModel(eboardFluidUIBaseViewModel, brushTargets, okAction);

        this.brushTargets = brushTargets;

        this.OnPropertyChanged(nameof(this.SolidBrush));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public SolidBrushSetupViewModel SolidBrush => this.solidBrushViewModel;

    public LinearBrushSetupViewModel LinearBrush => this.linearBrushViewModel;

    public RadialBrushSetupViewModel RadialBrush => this.radialBrushViewModel;

    [RelayCommand]
    private void ResetForegroundImage()
    {
<<<<<<< Updated upstream
        this.fluidUIDesignManager.ResetBrush(BrushTargets.Foreground);
=======
        VisualBrush visualBrushExample = new VisualBrush();

        switch (examplenr)
        {
            case 0:
                // below some code examples for visual brushes
                // and ways to construct the visual, from:
                // https://learn.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/how-to-paint-an-area-with-a-visual

                // Create the visual brush's contents.
                StackPanel myStackPanel = new StackPanel();
                myStackPanel.Background = Brushes.White;

                Rectangle redRectangle = new Rectangle();
                redRectangle.Width = 25;
                redRectangle.Height = 25;
                redRectangle.Fill = Brushes.Red;
                redRectangle.Margin = new Thickness(2);
                myStackPanel.Children.Add(redRectangle);

                TextBlock someText = new TextBlock();
                FontSizeConverter myFontSizeConverter = new FontSizeConverter();
                someText.FontSize = (double)myFontSizeConverter.ConvertFrom("10pt");
                someText.Text = "Hello, World!";
                someText.Margin = new Thickness(2);
                myStackPanel.Children.Add(someText);

                Button aButton = new Button();
                aButton.Content = "A Button";
                aButton.Margin = new Thickness(2);
                myStackPanel.Children.Add(aButton);

                visualBrushExample.Visual = myStackPanel;
                break;
            case 1:
                // https://stackoverflow.com/questions/51743419/create-visualbrush-by-code-behind
                visualBrushExample.Visual = new Path
                {
                    Data = Geometry.Parse("M 0 5 L 5 0 M -2 2 L 2 -2 M 3 7 L 7 3"),
                    Stroke = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Color.FromArgb(0xee, 0xff, 0xbb, 0xff)),
                };
                break;

            case 2:
                // https://www.c-sharpcorner.com/UploadFile/mahesh/visual-brush-in-wpf/
                // Create a StackPanel and add a few controls to it

                StackPanel stkPanel = new StackPanel();

                // Create a Rectangle and add it to StackPanel
                Rectangle yellowGreenRectangle = new Rectangle();

                yellowGreenRectangle.Height = 100;

                yellowGreenRectangle.Width = 20;

                LinearGradientBrush yellowGreenLGBrush = new LinearGradientBrush();

                yellowGreenLGBrush.StartPoint = new Point(0, 0);

                yellowGreenLGBrush.EndPoint = new Point(1, 1);

                GradientStop blueGS = new GradientStop();

                blueGS.Color = Colors.Yellow;

                blueGS.Offset = 0.0;

                yellowGreenLGBrush.GradientStops.Add(blueGS);

                GradientStop orangeGS = new GradientStop();

                orangeGS.Color = Colors.Green;

                orangeGS.Offset = 0.25;

                yellowGreenLGBrush.GradientStops.Add(orangeGS);

                yellowGreenRectangle.Fill = yellowGreenLGBrush;

                stkPanel.Children.Add(yellowGreenRectangle);

                // Create a TextBlock and add it to StackPanel

                TextBlock redTextBlock = new TextBlock();

                redTextBlock.Text = "Visual Brush";

                redTextBlock.FontWeight = FontWeights.Bold;

                redTextBlock.FontFamily = new FontFamily("Georgia");

                redTextBlock.TextAlignment = TextAlignment.Center;

                stkPanel.Children.Add(redTextBlock);

                // Create a Button and add it to StackPanel

                Button blueButton = new Button();

                blueButton.Background = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Colors.LightBlue);

                blueButton.Foreground = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Colors.Orange);

                blueButton.Content = "Button for Visual";

                stkPanel.Children.Add(blueButton);

                // Set Viewport and TileMode

                visualBrushExample.Viewport = new Rect(0, 0, 0.25, 0.25);

                visualBrushExample.TileMode = TileMode.Tile;

                // Set Visual of VisualBrush

                visualBrushExample.Visual = stkPanel;
                break;

            case 3:

                VisualBrush dotFillBrush = new VisualBrush();
                dotFillBrush.TileMode = TileMode.Tile;
                dotFillBrush.Viewport = new Rect(0, 0, 10, 10);
                dotFillBrush.ViewportUnits = BrushMappingMode.Absolute;
                dotFillBrush.Viewbox = new Rect(0, 0, 12, 12);
                dotFillBrush.ViewboxUnits = BrushMappingMode.Absolute;
                dotFillBrush.Visual = new Ellipse()
                {
                    Fill = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Color.FromArgb(255, 0, 200, 255)),
                    Width = 10.0,
                    Height = 10.0,
                };

                VisualBrush hatchBrush = new VisualBrush();
                hatchBrush.TileMode = TileMode.Tile;
                hatchBrush.Viewport = new Rect(0, 0, 10, 10);
                hatchBrush.ViewportUnits = BrushMappingMode.Absolute;
                hatchBrush.Viewbox = new Rect(0, 0, 10, 10);
                hatchBrush.ViewboxUnits = BrushMappingMode.Absolute;

                var canvas = new Canvas();
                canvas.Children.Add(new Rectangle()
                {
                    Fill = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Colors.Azure),
                    Width = 10,
                    Height = 10,
                });

                canvas.Children.Add(new Path()
                {
                    Stroke = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Colors.Azure),
                    Data = Geometry.Parse("M 0 0 l 10 10"),
                });

                canvas.Children.Add(new Path()
                {
                    Stroke = FluidUIDesignDefaultPropertyFactory.GetSolidColorBrush(Colors.Azure),
                    Data = Geometry.Parse("M 0 10 l 10 -10"),
                });

                hatchBrush.Visual = canvas;

                Canvas outerCanvas = new Canvas();

                var rect1 = new Rectangle()
                {
                    Width = 80,
                    Height = 40,
                    Fill = dotFillBrush,
                };
                Canvas.SetTop(rect1, 20);
                Canvas.SetLeft(rect1, 20);
                outerCanvas.Children.Add(rect1);

                var rect2 = new Rectangle()
                {
                    Width = 80,
                    Height = 40,
                    Fill = hatchBrush,
                };
                Canvas.SetTop(rect2, 20);
                Canvas.SetLeft(rect2, 120);
                outerCanvas.Children.Add(rect2);

                var tb1 = new TextBlock()
                {
                    Text = "Hello",
                    FontSize = 80,
                    Foreground = dotFillBrush,
                };
                Canvas.SetTop(tb1, 80);
                Canvas.SetLeft(tb1, 20);
                outerCanvas.Children.Add(tb1);

                var tb2 = new TextBlock()
                {
                    Text = "World",
                    FontSize = 80,
                    Foreground = hatchBrush,
                };
                Canvas.SetTop(tb2, 80);
                Canvas.SetLeft(tb2, 220);
                outerCanvas.Children.Add(tb2);

                visualBrushExample.Visual = outerCanvas;
                break;

            case 4:
                visualBrushExample.Visual = new Path()
                {
                    Data = Geometry.Parse("M24 46.997c9.384 0 17 .225 17 .501C41 47.775 33.384 48 24 48S7 47.775 7 47.499C7 47.223 14.616 46.997 24 46.997zM1.991 41.003V7.001h44.001v34.001H1.991zM44.001 35.964V12.037c0-1.677-1.361-3.039-3.039-3.039H7.036c-1.677 0-3.039 1.361-3.039 3.039v23.927c0 1.677 1.361 3.039 3.039 3.039h33.927C42.64 39.003 44.001 37.641 44.001 35.964zM14.741 36.472c-1.008-.379-1.964-.747-2.849-1.023-.889-.277-1.663-.447-2.303-.447-.676 0-1.357.157-1.853.439C7.249 35.719 7 36.075 7 36.503c0 .276-.225.5-.5.5-.276 0-.5-.224-.5-.5 0-.896.557-1.54 1.241-1.929.679-.388 1.541-.571 2.348-.571.793 0 1.684.207 2.6.492.92.287 1.911.668 2.904 1.04 2.031.761 4.047 1.468 5.821 1.468.767 0 1.44-.2 1.905-.501.471-.304.677-.667.677-.999 0-.659 1-.659 1 0 0 .332.205.695.677.999.468.301 1.143.501 1.911.501 1.761 0 3.779-.703 5.82-1.468 1.892-.708 3.955-1.532 5.505-1.532.805 0 1.668.183 2.347.571C41.445 34.965 42 35.611 42 36.503c0 .276-.225.5-.5.5-.276 0-.5-.224-.5-.5 0-.425-.247-.781-.737-1.061-.496-.281-1.177-.439-1.852-.439-.641 0-1.415.169-2.304.447-.885.276-1.841.644-2.849 1.023-1.975.739-4.185 1.531-6.172 1.531-.931 0-1.8-.241-2.452-.661-.163-.104-.315-.221-.452-.352l-.184-.197-.185.197c-.136.131-.288.248-.449.352-.651.42-1.519.661-2.448.661C18.915 38.003 16.705 37.207 14.741 36.472zM6 32.501c0-1.423.563-2.439 1.069-3.289.073-.124.145-.243.213-.359C6.531 28.528 6 27.673 6 26.891c0-.937.449-1.688.921-2.396C6.449 23.78 6 23.024 6 22.093c0-.773.532-1.629 1.28-1.961-.068-.111-.137-.228-.209-.349C6.563 18.933 6 17.919 6 16.496c0-.275.224-.5.5-.5.275 0 .5.225.5.5 0 1.135.436 1.949.928 2.773.141.239.293.487.436.741 1.636.143 2.224 1.095 2.588 1.872.117.249.009.548-.241.664-.249.117-.548.009-.664-.241-.167-.355-.34-.665-.631-.897-.141-.112-.321-.216-.563-.292C8.944 21.42 9 21.744 9 22.093c0 .948-.423 1.707-.879 2.401C8.577 25.196 9 25.959 9 26.897c0 .348-.056.669-.144.971.24-.076.42-.18.56-.292.291-.232.464-.543.631-.897.117-.249.415-.357.665-.24.249.117.357.415.24.665-.361.771-.947 1.724-2.581 1.869-.144.259-.297.509-.441.751C7.436 30.551 7 31.367 7 32.501c0 .276-.225.5-.5.5C6.224 33.001 6 32.777 6 32.501zM41.003 32.501c0-1.135-.437-1.951-.931-2.777-.145-.241-.299-.492-.443-.751-1.631-.145-2.219-1.095-2.58-1.869-.117-.251-.009-.548.24-.665.251-.117.548-.009.665.24.167.355.34.665.631.897.139.112.319.216.56.292-.089-.301-.143-.623-.143-.971 0-.948.421-1.707.877-2.403-.456-.7-.877-1.461-.877-2.401 0-.349.055-.673.145-.977-.243.076-.423.18-.563.292-.291.232-.464.543-.631.897-.116.251-.415.359-.664.241-.251-.116-.359-.415-.241-.664.36-.773.947-1.728 2.587-1.872.143-.255.295-.501.437-.741.492-.824.929-1.639.929-2.773 0-.275.224-.5.5-.5.275 0 .5.225.5.5 0 1.423-.564 2.437-1.072 3.287-.073.121-.143.237-.208.349.749.328 1.28 1.181 1.28 1.961 0 .94-.451 1.692-.923 2.401.472.713.923 1.467.923 2.396 0 .776-.537 1.635-1.287 1.963.069.116.141.235.216.359.507.851 1.071 1.867 1.071 3.289 0 .276-.225.5-.5.5C41.227 33.001 41.003 32.777 41.003 32.501zM14.995 31.999c0-.551.448-1 1-1H32c.551 0 1 .449 1 1 0 .552-.449 1-1 1H15.995C15.443 32.999 14.995 32.551 14.995 31.999zM22.999 29.652v-.717c-.776-.072-1.703-.176-1.997-.176-.192 0-.359.244-.563.244h-.489c-.277 0-.387-.447-.456-.7-.172-.629-.392-1.32-.484-1.868-.043-.257.037-.432.577-.432.417 0 .697.16.883.428l.659.951c.265.392 1.127.537 1.871.591v-3.796c-.648-.383-1.337-.772-1.999-1.173-.872-.601-2-1.521-2-3 0-1.609 1.563-2.717 3.997-2.952l.001-.715c0-.191.156-.347.347-.347h1.308c.191 0 .347.156.347.347v.78c.387.064.76.131.999.131.192 0 .359-.244.563-.244h.489c.277 0 .387.447.456.7.172.629.392 1.32.484 1.868.043.257-.037.432-.577.432-.417 0-.697-.16-.883-.428l-.659-.951c-.209-.307-.528-.463-.872-.541v3.748c.647.383 1.336.772 2 1.172.872.601 2 1.521 2 3 0 1.611-1.564 2.719-4 2.955v.695c0 .191-.156.347-.347.347h-1.308C23.155 29.999 22.999 29.843 22.999 29.652zM8 26.897c0-.529-.205-1.012-.484-1.487C7.22 25.892 7 26.369 7 26.891c0 .475.383.952.757 1.072C7.911 27.612 8 27.275 8 26.897zM41.003 26.891c0-.516-.223-.995-.52-1.479-.277.471-.48.952-.48 1.485 0 .373.088.715.239 1.065C40.613 27.841 41.003 27.359 41.003 26.891zM26 27.003c0-.5-.389-.995-1-1.496v2.387C25.8 27.685 25.988 27.235 26 27.003zM41.003 22.093c0-.471-.383-.949-.756-1.071-.155.353-.244.693-.244 1.071 0 .531.204 1.011.481 1.485C40.78 23.096 41.003 22.617 41.003 22.093zM8 22.093c0-.372-.092-.717-.245-1.071C7.384 21.147 7 21.628 7 22.093c0 .519.223.999.519 1.484C7.796 23.108 8 22.628 8 22.093zM22.997 20.499v-2.384c-.799.208-.987.657-.997.888C22 19.503 22.388 19.999 22.997 20.499zM7.207 14.428C6.533 14.032 6 13.387 6 12.5c0-.889.536-1.533 1.207-1.927C7.869 10.184 8.712 10 9.5 10c.776 0 1.643.208\r\n2.533.493.895.287 1.857.669 2.821 1.041.247.096.492.191.739.283l.417.152.715-.244C17.995 11.312 19.289 11 20.5 11c.908 0 1.757.244 2.395.665.613.409 1.075 1.02 1.103 1.751.029-.731.489-1.341 1.104-1.751C25.739 11.244 26.588 11 27.497 11c1.211 0 2.509.313 3.777.727l.709.243.415-.151c.247-.093.493-.188.743-.284C34.972 10.828 36.984 10 38.497 10c.787 0 1.629.184 2.292.573.673.396 1.208 1.041 1.208 1.927 0 .891-.537 1.535-1.208 1.928C40.127 14.817 39.284 15 38.497 15c-.777 0-1.644-.207-2.535-.492-.895-.287-1.857-.669-2.821-1.041-.248-.095-.493-.189-.74-.281l-.417-.152-.715.244C30 13.691 28.705 14 27.497 14c-.276 0-.5-.224-.5-.5 0-.275.224-.5.5-.5.856 0 1.771-.175 2.719-.445l.177-.053-.171-.053C29.273 12.177 28.356 12 27.497 12c-1.449 0-2.5.752-2.5 1.5 0 .276-.225.5-.5.5-.264 0-.48-.204-.5-.46C23.979 13.796 23.761 14 23.5 14c-.276 0-.5-.224-.5-.5 0-.756-1.061-1.5-2.5-1.5-.857 0-1.772.176-2.721.447l-.177.055.172.052C18.723 12.824 19.64 13 20.5 13c.275 0 .5.225.5.5 0 .276-.225.5-.5.5-1.213 0-2.512-.311-3.78-.724l-.709-.243-.415.151c-.245.093-.493.188-.741.283C13.024 14.173 11.012 15 9.5 15 8.712 15 7.869 14.817 7.207 14.428zM11.728 13.556c.86-.276 1.787-.644 2.768-1.021l.088-.033-.088-.035C12.432 11.672 10.776 11 9.5 11c-.652 0-1.309.156-1.787.436C7.247 11.709 7 12.064 7 12.5c0 .433.243.789.713 1.065C8.191 13.845 8.848 14 9.5 14 10.117 14 10.865 13.832 11.728 13.556zM40.283 13.565c.467-.273.715-.628.715-1.065 0-.432-.244-.788-.715-1.064C39.805 11.156 39.148 11 38.497 11c-.619 0-1.367.169-2.229.445-.86.276-1.787.644-2.768 1.021l-.089.035.089.033C35.564 13.329 37.22 14 38.497 14 39.148 14 39.805 13.845 40.283 13.565z"),
                    StrokeThickness = 2,
                    Stroke = new SolidColorBrush(Colors.Navy),
                };
                break;
            default:
                break;
        }

        return visualBrushExample;
    }

    internal void SetBrush(Brush brush)
    {
        this.brush = brush;

        this.OnPropertyChanged(nameof(this.Brush));

        this.okAction?.Invoke();
    }

    private void ChangeToSolidColorBrush()
    {
        this.brush = this.SolidBrush.ColorBrush;

        this.OnPropertyChanged(nameof(this.Brush));

        this.okAction?.Invoke();
    }

    private void SetImageBrush(string path = "")
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            path = new SharedMethod_UI().UserSelectImage(path);
        }

        this.SetBrush(FluidUIDesignDefaultPropertyFactory.CreateImageBrush(path));
    }

    partial void OnImagePathChanged(string value)
    {
        this.SetImageBrush(value);
>>>>>>> Stashed changes
    }

    [RelayCommand]
    private void ResetHighlightImage()
    {
        this.fluidUIDesignManager.ResetBrush(BrushTargets.Highlight);
    }

    [RelayCommand]
    private void ResetImage()
    {
        this.fluidUIDesignManager.ResetBrush(BrushTargets.Background);
    }

    [RelayCommand]
    private void ResetImageBorder()
    {
        this.fluidUIDesignManager.ResetBrush(BrushTargets.Border);
    }

    [RelayCommand]
    private void SetBackgroundImage()
    {
        this.fluidUIDesignManager.SetUserChosenImagePath(BrushTargets.Background);
    }

    [RelayCommand]
    private void SetForegroundImage()
    {
        this.fluidUIDesignManager.SetUserChosenImagePath(BrushTargets.Foreground);
    }

    [RelayCommand]
    private void SetBorderImage()
    {
        this.fluidUIDesignManager.SetUserChosenImagePath(BrushTargets.Border);
    }

    [RelayCommand]
    private void SetHighlightImage()
    {
        this.fluidUIDesignManager.SetUserChosenImagePath(BrushTargets.Highlight);
    }
}

// EOF