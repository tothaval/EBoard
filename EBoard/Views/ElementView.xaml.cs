// <copyright file="ElementView.xaml.cs" company=".">
// Stephan Kammel
// </copyright>

using EBoard.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard.Views;

/// <summary>
/// Interaktionslogik für ElementView.xaml
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
        get { return this._X; }

        set
        {
            this._X = value;
        }
    }

    private double _Y;

    public double Y
    {
        get { return this._Y; }

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
        get { return this._Z; }

        set
        {
            this._Z = value;
        }
    }

    public ElementView()
    {
        this.InitializeComponent();
    }

    private void _ElementViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        this.UpdatePlacement();
    }

    private void UpdatePlacement()
    {
        this.X = Canvas.GetLeft(this._VisualParent);

        this.Y = Canvas.GetTop(this._VisualParent);

        if (Panel.GetZIndex(this._VisualParent) != this._ElementViewModel.ZIndexValue)
        {
            Panel.SetZIndex(this._VisualParent, this._ElementViewModel.ZIndexValue);
        }

        this._ElementViewModel.XMaximumValue = (int)this._Canvas.ActualWidth;
        this._ElementViewModel.YMaximumValue = (int)this._Canvas.ActualHeight;
    }

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!Keyboard.IsKeyDown(Key.LeftCtrl) || !Keyboard.IsKeyDown(Key.RightCtrl))
        {
            this.Z = this._ElementViewModel.ZIndexValue;

            if (this.Z < this._ElementViewModel.ZMaximumValue && this.Z != this.fallbackZ)
            {
                this.fallbackZ = this.Z;
            }

            this._IsDragging = true;
            this._IsResizing = false;

            this._Position = e.GetPosition(this._VisualParent);

            this._ElementViewModel.EBoardViewModel.BeginElementSelectionMovement(this._ElementViewModel);

            this.oldMousePosition = e.GetPosition(this._Canvas);

            e.Handled = true;
        }
    }

    Point oldMousePosition = new Point();

    private void Border_MouseMove(object sender, MouseEventArgs e)
    {
        if (!this._IsDragging)
            return;

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
            this._ElementViewModel.ZIndexValue = 1000;

            Point delta = (Point)(this.oldMousePosition - canvasRelativePosition);

            this._ElementViewModel.EBoardViewModel.MoveElementSelection(this._ElementViewModel, delta);

            this.oldMousePosition = canvasRelativePosition;
        }
    }

    private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        //if (!_IsDragging)
        //    return;

        if (this._IsDragging)
        {
            this._IsDragging = false;

            this.X = Canvas.GetLeft(this._VisualParent);

            this.Y = Canvas.GetTop(this._VisualParent);

            this._Position = new Point(this.X, this.Y);

            this._ElementViewModel.PlacementManager.Position = new Point(this.X, this.Y);

            if (this.Z > this._ElementViewModel.ZMaximumValue)
            {
                this.Z = this.fallbackZ;
                this._ElementViewModel.ZIndexValue = this.fallbackZ;
            }
            else
            {
                this._ElementViewModel.ZIndexValue = this.Z;
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

        this._ElementViewModel.PropertyChanged += this._ElementViewModel_PropertyChanged;

        this._ElementViewModel.SetView(this);

        this.UpdatePlacement();
    }

    private void Element_Unloaded(object sender, RoutedEventArgs e)
    {
    }

    private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            this._ElementViewModel.ApplyRotationAngleValueByMouseWheel(e.Delta);

            return;
        }

        this._ElementViewModel.ApplyZIndexValueByMouseWheel(e.Delta);
    }

    private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
    {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            this._IsResizing = true; this._IsDragging = false;

            this._Position = e.GetPosition(this._VisualParent);

            //_ElementViewModel.EBoardViewModel.BeginElementSelectionMovement(_ElementViewModel);

            this.X = Canvas.GetLeft(this._VisualParent);

            this.Y = Canvas.GetTop(this._VisualParent);

            this.oldMousePosition = e.GetPosition(this._Canvas);

            e.Handled = true;
        }
    }

    private void Border_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
    {
        if (!this._IsResizing)
            return;

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

            this._ElementViewModel.WidthValue = (int)x;
            this._ElementViewModel.HeightValue = (int)y;

            this.oldMousePosition = canvasRelativePosition;
        }
    }
}

// EOF