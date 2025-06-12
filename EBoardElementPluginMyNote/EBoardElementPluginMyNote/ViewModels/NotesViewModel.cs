using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

//using EBoardElementPluginMyNote.Commands;
using EBoardElementPluginMyNote.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace EBoardElementPluginMyNote.ViewModels
{
    public partial class NotesViewModel : ObservableObject
    {
        private ObservableCollection<NoteViewModel> _notes;

        private NoteViewModel _Note;
        public NoteViewModel Note
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


        public NotesViewModel(ObservableCollection<NoteViewModel> notes)
        {
            _notes = notes;

            Notes = CollectionViewSource.GetDefaultView(_notes);
            Notes.SortDescriptions.Add(new SortDescription("DateTime_Created", ListSortDirection.Descending));

            if (_notes.Count > 0)
            {
                Note = _notes.Last();
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
                    var selected = selection.Cast<NoteViewModel>().ToArray();

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
            _notes.Insert(0, new NoteViewModel(
                new Note(
                    _notes.Count,
                    "new",
                    DateTime.Now,
                    ""
                    )
                ));

            Note = _notes[0];
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
    }
}
