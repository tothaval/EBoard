/*  MyNote (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PersistanceHandler 
 * 
 *  helper class for serializing data
 */

using EBoardElementPluginMyNote.Models;
using EBoardElementPluginMyNote.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace EBoardElementPluginMyNote.Utility
{
    internal class PersistanceHandler
    {
        string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\notes\\";
        string filter = "*.xml";

        private async Task ClearFolder(ObservableCollection<NoteViewModel> notes)
        {

            List<string> files = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly).ToList();

            if (files.Count > notes.Count)
            {
                foreach (string file in files)
                {
                    if (!file.EndsWith("protocol.xml"))
                    {
                        File.Delete(file);
                    }
                }
            }
        }


        public ObservableCollection<NoteViewModel> DeSerializeNotes()
        {
            ObservableCollection<NoteViewModel> notes = new ObservableCollection<NoteViewModel>();

            if (Directory.Exists(folder))
            {
                List<string> files = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly).ToList();
                var xmlSerializer = new XmlSerializer(typeof(Note));

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);

                    if (fileName.StartsWith("note_"))
                    {
                        using (var writer = new StreamReader(file))
                        {
                            try
                            {
                                var member = (Note)xmlSerializer.Deserialize(writer);

                                notes.Add(new NoteViewModel(member));
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }

            return notes;
        }


        public NoteViewModel? DeSerializeProtocol()
        {
            if (Directory.Exists(folder))
            {
                List<string> files = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly).ToList();

                var xmlSerializer = new XmlSerializer(typeof(Note));

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);

                    if (fileName.Equals("protocol.xml"))
                    {
                        using (var writer = new StreamReader(file))
                        {
                            try
                            {
                                var member = (Note)xmlSerializer.Deserialize(writer);

                                return new NoteViewModel(member);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }

            return null;
        }


        public void SerializeNote(NoteViewModel noteViewModel)
        {

            if (noteViewModel != null)
            {
                Note note = noteViewModel.GetNote;

                var xmlSerializer = new XmlSerializer(typeof(Note));

                using (var writer = new StreamWriter($"{folder}protocol.xml"))
                {
                    xmlSerializer.Serialize(writer, note);
                }
            }
        }


        public async void SerializeNotes(ObservableCollection<NoteViewModel> notes)
        {
            await ClearFolder(notes);

            foreach (NoteViewModel noteViewModel in notes)
            {
                var xmlSerializer = new XmlSerializer(typeof(Note));

                using (var writer = new StreamWriter($"{folder}note_{noteViewModel.ID}.xml"))
                {
                    xmlSerializer.Serialize(writer, noteViewModel.GetNote);
                }
            }
        }
    }
}
