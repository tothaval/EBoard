/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardViewModel 
 * 
 *  view model class for EBoardView
 *  
 *  it is basically a canvas within a frame and some properties, that can be edited,
 *  stored and loaded
 */
using EBoard.Commands.ContextMenuCommands;
using EBoard.Commands.ContextMenuCommands.EBoardContextMenu;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Utilities.SharedMethods;
using EBoard.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EBoard.ViewModels
{
    public class EBoardViewModel : BaseViewModel, IElementBackgroundImage
    {

        // Properties & Fields
        #region Properties & Fields

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


        private int _EBoardDepth;
        public int EBoardDepth
        {
            get { return _EBoardDepth; }
            set
            {
                _EBoardDepth = value;

                OnPropertyChanged(nameof(EBoardDepth));

                UpdateElementsZIndexProperties(value);
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


        private string _EBID;
        /// <summary>
        /// EBoard ID, created upon first creation,
        /// built using $"EBoard_{DateTime().Ticks}"        
        /// </summary>
        public string EBID => _EBID;


        private int _Height;
        public int Height
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


        /// <summary>
        /// property to set width for eboard instance, somehow it seemed to work better with
        /// a separate value for width besides BorderManager model and there was a problem
        /// with StringFormat in XAML that didn't work as intended, which would have been to
        /// cut all or almost all digits on the value label left to the slider, since the slider
        /// must not set or define every space between integer values, it is intended as a fast
        /// regulation tool. i intend to change the value label with textboxes, to allow the
        /// user to input a detailed value
        /// </summary>
        private int _Width;
        public int Width
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

        #endregion


        // Collections
        #region Collections

        /// <summary>
        /// i tested an approach with a separate collection for a selection, but 
        /// since it got instanced a second time somehow, i removed it. if a list
        /// is necessary or means a lot of simplification in maintainance or development, 
        /// make sure to check if it is only one active instance, maybe via singleton,
        /// maybe via ref keyword. this would have been my next approaches to solve the issue.
        /// </summary>
        private ObservableCollection<ElementViewModel> _Elements;
        public ObservableCollection<ElementViewModel> Elements
        {
            get { return _Elements; }
            set
            {
                _Elements = value;
                OnPropertyChanged(nameof(Elements));
            }
        }

        #endregion


        // Commands
        #region Commands

        public ICommand EBoardImageCommand { get; }


        public ICommand ResetBackgroundCommand { get; set; }


        public ICommand SwitchToFirstEBoardCommand { get; }


        public ICommand SwitchToNextEBoardCommand { get; }


        public ICommand SwitchToPrevEBoardCommand { get; }


        public ICommand SwitchToLastEBoardCommand { get; }

        
        public ICommand DeleteEBoardCommand { get; } 

        #endregion


        // Constructors
        #region Constructors

        public EBoardViewModel(string name, BorderManagement borderManagement, int depth = 0, string eboardID = "-1")
        {

            EBoardImageCommand = new EBoardImageCommand(this);

            ResetBackgroundCommand = new ResetBackgroundCommand(this);

            SwitchToNextEBoardCommand = new SwitchToNextEBoardCommand(this);
            SwitchToPrevEBoardCommand = new SwitchToPrevEBoardCommand(this);
            SwitchToFirstEBoardCommand = new SwitchToFirstEBoardCommand(this);
            SwitchToLastEBoardCommand = new SwitchToLastEBoardCommand(this);

            DeleteEBoardCommand = new DeleteEBoardCommand(this);

            BorderManager = new BorderManagement();

            BrushManager = new BrushManagement();
            BrushManager.Background = new SolidColorBrush(Colors.CornflowerBlue);

            _Elements = new ObservableCollection<ElementViewModel>();

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

            CornerRadiusValue = (int)BorderManager.CornerRadius.TopLeft;
            Height = (int)BorderManager.Height;
            Width = (int)BorderManager.Width;


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
        #endregion


        // Methods
        #region Methods

        public int GetContainerCount()
        {
            int containerCount = 0;

            foreach (ElementViewModel item in Elements)
            {
                if (item.IsContent)
                {
                    containerCount++;
                }
            }
       

            return containerCount;
        }
        public int GetElementCount()
        {
            return Elements.Count;
        }
        public int GetShapeCount()
        {
            int containerCount = 0;

            foreach (ElementViewModel item in Elements)
            {
                if (item.IsShape)
                {
                    containerCount++;
                }
            }

            return containerCount;
        }


        public DateTime GetCreatedDate()
        {
            string cutEBID = _EBID.Replace("EBoard_", "");

            long ticks = long.Parse(cutEBID);

            DateTime dateTime = new DateTime(ticks);

            return dateTime;
        }




        internal void AddElement(ElementViewModel elementViewModel)
        {
            if (!Elements.Contains(elementViewModel))
            {
                Elements.Add(elementViewModel);
            }

            OnPropertyChanged(nameof(Elements));
        }


        internal void BeginElementSelectionMovement(ElementViewModel elementViewModel)
        {
            foreach (ElementViewModel item in Elements)
            {
                if (item.EID.Equals(elementViewModel.EID))
                {
                    continue;
                }

                item.BeginMovement(elementViewModel);
            }
        }


        public void ChangeElementBackgroundToImage()
        {
            BrushManager.Background = new SharedMethod_UI().ChangeBackgroundToImage(BrushManager.Background, ImagePath);

            OnPropertyChanged(nameof(BrushManager));

            OnPropertyChanged(nameof(BrushManager.Background));
        }


        internal void ChangeSelection_CornerRadius(ElementViewModel elementViewModel, int cornerRadius)
        {
            foreach (ElementViewModel item in Elements)
            {
                if (item.Equals(elementViewModel))
                {
                    continue;
                }

                if (item.ContentContainer.IsSelected)
                {
                    item.Apply_CornerRadiusValue(cornerRadius);
                }
            }
        }


        internal void ChangeSelection_BackgroundBrush(ElementViewModel elementViewModel, Brush brush)
        {
            foreach (ElementViewModel item in Elements)
            {
                if (item.Equals(elementViewModel))
                {
                    continue;
                }

                if (item.ContentContainer.IsSelected)
                {
                    item.ApplyBackgroundBrush(brush);
                }
            }
        }


        internal void ChangeSelection_Height(ElementViewModel elementViewModel, int heightValue)
        {
            foreach (ElementViewModel item in Elements)
            {
                if (item.Equals(elementViewModel))
                {
                    continue;
                }

                if (item.ContentContainer.IsSelected)
                {
                    item.Apply_HeightValue(heightValue);
                }
            }
        }

        internal void ChangeSelection_RotationAngle(ElementViewModel elementViewModel, int rotationAngleValue)
        {
            foreach (ElementViewModel item in Elements)
            {
                if (item.Equals(elementViewModel))
                {
                    continue;
                }

                if (item.ContentContainer.IsSelected)
                {
                    item.ApplyRotationAngleValue(rotationAngleValue);
                }
            }
        }
        internal void ChangeSelection_WidthValue(ElementViewModel elementViewModel, int widthValue)
        {
            foreach (ElementViewModel item in Elements)
            {
                if (item.Equals(elementViewModel))
                {
                    continue;
                }

                if (item.ContentContainer.IsSelected)
                {
                    item.ApplyWidthValue(widthValue);
                }
            }
        }
        internal void ChangeSelection_ZIndex(ElementViewModel elementViewModel, int zIndexValue)
        {
            foreach (ElementViewModel item in Elements)
            {
                if (item.Equals(elementViewModel))
                {
                    continue;
                }

                if (item.ContentContainer.IsSelected)
                {
                    item.ApplyZIndexValue(zIndexValue);
                }
            }
        }


        internal void DeselectElements()
        {
            foreach (ElementViewModel item in Elements)
            {
                if (item.ContentContainer.IsSelected)
                {
                    item.ContentContainer.DeselectElement();
                }
            }
        }


        public void MoveElementSelection(ElementViewModel elementViewModel, Point newPosition)
        {
            foreach (ElementViewModel item in Elements)
            {
                if (item.EID.Equals(elementViewModel.EID))
                {
                    continue;
                }


                if (item.ContentContainer.IsSelected)
                {
                    item.MoveXY(elementViewModel, newPosition); 
                }
            }

            OnPropertyChanged(nameof(Elements));

        }


        internal void MoveLastClickedElement(ElementViewModel elementViewModel)
        {
            Elements.Move(Elements.IndexOf(elementViewModel), Elements.Count - 1);
        }


        internal void RemoveElement(ElementViewModel elementViewModel)
        {
            Elements.Remove(elementViewModel);

            OnPropertyChanged(nameof(Elements));

            List<ElementViewModel> selectedElements = new List<ElementViewModel>();

            foreach (ElementViewModel item in Elements)
            {
                if (item.ContentContainer.IsSelected)
                {
                    selectedElements.Add(item);
                }
            }

            foreach (ElementViewModel item in selectedElements)
            {
                Elements.Remove(item);
            }


            OnPropertyChanged(nameof(Elements));
        }


        internal void StopElementSelectionMovement(ElementViewModel elementViewModel)
        {
            foreach (ElementViewModel item in Elements)
            {
                if (item.EID.Equals(elementViewModel.EID))
                {
                    continue;
                }


                if (item.ContentContainer.IsSelected)
                {
                    item.StopMovement(); 
                }

            }
            OnPropertyChanged(nameof(Elements));
        }


        private void UpdateElementsZIndexProperties(int newEBoardDepth)
        {
            if (Elements != null && Elements.Count > 0)
            {
                foreach (ElementViewModel item in Elements)
                {
                    item.CalibrateZSliderValues(newEBoardDepth);
                }
            }
        } 
        #endregion


    }
}
// EOF