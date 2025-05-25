namespace EBoardSDK;

using EBoardConfigManager;
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardConfigManager.Models;
using EBoardSDK.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Input;

public class Runner
{
    public PresetFilenames PresetFilenames => new PresetFilenames();

    public Runner()
    {

    }

    public async Task<EBoardFeedbackMessage> Run()
    {
        var dtest = await new ConfigurationSetup().Run<EboardConfig>();

        var d = new EBoardFeedbackMessage() { TaskResult = dtest ? EBoardTaskResult.Success : EBoardTaskResult.Unknown, ResultMessage = string.Empty };

        return d;
    }

    public async Task<ConfigPaths> GetConfigPathsAsync()
    {
        ConfigPaths configpaths = await Loader.LoadJsonFile<ConfigPaths>(ConfigurationSetup.PresetFilenames.PathsFilename);

        //ConfigPaths configpaths;

        //    using (FileStream openStream = File.OpenRead(ConfigurationSetup.PresetFilenames.PathsFilename))
        //    {
        //    configpaths = JsonSerializer.Deserialize<ConfigPaths>(openStream);
        //    }

        return configpaths;
    }

    public async Task<EboardConfig> GetConfigAsync(bool loadFromDefault, string path = "")
    {
        if (loadFromDefault)
        {
            var configPaths = await GetConfigPathsAsync();

            if (configPaths != null)
            {
                var defaultPath = Path.Combine(configPaths.ConfigFolder, PresetFilenames.ConfigFilename);

                return await Loader.LoadJsonFile<EboardConfig>(defaultPath);
            }
        }

        return await Loader.LoadJsonFile<EboardConfig>(path);
    }

    public async Task<IList<EboardScreen>> GetScreensAsync()
    {
        IList<EboardScreen> eboardScreens = [];

        var configPaths = await GetConfigPathsAsync();

        if (configPaths != null)
        {
            var files = Loader.GetFiles(configPaths.ScreensFolder, "*.screen");

            files.Select(x => x).ToList().ForEach(async fileInfo =>
            {
                var escreen = await Loader.LoadJsonFile<EboardScreen>(fileInfo.FullName);

                var elementPath = Path.Combine(configPaths.ScreensFolder, escreen.EBID);

                var elements = Loader.GetFiles(elementPath, "*.config");

                escreen.ContentDataFilePaths = Loader.GetFiles(elementPath, "*.data");

                elements.Select(x => x).ToList().ForEach(
                    async elementFileName =>
                    {
                        var elementConfig = await Loader.LoadJsonFile<ElementConfig>(elementFileName.FullName);

                        escreen.Elements.Add(elementConfig);
                    }
                    );
               
                eboardScreens.Add(escreen);                
            });
        }

        return eboardScreens;
    }

    private async Task<string> GetDefaultPathAsync()
    {
        var configPaths = await GetConfigPathsAsync();

        if (configPaths != null)
        {
            var defaultPath = Path.Combine(configPaths.ConfigFolder, PresetFilenames.ConfigFilename);

            return defaultPath;
        }

        return string.Empty;
    }


    public async Task<EboardConfig> GetPlugins(EboardConfig eboardConfig)
    {
        var configPaths = await GetConfigPathsAsync();

        if (configPaths != null)
        {
            eboardConfig.InstalledPlugins = await PluginLoader.LoadPlugins(configPaths.PluginFolder);
        }

        return eboardConfig;
    }

    public async Task<EBoardFeedbackMessage> SaveConfig(EboardConfig eboardConfig)
    {
        var configFilePath = await GetDefaultPathAsync();

        if (!string.IsNullOrWhiteSpace(configFilePath))
        {
            var result = Saver.SaveJsonFile<EboardConfig>(configFilePath, eboardConfig);

            return new EBoardFeedbackMessage()
            {
                ResultMessage = result.ToString(),
                TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown
            };
        }

        return new EBoardFeedbackMessage()
        {
            ResultMessage = "file string corrupted or operation unsuccessful",
            TaskResult = EBoardTaskResult.Failure
        };
    }

    public Task<IList<EBoardFeedbackMessage>> SaveScreenElements(IList<ElementConfig> elements, string screenfolderpath)
    {
        _ = new ConfigurationSetup().CleanFolderAsync(screenfolderpath);

        IList<EBoardFeedbackMessage> feedbackMessages = [];

        elements.AsParallel().ForAll(
           async element =>
           {
               //eboard.screen
               var filename = string.Join("", PresetFilenames.ElementFilename.Replace("element", element.EID));

               var path = Path.Combine(screenfolderpath, filename);

               var result = Saver.SaveJsonFile<ElementConfig>(path, element);

               var datafilepath = path.Replace("config", "data");

               element.Plugin.Save(datafilepath);

               feedbackMessages.Add(new EBoardFeedbackMessage()
               {
                   ResultMessage = $"{result}",
                   TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown
               });
           });

        return Task.FromResult(feedbackMessages);
    }

    public Task<IList<EBoardFeedbackMessage>> SaveScreens(IList<EboardScreen> eboardScreens)
    {
        IList<EBoardFeedbackMessage> feedbackMessages = [];
        var screensPath = GetConfigPathsAsync().Result.ScreensFolder;

        eboardScreens.AsParallel().ForAll(
           async escreen =>
           {
               //eboard.screen
               var filename = string.Join("", PresetFilenames.ScreensFilename.Replace("eboard", escreen.EBID));

               var path = Path.Combine(screensPath, filename);

               var result = Saver.SaveJsonFile<EboardScreen>(path, escreen);

               // do saving of elements, after creating the required folder

               var elementfolderpath = Path.Combine(screensPath, escreen.EBID);

               var configsetup = new ConfigurationSetup();

               await Saver.CreateFolderAsync(elementfolderpath);

               var subfolderExists = Loader.DirExists(elementfolderpath);

               if (Loader.DirExists(elementfolderpath))
               {
                   var escreenElementSaveResult = await SaveScreenElements(escreen.Elements, elementfolderpath);

                   // TODO: find a solution for logging or processing the resultmessage list of the element save function if need be.
               }

               feedbackMessages.Add(new EBoardFeedbackMessage()
               {
                   ResultMessage = $"{result}",
                   TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown
               });
           });

        return Task.FromResult(feedbackMessages);
    }

}
