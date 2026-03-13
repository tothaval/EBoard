// <copyright file="App.xaml.cs" company=".">
// Stephan Kammel
// </copyright>
/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
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
            AllowsTransparency = true,
            WindowStyle = WindowStyle.None,
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

        var saveResult = await this.runner.SaveEboard();

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