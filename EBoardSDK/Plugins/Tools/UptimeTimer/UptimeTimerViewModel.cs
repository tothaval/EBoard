// <copyright file="UptimeTimerViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Tools.Uptime;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

public partial class UptimeViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private string clock;

    [ObservableProperty]
    private string uptime;

    private DispatcherTimer timer;

    private string pluginHeader = "Uptime";

    private string pluginName = "Uptime";

    public UptimeViewModel()
    {
        this.ElementScreenIntegrationConstraints = new ElementScreenIntegrationConstraints(ElementInstantiationPolicy.OnePerScreen);

        Task.Delay(500);

        this.InstantiateProperties();
    }

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override PluginCategories PluginCategory => PluginCategories.Tool;

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "Uptimer";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(UptimeView);

    public override Type ElementPluginViewModel => typeof(UptimeViewModel);

    public static string ToolTipMessage => "Days : Hours : Minutes : Seconds";

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

    private void InstantiateProperties()
    {

        this.BorderManagement = new BorderManagement();
        this.BrushManagement = new BrushManagement();

        this.timer = new DispatcherTimer();
        this.timer.Interval = TimeSpan.FromMilliseconds(200);
        this.timer.Tick += this.Timer_Tick; ;

        this.timer.Start();
    }

    private void UpdateOutput()
    {
        long tickCountMs = Environment.TickCount64;

        var uptimeValue = TimeSpan.FromMilliseconds(tickCountMs);
        DateTime currentTime = DateTime.Now;
        this.Clock = $"{DateTime.Now.ToLocalTime()}";
        this.Uptime = $"{uptimeValue.Days:D2}:{uptimeValue.Hours:D2}:{uptimeValue.Minutes:D2}:{uptimeValue.Seconds:D2}";
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        this.UpdateOutput();
    }
}

// EOF