namespace EBoardSDK.Interfaces;

using EBoardSDK.Enums;
using EBoardSDK.Models;
using System.Windows.Controls;
using System.Windows.Media;

public interface IPlugin : IElementTransform
{
    public BorderManagement BorderManagement { get; set; }

    public BrushManagement BrushManagement { get; set; }

    public abstract ImageBrush PluginLogo { get; set; }

    public abstract UserControl Plugin { get; }

    public abstract PluginCategories PluginCategory { get; }

    public abstract string PluginHeader { get; set; }

    public abstract string PluginName { get; set; }

    public abstract bool NoDefaultBorders { get; }

    public bool ApplyBrush(Brush brush, BrushTargets brushTargets);

    public bool ApplyRedraw();

    public Task<EBoardFeedbackMessage> Load(string path);

    public Task<EBoardFeedbackMessage> Save(string path);
}
