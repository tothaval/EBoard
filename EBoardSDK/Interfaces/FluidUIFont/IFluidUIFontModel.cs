// <copyright file="IFluidUIFontModel.cs" company=".">
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
namespace EBoardSDK.Interfaces.FluidUIFont;

using System.Text.Json.Serialization;
using System.Windows;

using FontFamily = System.Windows.Media.FontFamily;

/// <summary>
/// This interface provides a blueprint for the model class of
/// FluidUI-Font logic and its properties.
/// </summary>
public interface IFluidUIFontModel : IFluidUIChangedAction
{
    /// <summary>
    /// Gets or sets the FontSize used or to
    /// be used in the FluidUI context area.
    /// </summary>
    public double FontSize { get; set; }

    /// <summary>
    /// Gets or sets the FontFamily.Source used or to
    /// be used in the FluidUI context area.
    /// </summary>
    public string FontFamilyName { get; set; }

    /// <summary>
    /// Gets or sets the FontFamily used or to
    /// be used in FluidUI context area.
    /// </summary>
    [JsonIgnore]
    public FontFamily FontFamily { get; set; }

    /// <summary>
    /// Gets or sets the FontWeight used or to
    /// be used in the FluidUI context area.
    /// </summary>
    public FontWeight FontWeight { get; set; }

    /// <summary>
    /// Gets or sets the FontWeigts ToOpenTypeWeight used or to
    /// be used in the FluidUÍ context area.
    /// </summary>
    public int FontWeightValue { get; set; }

    /// <summary>
    /// Gets the (int)FontSize that is used
    /// by the FluidUÍ context area.
    /// </summary>
    public int FontSizeDisplay { get; }
}

// EOF