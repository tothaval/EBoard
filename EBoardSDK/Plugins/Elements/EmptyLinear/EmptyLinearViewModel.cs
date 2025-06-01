namespace EBoardSDK.Plugins.Elements.EmptyLinear;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "Empty Linear Element";

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    private string pluginName = "EmptyLinear";

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "Linear";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(EmptyLinearView);

    public override Type ElementPluginViewModel => typeof(EmptyLinearViewModel);

    public EmptyLinearViewModel() => this.InstantiateProperties();

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