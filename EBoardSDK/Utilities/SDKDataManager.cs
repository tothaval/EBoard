// <copyright file="SDKDataManager.cs" company=".">
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
namespace EBoardSDK.Utilities;

using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardConfigManager.Models;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
<<<<<<< Updated upstream
=======
using EBoardSDK.Models.FluidUIDataBlock;
>>>>>>> Stashed changes
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Elements.About;
using EBoardSDK.Plugins.Elements.Manual;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

public class SDKDataManager
{
    private DirectoryInfo? assemblyLocation = null;
    private FileInfo? edfZeroFile = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="SDKDataManager"/> class.
    /// </summary>
    public SDKDataManager()
    {
        this.Initialize();
    }

<<<<<<< Updated upstream
=======
    internal static IFluidUIContext DefaultFluidUIContext => new FluidUIContext() { DataBlock = new FluidUIDataBlockModel() { Title = "default" } };

    internal SupportedFileTypeCategories FilenameCheck(FileInfo fileInfo)
    {
        // TODO implement mime check or something better suited
        SupportedFileTypeCategories fileTypeCategory = SupportedFileTypeCategories.Unknown;

        var extension = fileInfo.Extension.ToLower();

        if (extension.Equals(".stf"))
        {
            return SupportedFileTypeCategories.Text;
        }

        if (extension.Equals(".edf")
            || extension.Equals(".fcf"))
        {
            return SupportedFileTypeCategories.FluidUI;
        }

        if (extension.Equals(".png")
            || extension.Equals(".jpg")
            || extension.Equals(".jpeg"))
        {
            return SupportedFileTypeCategories.Image;
        }

        if (extension.Equals(".dll")
            || extension.Equals(".pif"))
        {
            return SupportedFileTypeCategories.Plugin;
        }

        if (extension.Equals(".aiff")
            || extension.Equals(".avi")
            || extension.Equals(".mpeg")
            || extension.Equals(".mp3")
            || extension.Equals(".m4a")
            || extension.Equals(".mp4")
            || extension.Equals(".mkv")
            || extension.Equals(".wma")
            || extension.Equals(".3gp"))
        {
            return SupportedFileTypeCategories.Media;
        }

        return fileTypeCategory;
    }

    public async Task<T?> LoadPluginContent<T>(string path)
    {
        var data = await Loader.LoadJsonFile<T>(path);

        return data;
    }

    public async Task<EboardFeedbackMessage> SavePluginContent<T>(T model, string path)
    {
        var result = Saver.SaveJsonFile(path, model);

        var message = $"{path} :: {model?.GetType().FullName ?? "model was null"} :: saving plugin content: {result}";

        return FeedbackMessageFactory.Determine(EBoardTaskResult.Success, Result.Success, result, message);
    }

>>>>>>> Stashed changes
    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<string> GetInstalledPluginsDirectory()
    {
        var path = string.Empty;
        var dataLocation = await this.GetDataLocationsAsync();

        if (dataLocation != null)
        {
            path = Path.Combine(dataLocation.EBoardDataPath, DataLocations.EBoardInstalledPluginsPath);
        }

        return path;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<EboardConfig> LoadEboardConfigAsync()
    {
        EboardConfig? eboardConfig = new();

        var dataLocations = await this.GetDataLocationsAsync();

        if (dataLocations != null)
        {
            var configPath = $"{dataLocations.EBoardDataPath}{PresetFilenames.EBOARDCONFIGFILENAME}";

            if (File.Exists(configPath))
            {
                eboardConfig = await Loader.LoadJsonFile<EboardConfig>(configPath);
            }
        }

        return eboardConfig!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<List<EboardScreen>> LoadEboardScreenConfigsAsync()
    {
        List<EboardScreen> eboardScreens = [];
        var dataLocations = await this.GetDataLocationsAsync();

        if (dataLocations != null)
        {
            var eboardFolderPath = Path.Combine(dataLocations.EBoardDataPath, DataLocations.EBoardScreenDataPath);
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

                var loadedElements = await this.LoadElementConfigsAsync(screenfiles);
                escreen.Elements = loadedElements;

                eboardScreens.Add(escreen);
            }
        }

        return eboardScreens;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="screenfiles"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<List<ElementConfig>> LoadElementConfigsAsync(List<FileInfo> screenfiles)
    {
        List<ElementConfig> elements = [];

        foreach (var screen in screenfiles)
        {
            if (screen.Name.Equals(PresetFilenames.EBOARDSCREENFILENAME))
            {
                continue;
            }

            if (screen.Exists && !string.IsNullOrWhiteSpace(screen.DirectoryName))
            {
                var elementConfigData = await Loader.LoadJsonFile<ElementConfig>(screen.FullName);

                if (elementConfigData != null)
                {
                    var elementContentFiles = Loader.GetFiles(screen.DirectoryName, "*.ecf");

                    var contentPath = $"{elementConfigData.EID}.ecf";

                    var contentFilePath = elementContentFiles.Where(cf => cf.Name.Equals(contentPath)).FirstOrDefault();

                    if (contentFilePath != null)
                    {
                        elementConfigData.ContentFilePath = contentFilePath.FullName;
                    }

                    elements.Add(elementConfigData);
                }
            }
        }

        return elements;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<AboutModel> LoadLicenseAndAboutAsync()
    {
        AboutModel? aboutModel = new();

        var configPath = Path.Combine(Environment.CurrentDirectory, PresetFilenames.EBOARDLICENCEANDAABOUTFILENAME);

        if (File.Exists(configPath))
        {
            aboutModel = await Loader.LoadJsonFile<AboutModel>(configPath);
        }

        return aboutModel!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<ManualModel> LoadManualAsync()
    {
        ManualModel? manual = new();

        var configPath = Path.Combine(Environment.CurrentDirectory, PresetFilenames.EBOARDMANUALFILENAME);

        if (File.Exists(configPath))
        {
            manual = await Loader.LoadJsonFile<ManualModel>(configPath);
        }

        return manual!;
    }

    /// <summary>
    /// Uses installedplugin directory to search for plugin files.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<List<PluginRepresentationItem>> LoadPluginsFromInstalledPluginsDirectoryAsync()
    {
        List<PluginRepresentationItem> plugins = [];

        var pluginSavePath = await this.GetInstalledPluginsDirectory();

        plugins = await this.LoadPluginRepresentationItemsAsync(pluginSavePath);

        return plugins;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="path"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<List<PluginRepresentationItem>> LoadPluginRepresentationItemsAsync(string path)
    {
        List<PluginRepresentationItem> plugins = [];

        var dir = new DirectoryInfo(path);

        if (dir == null || !dir.Exists)
        {
            return plugins;
        }

        var assemblies = Loader.GetFiles(dir.FullName, "*.dll", SearchOption.TopDirectoryOnly);

        if (assemblies == null || assemblies.Count == 0)
        {
            return plugins;
        }

        assemblies.Select(x => x).ToList().ForEach(dllfile =>
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllfile.FullName);
                var types = assembly.GetExportedTypes();

                var baseType = types.Where(dlltype => dlltype.BaseType != null && dlltype.BaseType.Equals(typeof(EBoardElementPluginBaseViewModel))).Any();

                if (!baseType)
                {
                    return;
                }

                var baseviewmodeltype = types.Where(dlltype => dlltype.BaseType!.Equals(typeof(EBoardElementPluginBaseViewModel))).FirstOrDefault();

                if (baseviewmodeltype == null)
                {
                    return;
                }

                try
                {
                    var baseviewmodel = Activator.CreateInstance(baseviewmodeltype) as EBoardElementPluginBaseViewModel;

                    if (baseviewmodel != null)
                    {

                        try
                        {
                            _ = baseviewmodel.Initialize();

                            if (!Application.Current.Resources.MergedDictionaries.Contains(baseviewmodel.ResourceDictionary))
                            {
                                Application.Current.Resources.MergedDictionaries.Add(baseviewmodel.ResourceDictionary);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, "plugin initialization error");
                        }

                        plugins.Add(new PluginRepresentationItem()
                        {
                            PluginName = baseviewmodel.PluginName,
                            PluginHeader = baseviewmodel.PluginHeader,
                            PluginLogo = baseviewmodel.PluginLogo,
                            PluginCategory = baseviewmodel.PluginCategory,
                            ScreenConstraints = baseviewmodel.ElementScreenIntegrationConstraints,
                            PluginMainViewModelType = baseviewmodel.ElementPluginViewModel,
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);

                    throw;
                }
            }
            catch (Exception)
            {
                // TODO log events of try catch block, specify what dll was skipped
                // prevent not updated external dlls from crashing the application
                // rethrow or pass exception to caller
                return;
            }
        });

        return plugins;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="eboardConfig"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<EBoardFeedbackMessage> SaveEboardConfigAsync(EboardConfig eboardConfig)
    {
        var dataLocations = await this.GetDataLocationsAsync();

        if (this.assemblyLocation == null || dataLocations == null || eboardConfig == null)
        {
            return new EBoardFeedbackMessage()
            {
                ResultMessage = $"SDKDataManager.SaveEboardConfigAsync ::: assemblyLocation or local dataLocations or parameter manual is null",
                TaskResult = EBoardTaskResult.Failure,
            };
        }

        var configFilePath = Path.Combine(dataLocations.EBoardDataPath, PresetFilenames.EBOARDCONFIGFILENAME);

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
            ResultMessage = "fileInfo string corrupted or operation unsuccessful",
            TaskResult = EBoardTaskResult.Failure,
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="screenfolderpath"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<List<EBoardFeedbackMessage>> SaveElementConfigsAsync(List<ElementConfig> elements, string screenfolderpath)
    {
        List<EBoardFeedbackMessage> feedbackMessages = [];

        elements.AsParallel().ForAll(
           async element =>
           {
               var contentfilename = $"{element.EID}.ecf";
               var contentpath = Path.Combine(screenfolderpath, contentfilename);
               var contentSaveResult = await element.Plugin.Save(contentpath);

               if (contentSaveResult == null)
               {
                   contentSaveResult = new EBoardFeedbackMessage()
                   {
                       ResultMessage = $"{contentpath} :: saving element content {element.ID}: {contentSaveResult?.Exception?.Message}",
                       TaskResult = EBoardTaskResult.Unknown,
                   };
               }

               feedbackMessages.Add(contentSaveResult);

               var filename = $"{element.EID}.edf";

               var path = Path.Combine(screenfolderpath, filename);

               var result = Saver.SaveJsonFile<ElementConfig>(path, element);

               feedbackMessages.Add(new EBoardFeedbackMessage()
               {
                   ResultMessage = $"{path} :: saving element {element.ID}: {result}",
                   TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
               });
           });

        return feedbackMessages;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="eboardScreens"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    internal async Task<List<EBoardFeedbackMessage>> SaveEboardScreensAsync(List<EboardScreen> eboardScreens)
    {
        List<EBoardFeedbackMessage> feedbackMessages = [];

        var dataLocations = await this.GetDataLocationsAsync();

        if (dataLocations == null)
        {
            feedbackMessages.Add(new EBoardFeedbackMessage()
            {
                ResultMessage = $"SDKDataManager.SaveEboardScreensAsync::: dataLocations is null",
                TaskResult = EBoardTaskResult.Failure,
            });
        }

        if (dataLocations != null)
        {
            var screensPath = Path.Combine(dataLocations.EBoardDataPath, DataLocations.EBoardScreenDataPath);

            if (Directory.Exists(screensPath))
            {
                _ = Saver.CleanFolderAsync(screensPath);
            }

            try
            {
                eboardScreens.AsParallel().ForAll(
                       async escreen =>
                       {
                           var folderName = Path.Combine(screensPath, escreen.EBID);

                           Directory.CreateDirectory(folderName);

                           var path = Path.Combine(folderName, PresetFilenames.EBOARDSCREENFILENAME);

                           var result = Saver.SaveJsonFile<EboardScreen>(path, escreen);

                           var resultMessage = $"{path} :: saving eboard {escreen.ID}: {result}";
                           var taskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown;

                           feedbackMessages.Add(new EBoardFeedbackMessage()
                           {
                               ResultMessage = resultMessage,
                               TaskResult = taskResult,
                           });

                           var elementfolderpath = Path.Combine(screensPath, escreen.EBID);

                           if (Loader.DirExists(elementfolderpath))
                           {
                               var escreenElementSaveResult = await this.SaveElementConfigsAsync(escreen.Elements, elementfolderpath);

                               feedbackMessages.AddRange(escreenElementSaveResult);
                           }
                       });
            }
            catch (ArgumentOutOfRangeException aoorex)
            {
                Log.Error(aoorex.Message);

                feedbackMessages.Add(new EBoardFeedbackMessage()
                {
                    ResultMessage = aoorex.Message,
                    TaskResult = EBoardTaskResult.Exception,
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                feedbackMessages.Add(new EBoardFeedbackMessage()
                {
                    ResultMessage = ex.Message,
                    TaskResult = EBoardTaskResult.Exception,
                });
            }
        }

        return await Task.FromResult(feedbackMessages);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="path"></param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    internal async Task SaveNewEBoardDataPath(string path)
    {
        var dataLocations = await this.GetDataLocationsAsync();

        if (dataLocations != null && !string.IsNullOrWhiteSpace(path))
        {
            dataLocations.EBoardDataContextPath = path;

            Saver.SaveJsonFile(this.edfZeroFile!.FullName, dataLocations);
        }
    }

    private async Task<DataLocations?> GetDataLocationsAsync()
    {
        if (this.assemblyLocation == null)
        {
            this.Initialize();
        }

        if (this.assemblyLocation == null || this.edfZeroFile == null)
        {
            return null;
        }

        DataLocations? dataLocations = null;

        try
        {
            dataLocations = await Loader.LoadJsonFile<DataLocations>(this.edfZeroFile.FullName);

            if (dataLocations != null)
            {
                string path = Path.Combine(dataLocations.EBoardDataContextPath, DataLocations.EBoardDataRootPath);

                // create folders if necessary
                await Saver.CreateFolderAsync(path);
                await Saver.CreateFolderAsync(Path.Combine(path, DataLocations.EBoardFluidUIConfigurationsPath));
                await Saver.CreateFolderAsync(Path.Combine(path, DataLocations.EBoardInstalledPluginsPath));
                await Saver.CreateFolderAsync(Path.Combine(path, DataLocations.EBoardScreenDataPath));
            }
        }
        catch (JsonException jsonEx)
        {
            var exceptionString = $"loading \"{this.edfZeroFile.FullName}\" threw exception\n{jsonEx.Message}";

            Log.Error(exceptionString);
        }
        catch (NullReferenceException nullEx)
        {
            var exceptionString = $"loading \"{this.edfZeroFile.FullName}\" threw exception\n{nullEx.Message}";

            Log.Error(exceptionString);
        }
        catch (ArgumentNullException argNullEx)
        {
            var exceptionString = $"loading \"{this.edfZeroFile.FullName}\" threw exception\n{argNullEx.Message}";

            Log.Error(exceptionString);
        }
        catch (Exception ex)
        {
            var exceptionString = $"loading \"{this.edfZeroFile.FullName}\" threw exception\n{ex.Message}";

            Log.Error(exceptionString);
        }

        return dataLocations!;
    }

    private void Initialize()
    {
        this.InitializeAssemblyLocation();
        this.InitializeEdfZeroFile();
    }

    private void InitializeAssemblyLocation()
    {
        if (this.assemblyLocation == null)
        {
            if (!string.IsNullOrWhiteSpace(System.Environment.ProcessPath))
            {
                var executable = new FileInfo(System.Environment.ProcessPath);

                if (executable != null)
                {
                    if (executable.Directory != null)
                    {
                        this.assemblyLocation = executable.Directory;
                    }
                }
            }
        }
    }

    private void InitializeEdfZeroFile()
    {
        if (this.edfZeroFile == null && this.assemblyLocation != null)
        {
            var edfZeroPath = Path.Combine(this.assemblyLocation.FullName, PresetFilenames.DATALOCATIONSFILENAME);

            if (!string.IsNullOrWhiteSpace(edfZeroPath))
            {
                var fileInfo = new FileInfo(edfZeroPath);

                if (fileInfo != null)
                {
                    this.edfZeroFile = fileInfo;
                }
            }
        }
    }
}

// EOF