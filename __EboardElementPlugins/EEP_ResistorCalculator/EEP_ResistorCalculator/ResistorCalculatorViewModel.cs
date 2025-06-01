namespace EEP_ResistorCalculator;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class ResistorCalculatorViewModel : EBoardElementPluginBaseViewModel
{
    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;
    private string pluginHeader = "ResistorCalculator Element";

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "ResistorCalculator";

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "ResistorCalculator";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary? ResourceDictionary => new() { Source = new Uri("/EEP_ResistorCalculator;component/DefaultResourceDictionary.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(ResistorCalculatorView);

    public override Type ElementPluginViewModel => typeof(ResistorCalculatorViewModel);

    public override Task<EBoardFeedbackMessage> Load(string path)
    {

        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }


    [ObservableProperty]
    private SolidColorBrush band1ColorIndicatorRectangle = new SolidColorBrush(Colors.Black);

    [ObservableProperty]
    private SolidColorBrush band2ColorIndicatorRectangle = new SolidColorBrush(Colors.Red);

    [ObservableProperty]
    private SolidColorBrush band3ColorIndicatorRectangle = new SolidColorBrush(Colors.Green);

    [ObservableProperty]
    private SolidColorBrush band4ColorIndicatorRectangle = new SolidColorBrush(Colors.Black);

    public ResistorCalculatorViewModel()
    {

    }

    //public new bool Initialize()
    //{
    //    try
    //    {
    //        if (string.IsNullOrWhiteSpace(this.PluginName))
    //        {
    //            return false;
    //        }

    //        var resourcestring = $"{PluginName}_Logo.png";

    //        var imagefile = new FileInfo(resourcestring);

    //        if (!imagefile.Exists)
    //        {
    //            return false;
    //        }

    //        var imagesource = new BitmapImage(new Uri(imagefile.FullName));

    //        this.PluginLogo = new ImageBrush(imagesource);

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error(ex.Message);
    //        //throw;
    //    }

    //    return false;
    //}
}
