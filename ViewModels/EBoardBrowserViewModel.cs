using EBoard.Commands;
using EBoard.Navigation;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard.ViewModels
{
    internal class EBoardBrowserViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields

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
        public NavigationStore NavigationStore
        {
            get { return _NavigationStore; }
            set
            {
                _NavigationStore = value;
                OnPropertyChanged(nameof(NavigationStore));
            }
        }


        private EBoardViewModel _SelectedEBoard;
        public EBoardViewModel SelectedEBoard
        {
            get { return _SelectedEBoard; }
            set
            {
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
        }


        private void DeleteSelectedEBoard(object s)
        {
            if (SelectedEBoard != null && EBoards.Count > 0)
            {
                EBoards.Remove(SelectedEBoard);
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
            }
        }

        #endregion

    }
}
