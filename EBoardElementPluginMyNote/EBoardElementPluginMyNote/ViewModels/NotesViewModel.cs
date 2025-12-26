// <copyright file="NotesViewModel.cs" company=".">
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
namespace EBoardElementPluginMyNote.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

//using EBoardElementPluginMyNote.Commands;
using EBoardSDK.Plugins.Elements.Protocol;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

/// <summary>
/// TODO +
/// refactor and continue development, í am just fixing crude stuff
/// </summary>
public partial class NotesViewModel : ObservableObject
{
    private ObservableCollection<ProtocolViewModel> _notes;

    private ProtocolViewModel _Note;
    public ProtocolViewModel Note
    {
        get { return _Note; }
        set
        {
            _Note = value;
            OnPropertyChanged(nameof(Note));
        }
    }

    private bool _ShowHideClicked;
    public bool ShowHideClicked
    {
        get { return _ShowHideClicked; }
        set
        {
            _ShowHideClicked = value;
            OnPropertyChanged(nameof(ShowHideClicked));
        }
    }

    public ICollectionView Notes { get; set; }

    public NotesViewModel()
    {
        _notes = new ObservableCollection<ProtocolViewModel>();

        Notes = CollectionViewSource.GetDefaultView(_notes);
        Notes.SortDescriptions.Add(new SortDescription("DateTime_Created", ListSortDirection.Descending));

        if (_notes.Count == 0)
        {
            this.NewNote();
        }
    }

    public NotesViewModel(ObservableCollection<ProtocolViewModel> notes)
    {
        if (notes != null)
        {
            _notes = notes;
        }
        else
        {
            _notes = new ObservableCollection<ProtocolViewModel>();
        }

        Notes = CollectionViewSource.GetDefaultView(_notes);
        Notes.SortDescriptions.Add(new SortDescription("DateTime_Created", ListSortDirection.Descending));

        if (_notes.Count == 0)
        {
            this.NewNote();
        }
    }

    public List<ProtocolViewModel> GetNotes => this._notes.ToList();

    public void SetNotes(List<Note> notes)
    {
        if (notes.Count > 0)
        {
            foreach (var note in notes)
            {
                this.InsertNewNote(note);
            }
        }
    }

    [RelayCommand]
    private void DeleteNote(object s)
    {
        IList selection = (IList)s;

        if (selection != null)
        {
            MessageBoxResult result = MessageBox.Show(
                $"Do you wan't to delete selected note(s)?",
                "Remove Note(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selected = selection.Cast<ProtocolViewModel>().ToArray();

                foreach (var item in selected)
                {
                    _notes.Remove(item);
                }
            }
        }

        if (_notes.Count > 0)
        {
            Note = _notes[0];
        }
    }

    [RelayCommand]
    private void NewNote()
    {
        if (this._notes != null)
        {
            this.InsertNewNote();

            Notes.Refresh();
        }
    }


    [RelayCommand]
    private void ShowHide()
    {
        if (ShowHideClicked) { ShowHideClicked = false; }
        else
        {
            ShowHideClicked = true;
        }
    }

    private void InsertNewNote(Note? note = null)
    {
        ProtocolViewModel protocol;

        if (this._notes != null)
        {

            if (note == null)
            {
                protocol = new ProtocolViewModel(
                    new Note(
                        _notes.Count,
                        "new",
                        DateTime.Now,
                        ""));
            }
            else
            {
                protocol = new ProtocolViewModel(note);
            }

            this._notes?.Insert(0, protocol);
        }

        if (this._notes?.Count > 0)
        {
            Note = this._notes.FirstOrDefault()!;
            this.OnPropertyChanged(nameof(this.Note));
        }

        this.OnPropertyChanged(nameof(this.Notes));
    }
}

// EOF