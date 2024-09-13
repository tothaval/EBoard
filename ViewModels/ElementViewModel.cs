using EBoard.Commands;
using EBoard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard.ViewModels
{
    internal class ElementViewModel : BaseViewModel
    {

        private EBoardViewModel _EBoardViewModel;


        private Brush _elementBackgroundBrush;
        public Brush ElementBackgroundBrush
        {
            get { return _elementBackgroundBrush; }
            set
            {
                _elementBackgroundBrush = value;
                OnPropertyChanged(nameof(ElementBackgroundBrush));
            }
        }


        private Brush _ElementBorderBrush;
        public Brush ElementBorderBrush
        {
            get { return _ElementBorderBrush; }
            set
            {
                _ElementBorderBrush = value;
                OnPropertyChanged(nameof(ElementBorderBrush));
            }
        }


        private Thickness _ElementBorderThickness;
        public Thickness ElementBorderThickness
        {
            get { return _ElementBorderThickness; }
            set
            {
                _ElementBorderThickness = value;
                OnPropertyChanged(nameof(ElementBorderThickness));
            }
        }


        private FrameworkElement _elementContent;
        public FrameworkElement ElementContent
        {
            get { return _elementContent; }
            set
            {
                _elementContent = value;
                OnPropertyChanged(nameof(ElementContent));
            }
        }


        private string _elementHeaderText;
        public string ElementHeaderText
        {
            get { return _elementHeaderText; }
            set
            {
                _elementHeaderText = value;
                OnPropertyChanged(nameof(ElementHeaderText));
            }
        }

        public double X { get; set; }


        public double Y { get; set; }


        public double Z { get; set; }


        #region Commands

        public ICommand LeftClickCommand { get; }


        public ICommand RightClickCommand {get;}

        #endregion


        public ElementViewModel(EBoardViewModel eBoardViewModel)
        {
            LeftClickCommand = new RelayCommand((s) => DragMove(s), (s) => true);

            RightClickCommand = new RelayCommand((s) => RemoveElement(s), (s) => true);

            ConfigurateElementProperties();
            _EBoardViewModel = eBoardViewModel;
        }


        public ElementViewModel(EBoardViewModel eBoardViewModel, double x, double y, double z, string text, Brush brush, FrameworkElement control)
        {
            X = x; Y = y; Z = z;

            ElementHeaderText = text;

            ElementBackgroundBrush = brush;

            ElementContent = control;

            LeftClickCommand = new RelayCommand((s) => DragMove(s), (s) => true);

            RightClickCommand = new RelayCommand((s) => RemoveElement(s), (s) => true);

            ConfigurateElementProperties(brush);
            _EBoardViewModel = eBoardViewModel;
        }


        // Methods
        #region Methods

        private void ConfigurateElementProperties(Brush brush = null)
        {
            if (brush == null)
            {
                ElementBackgroundBrush = new SolidColorBrush(Colors.AntiqueWhite);
            }
            else
            {
                ElementBackgroundBrush = brush;
            }            
                        

            ElementBorderThickness = new Thickness(1,2,1,1);

            ElementBorderBrush = new SolidColorBrush(Colors.Blue);


            OnPropertyChanged(nameof(ElementHeaderText));
            OnPropertyChanged(nameof(ElementContent));
        }

        private void DragMove(object s)
        {
            /// use visualtreehelper to find canvas upwards from element
            /// 

            UIElement uIElement = VisualTreeHelper.GetParent(s as UIElement) as UIElement;

            Canvas canvas = VisualTreeHelper.GetParent(uIElement as UIElement) as Canvas;


            DragDrop.DoDragDrop(canvas, new DataObject(DataFormats.Serializable, uIElement), DragDropEffects.Move); // ???


            //Canvas.SetLeft(uIElement, Mouse.GetPosition(canvas).X);
            //Canvas.SetTop(uIElement, Mouse.GetPosition(canvas).Y);

            //Panel.SetZIndex(uIElement, (int)Z);

            //Point pos = Mouse.GetPosition(canvas);
        }


        private void RemoveElement(object s)
        {
            /// use visualtreehelper to find canvas upwards from element
            //UIElement uIElement = VisualTreeHelper.GetParent(s as UIElement) as UIElement;

            //Canvas canvas = VisualTreeHelper.GetParent(uIElement as UIElement) as Canvas;
                 
            _EBoardViewModel.Elements.Remove(this);
        }

        #endregion

    }
}
// EOF