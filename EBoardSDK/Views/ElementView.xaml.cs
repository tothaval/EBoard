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

using EBoardSDK.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Interaktionslogik für ElementView.xaml.
/// </summary>
public partial class ElementView : UserControl
{
    private ElementViewModel _ElementViewModel;

    private bool _IsDragging;

    private Point _Position;

    public Point Position => this._Position;

    private UIElement _VisualParent;

    public UIElement VisualParent => this._VisualParent;

    private Canvas _Canvas;

    private double _X;

    public double X
    {
        get
        {
            return this._X;
        }

        set
        {
            this._X = value;
        }
    }

    private double _Y;

    public double Y
    {
        get
        {
            return this._Y;
        }

        set
        {
            this._Y = value;
        }
    }

    private int fallbackZ = 0;

    private int _Z;
    private bool _IsResizing;

    public int Z
    {
        get
        {
            return this._Z;
        }

        set
        {
            this._Z = value;
        }
    }

    public ElementView()
    {
        this.InitializeComponent();

        this.SetPlacement();
    }

    public void SetPlacement()
    {
        if (this.DataContext != null)
        {
            this._ElementViewModel = (ElementViewModel)this.DataContext;

            this._ElementViewModel.SetView(this);
        }

        if (this._ElementViewModel != null)
        {
            this.X = this._ElementViewModel.FluidUI.Stand.Position.X;
            this.Y = this._ElementViewModel.FluidUI.Stand.Position.Y;
            this.Z = this._ElementViewModel.FluidUI.Stand.Z;

            Canvas.SetLeft(this._VisualParent, this.X);
            Canvas.SetTop(this._VisualParent, this.Y);
            Panel.SetZIndex(this._VisualParent, this.Z);

            this.ElementBorder.RenderTransformOrigin = this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.TransformOriginPoint;
            this.ElementBorder.RenderTransform = this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.RotateTransformValue;
        }
    }

    public void UpdatePlacement()
    {
        this.X = Canvas.GetLeft(this._VisualParent);

        this.Y = Canvas.GetTop(this._VisualParent);

        if (Panel.GetZIndex(this._VisualParent) != this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ZIndexValue)
        {
            Panel.SetZIndex(this._VisualParent, this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ZIndexValue);
        }

        this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XMaximumValue = (int)this._Canvas.ActualWidth;
        this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YMaximumValue = (int)this._Canvas.ActualHeight;
    }

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!Keyboard.IsKeyDown(Key.LeftCtrl) || !Keyboard.IsKeyDown(Key.RightCtrl))
        {
            this.Z = this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ZIndexValue;

            if (this.Z < this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ZMaximumValue && this.Z != this.fallbackZ)
            {
                this.fallbackZ = this.Z;
            }

            this._IsDragging = true;
            this._IsResizing = false;

            this._Position = e.GetPosition(this._VisualParent);

            this._ElementViewModel.EBoardViewModel?.BeginElementSelectionMovement(this._ElementViewModel);

            this.oldMousePosition = e.GetPosition(this._Canvas);

            e.Handled = true;
        }
    }

    private Point oldMousePosition = default;

    private void Border_MouseMove(object sender, MouseEventArgs e)
    {
        if (!this._IsDragging)
        {
            return;
        }

        if (this._IsResizing)
        {
            return;
        }

        if (e.LeftButton == MouseButtonState.Pressed)
        {
            Point canvasRelativePosition = e.GetPosition(this._Canvas);

            double x, y;

            x = canvasRelativePosition.X - this._Position.X;

            y = canvasRelativePosition.Y - this._Position.Y;

            Canvas.SetLeft(this._VisualParent, x);
            Canvas.SetTop(this._VisualParent, y);

            Panel.SetZIndex(this._VisualParent, 1000);
            this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ZIndexValue = 1000;

            Point delta = (Point)(this.oldMousePosition - canvasRelativePosition);

            this._ElementViewModel.EBoardViewModel.MoveElementSelection(this._ElementViewModel, delta);

            this.oldMousePosition = canvasRelativePosition;
        }
    }

    private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        // if (!_IsDragging)
        //    return;
        if (this._IsDragging)
        {
            this._IsDragging = false;

            this.X = Canvas.GetLeft(this._VisualParent);

            this.Y = Canvas.GetTop(this._VisualParent);

            this._ElementViewModel.FluidUI.Stand.Position = new Point(this.X, this.Y);

            this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = this.X;
            this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = this.Y;

            if (this.Z > this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ZMaximumValue)
            {
                this.Z = this.fallbackZ;
                this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ZIndexValue = this.fallbackZ;
            }
            else
            {
                this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ZIndexValue = this.Z;
            }

            Panel.SetZIndex(this._VisualParent, this.Z);

            this._ElementViewModel.EBoardViewModel.StopElementSelectionMovement(this._ElementViewModel);
        }

        this._ElementViewModel.WasLastActive();

        e.Handled = true;
    }

    private void Element_Loaded(object sender, RoutedEventArgs e)
    {
        this._VisualParent = VisualTreeHelper.GetParent(this) as UIElement;

        if (this._VisualParent != null)
        {
            this._Canvas = VisualTreeHelper.GetParent(this._VisualParent) as Canvas;
        }

        this._ElementViewModel = (ElementViewModel)this.DataContext;

        this._ElementViewModel.SetView(this);

        this.X = this._ElementViewModel.FluidUI.Stand.Position.X;
        this.Y = this._ElementViewModel.FluidUI.Stand.Position.Y;
        this.Z = this._ElementViewModel.FluidUI.Stand.Z;

        Canvas.SetLeft(this._VisualParent, this.X);
        Canvas.SetTop(this._VisualParent, this.Y);
        Panel.SetZIndex(this._VisualParent, this.Z);

        this.ElementBorder.RenderTransformOrigin = this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.TransformOriginPoint;
        this.ElementBorder.RenderTransform = this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.RotateTransformValue;
    }

    private void Element_Unloaded(object sender, RoutedEventArgs e)
    {
    }

    private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ApplyRotationAngleValueByMouseWheel(e.Delta);

            return;
        }

        this._ElementViewModel.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ApplyZIndexValueByMouseWheel(e.Delta);

        this.UpdatePlacement();
    }

    private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
    {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            this._IsResizing = true;

            this._IsDragging = false;

            this._Position = e.GetPosition(this._VisualParent);

            this.X = Canvas.GetLeft(this._VisualParent);

            this.Y = Canvas.GetTop(this._VisualParent);

            this.oldMousePosition = e.GetPosition(this._Canvas);

            e.Handled = true;
        }
    }

    private void Border_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
    {
        if (!this._IsResizing)
        {
            return;
        }

        if (this._IsResizing)
        {
            this._IsResizing = false;

            this.X = Canvas.GetLeft(this._VisualParent);

            this.Y = Canvas.GetTop(this._VisualParent);
        }

        this._ElementViewModel.WasLastActive();

        e.Handled = true;
    }

    private void Border_MouseMove_1(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed && this._IsResizing)
        {
            Point canvasRelativePosition = e.GetPosition(this._Canvas);

            double x, y;

            x = canvasRelativePosition.X - this._Position.X;

            y = canvasRelativePosition.Y - this._Position.Y;

            this._ElementViewModel.Width = (int)x;
            this._ElementViewModel.Height = (int)y;

            this.oldMousePosition = canvasRelativePosition;
        }
    }
}

// EOF