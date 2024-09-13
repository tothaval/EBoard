using EBoard.Commands;
using EBoard.Commands.ContextMenuCommands;
using EBoard.Navigation;
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
    internal class MainViewModel : BaseViewModel
    {

        private readonly NavigationStore _navigationStore;


        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;


        private Brush _backgroundBrush;
        public Brush BackgroundBrush
        {
            get { return _backgroundBrush; }
            set { _backgroundBrush = value;
                OnPropertyChanged(nameof(BackgroundBrush));
            }
        }


        private Brush _borderBrush;
        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set { _borderBrush = value;
                OnPropertyChanged(nameof(BorderBrush));
            }
        }


        private Thickness _borderThickness;
        public Thickness BorderThickness
        {
            get { return _borderThickness; }
            set {
                _borderThickness = value;
                OnPropertyChanged(nameof(BorderThickness));
            }
        }
        
        
        private CornerRadius _cornerRadius;
        public CornerRadius CornerRadius
        {
            get { return _cornerRadius; }
            set {
                _cornerRadius = value;
                OnPropertyChanged(nameof(CornerRadius));
            }
        }


        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set {
                _margin = value;
                OnPropertyChanged(nameof(Margin));
            }
        }


        private Thickness _padding;
        public Thickness Padding
        {
            get { return _padding; }
            set {
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
            _EBoardBrowserViewModel = new EBoardBrowserViewModel(_navigationStore);

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            CloseCommand = new CloseCommand();
            LeftDoubleClickCommand = new MaximizeCommand();
            LeftPressCommand = new RelayCommand((s) => DragMove(), (s) => true);
            MaximizeCommand = new MaximizeCommand();
            MinimizeCommand = new MinimizeCommand();


            _backgroundBrush = new SolidColorBrush(Colors.White);
            _borderBrush = new SolidColorBrush(Colors.BlueViolet);
            _borderThickness = new Thickness(3);
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

        private void DragMove()
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            window.DragMove();
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