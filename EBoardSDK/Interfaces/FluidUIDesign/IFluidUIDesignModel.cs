// <copyright file="IFluidUIDesignModel.cs" company=".">
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
namespace EBoardSDK.Interfaces.FluidUIDesign;

using EBoardSDK.Models;
using System.Text.Json.Serialization;
using System.Windows.Media;

public interface IFluidUIDesignModel : IFluidUIChangedAction
{
    [JsonIgnore]
    public Brush Background { get; set; }

    [JsonIgnore]
    public Brush Foreground { get; set; }

    [JsonIgnore]
    public Brush Border { get; set; }

    [JsonIgnore]
    public Brush Highlight { get; set; }

    [JsonIgnore]
    public Brush SelectionFallbackBrush { get; set; }

    public ColorDataModel BackgroundColor { get; set; }

    public ColorDataModel BorderColor { get; set; }

    public ColorDataModel ForegroundColor { get; set; }

    public ColorDataModel HighlightColor { get; set; }

    public ColorDataModel SelectionFallbackColor { get; set; }

    public string ImagePath { get; set; }

    public string ImageForegroundPath { get; set; }

    public string ImageBorderPath { get; set; }

    public string ImageHighlightPath { get; set; }
}

// EOF