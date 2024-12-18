/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  App 
 */
using EBoard.IOProcesses;
using EBoard.IOProcesses.DataSets;
using EBoard.Navigation;
using EBoard.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace EBoard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel _MainViewModel;

        private NavigationStore _navigationStore;

        

        public App()
        {
            _navigationStore = new NavigationStore();


        }


        private async Task EBoardConfigInitialization(EBoardInitializationManager initializationManager)
        {
            await initializationManager.Load();

            EboardConfig eboardConfig = initializationManager.EBoardConfig;

            if (eboardConfig.EBoardIndex > -1 && eboardConfig.EBoardIndex  < _MainViewModel.EBoardBrowserViewModel.EBoards.Count)
            {
                _MainViewModel.EBoardBrowserViewModel.SelectedEBoard = _MainViewModel.EBoardBrowserViewModel.EBoards[initializationManager.EBoardConfig.EBoardIndex];
            }
            
            _MainViewModel.MainWindowMenuBarVM.EBoardBrowserSwitch = initializationManager.EBoardConfig.EBoardBrowserSwitch;

            _MainViewModel.UpdateBrushManager(new Models.BrushManagement(eboardConfig.EBoardMainWindowBrushManager));

            Current.MainWindow.Left = initializationManager.EBoardConfig.EBoardPosition.X;
            Current.MainWindow.Top = initializationManager.EBoardConfig.EBoardPosition.Y;

            Current.MainWindow.Width = initializationManager.EBoardConfig.EBoardWidth;
            Current.MainWindow.Height = initializationManager.EBoardConfig.EBoardHeight;
        }


        protected async override void OnExit(ExitEventArgs e)
        {
            EBoardShutdownManager eBoardShutdownManager = new EBoardShutdownManager(_MainViewModel);

            await eBoardShutdownManager.Save();

            base.OnExit(e);
        }


        protected async override void OnStartup(StartupEventArgs e)
        {
            IOProcessesInitializationManager ioProxIM = new IOProcessesInitializationManager();

            _MainViewModel = new MainViewModel(_navigationStore);

            EBoardInitializationManager initializationManager = new EBoardInitializationManager(ioProxIM, _MainViewModel);

            MainWindow mainWindow = new MainWindow()
            {
                DataContext = _MainViewModel
            };

            await EBoardConfigInitialization(initializationManager);

            mainWindow.Show();

            base.OnStartup(e);

        }


    }

}
// EOF