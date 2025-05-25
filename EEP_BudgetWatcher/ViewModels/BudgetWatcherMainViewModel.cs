namespace EEP_BudgetWatcher.ViewModels;

using EBoardSDK;
using EBoardSDK.Plugins;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using EEP_BudgetWatcher.Views;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Serialization;
using EEP_BudgetWatcher.Resources;
using CommunityToolkit.Mvvm.Input;
using EEP_BudgetWatcher.Models;

public partial class BudgetWatcherMainViewModel : EBoardElementPluginBaseViewModel
{
    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    private string pluginHeader = "BudgetWatcher Element";

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "BudgetWatcher";

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "BudgetWatcher";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EEP_BudgetWatcher;component/Themes/Default.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(BudgetWatcherMainView);

    public override Type ElementPluginViewModel => typeof(BudgetWatcherMainViewModel);

    private readonly BudgetChangeViewModel _BudgetChangeViewModel;
    public BudgetChangeViewModel BudgetChangeViewModel => _BudgetChangeViewModel;


    public SetupFieldViewModel SetupField { get; }


    [ObservableProperty]
    private bool _ShowBudget;


    [ObservableProperty]
    private bool _ShowBudgetOverview;


    [ObservableProperty]
    private bool _ShowNotes;


    [ObservableProperty]
    private bool _ShowSetup;

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

    public BudgetWatcherMainViewModel()
    {
        RegisterResources();

        _BudgetChangeViewModel = new BudgetChangeViewModel();

        BudgetChangeViewModel.Initialize();

        SetupField = new SetupFieldViewModel();

    }

    [RelayCommand]
    private void AddBudget()
    {
        BudgetChangeViewModel.AddBudget(new ViewLess.BudgetViewModel(new Budget()));       
    }

    [RelayCommand]
    private void RemoveBudget()
    {
        BudgetChangeViewModel.RemoveBudget(BudgetChangeViewModel.BudgetViewModel);
    }


    private void RegisterResources()
    {
        string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        string filter = "*.xml";
        List<string> files = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly).ToList();


        if (!Directory.EnumerateFiles(folder).Any(f => f.Contains("resources.xml")))
        {
            Application.Current.Resources["Language"] = "English";

            Application.Current.Resources["FS"] = 14.0;
            Application.Current.Resources["FF"] = new FontFamily("Verdana");

            Application.Current.Resources["HFS"] = 14 * 1.25;

            Application.Current.Resources["Button_CornerRadius"] = new CornerRadius(5);

            Application.Current.Resources["VisibilityField_CornerRadius"] = new CornerRadius(5);


            Application.Current.Resources["BackgroundBrush"] = new SolidColorBrush(Colors.White);
            Application.Current.Resources["TextBrush"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["HeaderBrush"] = new SolidColorBrush(Colors.YellowGreen);
            Application.Current.Resources["SelectionBrush"] = new SolidColorBrush(Colors.Gray);
            Application.Current.Resources["GainBrush"] = new SolidColorBrush(Colors.Green);
            Application.Current.Resources["ExpenseBrush"] = new SolidColorBrush(Colors.Red);

            //MessageBox.Show(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag).ToString());

            Application.Current.Resources["Culture"] = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);

        }


        foreach (string file in files)
        {
            if (file.EndsWith("resources.xml"))
            {
                var xmlSerializer = new XmlSerializer(typeof(ResourceSet));

                using (var writer = new StreamReader(file))
                {
                    try
                    {
                        var member = (ResourceSet)xmlSerializer.Deserialize(writer);

                        member.SetResources();

                        break;

                    }
                    catch (Exception)
                    {
                    }
                }
            }

        }

    }
}
