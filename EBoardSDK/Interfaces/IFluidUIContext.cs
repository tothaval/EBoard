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

using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Models.FluidUIFont;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;

/// <summary>
/// This interface contains every FluidUI model class as property
/// blueprint. The model classes are:
/// <see cref="FluidUIDataBlockModel"/>,
/// <see cref="FluidUIDesignModel"/>,
/// <see cref="FluidUIFontModel"/>,
/// <see cref="FluidUISizeModel"/>,
/// <see cref="FluidUIStandModel"/>.
/// <para/>
/// Any FluidUI model class has a manager class, that allows
/// changes to the model in a comfortable and easy to maintain
/// manner.
/// </summary>
public interface IFluidUIContext : IFluidUIChangedAction
{
    /// <summary>
    /// Gets or sets the FluidUI DataBlock model that is used
    /// for serialization or deserialization of data.
    /// </summary>
    public FluidUIDataBlockModel? DataBlock { get; set; }

    /// <summary>
    /// Gets or sets the FluidUI Design model that is used
    /// for serialization or deserialization of data.
    /// </summary>
    public FluidUIDesignModel? Design { get; set; }

    /// <summary>
    /// Gets or sets the FluidUI Font model that is used
    /// for serialization or deserialization of data.
    /// </summary>
    public FluidUIFontModel? Font { get; set; }

    /// <summary>
    /// Gets or sets the FluidUI Size model that is used
    /// for serialization or deserialization of data.
    /// </summary>
    public FluidUISizeModel? Size { get; set; }

    /// <summary>
    /// Gets or sets the FluidUI Stand model that is used
    /// for serialization or deserialization of data.
    /// </summary>
    public FluidUIStandModel? Stand { get; set; }
}

// EOF