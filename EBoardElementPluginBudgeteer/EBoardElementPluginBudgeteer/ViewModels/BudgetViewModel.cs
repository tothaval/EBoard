using BudgetManagement.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace EBoardElementPluginBudgeteer.ViewModels;

public partial class BudgetViewModel : ObservableObject
{

#if DEBUG
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
#endif

    private BudgetServiceViewModel _BudgetServiceViewModel;

    private IBudget _Budget;
    public IBudget Budget => _Budget;

    public bool RemoveBudgetChangeCanExecute => SelectedBudgetChange != null;

    public DateTime BudgetPeriodStart
    {
        get => _Budget.BudgetPeriodStart;

        set
        {
            if (_Budget.BudgetPeriodStart != value)
            {
                _Budget.BudgetPeriodStart = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime BudgetPeriodEnd
    {
        get => _Budget.BudgetPeriodEnd;

        set
        {
            if (_Budget.BudgetPeriodEnd != value)
            {
                _Budget.BudgetPeriodEnd = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal CurrentBudget
    {
        get => _Budget.GetCurrentBalance();
    }

    public string Identifier
    {
        get => _Budget.Identifier;

        set
        {
            if (_Budget.Identifier != value)
            {
                _Budget.Identifier = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal InitialBudget
    {
        get => _Budget.InitialBudget;

        set
        {
            if (_Budget.InitialBudget != value)
            {
                _Budget.InitialBudget = value;
                OnPropertyChanged();
            }
        }
    }

    public string BudgetName
    {
        get => _Budget.BudgetName;

        set
        {
            if (_Budget.BudgetName != value)
            {
                _Budget.BudgetName = value;
                OnPropertyChanged();
            }
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RemoveBudgetChangeCanExecute))]
    [NotifyCanExecuteChangedFor(nameof(RemoveChangeCommand))]
    private BudgetChangeViewModel? _SelectedBudgetChange;

    [ObservableProperty]
    private ObservableCollection<BudgetChangeViewModel> _BudgetChanges = new ObservableCollection<BudgetChangeViewModel>();

    public BudgetViewModel(IBudget budget, BudgetServiceViewModel budgetServiceViewModel)
    {
#if DEBUG
        Logger.Debug("BudgetViewModel(IBudget budget, BudgetServiceViewModel budgetServiceViewModel) constructor init.");
#endif
        _Budget = budget;

        BuildBudgetChangeViewModelCollection();

        AddBudgetChangeIfEmpty();

        SelectedBudgetChange = BudgetChanges?.First();
        _BudgetServiceViewModel = budgetServiceViewModel;

#if DEBUG
        Logger.Debug("BudgetViewModel(IBudget budget, BudgetServiceViewModel budgetServiceViewModel) constructor end.");
#endif
    }

    private void BuildBudgetChangeViewModelCollection()
    {
#if DEBUG
        Logger.Debug("BuildBudgetChangeViewModelCollection start.");
#endif
        foreach (IBudgetChange item in _Budget.BudgetChanges)
        {
            BudgetChanges.Add(new BudgetChangeViewModel(item, this));
        }

#if DEBUG
        Logger.Debug("BuildBudgetChangeViewModelCollection end.");
#endif
    }

    public bool AddBudgetChangeIfEmpty()
    {
        if (BudgetChanges.Count > 0) return false;

        AddChange();

        return true;
    }

    public Task CurrentBudgetRefresh()
    {
        OnPropertyChanged(nameof(CurrentBudget));

        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task AddChange()
    {
        BudgetChanges.Insert(0, new BudgetChangeViewModel(_Budget.NewBudgetChange(), this));

        OnPropertyChanged(nameof(BudgetChanges));
        OnPropertyChanged(nameof(CurrentBudget));
        SelectedBudgetChange = BudgetChanges?.First();

        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(RemoveBudgetChangeCanExecute))]
    public Task RemoveChange()
    {
        if (SelectedBudgetChange != null)
        {
            _Budget?.RemoveBudgetChange(SelectedBudgetChange.BudgetChange);

            BudgetChanges.Remove(SelectedBudgetChange);
        }

        SelectedBudgetChange = null;

        OnPropertyChanged(nameof(BudgetChanges));

        return Task.CompletedTask;
    }

}
