// <copyright file="FluidUISizeModel.cs" company=".">
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
/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  BorderManagement
 *
 *  model class for border properties
 */
namespace EBoardSDK.Models.FluidUISize;

using EBoardSDK.Interfaces.FluidUISize;
using System.Windows;

/// <summary>
/// Provides properties for FluidUI-Size logic
/// and is the model class for state serialization
/// or deserialization.
/// </summary>
public class FluidUISizeModel : IFluidUISizeModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUISizeModel"/> class.
    /// </summary>
    public FluidUISizeModel()
    {
    }

    /// <inheritdoc/>
    public event Action? PropertyChangedEvent;

    /// <inheritdoc/>
    public double Width { get; set; } = double.NaN;

    /// <inheritdoc/>
    public double Height { get; set; } = double.NaN;

    /// <inheritdoc/>
    public double ScaleX { get; set; } = 1.0;

    /// <inheritdoc/>
    public double ScaleY { get; set; } = 1.0;

    /// <inheritdoc/>
    public Thickness Margin { get; set; } = new Thickness(5);

    /// <inheritdoc/>
    public Thickness Padding { get; set; } = new Thickness(5);

    /// <inheritdoc/>
    public Thickness BorderThickness { get; set; } = new Thickness(2, 2, 2, 2);

    /// <inheritdoc/>
    public CornerRadius CornerRadius { get; set; } = new CornerRadius(5, 5, 5, 5);

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public void SetInitialValues()
    {
        this.Width = double.NaN;
        this.Height = double.NaN;

        this.ScaleX = 1.0;
        this.ScaleY = 1.0;

        this.Margin = new Thickness(5);
        this.Padding = new Thickness(5);
        this.BorderThickness = new Thickness(2, 2, 2, 2);
        this.CornerRadius = new CornerRadius(5, 5, 5, 5);
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