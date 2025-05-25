using System.Text.Json.Serialization;

namespace EBoardConfigManager.Models;

[JsonSerializable(typeof(ConfigPaths))]
public class ConfigPaths
{
    public string ConfigFolder { get; set; } = string.Empty;

    public string PluginFolder { get; set; } = string.Empty;

    public string ScreensFolder { get; set; } = string.Empty;
}
