namespace EBoardSDK.Plugins.Tools.Uptime;

using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.StandardText;

using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Reflection;

public partial class UptimeViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private string clock;

    [ObservableProperty]
    private string uptime;
    
    private DispatcherTimer _timer;

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    private string pluginHeader = "Uptime Timer Element";

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "UptimeTimer";

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "Uptimer";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() ;

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(UptimeView);

    public override Type ElementPluginViewModel => typeof(UptimeViewModel);

    public UptimeViewModel() => InstantiateProperties();

    private void InstantiateProperties()
    {
        BorderManagement = new BorderManagement();
        BrushManagement = new BrushManagement();

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(200);
        _timer.Tick += _timer_Tick; ;

        _timer.Start();
    }

    private void UpdateOutput()
    {
        long tickCountMs = Environment.TickCount64;

        var uptimeValue = TimeSpan.FromMilliseconds(tickCountMs);
        DateTime currentTime = DateTime.Now;
        Clock = $"{DateTime.Now.ToLocalTime()}";
        Uptime = $"{uptimeValue.Hours:D2}:{uptimeValue.Minutes:D2}:{uptimeValue.Seconds:D2}";
    }

    private void _timer_Tick(object? sender, EventArgs e)
    {
        UpdateOutput();
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