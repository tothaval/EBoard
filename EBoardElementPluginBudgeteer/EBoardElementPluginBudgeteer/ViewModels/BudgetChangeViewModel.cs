using BudgetManagement.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EBoardElementPluginBudgeteer.ViewModels;

public class BudgetChangeViewModel : ObservableObject
{

    private BudgetViewModel _BudgetViewModel;

    private IBudgetChange _BudgetChange;
    public IBudgetChange BudgetChange => _BudgetChange;

    public DateTime BudgetChangeDate
    {
        get => BudgetChange.BudgetChangeDate;
        set
        {
            if (BudgetChange.BudgetChangeDate != value)
            {
                BudgetChange.BudgetChangeDate = value;
                OnPropertyChanged();
            }
        }
    }

    public BudgetChangeType Type
    {
        get => BudgetChange.Type;
        set
        {
            if (BudgetChange.Type != value)
            {
                BudgetChange.Type = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPrice));

                _BudgetViewModel.CurrentBudgetRefresh();
            }
        }
    }

    public string Item
    {
        get => BudgetChange.Item;
        set
        {
            if (BudgetChange.Item != value)
            {
                BudgetChange.Item = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal Price
    {
        get => BudgetChange.Price;
        set
        {
            if (BudgetChange.Price != value)
            {
                BudgetChange.Price = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPrice));

                _BudgetViewModel.CurrentBudgetRefresh();
            }
        }
    }

    public int Quantity
    {
        get => BudgetChange.Quantity;
        set
        {
            if (BudgetChange.Quantity != value)
            {
                BudgetChange.Quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPrice));

                _BudgetViewModel.CurrentBudgetRefresh();
            }
        }
    }

    public decimal TotalPrice
    {
        get => BudgetChange.TotalPrice;
    }

    public BudgetChangeViewModel(IBudgetChange budgetChange, BudgetViewModel budgetViewModel)
    {
        this._BudgetChange = budgetChange;
        this._BudgetViewModel = budgetViewModel;
    }

}
