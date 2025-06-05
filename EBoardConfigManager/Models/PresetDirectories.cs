// <copyright file="PresetDirectories.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EBoardConfigManager.Models;

public class PresetDirectories
{
#if RELEASE

    public string DefaultConfigFolder => GenerateFolderName("config");

    public string DefaultPluginsFolder => GenerateFolderName("plugins");

    public string DefaultScreensFolder => GenerateFolderName("screens");
#endif

    public string DefaultDebugLogFolder => @"Logs\";

    private string GenerateFolderName(string subfolder)
    {
        var assemblyfile = new FileInfo(Environment.ProcessPath).DirectoryName;

        var folderpath = Path.Combine(assemblyfile, subfolder);

        return folderpath;
    }

#if DEBUG

    public string DefaultConfigFolder => @"..\..\..\__DEBUG_config\";

    public string DefaultPluginsFolder => @"..\..\..\__DEBUG_plugins\";

    public string DefaultScreensFolder => @"..\..\..\__DEBUG_screens\";
#endif
}
