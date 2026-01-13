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

using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using EBoardSDK.ViewModels;
using Serilog;
using System.Text;
using System.Windows;
using System.Windows.Media;
using SplashScreen = EBoardSDK.Views.SplashScreen;

public class Runner
{
    private MainViewModel mainViewModel;
    private SplashScreenViewModel? splashScreenViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="Runner"/> class.
    /// </summary>
    public Runner()
    {
    }

    public string CreateLogEventAsync(string v)
    {
        Log.Information(v);

        _ = this.splashScreenViewModel?.WriteLog(v);

        return v;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<EBoardFeedbackMessage> Run()
    {
        var sDKDataManager = new SDKDataManager();

        this.splashScreenViewModel = new SplashScreenViewModel();

        var splashScreen = new SplashScreen() { DataContext = this.splashScreenViewModel, SizeToContent = SizeToContent.WidthAndHeight };

        Application.Current.Dispatcher.Invoke(
            () => splashScreen.Show());

        var config = await sDKDataManager.LoadEboardConfigAsync();
        this.CreateLogEventAsync("config loading complete");

        var screens = await new SDKDataManager().LoadEboardScreenConfigsAsync();
        this.CreateLogEventAsync("screen loading complete");

        this.CreateLogEventAsync("creating mainview");
        this.mainViewModel = new MainViewModel(config, screens, this);

        MainWindow mainWindow = new MainWindow(this.mainViewModel)
        {
            AllowsTransparency = true,
            Background = new SolidColorBrush(Colors.Transparent),
            WindowStyle = WindowStyle.None,
        };

        this.CreateLogEventAsync("finalizing startup");

        mainWindow.Show();
        splashScreen.Close();

        this.splashScreenViewModel = null;

        return new EBoardFeedbackMessage()
        {
            Exception = null,
            ResultMessage = "runner done.",
            TaskResult = EBoardTaskResult.Success,
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<bool> SaveEboard()
    {
        var dataManager = new SDKDataManager();

        var configSaveResult = await dataManager.SaveEboardConfigAsync(this.mainViewModel.GetEboardConfig());

        var screenSaveResults = await dataManager.SaveEboardScreensAsync(this.mainViewModel.GetScreenData());

        return true;
    }
}

// EOF