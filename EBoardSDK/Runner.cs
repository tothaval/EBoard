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

using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardConfigManager.Models;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using EBoardSDK.ViewModels;
using Serilog;
using System.IO;
using System.Windows;
using System.Windows.Media;
using SplashScreen = EBoardSDK.Views.SplashScreen;

public class Runner
{
    private DirectoryInfo assemblyLocation;

    private MainViewModel mainViewModel;
    private SplashScreenViewModel? splashScreenViewModel;

    public Runner()
    {
    }

    public DataLocations DataLocations { get; set; }

    public string CreateLogEventAsync(string v)
    {
        Log.Information(v);

        _ = this.splashScreenViewModel?.WriteLog(v);

        return v;
    }

    public async Task<DataLocations> GetDataLocations()
    {
        var binPath = System.Environment.ProcessPath;

        string edfZeroPath = string.Empty;

        if (binPath != null)
        {
            this.assemblyLocation = new FileInfo(binPath).Directory;

            if (this.assemblyLocation != null)
            {
                edfZeroPath = Path.Combine(this.assemblyLocation.FullName, PresetFilenames.DATALOCATIONSFILENAME);
            }
        }

        try
        {
            var dataLocations = await Loader.LoadJsonFile<DataLocations>(edfZeroPath);

            return dataLocations!;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EBoardFeedbackMessage> Run()
    {
        this.DataLocations = await this.GetDataLocations().ConfigureAwait(true);
        string path = Path.Combine(this.DataLocations.EBoardDataContextPath, DataLocations.EBoardDataRootPath);

        var dataLocationPathExists = Directory.Exists(path);

        if (!dataLocationPathExists)
        {
            Directory.CreateDirectory(path);

            Directory.CreateDirectory(Path.Combine(path, DataLocations.EBoardInstalledPluginsPath));

            Directory.CreateDirectory(Path.Combine(path, DataLocations.EBoardScreenDataPath));
        }

        this.splashScreenViewModel = new SplashScreenViewModel();

        var splashScreen = new SplashScreen() { DataContext = this.splashScreenViewModel, SizeToContent = SizeToContent.WidthAndHeight };

        Application.Current.Dispatcher.Invoke(
            () => splashScreen.Show());

        var config = await this.GetConfigAsync();
        this.CreateLogEventAsync("config loading complete");

        config = await this.GetPluginsAsync(config);
        this.CreateLogEventAsync("plugin loading complete");

        this.mainViewModel = new MainViewModel(config, this.DataLocations, this);

        var screens = await this.GetScreensAsync();
        this.CreateLogEventAsync("screen loading complete");

        this.CreateLogEventAsync("applying data");
        this.mainViewModel.SetScreenData(screens);

        MainWindow mainWindow = new MainWindow(this.mainViewModel)
        {
            AllowsTransparency = true,
            Background = new SolidColorBrush(Colors.Transparent),
            WindowStyle = WindowStyle.None,
        };

        this.CreateLogEventAsync("finalizing startup");
        _ = await this.EBoardConfigInitialization(mainWindow, config);

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

    public async Task<EboardConfig> GetConfigAsync()
    {
        var configPath = $"{this.DataLocations.EBoardDataPath}{PresetFilenames.EBOARDCONFIGFILENAME}";
        EboardConfig? eboardConfig = new();

        if (File.Exists(configPath))
        {
            eboardConfig = await Loader.LoadJsonFile<EboardConfig>(configPath);
        }

        return eboardConfig!;
    }

    public async Task<IList<EboardScreen>> GetScreensAsync()
    {
        IList<EboardScreen> eboardScreens = [];

        var eboardFolderPath = Path.Combine(this.DataLocations.EBoardDataPath, DataLocations.EBoardScreenDataPath);
        var screenfolders = Loader.GetDirectories(eboardFolderPath);

        if (screenfolders == null || screenfolders.Count == 0)
        {
            return eboardScreens;
        }

        foreach (var screenfolder in screenfolders)
        {
            var screenfiles = Loader.GetFiles(screenfolder.FullName, "*.edf");

            var screenDataPath = Path.Combine(screenfolder.FullName, PresetFilenames.EBOARDSCREENFILENAME);
            var escreen = await Loader.LoadJsonFile<EboardScreen>(screenDataPath);

            if (escreen == null)
            {
                continue;
            }

            foreach (var screen in screenfiles)
            {
                if (screen.Name.Equals(PresetFilenames.EBOARDSCREENFILENAME))
                {
                    continue;
                }

                var elementConfigData = await Loader.LoadJsonFile<ElementConfig>(screen.FullName);

                var elementContentFiles = Loader.GetFiles(screen.DirectoryName, "*.ecf");

                if (elementConfigData != null)
                {
                    var contentPath = $"{elementConfigData.EID}.ecf";
                    var contentFilePath = elementContentFiles.Where(cf => cf.Name.Equals(contentPath)).FirstOrDefault();

                    if (contentFilePath != null)
                    {
                        elementConfigData.ContentFilePath = contentFilePath.FullName;
                    }

                    escreen.Elements.Add(elementConfigData);
                }
            }

            eboardScreens.Add(escreen);
        }

        return eboardScreens;
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

            if (eboardConfig.EBoardContext == null)
            {
                eboardConfig.EBoardContext = new FluidUIContext();
                eboardConfig.EBoardContext.SetInitialValues();
            }

            if (eboardConfig.EBoardBrowserViewContext == null)
            {
                eboardConfig.EBoardBrowserViewContext = new FluidUIContext();
                eboardConfig.EBoardBrowserViewContext.SetInitialValues();
            }

            var helper = new SharedMethod_UI();

            mainWindow.Left = eboardConfig.EBoardContext.Stand.Position.X;
            mainWindow.Top = eboardConfig.EBoardContext.Stand.Position.Y;

            mainWindow.Width = helper.ConvertNegativeSizeValuesToNaN(eboardConfig.EBoardContext.Size.Width);
            mainWindow.Height = helper.ConvertNegativeSizeValuesToNaN(eboardConfig.EBoardContext.Size.Height);

        }

        return Task.FromResult(eboardConfig);
    }

    public async Task<EboardConfig> GetPluginsAsync(EboardConfig eboardConfig)
    {
        try
        {
            var pluginFolder = Path.Combine(this.DataLocations.EBoardDataPath, DataLocations.EBoardInstalledPluginsPath);

            eboardConfig.ElementPlugins = await PluginLoader.LoadPluginsAsync(pluginFolder);
        }
        catch (Exception)
        {
            throw;
        }

        return eboardConfig;
    }

    public async Task<bool> SaveEboardDataAsync()
    {
        try
        {
            if (this.mainViewModel == null)
            {
                return false;
            }

            if (this.assemblyLocation == null)
            {
                var binPath = System.Environment.ProcessPath;

                if (binPath != null)
                {
                    var dirInfo = new FileInfo(binPath).Directory;

                    if (dirInfo == null)
                    {
                        return false;
                    }

                    this.assemblyLocation = dirInfo;
                }
            }

            var savePath = Path.Combine(this.assemblyLocation!.FullName, PresetFilenames.DATALOCATIONSFILENAME);
            var result = Saver.SaveJsonFile<DataLocations>(savePath, this.DataLocations);

            var logstring = "saving eboard data";

            Log.Debug(logstring);

            var saveConfigResult = await this.SaveConfig();

            logstring = $"{saveConfigResult}";

            Log.Debug(logstring);

            var saveScreensResult = await this.SaveScreens();

            saveScreensResult.ToList().ForEach(x =>
            {
                logstring = x.ToString();
                Log.Error(logstring);
            });
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<EBoardFeedbackMessage> SaveConfig()
    {
        if (this.mainViewModel == null)
        {
            return new EBoardFeedbackMessage()
            {
                ResultMessage = $"MainViewModel is null",
                TaskResult = EBoardTaskResult.Failure,
            };
        }

        var eboardConfig = this.mainViewModel.GetEboardConfig();

        var configFilePath = Path.Combine(this.DataLocations.EBoardDataPath, PresetFilenames.EBOARDCONFIGFILENAME);

        if (!string.IsNullOrWhiteSpace(configFilePath))
        {
            var result = Saver.SaveJsonFile<EboardConfig>(configFilePath, eboardConfig);

            return new EBoardFeedbackMessage()
            {
                ResultMessage = $"{configFilePath} :: saving eboard config: {result}",
                TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
            };
        }

        return new EBoardFeedbackMessage()
        {
            ResultMessage = "file string corrupted or operation unsuccessful",
            TaskResult = EBoardTaskResult.Failure,
        };
    }

    public Task<IList<EBoardFeedbackMessage>> SaveScreenElements(IList<ElementConfig> elements, string screenfolderpath)
    {
        IList<EBoardFeedbackMessage> feedbackMessages = [];

        elements.AsParallel().ForAll(
           async element =>
           {
               var filename = $"{element.EID}.edf";

               var path = Path.Combine(screenfolderpath, filename);

               var result = Saver.SaveJsonFile<ElementConfig>(path, element);

               feedbackMessages.Add(new EBoardFeedbackMessage()
               {
                   ResultMessage = $"{path} :: saving element {element.ID}: {result}",
                   TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
               });

               var contentfilename = $"{element.EID}.ecf";
               var contentpath = Path.Combine(screenfolderpath, contentfilename);
               var contentSaveResult = await element.Plugin.Save(contentpath);

               feedbackMessages.Add(new EBoardFeedbackMessage()
               {
                   ResultMessage = $"{contentpath} :: saving element content {element.ID}: {contentSaveResult}",
                   TaskResult = contentSaveResult.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
               });
           });

        return Task.FromResult(feedbackMessages);
    }

    public async Task<IList<EBoardFeedbackMessage>> SaveScreens()
    {
        IList<EboardScreen> eboardScreens = this.mainViewModel.GetScreenData();

        IList<EBoardFeedbackMessage> feedbackMessages = [];

        var screensPath = Path.Combine(this.DataLocations.EBoardDataPath, DataLocations.EBoardScreenDataPath);

        if (Directory.Exists(screensPath))
        {
            _ = this.CleanFolderAsync(screensPath);
        }

        eboardScreens.AsParallel().ForAll(
           async escreen =>
           {
               var folderName = Path.Combine(screensPath, escreen.EBID);

               Directory.CreateDirectory(folderName);

               var path = Path.Combine(folderName, PresetFilenames.EBOARDSCREENFILENAME);

               var result = Saver.SaveJsonFile<EboardScreen>(path, escreen);

               var elementfolderpath = Path.Combine(screensPath, escreen.EBID);

               var escreenElementSaveResult = await this.SaveScreenElements(escreen.Elements, elementfolderpath);
               if (Loader.DirExists(elementfolderpath))
               {
                   // TODO: find a solution for logging or processing the resultmessage list of the element save function if need be.
               }

               feedbackMessages.Add(new EBoardFeedbackMessage()
               {
                   ResultMessage = $"{path} :: saving eboard {escreen.ID}: {result}",
                   TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
               });
           });

        return await Task.FromResult(feedbackMessages);
    }

    public bool CleanFolderAsync(string folder)
    {
        string filter = "*.*";

        List<string> files = Directory.GetFiles(folder, filter, SearchOption.AllDirectories).ToList();

        if (files.Count > 0)
        {
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        List<string> folders = Directory.GetDirectories(folder).ToList();

        if (folders.Count > 0)
        {
            foreach (string f in folders)
            {
                try
                {
                    Directory.Delete(f, true);
                }
                catch (Exception)
                {
                    // diese exception mal handlen oder im try block prüfen,
                    // ob die datei frei oder in verwendung ist, ggf. ein paar
                    // mal wiederholen bis zum abbruch

                    // mitunter ist die shapedata.xml noch von einem anderen prozess
                    // blockiert, aktuell keine ahnung weswegen, low prio
                }
            }
        }

        return true;
    }
}

// EOF