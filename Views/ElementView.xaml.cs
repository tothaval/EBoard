﻿using EBoard.InnerComponents;
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

        private double _CurrentHeight = 125.0;
        public double CurrentHeight
        {
            get { return _CurrentHeight; }
            set
            {
                _CurrentHeight = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentHeight)));
            }
        }


        private double _CurrentWidth = 125.0;
        public double CurrentWidth
        {
            get { return _CurrentWidth; }
            set
            {
                _CurrentWidth = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentWidth)));
            }
        }


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


        public ElementView()
        {
            InitializeComponent();
        }


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _VisualParent = VisualTreeHelper.GetParent(this) as UIElement;


            _Canvas = VisualTreeHelper.GetParent(_VisualParent) as Canvas;


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

            X = Canvas.GetLeft(_VisualParent);

            Y = Canvas.GetTop(_VisualParent);

            ElementViewModel thisViewmodel = (ElementViewModel)this.DataContext;

            thisViewmodel.X = X;
            thisViewmodel.Y = Y;
            thisViewmodel.Z = CurrentZ;

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

        private void Element_Loaded(object sender, RoutedEventArgs e)
        {
            _VisualParent = VisualTreeHelper.GetParent(this as UIElement) as UIElement;

            if (_VisualParent != null)
            {                
                _Canvas = VisualTreeHelper.GetParent(_VisualParent as UIElement) as Canvas;

                if (_Canvas != null)
                {
                    AdornerLayer.GetAdornerLayer(
                        _Canvas).Add(new ResizeAdorner(Element)); 
                }
            }
        }

        private void Element_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
