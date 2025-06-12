// <copyright file="IPlugin.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Interfaces;

using EBoardSDK.Enums;
using EBoardSDK.Models;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public interface IPlugin
{
    public BorderManagement BorderManagement { get; set; }

    public BrushManagement BrushManagement { get; set; }

    public Assembly? ElementPluginAssembly { get; }

    public abstract ImageBrush PluginLogo { get; set; }

    public Type? ElementPluginModel { get; }

    public string ElementPluginName { get; }

    public Type ElementPluginView { get; }

    public Type ElementPluginViewModel { get; }

    public abstract ElementScreenIntegrationConstraints? ElementScreenIntegrationConstraints { get; set; }

    public abstract bool NoDefaultBorders { get; }

    public abstract UserControl Plugin { get; }

    public abstract PluginCategories PluginCategory { get; }

    public abstract string PluginHeader { get; set; }

    public abstract string PluginName { get; set; }

    public ResourceDictionary ResourceDictionary { get; }

    public bool ApplyBrush(Brush brush, BrushTargets brushTargets);

    public bool ApplyRedraw();

    public Task<EBoardFeedbackMessage> Load(string path);

    public Task<EBoardFeedbackMessage> Save(string path);
}
