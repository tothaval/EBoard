/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ContentViewModel 
 * 
 *  view model for usercontrol content elements
 */
using EBoard.Commands;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Utilities.SharedMethods;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EBoard.ViewModels
{
    public class ContentViewModel : BaseViewModel, IElementBackgroundImage, IUIManager
    {
        private ElementViewModel _ElementViewModel;
        public ElementViewModel ElementViewModel => _ElementViewModel;


        public bool IsSelected { get; set; } = false;

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


        private int _CornerRadius;
        public int CornerRadiusValue
        {
            get { return _CornerRadius; }
            set
            {
                _CornerRadius = value;
                
                BorderManager.CornerRadius = new CornerRadius(value);

                OnPropertyChanged(nameof(BorderManager));
                OnPropertyChanged(nameof(CornerRadiusValue));
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
            }
        }



        public ElementDataSet ElementDataSet { get; }
        public IElementContent Control { get; }


        public ICommand SelectCommand { get; }


        public ContentViewModel(ElementDataSet elementDataSet, ElementViewModel elementViewModel)
        {
            _ElementViewModel = elementViewModel;
            ElementDataSet = elementDataSet;
            BorderManager = new BorderManagement(elementDataSet.BorderDataSet);
            BrushManager = new BrushManagement(elementDataSet.BrushDataSet);
            Control = elementDataSet.ElementContent;

            SelectCommand = new RelayCommand((s) => SelectElement(), (s) => true);

            if (BorderManager == null)
            {
                BorderManager = new BorderManagement();
            }

            if (BrushManager == null)
            {
                BrushManager = new BrushManagement();
            }


            ElementHeaderText = elementDataSet.ElementHeader;

            ImagePath = BrushManager.ImagePath;

            CornerRadiusValue = (int)elementDataSet.BorderDataSet.CornerRadius.TopLeft;

        }



        public void ApplyBackgroundBrush(Brush brush)
        {
            BrushManager.Background = brush;

            OnPropertyChanged(nameof(BrushManager));

            OnPropertyChanged(nameof(BrushManager.Background));
        }


        public void ChangeElementBackgroundToImage()
        {
            if (ImagePath != null && ImagePath != string.Empty)
            {
                BrushManager.Background = new SharedMethod_UI().ChangeBackgroundToImage(BrushManager.Background, ImagePath);

                _ElementViewModel.EBoardViewModel.ChangeSelection_BackgroundBrush(_ElementViewModel, BrushManager.Background); 
            }


            OnPropertyChanged(nameof(BrushManager));

            OnPropertyChanged(nameof(BrushManager.Background));
        }


        public void DeselectElement()
        {
            IsSelected = false;

            BrushManager.SwitchBorderToBorder();

            OnPropertyChanged(nameof(BrushManager));
        }


        public void SelectElement()
        {
            if (IsSelected)
            {
                DeselectElement();
                return;
            }

            IsSelected = true;

            BrushManager.SwitchBorderToHighlight();

            OnPropertyChanged(nameof(BrushManager));
        }

    }
}
// EOF