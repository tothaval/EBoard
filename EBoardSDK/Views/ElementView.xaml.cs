// <copyright file="ElementView.xaml.cs" company=".">
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
namespace EBoardSDK.Views;

using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.Utilities;
using EBoardSDK.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Interaktionslogik für ElementView.xaml.
/// </summary>
public partial class ElementView : UserControl
{
    private ElementViewModel? elementViewModel;

    private bool isDragging;
    private bool isResizing;

    private Point oldMousePosition = default;
    private Point position;

    private UIElement? visualParent;
    private Canvas? canvas;

    private RotateTransform rotateTransform = new ();

    private int fallbackZ = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="ElementView"/> class.
    /// </summary>
    public ElementView()
    {
        this.InitializeComponent();

        this.SetPlacement();
    }

    public Canvas? Canvas => this.canvas;

    public bool IsDragging => this.isDragging;

    public Point Position => this.position;

    public UIElement? VisualParent => this.visualParent;

    public double X { get; set; }

    public double Y { get; set; }

    public int Z { get; set; }

    public void SetPlacement()
    {
        if (this.DataContext != null)
        {
            this.elementViewModel = (ElementViewModel)this.DataContext;
        }

        if (this.elementViewModel != null)
        {
            var manager = new FluidUIStandManager(this.elementViewModel);

            this.X = manager.GetPosition().X;
            this.Y = manager.GetPosition().Y;
            this.Z = manager.GetZ();

            Canvas.SetLeft(this.visualParent, this.X);
            Canvas.SetTop(this.visualParent, this.Y);
            Panel.SetZIndex(this.visualParent, this.Z);

            this.ElementBorder.RenderTransformOrigin = manager.GetTransformOriginPoint();
        }
    }

    public void UpdatePlacement()
    {
        if (this.elementViewModel != null)
        {
            var manager = new FluidUIStandManager(this.elementViewModel);

            this.X = Canvas.GetLeft(this.visualParent);

            this.Y = Canvas.GetTop(this.visualParent);

            if (Panel.GetZIndex(this.visualParent) != manager.GetZ())
            {
                Panel.SetZIndex(this.visualParent, manager.GetZ());
            }
        }
    }

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!Keyboard.IsKeyDown(Key.LeftCtrl) || !Keyboard.IsKeyDown(Key.RightCtrl))
        {
            if (this.elementViewModel != null && this.elementViewModel.ScreenViewModel.FluidUI.Stand != null)
            {
                if (this.Z < this.elementViewModel.ScreenViewModel.FluidUI.Stand.Zmaximum && this.Z != this.fallbackZ)
                {
                    this.fallbackZ = this.Z;
                }

                this.isDragging = true;
                this.isResizing = false;

                this.position = e.GetPosition(this.visualParent);

                this.elementViewModel.ScreenViewModel?.BeginElementSelectionMovement(this.elementViewModel);

                this.oldMousePosition = e.GetPosition(this.canvas);

                e.Handled = true;
            }
        }
    }

    private void Border_MouseMove(object sender, MouseEventArgs e)
    {
        if (!this.isDragging)
        {
            return;
        }

        if (this.isResizing)
        {
            return;
        }

        if (e.LeftButton == MouseButtonState.Pressed && this.elementViewModel != null)
        {
            Point canvasRelativePosition = e.GetPosition(this.canvas);

            double x, y;

            x = canvasRelativePosition.X - this.position.X;

            y = canvasRelativePosition.Y - this.position.Y;

            Canvas.SetLeft(this.visualParent, x);
            Canvas.SetTop(this.visualParent, y);

            Panel.SetZIndex(this.visualParent, 1000);

            Point delta = (Point)(this.oldMousePosition - canvasRelativePosition);

            this.elementViewModel.ScreenViewModel.MoveElementSelection(this.elementViewModel, delta);

            this.oldMousePosition = canvasRelativePosition;
        }
    }

    private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        // if (!isDragging)
        //    return;
        if (this.elementViewModel != null && this.elementViewModel.ScreenViewModel.FluidUI.Stand != null)
        {
            if (this.isDragging)
            {
                this.isDragging = false;

                this.X = Canvas.GetLeft(this.visualParent);

                this.Y = Canvas.GetTop(this.visualParent);

                if (this.Z > this.elementViewModel.ScreenViewModel.FluidUI.Stand.Zmaximum)
                {
                    this.Z = this.fallbackZ;
                }

                this.elementViewModel.StopMovement();

                Panel.SetZIndex(this.visualParent, this.Z);

                this.elementViewModel.ScreenViewModel?.StopElementSelectionMovement(this.elementViewModel);
            }

            this.elementViewModel.WasLastActive();
        }

        e.Handled = true;
    }

    private void Element_Loaded(object sender, RoutedEventArgs e)
    {
        this.visualParent = VisualTreeHelper.GetParent(this) as UIElement;

        if (this.visualParent != null)
        {
            this.canvas = VisualTreeHelper.GetParent(this.visualParent) as Canvas;
        }

        this.elementViewModel = (ElementViewModel)this.DataContext;

        if (this.elementViewModel != null)
        {
            var manager = new FluidUIStandManager(this.elementViewModel);

            this.X = manager.GetPosition().X;
            this.Y = manager.GetPosition().Y;
            this.Z = manager.GetZ();

            Canvas.SetLeft(this.visualParent, this.X);
            Canvas.SetTop(this.visualParent, this.Y);
            Panel.SetZIndex(this.visualParent, this.Z);

            this.elementViewModel.SetView(this);
        }
    }

    private void Element_Unloaded(object sender, RoutedEventArgs e)
    {
    }

    private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (this.elementViewModel == null)
        {
            return;
        }

        var manager = new FluidUIStandManager(this.elementViewModel);

        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            if (this.elementViewModel != null)
            {
                manager.ApplyRotationAngleValueByMouseWheel(e.Delta);
            }

            return;
        }

        manager.ApplyZIndexValueByMouseWheel(e.Delta);

        this.UpdatePlacement();
    }

    private void Element_Drop(object sender, DragEventArgs e)
    {

        if (e.Data.GetDataPresent(DataFormats.FileDrop) && this.elementViewModel != null)
        {
            this.elementViewModel.Drop(e, this.elementViewModel);
        }
    }
}

// EOF