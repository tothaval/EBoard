namespace EEP_BudgetWatcher.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.SharedMethods;
using EEP_BudgetWatcher.Models;
using EEP_BudgetWatcher.Resources;
using EEP_BudgetWatcher.Views;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Serialization;

public partial class BudgetWatcherMainViewModel : EBoardElementPluginBaseViewModel
{
    public override PluginCategories PluginCategory => PluginCategories.Addon;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

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

    public async override Task<EBoardFeedbackMessage> Load(string path)
    {
        if (new DirectoryInfo(path).Exists)
        {
            string contentfilename = "content.xml";

            path = Path.Combine(path, contentfilename);
        }

        try
        {
            var data = await new SharedMethod_Plugins().DeserializeConfigFiles<BudgetOverviewModel>(path)!;

            if (data != null)
            {
                BudgetChangeViewModel.ApplyData(data);

                return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"deserialized {path}" };
            }
        }
        catch (Exception ex)
        {
            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message };
        }

        return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Unknown, ResultMessage = "" };
    }

    public async override Task<EBoardFeedbackMessage> Save(string path)
    {
        EBoardFeedbackMessage? serializationResult = null;

        List<Budget> budgets = [];

        BudgetChangeViewModel.Budgets.ToList().ForEach(budgetviewmodel =>
        {
            budgets.Add(budgetviewmodel.GetBudget);
        });

        var budgetdata = new BudgetOverviewModel(budgets);

        serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(budgetdata, path);

        if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        {
            // TODO do stuff
        }

        return serializationResult!;
    }

    public BudgetWatcherMainViewModel()
    {
        SetInitialResources();

        _BudgetChangeViewModel = new BudgetChangeViewModel();

        SetupField = new SetupFieldViewModel();

        BrushManagement.PropertyChangedEvent += BrushManagement_PropertyChangedEvent;

    }

    private void BrushManagement_PropertyChangedEvent()
    {
        RegisterResources();
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
        var resourceChangeApplied = new ResourceSet().ApplyBrushManagement(BrushManagement);
    }
    private void SetInitialResources()
    {
        new ResourceSet().SetResources();
    }
}
