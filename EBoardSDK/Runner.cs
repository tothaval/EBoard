// <copyright file="Runner.cs" company=".">
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
namespace EBoardSDK;

using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using EBoardSDK.ViewModels;
using EBoardSDK.Windows;
using Serilog;
using System.Windows;
using System.Windows.Media;
using SplashScreen = EBoardSDK.Windows.SplashScreen;

/// <summary>
/// This class is tasked with program state management and
/// program execution.
///
/// It loads the programs data on beginning of runtime,
/// instantiates and calls <see cref="MainWindow"/> and
/// stores the programs data on runtime end.
///
/// It holds the programs only <see cref="MainViewModel"/> instance,
/// and sets it as DataContext property for <see cref="MainWindow"/>.
/// </summary>
public class Runner
{
    private MainViewModel? mainViewModel;
    private SplashScreenViewModel? splashScreenViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="Runner"/> class.
    /// </summary>
    public Runner()
    {
    }

    /// <summary>
    /// This method writes everything to <see cref="Log.Information(string)"/>
    /// and calls <see cref="SplashScreenViewModel.WriteLog(string)"/> to update its text.
    /// </summary>
    /// <param name="logMessage">Desired is the message that should be logged.</param>
    /// <returns>Returns true if successful or false if not.</returns>
    public bool CreateLogEventAsync(string logMessage)
    {
        Log.Information(logMessage);

        var result = this.splashScreenViewModel?.WriteLog(logMessage);

        return result ?? false;
    }

    /// <summary>
    /// This async task executes program start and triggers
    /// loading the last program state.
    ///
    /// It calls <see cref="Window.Show()"/> on <see cref="MainWindow"/> and  <see cref="SplashScreen"/>.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<EboardFeedbackMessage> Run()
    {
        var sDKDataManager = new SDKDataManager();

        this.splashScreenViewModel = new SplashScreenViewModel();

        var splashScreen = new SplashScreen() { DataContext = this.splashScreenViewModel, SizeToContent = SizeToContent.WidthAndHeight };
        splashScreen.Show();

        this.CreateLogEventAsync("loading eboard config..");
        var config = await sDKDataManager.LoadEboardConfigAsync();

        this.CreateLogEventAsync("loading screen data..");
        var screens = await new SDKDataManager().LoadEboardScreenConfigsAsync();

        this.mainViewModel = new MainViewModel(config, this);
        await this.mainViewModel.Initialize(screens);

        this.CreateLogEventAsync("loading window data..");
        MainWindow mainWindow = new(this.mainViewModel)
        {
            AllowsTransparency = true,
            Background = new SolidColorBrush(Colors.Transparent),
            WindowStyle = WindowStyle.None,
        };

        var helper = new SharedMethod_UI();

        if (config.EBoardContext.Stand != null)
        {
            mainWindow.Left = config.EBoardContext.Stand.Position.X;
            mainWindow.Top = config.EBoardContext.Stand.Position.Y;
        }

        if (config.EBoardContext.Size != null)
        {
            mainWindow.Width = helper.ConvertNegativeSizeValuesToNaN(config.EBoardContext.Size.Width);
            mainWindow.Height = helper.ConvertNegativeSizeValuesToNaN(config.EBoardContext.Size.Height);
        }

        this.CreateLogEventAsync("finalizing startup..");

        mainWindow.Show();

        splashScreen.Close();

        this.splashScreenViewModel = null;

        return new EboardFeedbackMessage()
        {
            Exception = null,
            ResultMessage = "runner done.",
            TaskResult = EBoardTaskResult.Success,
        };
    }

    /// <summary>
    /// This task saves eboard data as json files to the designated location.
    /// Beginning by the outmost FluidUI Context Area(CA) data will be saved inwards.
    /// The order is: MainWindow CA & NavigationContext CA -> Screen CAs -> per each
    /// Screen CA for each Element CA -> for each Element CA the Plugin.Save() method
    /// is called.
    ///
    /// Every step gathers Results and Exceptions and stores them as <see cref="EboardFeedbackMessage"/>
    /// for further processing if required.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<bool> SaveEboard()
    {
        if (this.mainViewModel == null)
        {
            return false;
        }

        var dataManager = new SDKDataManager();

        var configSaveResult = await dataManager.SaveEboardConfigAsync(this.mainViewModel.GetEboardConfig());

        var screenSaveResults = await dataManager.SaveEboardScreensAsync(this.mainViewModel.GetScreenData());

        // TODO write results to a log file (if necessary or desired).
        return true;
    }
}

// EOF