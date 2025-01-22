
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
    public Point Position => _Position;


    private UIElement _VisualParent;
    public UIElement VisualParent => _VisualParent;


    private Canvas _Canvas;


    private double _X;
    public double X
    {
        get { return _X; }
        set
        {
            _X = value;
        }
    }


    private double _Y;
    public double Y
    {
        get { return _Y; }
        set
        {
            _Y = value;
        }
    }


    private int fallbackZ = 0;

    private int _Z;
    public int Z
    {
        get { return _Z; }
        set
        {
            _Z = value;
        }
    }


    public ElementView()
    {
        InitializeComponent();

    }


    private void _ElementViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        UpdatePlacement();
    }



    private void UpdatePlacement()
    {
        X = Canvas.GetLeft(_VisualParent);
        
        Y = Canvas.GetTop(_VisualParent);
        
        Panel.SetZIndex(_VisualParent, _ElementViewModel.ZIndexValue);
    }


    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Z = _ElementViewModel.ZIndexValue;

        if (Z < _ElementViewModel.ZMaximumValue && Z != fallbackZ)
        {
            fallbackZ = Z;
        }

        _IsDragging = true;

        _Position = e.GetPosition(_VisualParent);

        _ElementViewModel.EBoardViewModel.BeginElementSelectionMovement(_ElementViewModel);

        oldMousePosition = e.GetPosition(_Canvas);

        e.Handled = true;
    }

    Point oldMousePosition = new Point();

    private void Border_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_IsDragging)
            return;

        if (e.LeftButton == MouseButtonState.Pressed)
        {
            Point canvasRelativePosition = e.GetPosition(_Canvas);

            double x, y;

            x = canvasRelativePosition.X - _Position.X;

            y = canvasRelativePosition.Y - _Position.Y;

            Canvas.SetLeft(_VisualParent, x);
            Canvas.SetTop(_VisualParent, y);

            Panel.SetZIndex(_VisualParent, 1000);
            _ElementViewModel.ZIndexValue = 1000;

            Point delta = (Point)(oldMousePosition - canvasRelativePosition);

            _ElementViewModel.EBoardViewModel.MoveElementSelection(_ElementViewModel, delta);

            oldMousePosition = canvasRelativePosition;
        }
    }


    private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        //if (!_IsDragging)
        //    return;

        if (_IsDragging)
        {
            _IsDragging = false;

            X = Canvas.GetLeft(_VisualParent);

            Y = Canvas.GetTop(_VisualParent);

            _Position = new Point(X, Y);

            _ElementViewModel.PlacementManager.Position = new Point(X, Y);

            if (Z > _ElementViewModel.ZMaximumValue)
            {
                Z = fallbackZ;
                _ElementViewModel.ZIndexValue = fallbackZ;
            }
            else
            {
                _ElementViewModel.ZIndexValue = Z;
            }

            Panel.SetZIndex(_VisualParent, Z);

            _ElementViewModel.EBoardViewModel.StopElementSelectionMovement(_ElementViewModel);
        }

        _ElementViewModel.WasLastActive();

        e.Handled = true;
    }





    private void Element_Loaded(object sender, RoutedEventArgs e)
    {
        _VisualParent = VisualTreeHelper.GetParent(this) as UIElement;

        if (_VisualParent != null)
        {
            _Canvas = VisualTreeHelper.GetParent(_VisualParent) as Canvas;

            //if (_Canvas != null)
            //{
            //    AdornerLayer.GetAdornerLayer(
            //        _Canvas).Add(new ResizeAdorner(Element));
            //}
        }

        _ElementViewModel = (ElementViewModel)DataContext;

        _ElementViewModel.PropertyChanged += _ElementViewModel_PropertyChanged;

        _ElementViewModel.SetView(Element);

        UpdatePlacement();
    }


    private void Element_Unloaded(object sender, RoutedEventArgs e)
    {

    }

    private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            _ElementViewModel.ApplyRotationAngleValueByMouseWheel(e.Delta);

            return;
        }

        _ElementViewModel.ApplyZIndexValueByMouseWheel(e.Delta);
    }


}
// EOF