// <copyright file="IFluidUIStandModel.cs" company=".">
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
namespace EBoardSDK.Interfaces.FluidUIStand;

using System.Windows;

/// <summary>
/// This interface provides a blueprint for the model class of
/// FluidUI-Stand logic and its properties.
/// </summary>
public interface IFluidUIStandModel : IFluidUIChangedAction
{
    /// <summary>
    /// Gets or sets the rotation of an element.
    /// </summary>
    public double Angle { get; set; }

    /// <summary>
    /// Gets or sets the position of an element.
    /// </summary>
    public Point Position { get; set; }

    /// <summary>
    /// Gets or sets the skew value for x angle.
    /// </summary>
    public double SkewAngleX { get; set; }

    /// <summary>
    /// Gets or sets the skew value for y angle.
    /// </summary>
    public double SkewAngleY { get; set; }

    /// <summary>
    /// Gets or sets the transform center point
    /// for skew transformations.
    /// </summary>
    public Point SkewCenterPoint { get; set; }

    /// <summary>
    /// Gets or sets the transform center point
    /// for rotation transformations.
    /// </summary>
    public Point TransformOriginPoint { get; set; }

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Xmax { get; set; }

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Xmin { get; set; }

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Ymax { get; set; }

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Ymin { get; set; }

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Z { get; set; }

    /// <summary>
    /// Gets or sets the maximum z-index of an element.
    /// </summary>
    public int Zmaximum { get; set; }

    /// <summary>
    /// Gets or sets the minimum z-index of an element.
    /// </summary>
    public int Zminimum { get; set; }
}

// EOF