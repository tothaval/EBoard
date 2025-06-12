/*  BudgetWatcher (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Budget
 * 
 *  serializable data model class
 */

using System.Collections.ObjectModel;
using System.Xml.Serialization;


namespace EEP_BudgetWatcher.Models;


[Serializable]
public class Budget
{

    // Properties & Fields
    #region Properties & Fields        

    public DateTime Begin { get; set; } = DateTime.Now;
    public DateTime End { get; set; } = DateTime.Now;

    public decimal CurrentBalance => InitialBudget + Gains - Expenses;
    public decimal InitialBudget { get; set; } = 0.0m;
    public decimal Expenses { get; set; } = 0.0m;
    public decimal Gains { get; set; } = 0.0m;
    public Note Note { get; set; } = new Note();

    #endregion


    // Collections
    #region Collections

    [XmlArray("BudgetChanges")]
    public ObservableCollection<BudgetItem> BudgetChanges { get; set; } = new ObservableCollection<BudgetItem>();

    #endregion


    // Constructors
    #region Constructors

    public Budget()
    {

    }

    #endregion


}
// EOF