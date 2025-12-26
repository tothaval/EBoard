// <copyright file="ElementConfig.cs" company=".">
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
namespace EBoardSDK.Models;

using EBoardSDK.Interfaces;
using EBoardSDK.ViewModels;
using System.Text.Json.Serialization;

public class ElementConfig
{
    [JsonIgnore]
    private EBoardViewModel eBoardViewModel;

    [JsonIgnore]
    private ElementViewModel elementViewModel;

    [JsonIgnore]
    public IPlugin Plugin { get; set; }

    [JsonIgnore]
    public EBoardViewModel EBoardViewModel => this.eBoardViewModel;

    [JsonIgnore]
    public ElementViewModel ElementViewModel => this.elementViewModel;

    /// <summary>
    /// Gets or sets element ID, built using $"Element_{DateTime().Ticks} on first
    /// creation of an element.
    /// </summary>
    public string EID { get; set; } = string.Empty;

    public string ContentFilePath { get; set; } = string.Empty;

    public int ID { get; set; } = -1;

    public FluidUIContext ElementContext { get; set; }

    /// <summary>
    /// a string representation of an assembly, where the element type can be found
    /// </summary>
    public string PluginHeader { get; set; } = string.Empty;

    // unification effort due to reduction, build abstraction layer can be removed and simplified
    // loading can be simplified, but element as fluidUIDesign container needs some rework
    // every fluidUIDesign can implement certain features, and maybe should be required to do so.
    public string PluginName { get; set; } = string.Empty;

    public string PluginType { get; set; } = string.Empty;

    public string AssemblyName { get; set; } = string.Empty;

    public void SetEBoardAndElementViewModel(EBoardViewModel eBoardViewModel, ElementViewModel elementViewModel)
    {
        this.eBoardViewModel = eBoardViewModel;
        this.elementViewModel = elementViewModel;
    }
}

// EOF