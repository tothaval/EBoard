// <copyright file="ColorDataModel.cs" company=".">
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

namespace EBoardSDK.Models.FluidUIDesign;

using EBoardSDK.Enums;
using System.Windows;
using System.Windows.Media;

public class FluidUIBrushModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIBrushModel"/> class.
    /// </summary>
    public FluidUIBrushModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIBrushModel"/> class.
    /// </summary>
    /// <param name="brush"></param>
    public FluidUIBrushModel(Brush brush)
    {
        var manager = new FluidUIBrushManager(this);
        manager.ParseBrushToModel(brush);
    }

    /// <summary>
    /// Gets or sets can be used to store the brushtype, this is used to rebuild the
    /// brush data on load.
    /// </summary>
    public string BrushTypeName { get; set; } = "SolidColorBrush";

    /// <summary>
    /// Gets or sets can be used to store the brushtype, this is used to rebuild the
    /// brush data on load.
    /// </summary>
    public BrushTypes BrushTypeEnum { get; set; } = BrushTypes.SolidColorBrush;

    /// <summary>
    /// Gets or sets gradientColor[0].
    /// </summary>
    public Color Color { get; set; } = Colors.Black;

    /// <summary>
    /// Gets or sets in case the brush is an image, the path to the image can be stored here.
    /// </summary>
    public string ImagePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets can be used to store gradient color strings, they will
    /// be matched with GradientStops list.
    /// </summary>
    public List<Color> GradientColors { get; set; } = new ();

    /// <summary>
    /// Gets or sets can be used to store gradient stop points, they will
    /// be matched with GradientColors list.
    /// </summary>
    public List<double> GradientStops { get; set; } = new ();

    /// <summary>
    /// Gets or sets can be used to store gradient points for radialgradientbrush and lineargradientbrush.
    /// </summary>
    public List<Point> GradientPoints { get; set; } = new ();
}

// EOF