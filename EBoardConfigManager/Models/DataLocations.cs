// <copyright file="BasicConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace EBoardConfigManager.Models;

using System.Text.Json.Serialization;

[JsonSerializable(typeof(DataLocations))]
public class DataLocations
{
    public string EBoardDataContextPath { get; set; } = string.Empty;

    public static string EBoardDataRootPath { get; } = @"Eboard\";

    public static string EBoardInstalledPluginsPath { get; } = @"InstalledPlugins\";

    public static string EBoardScreenDataPath { get; } = @"EboardData\";

    [JsonIgnore]
    public string EBoardDataPath => Path.Combine(this.EBoardDataContextPath, EBoardDataRootPath);
}

// EOF