using EBoard.InnerComponents;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace EBoard.Views
{
    /// <summary>
    /// Interaktionslogik für ElementView.xaml
    /// </summary>
    public partial class ElementView : UserControl, INotifyPropertyChanged
    {

        private ElementViewModel _ElementViewModel;

        private bool _IsDragging;


        private Point _Position;
        public Point Position => _Position;


        private UIElement _VisualParent;
        public UIElement VisualParent => _VisualParent;


        private Canvas _Canvas;


        public event PropertyChangedEventHandler? PropertyChanged;


        private double _X;
        public double X
        {
            get { return _X; }
            set
            {
                _X = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X)));
            }
        }


        private double _Y;
        public double Y
        {
            get { return _Y; }
            set
            {
                _Y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y)));
            }
        }


        private int _Z;
        public int Z
        {
            get { return _Z; }
            set
            {
                _Z = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Z)));
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

            _ElementViewModel.SetView(Element);
        }


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Z = _ElementViewModel.ZIndexValue;

            _IsDragging = true;

            _Position = e.GetPosition(_VisualParent);

            _ElementViewModel.EBoardViewModel.BeginElementSelectionMovement(_ElementViewModel);

            e.Handled = true;
        }


        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_IsDragging)
                return;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point canvasRelativePosition = e.GetPosition(_Canvas);

                double x, y;

                x = canvasRelativePosition.X - _Position.X ;
                
                y = canvasRelativePosition.Y - _Position.Y;

                Canvas.SetLeft(_VisualParent,x) ;
                Canvas.SetTop(_VisualParent, y);

                Panel.SetZIndex(_VisualParent, 1000);
                _ElementViewModel.ZIndexValue = 1000;

                Point delta = new Point(x, y);

                _ElementViewModel.EBoardViewModel.MoveElementSelection(_ElementViewModel, delta);
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

                _Position = new Point(X,Y);

                Panel.SetZIndex(_VisualParent, Z);

                _ElementViewModel.PlacementManager.Position = new Point(X, Y);

                _ElementViewModel.ZIndexValue = Z;

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

            UpdatePlacement();
        }


        private void Element_Unloaded(object sender, RoutedEventArgs e)
        {

        }


    }
}
// EOF