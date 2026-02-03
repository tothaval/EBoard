// <copyright file="MyNoteModel.cs" company=".">
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
namespace EBoardElementPluginMyNote.Models;

using EBoardSDK.Plugins.Elements.Protocol.Models;
using System.Collections.Generic;
using System.Linq;

public class MyNoteModel
{
    public MyNoteModel()
    {
    }

    public MyNoteModel(MyNoteViewModel myNoteViewModel)
    {
        this.ShowMatrixControls = myNoteViewModel.AreaViewModel?.ShowMatrixControls ?? false;

        // Protocol Tab
        this.Note = myNoteViewModel.GetProtocol;

        // Notes Tab
        Notes = [.. myNoteViewModel.GetNotes.Select(x => x.Note)];

        // Area Tab
        this.NotesAreaList = new();
                
        var areavm = myNoteViewModel.AreaViewModel;

        if (areavm != null)
        {
            foreach (var item in areavm.AreaVerticalOuterViewModel.HorizontalInnerViewModels)
            {
                var list = new List<Note>();

                foreach (var horizontalelement in item.Elements)
                {
                    var noteModel = new Note(horizontalelement);

                    list.Add(noteModel);
                }

                this.NotesAreaList.Add(list);
            } 
        }
    }

    public Note Note { get; set; } = new();

    public List<Note> Notes { get; set; } = new();

    public List<List<Note>> NotesAreaList { get; set; } = new();

    public bool ShowMatrixControls { get; set; } = true;
}

// EOF