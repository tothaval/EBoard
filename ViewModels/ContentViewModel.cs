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

namespace EBoard.ViewModels
{
    public class ContentViewModel : BaseViewModel
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
        public IElementContent Control { get; }



        public ContentViewModel(ElementDataSet elementDataSet)
        {
            ElementDataSet = elementDataSet;
            BrushManager = elementDataSet.BrushManager;
            Control = elementDataSet.ElementContent;

            if (BrushManager == null)
            {
                BrushManager = new BrushManagement();
            }

            ElementHeaderText = elementDataSet.ElementHeader;

            ElementHeight = elementDataSet.ElementHeight;
            ElementWidth = elementDataSet.ElementWidth;

            ElementImagePath = BrushManager.ImagePath;
        }


        public void ChangeElementBackgroundToImage()
        {
            //if (_ElementImagePath == null || !File.Exists(_ElementImagePath) || _ElementImagePath.Equals(string.Empty))
            //{                
            //    BrushManager.ElementBackground = new SolidColorBrush(Colors.BlanchedAlmond);

            //    return;
            //}

            try
            {

                BrushManager.Background = new ImageBrush(new BitmapImage(
                    new Uri(_ElementImagePath, UriKind.Absolute)));
            }
            catch (Exception)
            {
            }


            OnPropertyChanged(nameof(BrushManager));

            OnPropertyChanged(nameof(BrushManager.Background));
        }

    }
}
