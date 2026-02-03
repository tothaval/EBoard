// <copyright file="FluidUIStandModel.cs" company=".">
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
namespace EBoardSDK.Models.FluidUIStand;

using EBoardSDK.Interfaces.FluidUIStand;
using System.Windows;

/// <summary>
/// Provides properties for FluidUI-Stand logic
/// and is the model class for state serialization
/// or deserialization.
/// </summary>
public class FluidUIStandModel : IFluidUIStandModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIStandModel"/> class.
    /// </summary>
    public FluidUIStandModel()
    {
    }

    /// <summary>
    /// In case an event is needed.
    /// </summary>
    public event Action? PropertyChangedEvent;

    /// <summary>
    /// Gets or sets the rotation of an element.
    /// </summary>
    public double Angle { get; set; } = 0;

    /// <summary>
    /// Gets or sets the position of an element.
    /// </summary>
    public Point Position { get; set; } = default;

    /// <summary>
    /// Gets or sets the skew value for x angle.
    /// </summary>
    public double SkewAngleX { get; set; } = 0;

    /// <summary>
    /// Gets or sets the skew value for y angle.
    /// </summary>
    public double SkewAngleY { get; set; } = 0;

    /// <summary>
    /// Gets or sets the transform center point
    /// for skew transformations.
    /// </summary>
    public Point SkewCenterPoint { get; set; } = new Point(0.5, 0.5);

    /// <summary>
    /// Gets or sets the transform center point
    /// for rotation transformations.
    /// </summary>
    public Point TransformOriginPoint { get; set; } = new Point(0.5, 0.5);

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Xmax { get; set; } = -1;

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Xmin { get; set; } = 0;

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Ymax { get; set; } = -1;

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Ymin { get; set; } = 0;

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Z { get; set; } = 0;

    /// <summary>
    /// Gets or sets the maximum z-index of an element.
    /// </summary>
    public int Zmaximum { get; set; } = 100;

    /// <summary>
    /// Gets or sets the minimum z-index of an element.
    /// </summary>
    public int Zminimum { get; set; } = 0;

    /// <summary>
    /// In case an needed event or any other property
    /// must be dereferenced.
    /// </summary>
    public void Dispose()
    {
        // TODO look into GarbageCollection to set it up right if necessary.
        // GC.SuppressFinalize(this);
    }

    /// <summary>
    /// In case a reset to defaullt values is needed
    /// without destroying the object instance.
    /// </summary>
    public void SetInitialValues()
    {
        this.Angle = 0.0;
        this.Position = new Point(25.0, 25.0);

        this.SkewAngleX = 0.0;
        this.SkewAngleY = 0.0;

        this.SkewCenterPoint = new Point(0.0, 0.0);
        this.TransformOriginPoint = new Point(0.0, 0.0);

        this.Xmax = 100;
        this.Xmin = -10;

        this.Ymax = 100;
        this.Ymin = -10;

        this.Z = 0;
        this.Zminimum = -10;
        this.Zmaximum = 100;
    }

    /// <inheritdoc/>
    public void UpdateValues()
    {
        this.InvokePropertyChangedEvent();
    }

    private void InvokePropertyChangedEvent()
    {
        this.PropertyChangedEvent?.Invoke();
    }
}

// EOF