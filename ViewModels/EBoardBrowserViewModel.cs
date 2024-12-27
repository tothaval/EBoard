/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardBrowserViewModel 
 * 
 *  helper class for
 */
using EBoard.Commands;
using EBoard.Commands.ContextMenuCommands.EBoardBrowserContextMenu;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Navigation;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EBoard.ViewModels
{
    public class EBoardBrowserViewModel : BaseViewModel, IElementBackgroundImage
    {

        // Properties & Fields
        #region Properties & Fields



        private double _NewEBoardHeight;
        public double NewEBoardHeight
        {
            get { return _NewEBoardHeight; }
            set
            {
                _NewEBoardHeight = value;
                OnPropertyChanged(nameof(NewEBoardHeight));
            }
        }


        private double _NewEBoardWidth;
        public double NewEBoardWidth
        {
            get { return _NewEBoardWidth; }
            set
            {
                _NewEBoardWidth = value;
                OnPropertyChanged(nameof(NewEBoardWidth));
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


        public ICommand EBoardBrowserImageCommand { get; set; }


        public ICommand EditEBoardParametersCommand { get; set; }


        public ICommand ResetEBoardBrowserBackgroundCommand { get; set; }


        public ICommand ShowEBoardCommand { get; }

        #endregion


        // Constructors
        #region Constructors

        public EBoardBrowserViewModel()
        {
            AddEBoardCommand = new RelayCommand((s) => AddEBoard(), (s) => true);

            DeleteEBoardCommand = new RelayCommand((s) => DeleteSelectedEBoard(s), (s) => true);

            EBoardBrowserImageCommand = new EBoardBrowserImageCommand(this);

            EditEBoardParametersCommand = new EditEBoardParametersCommand(this);

            ResetEBoardBrowserBackgroundCommand = new ResetEBoardBrowserBackgroundCommand(this);

            BrushManager = new BrushManagement();
            BrushManager.Background = new SolidColorBrush(Colors.CornflowerBlue);

            EBoards = new ObservableCollection<EBoardViewModel>();
        }


        public EBoardBrowserViewModel(NavigationStore navigationStore)
        {
            AddEBoardCommand = new RelayCommand((s) => AddEBoard(), (s) => true);

            DeleteEBoardCommand = new RelayCommand((s) => DeleteSelectedEBoard(s), (s) => true);

            _NavigationStore = navigationStore;

            EBoardBrowserImageCommand = new EBoardBrowserImageCommand(this);

            EditEBoardParametersCommand = new EditEBoardParametersCommand(this);

            ResetEBoardBrowserBackgroundCommand = new ResetEBoardBrowserBackgroundCommand(this);

            BrushManager = new BrushManagement();
            BrushManager.Background = new SolidColorBrush(Colors.White);

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
                NewEBoardHeight = SelectedEBoard.BorderManager.Height;
                NewEBoardWidth = SelectedEBoard.BorderManager.Width;
                SelectedEBoard.EBoardActive = true;
            }
        }

        #endregion

    }
}
// EOF