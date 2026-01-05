// <copyright file="Budget.cs" company=".">
// Stephan Kammel
// </copyright>
/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may 
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial 
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
/*  BudgetWatcher (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Budget
 * 
 *  serializable data model class
 */
namespace EEP_BudgetWatcher.Models;

using System.Collections.ObjectModel;
using System.Xml.Serialization;

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