// <copyright file="IFluidUIContext.cs" company=".">
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
namespace EBoardSDK.Interfaces;

using EBoardSDK.Interfaces.FluidUIDataBlock;
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.Interfaces.FluidUISize;
using EBoardSDK.Interfaces.FluidUIStand;
using EBoardSDK.Interfaces.FluidUIText;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Models.FluidUIFont;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;

public interface IFluidUIContext : IFluidUIChangedAction
{
    public DataBlockManagement DataBlock { get; set; }

    public BrushManagement Design { get; set; }

    public FontManagement Font { get; set; }

    public BorderManagement Size { get; set; }

    public PlacementManagement Stand { get; set; }

    public void Apply_FluidUI(
        IFluidUIDataBlockModel? dataBlock = null,
        IFluidUIDesignModel? design = null,
        IFluidUIFontModel? font = null,
        IFluidUISizeModel? size = null,
        IFluidUIStandModel? stand = null);

    public void Apply_FluidUIDataBlock(IFluidUIDataBlockModel? dataBlock = null);

    public void Apply_FluidUIDesign(IFluidUIDesignModel? design = null);

    public void Apply_FluidUIFont(IFluidUIFontModel? font = null);

    public void Apply_FluidUISize(IFluidUISizeModel? size = null);

    public void Apply_FluidUIStand(IFluidUIStandModel? stand = null);
}

// EOF