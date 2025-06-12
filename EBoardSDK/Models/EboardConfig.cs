// <copyright file="EboardConfig.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Models;

using EBoardSDK.Models.DataSets;
using EBoardSDK.Plugins;
using System.Text.Json.Serialization;

public class EboardConfig
{
    public int EBoardCount { get; set; } = 1;

    public int EBoardIndex { get; set; } = 0;

    public bool EBoardBrowserSwitch { get; set; } = true;

    public BorderDataSet BorderDataSet { get; set; } = new(new BorderManagement());

    public BrushDataSet BrushDataSet { get; set; } = new(new BrushManagement());

    public PlacementDataSet PlacementDataSet { get; set; } = new(new PlacementManagement());

    public BorderDataSet EBVBorderDataSet { get; set; } = new(new BorderManagement());

    public BrushDataSet EBVBrushDataSet { get; set; } = new(new BrushManagement());

    [JsonIgnore]
    public IList<EBoardElementPluginBaseViewModel> ElementPlugins { get; set; } = [];

    public IList<EBoardElementPluginBaseViewModel> ShapePlugins { get; set; } = [];

    public IList<EBoardElementPluginBaseViewModel> ToolPlugins { get; set; } = [];

#if DEBUG
    [JsonIgnore]
    public IList<EBoardElementPluginBaseViewModel> CurrentDevelopmentPlugins { get; set; } = [];
#endif
}
