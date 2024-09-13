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

namespace EBoard.Views
{
    /// <summary>
    /// Interaktionslogik für ElementView.xaml
    /// </summary>
    public partial class ElementView : UserControl, INotifyPropertyChanged
    {

        private int _CurrentZ;
        public int CurrentZ
        {
            get { return _CurrentZ; }
            set
            {
                _CurrentZ = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentZ)));
            }
        }


        private bool _IsDragging;

        private Point _Position;

        private UIElement _VisualParent;

        private UIElement _Canvas;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ElementView()
        {
            InitializeComponent();
        }


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _VisualParent = VisualTreeHelper.GetParent(this as UIElement) as UIElement;

            _Canvas = VisualTreeHelper.GetParent(_VisualParent as UIElement) as Canvas;

            CurrentZ = Panel.GetZIndex(_VisualParent);

            _IsDragging = true;


            Panel.SetZIndex(_VisualParent, 1000);

            _Position = e.GetPosition(_VisualParent);

            e.Handled = true;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_IsDragging)
                return;

            _IsDragging = false;

            Panel.SetZIndex(_VisualParent, 0);

            CurrentZ = Panel.GetZIndex(_VisualParent);

            e.Handled = true;
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_IsDragging)
                return;
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point canvasRelativePosition = e.GetPosition(_Canvas);

                Canvas.SetTop(_VisualParent, canvasRelativePosition.Y - _Position.Y);
                Canvas.SetLeft(_VisualParent, canvasRelativePosition.X - _Position.X);

                CurrentZ = Panel.GetZIndex(_VisualParent);
            }

        }

    }
}
