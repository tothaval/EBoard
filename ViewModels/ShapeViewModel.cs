/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ShapeViewModel 
 * 
 *  viewmodel for shape content elements
 */
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Utilities.SharedMethods;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EBoard.ViewModels
{
    public class ShapeViewModel : BaseViewModel, IElementBackgroundImage
    {
        // mit properties ersetzen um die größe zu ändern,
        // die änderung in BorderManagement reinschreiben

        private BorderManagement _BorderManager;
        public BorderManagement BorderManager
        {
            get { return _BorderManager; }
            set
            {
                _BorderManager = value;
                OnPropertyChanged(nameof(BorderManager));
            }
        }


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

        /// <summary>
        /// the path to an optional background image for the
        /// element, if empty, the stored brush or a default
        /// solidColorBrush will be used for the background
        /// </summary>
        private string _ImagePath;
        public string ImagePath
        {
            get { return _ImagePath; }
            set
            {
                _ImagePath = value;

                OnPropertyChanged(nameof(ImagePath));
                
                ChangeElementBackgroundToImage(); 
                
            }
        }


        private double _Height;
        public double Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                BorderManager.Height = value;

                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(BorderManager));

                UpdateDimensions();
            }
        }


        private double _Width;
        public double Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
                BorderManager.Width = value;
                OnPropertyChanged(nameof(Width));
                OnPropertyChanged(nameof(BorderManager));

                UpdateDimensions();
            }
        }


        public ElementDataSet ElementDataSet { get; }


        private Brush _FallbackBackgroundBrush;
        public Brush FallbackBackgroundBrush => _FallbackBackgroundBrush;


        public IElementContent Control { get; }


        public ShapeViewModel(ElementDataSet elementDataSet)
        {
            ElementDataSet = elementDataSet;
            BorderManager = new BorderManagement(elementDataSet.BorderDataSet);
            BrushManager = new BrushManagement(elementDataSet.BrushDataSet);
            Control = elementDataSet.ElementContent;


            if (BorderManager == null)
            {
                BorderManager = new BorderManagement();
            }

            if (BrushManager == null)
            {
                BrushManager = new BrushManagement();
            }
            
            ((Shape)Control.Element).Fill = BrushManager.Background;
            ((Shape)Control.Element).Stroke = BrushManager.Border;
            ((Shape)Control.Element).StrokeThickness = BorderManager.BorderThickness.Left;

            ElementHeaderText = elementDataSet.ElementHeader;

            ImagePath = BrushManager.ImagePath;

            if (ImagePath == null)
            {
                ImagePath = string.Empty;
            }

            if (ElementHeaderText == null)
            {
                ElementHeaderText = "Shape Element";
            }
        }


        public void ChangeElementBackgroundToImage()
        {
            BrushManager.Background = new SharedMethod_UI().ChangeBackgroundToImage(BrushManager.Background, ImagePath);

            ((Shape)Control.Element).Fill = BrushManager.Background;
            ((Shape)Control.Element).Stroke = BrushManager.Border;
            ((Shape)Control.Element).StrokeThickness = BorderManager.BorderThickness.Left;

            OnPropertyChanged(nameof(BrushManager));

            OnPropertyChanged(nameof(BrushManager.Background));
        }


        public void UpdateDimensions()
        {
            if (BorderManager != null && Control != null)
            {
                if (Control.Element != null)
                {
                    ((Shape)Control.Element).Width = BorderManager.Width;
                    ((Shape)Control.Element).Height = BorderManager.Height;  
                }
            }
        }


    }
}
// EOF