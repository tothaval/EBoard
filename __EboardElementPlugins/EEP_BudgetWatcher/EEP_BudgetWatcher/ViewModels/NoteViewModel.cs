// <copyright file="NoteViewModel.cs" company=".">
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
 *  NoteViewModel  : BaseViewModel
 * 
 *  viewmodel for Note model
 */
namespace EEP_BudgetWatcher.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EEP_BudgetWatcher.Models;

public partial class NoteViewModel : ObservableObject
{
    private Note _Note;
    public Note GetNote => _Note;

    public int ID
    {
        get { return _Note.ID; }
        set
        {
            _Note.ID = value;
            OnPropertyChanged(nameof(ID));
        }
    }

    public string Title
    {
        get { return _Note.Title; }
        set
        {
            _Note.Title = value;

            DateTime_Edited = CurrentDateTime;

            OnPropertyChanged(nameof(Title));
        }
    }

    public DateTime CurrentDateTime => DateTime.Now;


    public DateTime DateTime_Created
    {
        get { return _Note.DateTime_Created; }
        set
        {
            _Note.DateTime_Created = value;
            OnPropertyChanged(nameof(DateTime_Created));
        }
    }

    public DateTime DateTime_Edited
    {
        get { return _Note.DateTime_Edited; }
        set
        {
            _Note.DateTime_Edited = value;
            OnPropertyChanged(nameof(DateTime_Edited));
        }
    }

    public string Content
    {
        get { return _Note.Content; }
        set
        {
            _Note.Content = value;

            DateTime_Edited = CurrentDateTime;

            OnPropertyChanged(nameof(Content));
        }
    }

    public NoteViewModel(Note note)
    {
        _Note = note;

        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        Timer.Tick += Timer_Tick;

        Timer.Interval = new TimeSpan(0, 0, 0, 0, 125);

        Timer.Start();
    }

    [RelayCommand]
    private void NewEntry()
    {
        DateTime_Edited = CurrentDateTime;
        Content = Content.Insert(0, $"{DateTime_Edited}\n\n\n");
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(CurrentDateTime));
    }
}

// EOF