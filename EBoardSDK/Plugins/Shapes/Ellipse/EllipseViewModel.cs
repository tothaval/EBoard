namespace EBoardSDK.Plugins.Shapes.Ellipse;

using EBoardSDK.Enums;
using EBoardSDK.Models;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class EllipseViewModel : EBoardElementPluginBaseViewModel
{
    public override PluginCategories PluginCategory => PluginCategories.Shape;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "Ellipse Shape Element";

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    private string pluginName = "EllipseShape";

    public override bool NoDefaultBorders { get; } = true;

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "Ellipse";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(EllipseView);

    public override Type ElementPluginViewModel => typeof(EllipseViewModel);

    public EllipseViewModel() => this.InstantiateProperties();

    private void InstantiateProperties()
    {
        this.BorderManagement = new BorderManagement();
        this.BrushManagement = new BrushManagement();

        this.BrushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;
    }

    private void BrushManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.BrushManagement.Border));
        this.OnPropertyChanged(nameof(this.BrushManagement));
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