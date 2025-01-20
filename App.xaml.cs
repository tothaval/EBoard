/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  App 
 */
using EBoard.IOProcesses;
using EBoard.IOProcesses.DataSets;
using EBoard.Navigation;
using EBoard.ViewModels;
using System.Windows;

namespace EBoard;

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


    private async Task<EboardConfig?> EBoardConfigInitialization(EboardConfig eboardConfig)
    {
        if (eboardConfig != null)
        {
            if (eboardConfig.EBoardIndex > -1 && eboardConfig.EBoardIndex < _MainViewModel.EBoardBrowserViewModel.EBoards.Count)
            {
                _MainViewModel.EBoardBrowserViewModel.SelectedEBoard = _MainViewModel.EBoardBrowserViewModel.EBoards[eboardConfig.EBoardIndex];
            }

            _MainViewModel.MainWindowMenuBarVM.EBoardBrowserSwitch = eboardConfig.EBoardBrowserSwitch;

            Current.MainWindow.Left = eboardConfig.PlacementDataSet.Position.X;
            Current.MainWindow.Top = eboardConfig.PlacementDataSet.Position.Y;

            Current.MainWindow.Width = eboardConfig.BorderDataSet.Width;
            Current.MainWindow.Height = eboardConfig.BorderDataSet.Height;
        }

        return eboardConfig;
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
        EBoardConfigLoader eBoardConfigLoader = new EBoardConfigLoader(ioProxIM);
        EboardConfig eboardConfig = await eBoardConfigLoader.LoadEBoardConfig();

        _MainViewModel = new MainViewModel(_navigationStore, eboardConfig);
        
        MainWindow mainWindow = new MainWindow()
        {
            DataContext = _MainViewModel
        };

        EBoardInitializationManager initializationManager = new EBoardInitializationManager(ioProxIM, _MainViewModel);

        await initializationManager.LoadEBoardDataSets();

        await EBoardConfigInitialization(eboardConfig);          

        mainWindow.Show();

        base.OnStartup(e);
    }

}
// EOF