﻿using EBoard.Interfaces;
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
    public class ContentViewModel : BaseViewModel, IElementBackgroundImage
    {

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



        public ContentViewModel(ElementDataSet elementDataSet)
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


            ElementHeaderText = elementDataSet.ElementHeader;

            ImagePath = BrushManager.ImagePath;
            
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
                    new Uri(ImagePath, UriKind.Absolute)));
            }
            catch (Exception)
            {
            }


            OnPropertyChanged(nameof(BrushManager));

            OnPropertyChanged(nameof(BrushManager.Background));
        }

    }
}
