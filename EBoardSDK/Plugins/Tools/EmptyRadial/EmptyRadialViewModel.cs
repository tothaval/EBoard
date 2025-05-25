using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using EBoardSDK.Plugins.Elements.StandardText;

using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EBoardSDK.Plugins.Tools.Uptime;
using System.Reflection;

namespace EBoardSDK.Plugins.Tools.EmptyRadial;

public partial class EmptyRadialViewModel : EBoardElementPluginBaseViewModel
{

    [ObservableProperty]
    private string content = "\t\t\t\n\n\n";

    [ObservableProperty]
    private RadialGradientBrush background = new RadialGradientBrush(
        [new GradientStop(Colors.AliceBlue, 0.0), new GradientStop(Colors.DarkSlateGray, 0.5)]
    )
    {
        Center = new Point(0.5, 0.5),
        GradientOrigin = new Point(0.25, 0.5)
    };

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    private string pluginHeader = "Empty Radial Element";

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "EmptyRadial";

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "Radial";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(EmptyRadialView);

    public override Type ElementPluginViewModel => typeof(EmptyRadialViewModel);

    public EmptyRadialViewModel() => InstantiateProperties();

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