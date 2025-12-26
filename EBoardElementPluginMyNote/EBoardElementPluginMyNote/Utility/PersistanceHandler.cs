// <copyright file="PersistanceHandler.cs" company=".">
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
/*  MyNote (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PersistanceHandler 
 * 
 *  helper class for serializing data
 */

namespace EBoardElementPluginMyNote.Utility;

using EBoardSDK.Plugins.Elements.Protocol;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

internal class PersistanceHandler
{
    string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\notes\\";
    string filter = "*.xml";

    private async Task ClearFolder(ObservableCollection<ProtocolViewModel> notes)
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


    public ObservableCollection<ProtocolViewModel> DeSerializeNotes()
    {
        ObservableCollection<ProtocolViewModel> notes = new ObservableCollection<ProtocolViewModel>();

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

                            notes.Add(new ProtocolViewModel(member));
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


    public ProtocolViewModel? DeSerializeProtocol()
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

                            return new ProtocolViewModel(member);
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


    public void SerializeNote(ProtocolViewModel noteViewModel)
    {

        if (noteViewModel != null)
        {
            Note note = noteViewModel.Note;

            var xmlSerializer = new XmlSerializer(typeof(Note));

            using (var writer = new StreamWriter($"{folder}protocol.xml"))
            {
                xmlSerializer.Serialize(writer, note);
            }
        }
    }


    public async void SerializeNotes(ObservableCollection<ProtocolViewModel> notes)
    {
        await ClearFolder(notes);

        foreach (ProtocolViewModel noteViewModel in notes)
        {
            var xmlSerializer = new XmlSerializer(typeof(Note));

            using (var writer = new StreamWriter($"{folder}note_{noteViewModel.ID}.xml"))
            {
                xmlSerializer.Serialize(writer, noteViewModel.Note);
            }
        }
    }
}

// EOF