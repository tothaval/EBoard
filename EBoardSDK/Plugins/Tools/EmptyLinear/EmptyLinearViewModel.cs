namespace EBoardSDK.Plugins.Tools.EmptyLinear;

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

public partial class EmptyLinearViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private string content = "\t\t\t\n\n\n";

    [ObservableProperty]
    private LinearGradientBrush background = new LinearGradientBrush(
        [new GradientStop(Colors.AliceBlue, 0.0), new GradientStop(Colors.Navy, 0.5)],
        new Point(0, 0),
        new Point(0.5, 1)
    );

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    private string pluginHeader = "Empty Linear Element";

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "EmptyLinear";

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "Linear";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(UptimeView);

    public override Type ElementPluginViewModel => typeof(UptimeViewModel);

    public EmptyLinearViewModel() => InstantiateProperties();
     
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