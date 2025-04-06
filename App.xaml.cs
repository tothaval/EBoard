/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  App 
 */
using CommunityToolkit.Mvvm.DependencyInjection;
using EBoard.IOProcesses;
using EBoard.IOProcesses.DataSets;
using EBoard.Navigation;
using EBoard.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace EBoard;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private MainViewModel _MainViewModel;

    private NavigationStore _navigationStore;

    private static Mutex _InstanceMutex;
    private static string _ApplicationKey =
        @"EBoard .shOd:(1>tDOa!qI^`kl0s}[]sU,m$(?v6(p5$s?H?rP1Fb,zQ$PlUW'60tWE^~";// online generated uuid "cf645c2c-b646-4c2e-be76-6b15f1a4f6d3"


    public App() => _navigationStore = new NavigationStore();


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

        KillInstance(e.ApplicationExitCode);
    }


    protected async override void OnStartup(StartupEventArgs e)
    {
        SplashScreen splashScreen = new SplashScreen();
        splashScreen.Show();


        if (StartInstance())
        {
            Application.Current.Shutdown(1);
        }

        CommunityToolkit.Mvvm.DependencyInjection.Ioc.Default.ConfigureServices(
            new ServiceCollection()
                .AddTransient<IOProcessesInitializationManager>()
                .AddTransient<EBoardConfigLoader>()
                .BuildServiceProvider()
                );


        EBoardConfigLoader eBoardConfigLoader = Ioc.Default.GetRequiredService<EBoardConfigLoader>();

        EboardConfig eboardConfig = await eBoardConfigLoader.LoadEBoardConfig();

        _MainViewModel = new MainViewModel(_navigationStore, eboardConfig);
        
        MainWindow mainWindow = new MainWindow(_MainViewModel);

        EBoardInitializationManager initializationManager = new EBoardInitializationManager(Ioc.Default.GetRequiredService<IOProcessesInitializationManager>(), _MainViewModel);

        await initializationManager.LoadEBoardDataSets();

        await EBoardConfigInitialization(eboardConfig);

        splashScreen.Close();
        mainWindow.Show();

        base.OnStartup(e);
    }

    public static bool StartInstance()
    {
        _InstanceMutex = new Mutex(true, _ApplicationKey);

        bool _InstanceMutexIsInUse = false;

        try
        {
            _InstanceMutexIsInUse = !_InstanceMutex.WaitOne(TimeSpan.Zero, true);
        }
        catch (AbandonedMutexException)
        {
            KillInstance();
            _InstanceMutexIsInUse = false;
        }
        catch (Exception)
        {
            _InstanceMutex.Close();
            _InstanceMutexIsInUse = false;
        }

        //logger log: $"{_InstanceMutexIsInUse} {_EBoardKey} {_InstanceMutex.ToString()}");

        return _InstanceMutexIsInUse;
    }

    public static void KillInstance(int code = 0)
    {
        if (_InstanceMutex is null) return;

        if (code == 0)
        {
            try
            {
                _InstanceMutex.ReleaseMutex();
            }
            catch (Exception)
            {

            }

            _InstanceMutex.Close();
        }
    }


}
// EOF