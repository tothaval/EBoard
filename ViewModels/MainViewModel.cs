/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainViewModel 
 * 
 *  helper class for
 */
using EBoard.Commands;
using EBoard.Commands.ContextMenuCommands;
using EBoard.Models;
using EBoard.Navigation;
using EBoard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields
        private readonly NavigationStore _navigationStore;


        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;


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


        private CornerRadius _cornerRadius;
        public CornerRadius CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = value;
                OnPropertyChanged(nameof(CornerRadius));
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


        private double _PositionX;
        public double PositionX
        {
            get { return _PositionX; }
            set
            {
                _PositionX = value;
                OnPropertyChanged(nameof(PositionX));
            }
        }


        private double _PositionY;
        public double PositionY
        {
            get { return _PositionY; }
            set
            {
                _PositionY = value;
                OnPropertyChanged(nameof(PositionY));
            }
        }


        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;
                OnPropertyChanged(nameof(Margin));
            }
        }


        private Thickness _padding;
        public Thickness Padding
        {
            get { return _padding; }
            set
            {
                _padding = value;
                OnPropertyChanged(nameof(Padding));
            }
        }


        private double _opacity;
        public double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }


        private EBoardBrowserViewModel _EBoardBrowserViewModel;
        public EBoardBrowserViewModel EBoardBrowserViewModel
        {
            get { return _EBoardBrowserViewModel; }
            set
            {
                _EBoardBrowserViewModel = value;
            }
        }


        private MainWindowMenuBarViewModel _MainWindowMenuBarVM;
        public MainWindowMenuBarViewModel MainWindowMenuBarVM
        {
            get { return _MainWindowMenuBarVM; }
            set
            {
                _MainWindowMenuBarVM = value;
                OnPropertyChanged(nameof(MainWindowMenuBarVM));
            }
        }

        #endregion



        // Commands
        /// <summary>
        /// Commands region holds all command properties
        /// </summary>
        #region Commands

        public ICommand CloseCommand { get; }


        public ICommand LeftDoubleClickCommand {get;}


        public ICommand LeftPressCommand {get;}


        public ICommand MaximizeCommand {get;}


        public ICommand MinimizeCommand {get;}

        #endregion


        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            _EBoardBrowserViewModel = new EBoardBrowserViewModel(_navigationStore);

            CloseCommand = new CloseCommand();
            LeftDoubleClickCommand = new MaximizeCommand();
            //LeftPressCommand = new RelayCommand((s) => DragMove(), (s) => true);
            MaximizeCommand = new MaximizeCommand();
            MinimizeCommand = new MinimizeCommand();

            MainWindowMenuBarVM = new MainWindowMenuBarViewModel(this);

            BrushManager = new BrushManagement();
            BrushManager.Background = new SolidColorBrush(Colors.White);
            BrushManager.Border = new SolidColorBrush(Colors.BlueViolet);
            BrushManager.BorderThickness = new Thickness(3);

            _cornerRadius = new CornerRadius(10);
            _margin = new Thickness(10);
            _padding = new Thickness(10);
            _opacity = 1;

        }


        // Methods
        /// <summary>
        /// Methods region holds all non async private and public methods
        /// within the viewmodel, ordered alphabetically
        /// </summary>
        #region Methods

        //private void DragMove()
        //{
        //    MainWindow window = (MainWindow)Application.Current.MainWindow;

        //    window.DragMove();
        //}

        public void UpdateBrushManager(BrushManagement brushManagement)
        {
            BrushManager = brushManagement;

            _EBoardBrowserViewModel.UpdateBrushManager(brushManagement);

        }

        #endregion


        // Events
        /// <summary>
        /// Events region holds all event methods within the viewmodel
        /// alphabetically ordered
        /// </summary>
        #region Events

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        #endregion

    }
}
// EOF