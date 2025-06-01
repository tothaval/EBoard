/*  BudgetWatcher (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BudgetChangeViewModel : BaseViewModel
 * 
 *  viewmodel for BudgetChangeView
 *  
 *  allows for editing of BudgetViewModel
 *  
 *  is encapsulated within a MainViewModel
 */
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EEP_BudgetWatcher.Models;
using EEP_BudgetWatcher.ViewModels.ViewLess;
using Serilog;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;


namespace EEP_BudgetWatcher.ViewModels;


public partial class BudgetChangeViewModel : ObservableObject
{

    // Properties & Fields
    #region Properties & Fields

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BudgetItemViewModels))]
    private BudgetViewModel _BudgetViewModel;

    [ObservableProperty]
    private bool _ShowBugdetOverview;


    [ObservableProperty]
    private bool _ShowBugdetAccount;
    #endregion

    public BudgetChangeViewModel()
    {
        this.AddBudget(new BudgetViewModel(new Budget()));

    }

    public BudgetChangeViewModel(BudgetOverviewModel budgets)
    {        
        this.ApplyData(budgets);
    }

    public bool ApplyData(BudgetOverviewModel budgetOverviewModel)
    {
        if (budgetOverviewModel == null)
        {
            return false;
        }

        Budgets.Clear();

        budgetOverviewModel.Budgets.ForEach(budget =>
        {
            Budgets.Add(new BudgetViewModel(budget));
        });

        if (Budgets?.Count == 0)
        {
            this.AddBudget(new BudgetViewModel(new Budget()));
        }
        
        SelectFirst();

        return true;
    }


    // Collections
    #region Collections

    public ObservableCollection<BudgetItemViewModel> BudgetItemViewModels
    {
        get
        {
            if (BudgetViewModel != null)
            {
                return BudgetViewModel.BudgetItemViewModels;
            }
            else
            {
                return new ObservableCollection<BudgetItemViewModel>();
            }

        }

        set
        {
            BudgetViewModel.BudgetItemViewModels = value;
            OnPropertyChanged(nameof(BudgetItemViewModels));
        }
    }


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BudgetItemViewModels))]
    private ObservableCollection<BudgetViewModel> _Budgets = new ObservableCollection<BudgetViewModel>();

    #endregion



    // Methods
    #region Methods

    public void AddBudget(BudgetViewModel budgetViewModel)
    {
        Budgets?.Insert(0, budgetViewModel);

        SelectFirst();
        OnPropertyChanged(nameof(BudgetViewModel));
        OnPropertyChanged(nameof(Budgets));
        OnPropertyChanged(nameof(BudgetItemViewModels));

#if DEBUG
        Log.Information("added budget to budgets list");
#endif

    }


    [RelayCommand]
    private void AddExpense(object? parameter)
    {
        BudgetViewModel?.AddBudgetItem(
            new BudgetItem()
            {
                Interval = Enums.BudgetIntervals.Once,
                Type = Enums.BudgetTypes.Expense
            });

#if DEBUG

        Log.Information("added expense to budget");

#endif
    }


    [RelayCommand]
    private void AddGain(object? parameter)
    {
        BudgetViewModel?.AddBudgetItem(
            new BudgetItem()
            {
                Interval = Enums.BudgetIntervals.Once,
                Type = Enums.BudgetTypes.Gain
            });

#if DEBUG

        Log.Information("added gain to budget");
#endif
    }


    [RelayCommand]
    private void ClearAll(object? parameter)
    {
        BudgetViewModel?.Clear();
    }

    [RelayCommand]
    private void RemoveItem(object? parameter)
    {
        IList selection = (IList)parameter;

        if (selection != null)
        {
            MessageBoxResult result = MessageBox.Show(
                $"Do you want to delete selected budget item(s)?",
                "Remove Budget Item(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selected = selection.Cast<BudgetItemViewModel>().ToArray();

                foreach (var item in selected)
                {
                    BudgetViewModel?.RemoveBudgetItem(item.GetBudgetItem);
                }
            }
        }
    }


    public void RemoveBudget(BudgetViewModel budgetViewModel)
    {
        Budgets?.Remove(budgetViewModel);

        SelectFirst();
        OnPropertyChanged(nameof(BudgetViewModel));
        OnPropertyChanged(nameof(Budgets));
        OnPropertyChanged(nameof(BudgetItemViewModels));
    }


    private void SelectFirst()
    {
        if (Budgets?.Count > 0)
        {
            BudgetViewModel = Budgets.First();
        }
    }

    public void UpdateGainExpenseBrush()
    {
        BudgetViewModel?.UpdateGainExpenseBrush();
    }

    #endregion


}
// EOF