namespace EBoardSDK.Plugins.Shapes.Ellipse;

using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.StandardText;

using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EBoardSDK.Plugins.Tools.Uptime;
using System.Reflection;

public partial class EllipseViewModel : EBoardElementPluginBaseViewModel
{
    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    private string pluginHeader = "Ellipse Shape Element";

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "EllipseShape";

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "Ellipse";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(EllipseView);

    public override Type ElementPluginViewModel => typeof(EllipseViewModel);

    public EllipseViewModel() => InstantiateProperties();

    public bool ApplyBackgroundBrush(Brush brush)
    {
        try
        {
            BrushManagement.Background = brush;

            OnPropertyChanged(nameof(BrushManagement));

            OnPropertyChanged(nameof(BrushManagement.Background));

            return true;
        }
        catch (Exception)
        {

            return false;
        }
    }

    private void InstantiateProperties()
    {
        BorderManagement = new BorderManagement();
        BrushManagement = new BrushManagement();
    }

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

}// EOF