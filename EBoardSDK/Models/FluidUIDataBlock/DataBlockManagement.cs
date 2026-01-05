// <copyright file="DataBlockManagement.cs" company=".">
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

using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUIDataBlock;
using System;
using System.Collections.Generic;

public class DataBlockManagement : IFluidUIDataBlockModel
{
    public DataBlockManagement()
    {
    }

    public event Action PropertyChangedEvent;

    public string Title { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public List<FluidUIIndexText> IndexTextList { get; set; } = new();

    public List<FluidUIKeyText> KeyTextList { get; set; } = new();

    public QuadValue<FluidUIIndexText> IndexTextQuadValue1 { get; set; } = new();

    public QuadValue<FluidUIIndexText> IndexTextQuadValue2 { get; set; } = new();

    public QuadValue<FluidUIIndexText> IndexTextQuadValue3 { get; set; } = new();

    public QuadValue<FluidUIIndexText> IndexTextQuadValue4 { get; set; } = new();

    public QuadValue<FluidUIKeyText> KeyTextQuadValueA { get; set; } = new();

    public QuadValue<FluidUIKeyText> KeyTextQuadValueB { get; set; } = new();

    public QuadValue<FluidUIKeyText> KeyTextQuadValueC { get; set; } = new();

    public QuadValue<FluidUIKeyText> KeyTextQuadValueD { get; set; } = new();

    public void Dispose()
    {
    }

    public int AddFluidUIIndexTextToList()
    {
        var index = 0;

        if (this.IndexTextList == null)
        {
            this.IndexTextList = new List<FluidUIIndexText>();
        }

        if (this.IndexTextList.Count == 0)
        {
            index = this.IndexTextList.Count;
        }
        else if (this.IndexTextList.Count > 0)
        {
            var lastEntry = this.IndexTextList.LastOrDefault();

            if (lastEntry != null)
            {
                index = lastEntry.Index + 1;
            }
        }

        var indexText = new FluidUIIndexText() { Index = index, Time = DateTime.Now };

        this.IndexTextList.Add(indexText);

        return this.IndexTextList.Count();
    }

    public void AddFluidUIIndexTextToQuadValue(FluidUIIndexText indexText, QuadValueTargets quadValueTarget, QuadValueTargets subQuadValueTarget)
    {
        this.SelectIndexTextQuadValue(indexText, quadValueTarget, subQuadValueTarget);
    }

    public void AddFluidUIKeyTextToList(FluidUIKeyText keyText)
    {
        this.KeyTextList.Add(keyText);
    }

    public void AddFluidUIKeyTextToQuadValue(FluidUIKeyText keyText, QuadValueTargets quadValueTarget, QuadValueTargets subQuadValueTarget)
    {
        this.SelectKeyTextQuadValue(keyText, quadValueTarget, subQuadValueTarget);
    }

    private void SelectIndexTextQuadValue(FluidUIIndexText fluidUIIndexText, QuadValueTargets quadValueTargets, QuadValueTargets subquadTargets)
    {
        switch (quadValueTargets)
        {
            case QuadValueTargets.A:
                this.SelectSubQuadValue(this.IndexTextQuadValue1, fluidUIIndexText, subquadTargets);
                break;
            case QuadValueTargets.B:
                this.SelectSubQuadValue(this.IndexTextQuadValue2, fluidUIIndexText, subquadTargets);
                break;
            case QuadValueTargets.C:
                this.SelectSubQuadValue(this.IndexTextQuadValue3, fluidUIIndexText, subquadTargets);
                break;
            case QuadValueTargets.D:
                this.SelectSubQuadValue(this.IndexTextQuadValue4, fluidUIIndexText, subquadTargets);
                break;
            default:
                break;
        }
    }

    private void SelectKeyTextQuadValue(FluidUIKeyText fluidUIKeyText, QuadValueTargets quadValueTargets, QuadValueTargets subquadTargets)
    {
        switch (quadValueTargets)
        {
            case QuadValueTargets.A:
                this.SelectSubQuadValue(this.KeyTextQuadValueA, fluidUIKeyText, subquadTargets);
                break;
            case QuadValueTargets.B:
                this.SelectSubQuadValue(this.KeyTextQuadValueB, fluidUIKeyText, subquadTargets);
                break;
            case QuadValueTargets.C:
                this.SelectSubQuadValue(this.KeyTextQuadValueC, fluidUIKeyText, subquadTargets);
                break;
            case QuadValueTargets.D:
                this.SelectSubQuadValue(this.KeyTextQuadValueD, fluidUIKeyText, subquadTargets);
                break;
            default:
                break;
        }
    }

    private void SelectSubQuadValue<T>(QuadValue<T> quadValue, T fluidUIIndexText, QuadValueTargets subquadTargets)
    {
        switch (subquadTargets)
        {
            case QuadValueTargets.A:
                quadValue.Value1 = fluidUIIndexText;
                break;
            case QuadValueTargets.B:
                quadValue.Value2 = fluidUIIndexText;
                break;
            case QuadValueTargets.C:
                quadValue.Value3 = fluidUIIndexText;
                break;
            case QuadValueTargets.D:
                quadValue.Value4 = fluidUIIndexText;
                break;
            default:
                break;
        }
    }

    public void SetInitialValues()
    {
    }
}

// EOF