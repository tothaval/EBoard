namespace EBoardElementPluginMyNote;

using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public class MyNoteViewModel : EBoardElementPluginBaseViewModel
{
    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    private string pluginHeader = "MyNote Element";
    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "MyNote";
    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "MyNote";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EBoardElementPluginMyNote;component/ElementPluginResourceDictionary.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(MyNoteView);

    public override Type ElementPluginViewModel => typeof(MyNoteViewModel);

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }
}