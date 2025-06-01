using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EBoardSDK.Plugins.Elements.EmptyRadial;

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
        GradientOrigin = new Point(0.25, 0.5),
    };

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "Empty Radial Element";

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    private string pluginName = "EmptyRadial";

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "Radial";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(EmptyRadialView);

    public override Type ElementPluginViewModel => typeof(EmptyRadialViewModel);

    public EmptyRadialViewModel() => this.InstantiateProperties();

    private void InstantiateProperties()
    {
        this.BorderManagement = new BorderManagement();
        this.BrushManagement = new BrushManagement();
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