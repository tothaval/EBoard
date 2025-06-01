using BudgetManagement;
using BudgetManagement.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardElementPluginBudgeteer.Views;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
using Serilog;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EBoardElementPluginBudgeteer.ViewModels;

public partial class BudgetServiceViewModel : EBoardElementPluginBaseViewModel
{
    public override bool NoDefaultBorders { get; } = false;

    public override PluginCategories PluginCategory => PluginCategories.Addon;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    private string pluginHeader = "Budgeteer Element";
    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "Budgeteer";
    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "Budgeteer";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EBoardElementPluginBudgeteer;component/ElementPluginResourceDictionary.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(BudgetServiceView);

    public override Type ElementPluginViewModel => typeof(BudgetServiceViewModel);

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

    private BudgetService _BugdetService = new BudgetService();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RemoveBudgetCanExecute))]
    [NotifyCanExecuteChangedFor(nameof(RemoveBudgetCommand))]
    private BudgetViewModel _SelectedBudget;

    public bool RemoveBudgetCanExecute => SelectedBudget != null;

    [ObservableProperty]
    private ObservableCollection<BudgetViewModel> _Budgets = new ObservableCollection<BudgetViewModel>();

    //    public BudgetServiceViewModel(BudgetService bugdetService)
    //    {

    //#if DEBUG
    //        Logger.Debug("BudgetServiceViewModel(BudgetService bugdetService) constructor init.");
    //#endif

    //        _BugdetService = bugdetService;

    //        BuildBudgetViewModelCollection();

    //        AddBudgetIfEmpty();

    //        SelectedBudget = Budgets.First();

    //#if DEBUG
    //        Logger.Debug("BudgetServiceViewModel(BudgetService bugdetService) constructor end.");
    //#endif
    //    }

    private void BuildBudgetViewModelCollection()
    {

#if DEBUG
        Log.Debug("BuildBudgetViewModelCollection start.");
#endif

        foreach (IBudget item in _BugdetService.Budgets)
        {
            Budgets.Add(new BudgetViewModel(item, this));
        }

#if DEBUG
        Log.Debug("BuildBudgetViewModelCollection end.");
#endif

    }


    [RelayCommand]
    public Task AddBudget()
    {

#if DEBUG
        Log.Debug("AddBudget start.");
#endif

        Budgets?.Insert(0, new BudgetViewModel(_BugdetService.NewBudget(), this)); // das bugdetitem aus dem budgetservice holen oder anders?

        SelectedBudget = Budgets?.First()!;

#if DEBUG

        Log.Debug("Budgets:{0}", string.Join("", Budgets?.Select(x => $"\n{x.Identifier} - {x.BudgetName} : {x.InitialBudget} >> {x.CurrentBudget} :budget changes: {x.BudgetChanges.Count} :: {x.BudgetPeriodStart} - {x.BudgetPeriodEnd}")!));

        //foreach (BudgetViewModel item in Budgets)
        //{
        //    Logger.Debug("{0} - {1} : {2} >> {3} :budget changes: {4} :: {5} - {6}",
        //        item.Identifier,                // 0
        //        item.BudgetName,                // 1
        //        item.InitialBudget,             // 2
        //        item.CurrentBudget,             // 3
        //        item.BudgetChanges.Count,       // 4
        //        item.BudgetPeriodStart,         // 5
        //        item.BudgetPeriodEnd            // 6
        //        );
        //}

        Log.Debug("AddBudget end.");
#endif

        return Task.CompletedTask;
    }

    public bool AddBudgetIfEmpty()
    {
#if DEBUG
        Log.Debug("AddBudgetIfEmpty start.");
#endif

        if (Budgets.Count > 0) return false;

        AddBudget();

#if DEBUG
        Log.Debug("AddBudgetIfEmpty end.");
#endif
        return true;
    }

    [RelayCommand(CanExecute = nameof(RemoveBudgetCanExecute))]
    public Task RemoveBudget()
    {

#if DEBUG
        Log.Debug("RemoveBudget start.");
#endif

        /// make clean mvvm, move the messagebox.show to the view,
        /// use delegate command to invoke logic upon click
        /// implement on budgetChange as well

        MessageBoxResult result = MessageBox.Show(
                $"Delete selected budget?",
                "Remove Budget", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {

#if DEBUG
            Log.Debug("Removing {0}..", SelectedBudget.Identifier);
#endif

            _BugdetService.RemoveBudget(SelectedBudget.Budget);
            Budgets.Remove(SelectedBudget);

            SelectedBudget = null;
        }

#if DEBUG
        Log.Debug("RemoveBudget complete.");
#endif

        return Task.CompletedTask;
    }
}