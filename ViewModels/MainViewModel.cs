/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainViewModel 
 * 
 *  helper class for
 */
using EBoard.Commands;
using EBoard.Commands.ContextMenuCommands;
using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
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
using System.Windows.Media.Imaging;

namespace EBoard.ViewModels
{
    public class MainViewModel : BaseViewModel, IElementBackgroundImage
    {

        // Properties & Fields
        #region Properties & Fields
        private readonly NavigationStore _navigationStore;


        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;


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

        public ICommand MainWindowImageCommand { get; }

        public ICommand LeftDoubleClickCommand {get;}


        public ICommand LeftPressCommand {get;}


        public ICommand MaximizeCommand {get;}


        public ICommand MinimizeCommand {get;}

        #endregion


        public MainViewModel(NavigationStore navigationStore, EboardConfig? eboardConfig)
        {
            MainWindowImageCommand = new MainWindowImageCommand(this);

            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            _EBoardBrowserViewModel = new EBoardBrowserViewModel(_navigationStore);

            CloseCommand = new CloseCommand();
            LeftDoubleClickCommand = new MaximizeCommand();
            //LeftPressCommand = new RelayCommand((s) => DragMove(), (s) => true);
            MaximizeCommand = new MaximizeCommand();
            MinimizeCommand = new MinimizeCommand();

            MainWindowMenuBarVM = new MainWindowMenuBarViewModel(this);

            if (eboardConfig == null)
            {
                BrushManager = new BrushManagement();
                BrushManager.Background = new SolidColorBrush(Colors.White);
                BrushManager.Border = new SolidColorBrush(Colors.BlueViolet);

                BorderManager = new BorderManagement();
                BorderManager.Width = 640.0;
                BorderManager.Height = 320.0;

                PlacementManager = new PlacementManagement();
            }
            else
            {
                BorderManager = new BorderManagement(eboardConfig.BorderDataSet);
                BrushManager = new BrushManagement(eboardConfig.BrushDataSet);
                PlacementManager = new PlacementManagement(eboardConfig.PlacementDataSet);
            }
            
        }


        // Methods
        /// <summary>
        /// Methods region holds all non async private and public methods
        /// within the viewmodel, ordered alphabetically
        /// </summary>
        #region Methods

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


                OnPropertyChanged(nameof(BrushManager));

                OnPropertyChanged(nameof(BrushManager.Background));
            }
            catch (Exception)
            {
            }
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

            OnPropertyChanged(nameof(BrushManager));
            OnPropertyChanged(nameof(BrushManager.Background));
            OnPropertyChanged(nameof(BrushManager.Border));

            OnPropertyChanged(nameof(BorderManager));
            OnPropertyChanged(nameof(BorderManager.CornerRadius));
            OnPropertyChanged(nameof(BorderManager.BorderThickness));
        }

        #endregion

    }
}
// EOF