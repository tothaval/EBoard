/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardBrowserViewModel 
 * 
 *  view model class for EBoardBrowserView
 *  
 *  EBoardBrowserView presents the EBoardViewModels currently existent within the application,
 *  as well as functionality to create, edit or delete eboard instances.
 */
using EBoard.Commands;
using EBoard.Commands.ContextMenuCommands;
using EBoard.Commands.ContextMenuCommands.EBoardBrowserContextMenu;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Navigation;
using EBoard.Utilities.SharedMethods;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EBoard.ViewModels
{
    public class EBoardBrowserViewModel : BaseViewModel, IElementBackgroundImage
    {

        // Properties & Fields
        #region Properties & Fields
        
        private int _CurrentSelectionID;
        public int CurrentSelectionID
        {
            get { return _CurrentSelectionID; }
            set
            {
                _CurrentSelectionID = value;
                OnPropertyChanged(nameof(CurrentSelectionID));
            }
        }


        private DateTime _EBoardCreatedDate;
        public DateTime EBoardCreatedDate
        {
            get { return _EBoardCreatedDate; }
            set
            {
                _EBoardCreatedDate = value;
                OnPropertyChanged(nameof(EBoardCreatedDate));
            }
        }


        private int _EBoardContainerCount;
        public int EBoardContainerCount
        {
            get { return _EBoardContainerCount; }
            set
            {
                _EBoardContainerCount = value;
                OnPropertyChanged(nameof(EBoardContainerCount));
            }
        }


        private int _EBoardElementCount;
        public int EBoardElementCount
        {
            get { return _EBoardElementCount; }
            set
            {
                _EBoardElementCount = value;
                OnPropertyChanged(nameof(EBoardElementCount));
            }
        }


        private int _EBoardShapeCount;
        public int EBoardShapeCount
        {
            get { return _EBoardShapeCount; }
            set
            {
                _EBoardShapeCount = value;
                OnPropertyChanged(nameof(EBoardShapeCount));
            }
        }


        private double _NewEBoardHeight = 480;
        public double NewEBoardHeight
        {
            get { return _NewEBoardHeight; }
            set
            {
                _NewEBoardHeight = value;
                OnPropertyChanged(nameof(NewEBoardHeight));
            }
        }


        private double _NewEBoardWidth = 720;
        public double NewEBoardWidth
        {
            get { return _NewEBoardWidth; }
            set
            {
                _NewEBoardWidth = value;
                OnPropertyChanged(nameof(NewEBoardWidth));
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


        private int _EBoardCount;
        public int EBoardCount
        {
            get { return _EBoardCount; }
            set
            {
                _EBoardCount = value;
                OnPropertyChanged(nameof(EBoardCount));
            }
        }


        private int _EBoardDepth = 100;
        public int EBoardDepth
        {
            get { return _EBoardDepth; }
            set
            {
                _EBoardDepth = value;
                OnPropertyChanged(nameof(EBoardDepth));
            }
        }


        private string _EBoardName  = "new";
        public string EBoardName
        {
            get { return _EBoardName; }
            set
            {
                _EBoardName = value;
                OnPropertyChanged(nameof(EBoardName));
            }
        }


        private string _ImagePath = string.Empty;
        public string ImagePath
        {
            get { return _ImagePath; }
            set
            {
                _ImagePath = value;
                OnPropertyChanged(nameof(value));

                ChangeElementBackgroundToImage();
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
                    _SelectedEBoard.EBoardActive = false; // changing the active value of the old one, should do this in a different way
                }

                _SelectedEBoard = value;

                if (_SelectedEBoard != null)
                {
                    _SelectedEBoard.Elements.CollectionChanged += SelectedEBoardElements_CollectionChanged;
                }

                RefreshEBoardParameters();

                OnPropertyChanged(nameof(SelectedEBoard));
            }
        }

        private void SelectedEBoardElements_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RefreshEBoardParameters();
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


        public ICommand EBoardBrowserImageCommand { get; set; }


        public ICommand EditEBoardParametersCommand { get; set; }


        public ICommand ResetBackgroundCommand { get; set; }


        public ICommand ShowEBoardCommand { get; }

        #endregion


        // Constructors
        #region Constructors

        public EBoardBrowserViewModel()
        {
            AddEBoardCommand = new RelayCommand((s) => AddEBoard(), (s) => true);

            DeleteEBoardCommand = new RelayCommand((s) => DeleteSelectedEBoard(), (s) => true);

            EBoardBrowserImageCommand = new EBoardBrowserImageCommand(this);

            EditEBoardParametersCommand = new EditEBoardParametersCommand(this);

            ResetBackgroundCommand = new ResetBackgroundCommand(this);

            BorderManager = new BorderManagement();

            BrushManager = new BrushManagement();
            BrushManager.Background = new SolidColorBrush(Colors.White);
            BrushManager.Foreground = new SolidColorBrush(Colors.Orange);
            BrushManager.Border = new SolidColorBrush(Colors.Blue);
            BrushManager.Highlight = new SolidColorBrush(Colors.Red);

            EBoards = new ObservableCollection<EBoardViewModel>(); 
        }


        public EBoardBrowserViewModel(NavigationStore navigationStore)
        {
            AddEBoardCommand = new RelayCommand((s) => AddEBoard(), (s) => true);

            DeleteEBoardCommand = new RelayCommand((s) => DeleteSelectedEBoard(), (s) => true);

            _NavigationStore = navigationStore;

            EBoardBrowserImageCommand = new EBoardBrowserImageCommand(this);

            EditEBoardParametersCommand = new EditEBoardParametersCommand(this);

            ResetBackgroundCommand = new ResetBackgroundCommand(this);

            BorderManager = new BorderManagement();

            BrushManager = new BrushManagement();
            BrushManager.Background = new SolidColorBrush(Colors.White);
            BrushManager.Foreground = new SolidColorBrush(Colors.Black);
            BrushManager.Border = new SolidColorBrush(Colors.Blue);
            BrushManager.Highlight = new SolidColorBrush(Colors.Red);



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

            EBoards.Add(new EBoardViewModel(EBoardName, new BorderManagement() { Width = NewEBoardWidth, Height = NewEBoardHeight }, EBoardDepth));

            SelectedEBoard = EBoards.Last();
            RefreshEBoardParameters();

        }

        public void ChangeElementBackgroundToImage()
        {
            BrushManager.Background = new SharedMethod_UI().ChangeBackgroundToImage(BrushManager.Background, ImagePath);

            if (ImagePath.Equals(string.Empty))
            {
                BrushManager.Background = new SolidColorBrush(Colors.White);
            }

            OnPropertyChanged(nameof(BrushManager));

            OnPropertyChanged(nameof(BrushManager.Background));
        }


        public Task InstantiateEBoardDataSet(EboardDataSet eboardDataSet)
        {
            if (eboardDataSet != null)
            {
                EBoardViewModel eBoardViewModel = new EBoardViewModel(
                    eboardDataSet.EBoardName,
                    new BorderManagement(eboardDataSet.BorderDataSet),
                    eboardDataSet.EBoardDepth,
                    eboardDataSet.EBID);

                eBoardViewModel.Elements = eboardDataSet.EBoardViewModel.Elements;

                eBoardViewModel.BrushManager = new BrushManagement(eboardDataSet.BrushDataSet);

                EBoards.Add(eBoardViewModel);
            }

            return Task.CompletedTask;
        }


        private void DeleteSelectedEBoard()
        {
            if (SelectedEBoard != null && EBoards.Count > 0)
            {
                EBoards.Remove(SelectedEBoard);

                if (EBoards.Count > 0)
                {
                    SelectedEBoard = EBoards.Last();
                    RefreshEBoardParameters();
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
                NewEBoardHeight = SelectedEBoard.BorderManager.Height;
                NewEBoardWidth = SelectedEBoard.BorderManager.Width;
                SelectedEBoard.EBoardActive = true;

                CurrentSelectionID = EBoards.IndexOf(SelectedEBoard) + 1;
                EBoardContainerCount = SelectedEBoard.GetContainerCount();
                EBoardCount = EBoards.Count;
                EBoardCreatedDate = SelectedEBoard.GetCreatedDate();
                EBoardElementCount = SelectedEBoard.GetElementCount();
                EBoardShapeCount = SelectedEBoard.GetShapeCount();
            }
            else
            {
                EBoardName = "no board selected";
                EBoardDepth = 10;
                NewEBoardHeight = 620;
                NewEBoardWidth = 1240;

                CurrentSelectionID = 0;
                EBoardContainerCount = 0;
                EBoardCount = 0;
                EBoardCreatedDate = DateTime.Now;
                EBoardElementCount = 0;
                EBoardShapeCount = 0;
            }

        }

        #endregion

    }
}
// EOF