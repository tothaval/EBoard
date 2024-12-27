/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ElementViewModel 
 * 
 *  view model for ElementView, which offers some basic dragmove functionality,
 *  element placement properties within EBoardView canvas and basic content management
 */

using EBoard.Commands;
using EBoard.Commands.ContextMenuCommands;
using EBoard.Models;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EBoard.ViewModels
{
    public class ElementViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields

        private EBoardViewModel _EBoardViewModel;

        private ContentViewModel _ContentViewModel;
        public ContentViewModel ContentViewModel => _ContentViewModel;

        private ShapeViewModel _ShapeViewModel;
        public ShapeViewModel ShapeViewModel => _ShapeViewModel;


        private PlacementManagement _PlacementManagement;
        public PlacementManagement PlacementManager
        {
            get { return _PlacementManagement; }
            set
            {
                _PlacementManagement = value;
                OnPropertyChanged(nameof(PlacementManager));
            }
        }

        private string _EID;
        /// <summary>
        /// Element ID, created upon first creation,
        /// built using $"Element_{DateTime().Ticks}"        
        /// </summary>
        public string EID => _EID;


        private bool _IsShape = false;
        public bool IsShape
        {
            get { return _IsShape; }
            set
            {
                _IsShape = value;
                OnPropertyChanged(nameof(IsShape));
            }
        }


        private bool _IsContent = false;
        public bool IsContent
        {
            get { return _IsContent; }
            set
            {
                _IsContent = value;
                OnPropertyChanged(nameof(IsContent));
            }
        }


        private int _ZIndexValue = 0;
        public int ZIndexValue
        {
            get { return _ZIndexValue; }
            set
            {
                _ZIndexValue = value;

                PlacementManager.Z = value;

                OnPropertyChanged(nameof(PlacementManager.Z));
                OnPropertyChanged(nameof(ZIndexValue));
            }
        }


        private int _ZMinimumValue;
        public int ZMinimumValue
        {
            get { return _ZMinimumValue; }
            set
            {
                _ZMinimumValue = value;
                OnPropertyChanged(nameof(ZMinimumValue));
            }
        }


        private int _ZMaximumValue;
        public int ZMaximumValue
        {
            get { return _ZMaximumValue; }
            set
            {
                _ZMaximumValue = value;
                OnPropertyChanged(nameof(ZMaximumValue));
            }
        }


        #endregion


        #region Commands

        //public ICommand LeftClickCommand { get; }

        public ICommand ImageCommand { get; }


        public ICommand ResetBackgroundCommand { get; set; }


        public ICommand RightClickCommand { get; }

        #endregion


        public ElementViewModel(EBoardViewModel eBoardViewModel)
        {
            //LeftClickCommand = new RelayCommand((s) => DragMove(s), (s) => true);

            ImageCommand = new ImageCommand(this);

            RightClickCommand = new RelayCommand((s) => RemoveElement(s), (s) => true);

            _EBoardViewModel = eBoardViewModel;

            PlacementManager = new PlacementManagement();

            ZIndexValue = PlacementManager.Z;

            CalibrateZSliderValues(_EBoardViewModel.EBoardDepth);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="eBoardViewModel"></param>
        /// <param name="x">x axis</param>
        /// <param name="y">y axis</param>
        /// <param name="z">depth</param>
        /// <param name="elementHeaderText">element header</param>
        /// <param name="brush">background brush</param>
        /// <param name="control">element content</param>
        public ElementViewModel(
            EBoardViewModel eBoardViewModel,
            ElementDataSet elementDataSet
            )
        {

            //LeftClickCommand = new RelayCommand((s) => DragMove(s), (s) => true);
            ImageCommand = new ImageCommand(this);

            RightClickCommand = new RelayCommand((s) => RemoveElement(s), (s) => true);

            PlacementManager = new PlacementManagement();


            _EBoardViewModel = eBoardViewModel;

            _EID = elementDataSet.EID;


            if (elementDataSet.PlacementDataSet != null)
            {
                PlacementManager.Position = elementDataSet.PlacementDataSet.Position;
                PlacementManager.Z = elementDataSet.PlacementDataSet.Z;

                ZIndexValue = PlacementManager.Z;
            }

            if (_EID == null || _EID.Equals("-1"))
            {
                DateTime dateTime = DateTime.Now;

                _EID = $"Element_{dateTime.Ticks}";
            }


            if (elementDataSet.ElementContent != null)
            {
                if (elementDataSet.ElementContent.ContentIsUserControlAndNotShape)
                {
                    _ContentViewModel = new ContentViewModel(elementDataSet);
                    IsContent = true;

                    ResetBackgroundCommand = new ResetBackgroundCommand(_ContentViewModel);

                    OnPropertyChanged(nameof(ContentViewModel));
                }
                else
                {
                    _ShapeViewModel = new ShapeViewModel(elementDataSet);
                    IsShape = true;

                    ResetBackgroundCommand = new ResetBackgroundCommand(_ShapeViewModel);

                    OnPropertyChanged(nameof(ShapeViewModel));
                }


                CalibrateZSliderValues(_EBoardViewModel.EBoardDepth);
            }
        }

        // Methods
        #region Methods

        public void CalibrateZSliderValues(int eboardDepth)
        {
            if (eboardDepth >= 0)
            {
                ZMinimumValue = 0;
                ZMaximumValue = eboardDepth;

                if (eboardDepth == 0)
                {
                    ZMaximumValue = 1;
                }
            }
            else if (eboardDepth < 0)
            {
                ZMinimumValue = eboardDepth;
                ZMaximumValue = 0;
            }

            OnPropertyChanged(nameof(ZMinimumValue));
            OnPropertyChanged(nameof(ZMaximumValue));
        }


        private void RemoveElement(object s)
        {
            _EBoardViewModel.Elements.Remove(this);
        }

        internal void WasLastActive()
        {
            _EBoardViewModel.MoveLastClickedElement(this);
        }

        #endregion

    }
}
// EOF