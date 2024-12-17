using EBoard.Interfaces;
using EBoard.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EBoard.ViewModels
{
    public class ShapeViewModel : BaseViewModel, IElementBackgroundImage
    {
        private BrushManagement _BrushManager;
        public BrushManagement BrushManager
        {
            get { return _BrushManager; }
            set
            {
                _BrushManager = value;
                OnPropertyChanged(nameof(BrushManager));
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


        private string _ElementHeaderText;
        public string ElementHeaderText
        {
            get { return _ElementHeaderText; }
            set
            {
                _ElementHeaderText = value;
                OnPropertyChanged(nameof(ElementHeaderText));
            }
        }


        private double _ElementHeight = 64.0;
        public double ElementHeight
        {
            get { return _ElementHeight; }
            set
            {
                _ElementHeight = value;
                OnPropertyChanged(nameof(ElementHeight));

                UpdateDimensions();
            }
        }


        private double _ElementWidth = 128.0;
        public double ElementWidth
        {
            get { return _ElementWidth; }
            set
            {
                _ElementWidth = value;
                OnPropertyChanged(nameof(ElementWidth));

                UpdateDimensions();
            }
        }


        /// <summary>
        /// the path to an optional background image for the
        /// element, if empty, the stored brush or a default
        /// solidColorBrush will be used for the background
        /// </summary>
        private string _ElementImagePath;
        public string ElementImagePath
        {
            get { return _ElementImagePath; }
            set
            {
                _ElementImagePath = value;
                OnPropertyChanged(nameof(ElementImagePath));

                ChangeElementBackgroundToImage();
            }
        }


        public ElementDataSet ElementDataSet { get; }
        
        private Brush _FallbackBackgroundBrush;
        public Brush FallbackBackgroundBrush => _FallbackBackgroundBrush;

        public IElementContent Control { get; }

        public ShapeViewModel(ElementDataSet elementDataSet)
        {
            ElementDataSet = elementDataSet;
            BrushManager = elementDataSet.BrushManager;
            Control = elementDataSet.ElementContent;

            if (BrushManager == null)
            {
                BrushManager = new BrushManagement();
            }


            ((Shape)Control.Element).Stroke = BrushManager.ElementBorder;
            ((Shape)Control.Element).StrokeThickness = BrushManager.ElementBorderThickness.Left;


            ElementHeaderText = elementDataSet.ElementHeader;

            ElementHeight = elementDataSet.ElementHeight;
            ElementWidth = elementDataSet.ElementWidth;

            ElementImagePath = BrushManager.ElementImagePath;

            if (ElementImagePath == null)
            {
                ElementImagePath = string.Empty;
            }

            if (ElementHeaderText == null)
            {
                ElementHeaderText = "Shape Element";
            }
        }
        public void ChangeElementBackgroundToImage()
        {
            //if (ElementImagePath == null || !File.Exists(ElementImagePath) || ElementImagePath.Equals(string.Empty))
            //{
            //    BrushManager.ElementBackground = new SolidColorBrush(Colors.BlanchedAlmond);

            //    ((Shape)Control.Element).Fill = new SolidColorBrush(Colors.BlanchedAlmond);

            //    return;
            //}

            try
            {
                BrushManager.ElementBackground = new ImageBrush(new BitmapImage(
            new Uri(ElementImagePath, UriKind.Absolute)));

                ((Shape)Control.Element).Fill = BrushManager.ElementBackground;
            }
            catch (Exception)
            {

            }

            OnPropertyChanged(nameof(BrushManager.ElementBackground));
        }


        public void UpdateDimensions()
        {            
            ((Shape)Control.Element).Width = ElementWidth;
            ((Shape)Control.Element).Height = ElementHeight;
        }


    }
}
