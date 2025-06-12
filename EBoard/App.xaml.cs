// <copyright file="App.xaml.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  App.
 */
namespace EBoard;

using EBoardSDK.ViewModels;
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

    public App()
    {
        AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        this.ExitEBoard().Wait();

        base.OnExit(e);
    }

    public SplashScreenViewModel splashScreenViewModel { get; set; }

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
            CommunityToolkit.Mvvm.DependencyInjection.Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<Runner>()
                    .BuildServiceProvider());

            this.splashScreenViewModel = new SplashScreenViewModel();
            SplashScreen splashScreen = new SplashScreen() { DataContext = this.splashScreenViewModel, SizeToContent = SizeToContent.WidthAndHeight };

            splashScreen.Show();

            var eboardSdk = new Runner();

            var setup = eboardSdk.Run().Result;

            var config = await eboardSdk.GetConfigAsync(true);

            config = await eboardSdk.GetPluginsAsync(config);

            var screens = await eboardSdk.GetScreensAsync();

            this.mainViewModel = new MainViewModel(config);

            this.mainViewModel.SetScreenData(screens);

            MainWindow mainWindow = new MainWindow(this.mainViewModel);

            _ = await this.EBoardConfigInitialization(mainWindow, config);

            mainWindow.Show();

            splashScreen.Close();

            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            Log.Error(ex, string.Join("\n", "Unhandled exception", ex.Message));
        }
    }

    public string CreateLogEventAsync(string v)
    {
        Log.Information(v);

        this.splashScreenViewModel.LogMessage = v;

        return v;
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