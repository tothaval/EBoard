/*  BudgetWatcher (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BudgetItem
 * 
 *  serializable data model class
 */

using EEP_BudgetWatcher.Enums;


namespace EEP_BudgetWatcher.Models;


[Serializable]
public class BudgetItem
{

    // Properties & Fields
    #region Properties & Fields

    public BudgetIntervals Interval { get; set; } = BudgetIntervals.Once;

    public BudgetTypes Type { get; set; } = BudgetTypes.Expense;

    public DateTime Date { get; set; } = DateTime.Now;

    public string Item { get; set; } = "description";

    public int Quantity { get; set; } = 1;

    public decimal Sum { get; set; } = 0.0m;

    public decimal Result => Sum * Quantity;

    #endregion


    // Constructors
    #region Constructors

    public BudgetItem()
    {

    }

    #endregion

}
// EOF