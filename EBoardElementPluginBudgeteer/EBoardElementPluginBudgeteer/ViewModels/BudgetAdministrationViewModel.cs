using CommunityToolkit.Mvvm.ComponentModel;

namespace EBoardElementPluginBudgeteer.ViewModels;

public class BudgetAdministrationViewModel : ObservableObject
{

    private BudgetViewModel _BudgetViewModel;
    public BudgetViewModel BudgetViewModel => _BudgetViewModel;

    public BudgetAdministrationViewModel(BudgetViewModel budgetViewModel)
    {
        _BudgetViewModel = budgetViewModel;
    }

}