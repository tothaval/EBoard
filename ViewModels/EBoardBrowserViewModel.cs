/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardBrowserViewModel 
 * 
 *  helper class for
 */
using EBoard.Commands;
using EBoard.Models;
using EBoard.Navigation;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard.ViewModels
{
    public class EBoardBrowserViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields



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



        public int EBoardCount { get; set; }



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


        private double _EBoardHeight;
        public double EBoardHeight
        {
            get { return _EBoardHeight; }
            set
            {
                _EBoardHeight = value;
                OnPropertyChanged(nameof(EBoardHeight));
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


        private double _EBoardWidth;
        public double EBoardWidth
        {
            get { return _EBoardWidth; }
            set
            {
                _EBoardWidth = value;
                OnPropertyChanged(nameof(EBoardWidth));
            }
        }


        private NavigationStore _NavigationStore;
        //public NavigationStore NavigationStore
        //{
        //    get { return _NavigationStore; }
        //    set
        //    {
        //        _NavigationStore = value;
        //        OnPropertyChanged(nameof(NavigationStore));
        //    }
        //}


        private EBoardViewModel _SelectedEBoard;
        public EBoardViewModel SelectedEBoard
        {
            get { return _SelectedEBoard; }
            set
            {
                if (_SelectedEBoard != null)
                {
                    _SelectedEBoard.EBoardActive = false;
                }                

                _SelectedEBoard = value;

                RefreshEBoardParameters();

                OnPropertyChanged(nameof(SelectedEBoard));
            }
        }

        #endregion


        // Collections
        #region Collections
        private ObservableCollection<EBoardViewModel> _eboards;
        public ObservableCollection<EBoardViewModel> EBoards
        {
            get { return _eboards; }
            set { _eboards = value; }
        }
        #endregion


        // Commands
        #region Commands

        public ICommand AddEBoardCommand { get; }


        public ICommand DeleteEBoardCommand { get; }


        public ICommand EditEBoardParametersCommand { get; set; }


        public ICommand ShowEBoardCommand { get; }

        #endregion


        // Constructors
        #region Constructors

        public EBoardBrowserViewModel()
        {

        }


        public EBoardBrowserViewModel(NavigationStore navigationStore)
        {
            AddEBoardCommand = new RelayCommand((s) => AddEBoard() ,(s) => true);

            DeleteEBoardCommand = new RelayCommand((s) => DeleteSelectedEBoard(s), (s) => true);

            _NavigationStore = navigationStore;
            
            EditEBoardParametersCommand = new EditEBoardParametersCommand(this);

            EBoards = new ObservableCollection<EBoardViewModel>();

            if (EBoards.Count > 0)
            {
                navigationStore.CurrentViewModel = EBoards[0];
            }            
        }

        #endregion


        // Methods
        #region Methods

        private void AddEBoard()
        {
            if (EBoardName == null)
            {
                EBoardName = "";
            }

            EBoards.Add(new EBoardViewModel(EBoardName, EBoardWidth, EBoardHeight, EBoardDepth));

            SelectedEBoard = EBoards.Last();

        }


        public Task InstantiateEBoardDataSet(EboardDataSet eboardDataSet)
        {
            if (eboardDataSet != null)
            {
                EBoardViewModel eBoardViewModel = new EBoardViewModel(
                    eboardDataSet.EBoardName,
                    eboardDataSet.EBoardWidth,
                    eboardDataSet.EBoardHeight,
                    eboardDataSet.EBoardDepth,
                    eboardDataSet.EBID);

                eBoardViewModel.Elements = eboardDataSet.EBoardViewModel.Elements;
                
                eBoardViewModel.BrushManager = new BrushManagement(eboardDataSet.EBoardBrushManager);

                EBoards.Add(eBoardViewModel);                
            }

            return Task.CompletedTask;
        }


        private void DeleteSelectedEBoard(object s)
        {
            if (SelectedEBoard != null && EBoards.Count > 0)
            {
                EBoards.Remove(SelectedEBoard);

                if (EBoards.Count > 0)
                {
                    SelectedEBoard = EBoards.First();
                }
                else
                {
                    _NavigationStore.CurrentViewModel = null;
                }
            }
        }


        private void RefreshEBoardParameters()
        {
            if (SelectedEBoard != null)
            {
                _NavigationStore.CurrentViewModel = SelectedEBoard;

                EBoardName = SelectedEBoard.EBoardName;
                EBoardDepth = SelectedEBoard.EBoardDepth;
                EBoardHeight = SelectedEBoard.EBoardHeight;
                EBoardWidth = SelectedEBoard.EBoardWidth;
                SelectedEBoard.EBoardActive = true;
            }
        }


        public void UpdateBrushManager(BrushManagement brushManagement)
        {
            BrushManager = brushManagement;

        }

        #endregion

    }
}
// EOF