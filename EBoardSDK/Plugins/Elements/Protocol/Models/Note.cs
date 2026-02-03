// <copyright file="Note.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.Protocol.Models;

/// /// <summary>
/// Serializable data model for <see cref="ProtocolView"/>.
/// </summary>
public class Note
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Note"/> class.
    /// </summary>
    public Note()
    {
        this.ID = -1;
        this.DateTimeCreated = DateTime.Now;

        this.Title = "title";
        this.Content = "note";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Note"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    /// <param name="dateTime"></param>
    /// <param name="content"></param>
    public Note(int id, string title, DateTime dateTime, string content)
    {
        this.ID = id;
        this.Title = title;
        this.DateTimeCreated = dateTime;
        this.Content = content;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Note"/> class.
    /// </summary>
    /// <param name="protocolViewModel">Desired is the instance that has to be stored.</param>
    public Note(ProtocolViewModel protocolViewModel)
    {
        this.ID = protocolViewModel.ID;
        this.Title = protocolViewModel.Title;
        this.DateTimeCreated = protocolViewModel.DateTimeCreated;
        this.DateTimeEdited = protocolViewModel.DateTimeEdited;
        this.Content = protocolViewModel.Content;
    }

    public int ID { get; set; }

    public string Title { get; set; }

    public DateTime DateTimeCreated { get; set; }

    public DateTime DateTimeEdited { get; set; }

    public string Content { get; set; }
}

// EOF