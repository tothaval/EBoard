// <copyright file="IFluidUISizeModel.cs" company=".">
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
namespace EBoardSDK.Interfaces.FluidUISize;

using System.Windows;

/// <summary>
/// This interface provides a blueprint for the model class of
/// FluidUI-Size logic and its properties.
/// </summary>
public interface IFluidUISizeModel : IFluidUIChangedAction
{
    /// <summary>
    /// Gets or sets the width of the FluidUI context area.
    /// Width is the x axis size.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the FluidUI context area.
    /// Height is the y axis size.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Gets or sets the ScaleX value for x axis scaling transformation
    /// of the FluidUI context area.
    /// </summary>
    public double ScaleX { get; set; }

    /// <summary>
    /// Gets or sets the ScaleY value for y axis scaling transformation
    /// of the FluidUI context area.
    /// </summary>
    public double ScaleY { get; set; }

    /// <summary>
    /// Gets or sets the Thickness of the FluidUI context areas
    /// BorderThickness property.
    /// </summary>
    public Thickness BorderThickness { get; set; }

    /// <summary>
    /// Gets or sets the CornerRadius of the FluidUI context areas
    /// border CornerRadius property.
    /// </summary>
    public CornerRadius CornerRadius { get; set; }

    /// <summary>
    /// Gets or sets the Thickness of the FluidUI context areas
    /// border Margin property.
    /// </summary>
    public Thickness Margin { get; set; }

    /// <summary>
    /// Gets or sets the Thickness of the FluidUI context areas
    /// border Padding property.
    /// </summary>
    public Thickness Padding { get; set; }
}

// EOF