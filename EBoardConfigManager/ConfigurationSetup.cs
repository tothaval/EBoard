// <copyright file="ConfigurationSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardConfigManager.Models;

namespace EBoardConfigManager
{
    public class ConfigurationSetup
    {
        public static PresetDirectories PresetDirectories => new PresetDirectories();

        public static PresetFilenames PresetFilenames => new PresetFilenames();

        public ConfigurationSetup()
        {
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

        public async Task<bool> Run<T>()
        {
            var pathsearch = this.SearchForConfiguration().Result;
            var foldersearch = this.SearchForDefaultConfiguration().Result;

            if (foldersearch.Equals(Result.Success) && pathsearch.Equals(Result.Success))
            {
                return true;
            }

            _ = await this.SetupPresets<T>();

            var search = this.SearchForDefaultConfiguration().Result;

            return search.Equals(Result.Success);
        }

        private Task<Result> SearchForConfiguration()
        {
            if (!Loader.FileExists(PresetFilenames.PathsFilename))
            {
                return Task.FromResult(Result.Error);
            }

            return Task.FromResult(Result.Success);
        }

        private Task<Result> SearchForDefaultConfiguration()
        {
            if (Loader.DirExists(PresetDirectories.DefaultConfigFolder)
                && Loader.DirExists(PresetDirectories.DefaultPluginsFolder)
                && Loader.DirExists(PresetDirectories.DefaultScreensFolder))
            {
                return Task.FromResult(Result.Success);
            }

            return Task.FromResult(Result.Unkown);
        }

        private async Task<Result> SetupPresets<T>()
        {
            var configPaths = new ConfigPaths()
            {
                ConfigFolder = PresetDirectories.DefaultConfigFolder,
                PluginFolder = PresetDirectories.DefaultPluginsFolder,
                ScreensFolder = PresetDirectories.DefaultScreensFolder,
            };

            var result = Saver.SaveJsonFile(PresetFilenames.PathsFilename, configPaths);

            if (result.Equals(Result.Success))
            {
                await Saver.CreateFolderAsync(configPaths.ConfigFolder);
                await Saver.CreateFolderAsync(configPaths.PluginFolder);
                await Saver.CreateFolderAsync(configPaths.ScreensFolder);
            }

            var config = Activator.CreateInstance(typeof(T));

            if (new DirectoryInfo(configPaths.ConfigFolder).Exists)
            {
                Saver.SaveJsonFile(Path.Combine(configPaths.ConfigFolder, PresetFilenames.ConfigFilename), config);
            }

            return Result.Unkown;
        }
    }
}
