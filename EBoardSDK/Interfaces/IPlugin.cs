namespace EBoardSDK.Interfaces;

using EBoardSDK.Models;
using EBoardSDK.Plugins;
using System.Windows.Controls;
using System.Windows.Media;

public interface IPlugin : IElementTransform
{
    public BorderManagement BorderManagement { get; set; }

    public BrushManagement BrushManagement { get; set; }

    public abstract UserControl Plugin { get; }

    public abstract string PluginHeader { get; set; }

    public abstract string PluginName { get; set; }

    public bool ApplyBackgroundBrush(Brush brush);

    public Task Load(string path, IElementDataSet elementDataSet);


    public Task Save(string path, IElementDataSet elementDataSet);


    public bool SelectionChange(bool isSelected);



}
