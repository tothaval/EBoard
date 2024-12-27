/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardViewModel 
 * 
 *  view model class for EBoardView
 *  
 *  it is basically a canvas within a frame and some properties, that can be edited,
 *  stored(WIP) and loaded(WIP)
 */
using EBoard.Commands.ContextMenuCommands;
using EBoard.Commands.ContextMenuCommands.EBoardContextMenu;
using EBoard.Interfaces;
using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EBoard.ViewModels
{
    public class EBoardViewModel : BaseViewModel, IElementBackgroundImage
    {

        // Properties & Fields
        #region Properties & Fields

        private string _EBID;
        /// <summary>
        /// EBoard ID, created upon first creation,
        /// built using $"EBoard_{DateTime().Ticks}"        
        /// </summary>
        public string EBID => _EBID;


        private bool _EBoardActive;
        public bool EBoardActive
        {
            get { return _EBoardActive; }
            set
            {
                _EBoardActive = value;
                OnPropertyChanged(nameof(EBoardActive));
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


        private int _EBoardDepth;
        public int EBoardDepth
        {
            get { return _EBoardDepth; }
            set
            {
                _EBoardDepth = value;
                OnPropertyChanged(nameof(EBoardDepth));
            }
        }    

        private string _EBoardName;
        public string EBoardName
        {
            get { return _EBoardName; }
            set
            {
                _EBoardName = value;
                OnPropertyChanged(nameof(EBoardName));
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
        #endregion


        // Collections
        #region Collections

        private ObservableCollection<ElementViewModel> elements;
        public ObservableCollection<ElementViewModel> Elements
        {
            get { return elements; }
            set
            {
                elements = value;
                OnPropertyChanged(nameof(Elements));
            }
        }

        #endregion



        public ICommand EBoardImageCommand { get; }

        public ICommand SwitchToFirstEBoardCommand { get; }
        public ICommand SwitchToNextEBoardCommand { get; }
        public ICommand SwitchToPrevEBoardCommand { get; }
        public ICommand SwitchToLastEBoardCommand { get; }

        public ICommand DeleteEBoardCommand { get; }

              
               
        public EBoardViewModel(string name, BorderManagement borderManagement, int depth = 0, string eboardID = "-1")
        {

            EBoardImageCommand = new EBoardImageCommand(this);

            SwitchToNextEBoardCommand = new SwitchToNextEBoardCommand(this);
            SwitchToPrevEBoardCommand = new SwitchToPrevEBoardCommand(this);
            SwitchToFirstEBoardCommand = new SwitchToFirstEBoardCommand(this);
            SwitchToLastEBoardCommand = new SwitchToLastEBoardCommand(this);

            DeleteEBoardCommand = new DeleteEBoardCommand(this);

            BorderManager = new BorderManagement();

            BrushManager = new BrushManagement();
            BrushManager.Background = new SolidColorBrush(Colors.CornflowerBlue);

            elements = new ObservableCollection<ElementViewModel>();

            EBoardName = name;
            EBoardDepth = depth;
            BorderManager = borderManagement;

            _EBID = eboardID;

            if (name.Equals(""))
            {
                name = "eboard";
            }

            if (borderManagement == null)
            {
                BorderManager = new BorderManagement();                
                BorderManager.Width = 800;
                BorderManager.Height = 640;
            }
            
            if (depth == 0)
            {
                EBoardDepth = 64;
            }

            if (_EBID == null || (eboardID != null && eboardID.Equals("-1")))
            {

                DateTime dateTime = DateTime.Now;

                _EBID = $"EBoard_{dateTime.Ticks}";
            }
        }

        internal void AddElement(ElementViewModel elementViewModel)
        {
            elements.Add(elementViewModel);
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
// EOF