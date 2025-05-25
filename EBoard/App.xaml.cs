/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  App 
 */
using EBoard.IOProcesses;
using EBoard.Navigation;
using EBoard.ViewModels;
using EBoardSDK;
using EBoardSDK.Models;
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

    private Task<EboardConfig?> EBoardConfigInitialization(MainWindow mainWindow, EboardConfig eboardConfig)
    {
        if (eboardConfig != null)
        {
            if (eboardConfig.EBoardIndex > 0 && eboardConfig.EBoardIndex <= _MainViewModel.EBoardBrowserViewModel.EBoards.Count)
            {
                _MainViewModel.EBoardBrowserViewModel.SelectedEBoard = _MainViewModel.EBoardBrowserViewModel.EBoards[eboardConfig.EBoardIndex-1];
            }

            _MainViewModel.MainWindowMenuBarVM.EBoardBrowserSwitch = eboardConfig.EBoardBrowserSwitch;

            mainWindow.Left = eboardConfig.PlacementDataSet.Position.X;
            mainWindow.Top = eboardConfig.PlacementDataSet.Position.Y;
            
            mainWindow.Width = eboardConfig.BorderDataSet.Width;
            mainWindow.Height = eboardConfig.BorderDataSet.Height;
        }

        return Task.FromResult(eboardConfig);
    }

    protected async override void OnExit(ExitEventArgs e)
    {
        var runner = new Runner();

        var saveConfigResult = await runner.SaveConfig(_MainViewModel.GetEboardConfig());

        if (!saveConfigResult.TaskResult.Equals(EBoardTaskResult.Success))
        {
            // TODO do stuff like logging, exception throwing etc.
        }

        var saveScreensResult = await runner.SaveScreens(_MainViewModel.GetScreenData());

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
                .AddSingleton<Runner>()
                .BuildServiceProvider()
                );

        var eboardSdk = new Runner();

        var setup = await eboardSdk.Run();

        var config = await eboardSdk.GetConfigAsync(true);

        config = await eboardSdk.GetPlugins(config);

        var screens = await eboardSdk.GetScreensAsync();

        _MainViewModel = new MainViewModel(_navigationStore, config);

        _MainViewModel.SetScreenData(screens);

        MainWindow mainWindow = new MainWindow(_MainViewModel);

        await EBoardConfigInitialization(mainWindow, config);

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