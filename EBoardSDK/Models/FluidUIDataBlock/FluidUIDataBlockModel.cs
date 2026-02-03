// <copyright file="FluidUIDataBlockModel.cs" company=".">
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
namespace EBoardSDK.Models.FluidUIDataBlock;

using EBoardSDK.Interfaces.FluidUIDataBlock;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides properties for FluidUI-DataBlock logic
/// and is the model class for state serialization
/// or deserialization.
/// </summary>
public class FluidUIDataBlockModel : IFluidUIDataBlockModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIDataBlockModel"/> class.
    /// </summary>
    public FluidUIDataBlockModel()
    {
    }

    /// <inheritdoc/>
    public event Action? PropertyChangedEvent;

    /// <inheritdoc/>
    public string Title { get; set; } = "title";

    /// <inheritdoc/>
    public string Text { get; set; } = "fluidui context description";

    /// <inheritdoc/>
    public bool ShowToolTip { get; set; } = true;

    /// <inheritdoc/>
    public List<FluidUIIndexText> IndexTextList { get; set; } = new ();

    /// <inheritdoc/>
    public List<FluidUIKeyText> KeyTextList { get; set; } = new ();

    /// <inheritdoc/>
    public QuadValue<FluidUIIndexText> IndexTextQuadValue1 { get; set; } = new ();

    /// <inheritdoc/>
    public QuadValue<FluidUIIndexText> IndexTextQuadValue2 { get; set; } = new ();

    /// <inheritdoc/>
    public QuadValue<FluidUIIndexText> IndexTextQuadValue3 { get; set; } = new ();

    /// <inheritdoc/>
    public QuadValue<FluidUIIndexText> IndexTextQuadValue4 { get; set; } = new ();

    /// <inheritdoc/>
    public QuadValue<FluidUIKeyText> KeyTextQuadValueA { get; set; } = new ();

    /// <inheritdoc/>
    public QuadValue<FluidUIKeyText> KeyTextQuadValueB { get; set; } = new ();

    /// <inheritdoc/>
    public QuadValue<FluidUIKeyText> KeyTextQuadValueC { get; set; } = new ();

    /// <inheritdoc/>
    public QuadValue<FluidUIKeyText> KeyTextQuadValueD { get; set; } = new ();

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public void SetInitialValues()
    {
        this.Title = "title";
        this.Text = "text";

        this.ShowToolTip = true;

        this.IndexTextList.Clear();
        this.KeyTextList.Clear();

        this.IndexTextQuadValue1.SetValueToAll(new ());
        this.IndexTextQuadValue2.SetValueToAll(new ());
        this.IndexTextQuadValue3.SetValueToAll(new ());
        this.IndexTextQuadValue4.SetValueToAll(new ());

        this.KeyTextQuadValueA.SetValueToAll(new ());
        this.KeyTextQuadValueB.SetValueToAll(new ());
        this.KeyTextQuadValueC.SetValueToAll(new ());
        this.KeyTextQuadValueD.SetValueToAll(new ());
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