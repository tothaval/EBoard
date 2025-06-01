namespace EBoardSDK.Plugins.Addons.SoundMix;

using EBoardSDK.Enums;
using EBoardSDK.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public class SoundMixMainViewModel : EBoardElementPluginBaseViewModel
{
    private string pluginHeader = "SoundMix Addon Element";

    private string pluginName = "SoundMix";

    public SoundMixMainViewModel()
    {
        this.BrushManagement ??= new BrushManagement();

        this.BorderManagement ??= new BorderManagement();

        this.PluginLogo ??= new ImageBrush();
    }

    public override PluginCategories PluginCategory => PluginCategories.Addon;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    public override bool NoDefaultBorders { get; } = false;

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "SoundMix";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EBoardSDK;component/Plugins/Addons/EEP_SoundMix/SoundMixResources.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(SoundMixMainView);

    public override Type ElementPluginViewModel => typeof(SoundMixMainViewModel);

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }
}

// EOF