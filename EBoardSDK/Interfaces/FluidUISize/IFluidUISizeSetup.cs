// <copyright file="IFluidUISizeSetup.cs" company=".">
// Stephan Kammel
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
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  IFluidUISize
 *
 *  interface for BorderManagement
 */
namespace EBoardSDK.Interfaces.FluidUISize;

using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
using EBoardSDK.Models;

public interface IFluidUISizeSetup : IFluidUIChangedAction
{
    public bool FluidUIContextHasArea { get; }

    public QuadValueSetupViewModel CornerRadiusQuadSetup { get; set; }

    public QuadValueSetupViewModel MarginQuadSetup { get; set; }

    public QuadValueSetupViewModel PaddingQuadSetup { get; set; }

    public QuadValueSetupViewModel ThicknessQuadSetup { get; set; }

    public void Apply_FluidUISizeHeight(int heightValue);

    public void Apply_FluidUISizeQuadValue(QuadValue<int> quadValue, BorderTargets borderTargets);

    public void Apply_FluidUISizeWidth(int widthValue);

    public object Get_FluidUISizeObjectFromQuadValueSetup(QuadValueSetupViewModel quadValueSetupViewModel, BorderTargets borderTargets);

    public void Reset_FluidUISizeQuadValue(BorderTargets borderTargets);

    public void Reset_FluidUISizeWidthAndHeight();
}

// EOF