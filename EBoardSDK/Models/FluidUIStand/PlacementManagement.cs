// <copyright file="PlacementManagement.cs" company=".">
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
 *  PlacementManagement
 *
 *  model for placement property changes
 *  can be applied to MainWindowViewModel and ElementViewModel to define the initial
 *  values on load or upon creation, as well as to store changes during runtime.
 */
namespace EBoardSDK.Models.FluidUIStand;

using EBoardSDK.Interfaces.FluidUIStand;
using System.Windows;

public class PlacementManagement : IFluidUIStandModel
{
    public event Action PropertyChangedEvent;

    /// <summary>
    /// Gets or sets the rotation of an element.
    /// </summary>
    public double Angle { get; set; } = default;

    /// <summary>
    /// Gets or sets the position of an element.
    /// </summary>
    public Point Position { get; set; } = default;

    /// <summary>
    /// Gets or sets the z-index of an element.
    /// </summary>
    public int Z { get; set; } = default;

    public PlacementManagement()
    {
    }

    public void Dispose()
    {
    }

    public void SetInitialValues()
    {
        this.Angle = 0.0;
        this.Position = new Point(25.0, 25.0);
        this.Z = 0;
    }
}

// EOF