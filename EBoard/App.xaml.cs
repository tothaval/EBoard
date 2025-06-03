/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  App.
 */
namespace EBoard;

using EBoard.Navigation;
using EBoard.ViewModels;
using EBoardConfigManager.Models;
using EBoardSDK;
using EBoardSDK.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;
using System.Windows;

/// <summary>
/// Interaction logic for App.xaml .
/// </summary>
///
public partial class App : Application
{
    private MainViewModel mainViewModel;

    private NavigationStore navigationStore;

    public App()
    {
        AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        this.ExitEBoard().RunSynchronously();

        base.OnExit(e);
    }

    protected async override void OnStartup(StartupEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;

        try
        {
            var activationTime = DateTime.Now;
            var activationTimeString = string.Join(
                "_",
                $"{activationTime.Year}{activationTime.Month}{activationTime.Day}",
                $"{activationTime.Hour}{activationTime.Minute}{activationTime.Second}");

            var debugLogFolder = new PresetDirectories().DefaultDebugLogFolder;

            if (!Directory.Exists(debugLogFolder))
            {
                Directory.CreateDirectory(debugLogFolder);
            }

#if DEBUG
            var debugLogFileName = Path.Combine(debugLogFolder, $"debug.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    debugLogFileName,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();
#endif

#if RELEASE

        var eventLogFileName = Path.Combine(debugLogFolder, $"log{activationTimeString}.txt");

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                eventLogFileName,                
                rollingInterval: RollingInterval.Month,
                rollOnFileSizeLimit: true)
            .CreateLogger();
#endif

            this.navigationStore = new NavigationStore();

            SplashScreen splashScreen = new SplashScreen();
            splashScreen.Show();

            CommunityToolkit.Mvvm.DependencyInjection.Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<Runner>()
                    .BuildServiceProvider());

            var eboardSdk = new Runner();

            var setup = await eboardSdk.Run();

            var config = await eboardSdk.GetConfigAsync(true);

            config = await eboardSdk.GetPlugins(config);

            var screens = await eboardSdk.GetScreensAsync();

            this.mainViewModel = new MainViewModel(this.navigationStore, config);

            this.mainViewModel.SetScreenData(screens);

            MainWindow mainWindow = new MainWindow(this.mainViewModel);

            await this.EBoardConfigInitialization(mainWindow, config);

            splashScreen.Close();
            mainWindow.Show();

            base.OnStartup(e);

            Log.Information("Eboard");
        }
        catch (Exception ex)
        {
            Log.Error(ex, string.Join("\n", "Unhandled exception", ex.Message));
        }
    }

    private Task<EboardConfig?> EBoardConfigInitialization(MainWindow mainWindow, EboardConfig eboardConfig)
    {
        if (eboardConfig != null)
        {
            if (eboardConfig.EBoardIndex > 0 && eboardConfig.EBoardIndex <= this.mainViewModel.EBoardBrowserViewModel.EBoards.Count)
            {
                this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard = this.mainViewModel.EBoardBrowserViewModel.EBoards[eboardConfig.EBoardIndex - 1];
            }

            this.mainViewModel.MainWindowMenuBarVM.EBoardBrowserSwitch = eboardConfig.EBoardBrowserSwitch;

            mainWindow.Left = eboardConfig.PlacementDataSet.Position.X;
            mainWindow.Top = eboardConfig.PlacementDataSet.Position.Y;

            mainWindow.Width = eboardConfig.BorderDataSet.Width;
            mainWindow.Height = eboardConfig.BorderDataSet.Height;
        }

        return Task.FromResult(eboardConfig);
    }

    private async Task ExitEBoard()
    {
        var logstring = "exiting eboard";

        Log.Debug(logstring);

        var runner = new Runner();

        var saveConfigResult = await runner.SaveConfig(this.mainViewModel.GetEboardConfig());

        logstring = $"{saveConfigResult}";

        Log.Debug(logstring);

        var saveScreensResult = await runner.SaveScreens(this.mainViewModel.GetScreenData());

        saveScreensResult.ToList().ForEach(x =>
        {
            logstring = x.ToString();
            Log.Error(logstring);
        });

        return;
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        // MessageBox.Show($"{e.IsTerminating}\n{e}\n{e.ExceptionObject}");
        _ = this.ExitEBoard().IsCompleted;

        var s = $"{e.ExceptionObject}\n{e.IsTerminating}";

        Log.Error(s);

        Log.CloseAndFlush();
    }
}

// EOF