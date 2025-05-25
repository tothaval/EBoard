/*  BudgetWatcher (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BudgetItemViewModel : BaseViewModel
 *  
 */
using CommunityToolkit.Mvvm.ComponentModel;
using EEP_BudgetWatcher.Enums;
using EEP_BudgetWatcher.Models;

using System.Windows;
using System.Windows.Media;


namespace EEP_BudgetWatcher.ViewModels.ViewLess;


public class BudgetItemViewModel : ObservableObject
{

    // add a BudgetHolder enum?
    // plausible values could be BankAccount, Cash, Wallet, CreditCard
    // they could be used to build statistics or offer more detail on output
    // of the financial situation of the budget 


    // trennung mvvm, vielleicht so umbauen, dass nur auf eine instanz des modelcodes zugegriffen wird und
    // nicht auf zich verschiedene, dazu müsste ich erstmal sauber trennen, die verwaltung der daten bauen
    // und diese in eine sauberere lösung des programms als abhängigkeit einbinden.

    // Properties & Fields
    #region Properties & Fields

    private BudgetItem _BudgetItem;
    public BudgetItem GetBudgetItem => _BudgetItem;

    public BudgetViewModel BudgetViewModel { get; }


    public Brush GainExpenseBrush
    {
        get
        {
            if (Type.Equals(BudgetTypes.Expense))
            {
                return (SolidColorBrush)Application.Current.Resources["ExpenseBrush"];
            }
            else if (Type.Equals(BudgetTypes.Gain))
            {
                return (SolidColorBrush)Application.Current.Resources["GainBrush"];
            }
            else
            {

                // just in case: SelectionBrush was chosen as return value because
                // TextBrush would lead to problems with readonly textboxes and textblocks,
                // because TextBrush and BackgroundBrush values are switched on those.
                // anyway, right now (2024/07/11) there is no plan to expand BudgetTypes
                // beyond the two values Gain and Expense
                return (SolidColorBrush)Application.Current.Resources["SelectionBrush"];
            }
        }
    }


    public BudgetTypes Type
    {
        get { return _BudgetItem.Type; }
        set
        {
            _BudgetItem.Type = value;
            OnPropertyChanged(nameof(Type));


            OnPropertyChanged(nameof(GainExpenseBrush));

            ValueChange?.Invoke(this, EventArgs.Empty);
        }
    }


    public DateTime Date
    {
        get { return _BudgetItem.Date; }
        set
        {
            _BudgetItem.Date = value;
            OnPropertyChanged(nameof(Date));

            //ValueChange?.Invoke(this, EventArgs.Empty);
        }
    }


    public string Item
    {
        get { return _BudgetItem.Item; }
        set
        {
            _BudgetItem.Item = value;
            OnPropertyChanged(nameof(Item));

            //ValueChange?.Invoke(this, EventArgs.Empty);
        }
    }


    public int Quantity
    {
        get { return _BudgetItem.Quantity; }
        set
        {
            _BudgetItem.Quantity = value;
            OnPropertyChanged(nameof(Quantity));
            OnPropertyChanged(nameof(Result));

            ValueChange?.Invoke(this, EventArgs.Empty);
        }
    }


    public decimal Sum
    {
        get { return _BudgetItem.Sum; }
        set
        {
            _BudgetItem.Sum = value;
            OnPropertyChanged(nameof(Sum));
            OnPropertyChanged(nameof(Result));

            ValueChange?.Invoke(this, EventArgs.Empty);
        }
    }


    public decimal Result
    {
        get { return _BudgetItem.Result; }
    }

    #endregion


    // event Properties & Fields
    #region event Properties & Fields

    public EventHandler ValueChange;

    #endregion


    // Constructors
    #region Constructors

    public BudgetItemViewModel(BudgetItem budgetItem, BudgetViewModel budgetViewModel)
    {
        BudgetViewModel = budgetViewModel;
        _BudgetItem = budgetItem;
    }

    #endregion


    // Methods
    #region Methods

    public void UpdateGainExpenseBrush()
    {
        OnPropertyChanged(nameof(GainExpenseBrush));
    }

    #endregion


}
// EOF