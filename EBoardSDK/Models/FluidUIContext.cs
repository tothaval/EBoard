// <copyright file="FluidUIContext.cs" company=".">
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
namespace EBoardSDK.Models;

using EBoardSDK.Interfaces;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Models.FluidUIFont;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;
using System;

/// <summary>
/// This class serves as an implementation for the
/// <see cref="IFluidUIContext"/> interface and is
/// used for serialization and deserialization of
/// FluidUI data.
/// </summary>
public class FluidUIContext : IFluidUIContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIContext"/> class.
    /// </summary>
    public FluidUIContext()
    {
    }

    /// <inheritdoc/>
    public event Action? PropertyChangedEvent;

    /// <inheritdoc/>
    public FluidUIDataBlockModel? DataBlock { get; set; } = new FluidUIDataBlockModel();

    /// <inheritdoc/>
    public FluidUIDesignModel? Design { get; set; } = new FluidUIDesignModel();

    /// <inheritdoc/>
    public FluidUIFontModel? Font { get; set; } = new FluidUIFontModel();

    /// <inheritdoc/>
    public FluidUISizeModel? Size { get; set; } = new FluidUISizeModel();

    /// <inheritdoc/>
    public FluidUIStandModel? Stand { get; set; } = new FluidUIStandModel();

    /// <inheritdoc/>
    public void Dispose()
    {
        this.DataBlock?.Dispose();
        this.Design?.Dispose();
        this.Font?.Dispose();
        this.Size?.Dispose();
        this.Stand?.Dispose();

        //GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public void SetInitialValues()
    {
        this.DataBlock?.SetInitialValues();
        this.Design?.SetInitialValues();
        this.Font?.SetInitialValues();
        this.Size?.SetInitialValues();
        this.Stand?.SetInitialValues();
    }

    public void UpdateValues()
    {
        this.DataBlock?.UpdateValues();
        this.Design?.UpdateValues();
        this.Font?.UpdateValues();
        this.Size?.UpdateValues();
        this.Stand?.UpdateValues();
    }
}

// EOF