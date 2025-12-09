// <copyright file="App.xaml.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  App.
 */
namespace EBoard;

using CommunityToolkit.Mvvm.DependencyInjection;
using EBoardSDK;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;
using System.Windows;
using System.Windows.Media;

/// <summary>
/// Interaction logic for App.xaml .
/// </summary>
///
public partial class App : Application
{
    private Runner? runner;

    protected override void OnExit(ExitEventArgs e)
    {
        this.ExitEBoard().Wait();

        base.OnExit(e);
    }

    protected override void OnActivated(EventArgs e)
    {
        if (this.runner == null)
        {
            base.OnActivated(e);

            this.RunEBoardSDK();
        }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Window window = new Window()
        {
            Background = new SolidColorBrush(Colors.Transparent),
            Foreground = new SolidColorBrush(Colors.Transparent),
            Left = -50,
            Top = -50,
            Height = 0,
            Width = 0,
        };

        window.Show();

        AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;

        window.Close();
    }

    private async Task ExitEBoard()
    {
        if (this.runner == null)
        {
            return;
        }

        var saveResult = await this.runner.SaveEboardDataAsync();

        return;
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        _ = this.ExitEBoard().IsCompleted;

        var s = $"{e.ExceptionObject}\n{e.IsTerminating}";

        Log.Error(s);

        Log.CloseAndFlush();
    }

    private void RunEBoardSDK()
    {
        try
        {
            var activationTime = DateTime.Now;
            var activationTimeString = string.Join(
                "_",
                $"{activationTime.Year}{activationTime.Month}{activationTime.Day}",
                $"{activationTime.Hour}{activationTime.Minute}{activationTime.Second}");

#if DEBUG
            var debugLogFolder = @"debuglogs\";

            if (!Directory.Exists(debugLogFolder))
            {
                Directory.CreateDirectory(debugLogFolder);
            }

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

        var releaseLogFolder = @"debuglogs\";

            if (!Directory.Exists(releaseLogFolder))
            {
                Directory.CreateDirectory(releaseLogFolder);
            }


        var eventLogFileName = Path.Combine(releaseLogFolder, $"log{activationTimeString}.txt");

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

            this.runner = Ioc.Default.GetRequiredService<Runner>();

            var taskresult = this.runner.Run().Result;

            Log.Information(taskresult.ToString());
        }
        catch (Exception ex)
        {
            Log.Error(ex, string.Join("\n", "Unhandled exception", ex.Message));
        }
    }
}

// EOF