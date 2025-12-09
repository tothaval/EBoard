// <copyright file="EboardScreen.cs" company=".">
// Stephan Kammel
// </copyright>
namespace EBoardSDK.Models;

using EBoardSDK.Models.DataSets;
using System.IO;
using System.Text.Json.Serialization;

public class EboardScreen
{
    public string EBID { get; set; } = string.Empty;

    public int ID { get; set; } = -1;

    public int EBoardDepth { get; set; } = 1;

    public string EBoardName { get; set; } = string.Empty;

    public BorderDataSet BorderDataSet { get; set; } = new(new BorderManagement());

    public BrushDataSet BrushDataSet { get; set; } = new(new BrushManagement());

    [JsonIgnore]
    public IList<ElementConfig> Elements { get; set; } = [];

    [JsonIgnore]
    public IList<FileInfo> ContentDataFilePaths { get; set; } = [];
}
