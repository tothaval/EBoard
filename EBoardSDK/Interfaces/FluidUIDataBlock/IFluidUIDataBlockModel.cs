// <copyright file="IFluidUIDataBlockModel.cs" company=".">
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
namespace EBoardSDK.Interfaces.FluidUIDataBlock;

using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDataBlock;

/// <summary>
/// This interface provides a blueprint for the model class of
/// FluidUI-DataBlock logic and its properties.
/// </summary>
public interface IFluidUIDataBlockModel : IFluidUIChangedAction
{
    /// <summary>
    /// Gets or sets a value indicating whether tooltips
    /// should be visible over FLuidUI context areas.
    /// </summary>
    public bool ShowToolTip { get; set; }

    /// <summary>
    /// Gets or sets the Title property that is used in FluidUI
    /// context area right click context menus as context menu header.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the Text property that is used in FluidUI
    /// context area ToolTips as content, e.g. as description text.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets a list of FluidUIIndexText models.
    ///
    /// FluidUIIndexText stores an unchangeable index, a DateTime
    /// and a changeable text property.
    /// </summary>
    public List<FluidUIIndexText> IndexTextList { get; set; }

    /// <summary>
    /// Gets or sets a list of FluidUIKeyText models.
    ///
    /// FluidUIKeyText stores a DateTime, a changeable key string property
    /// and a changeable text property.
    /// </summary>
    public List<FluidUIKeyText> KeyTextList { get; set; }

    /// <summary>
    /// Gets or sets FluidUIIndexText models.
    ///
    /// Experimental storage 1 of 4 for a total of 16.
    /// 4 FluidUIIndexText items can be changed in one QuadValue.
    ///
    /// FluidUIIndexText stores an unchangeable index, a DateTime
    /// and a changeable text property.
    /// </summary>
    public QuadValue<FluidUIIndexText> IndexTextQuadValue1 { get; set; }

    /// <summary>
    /// Gets or sets FluidUIIndexText models.
    ///
    /// Experimental storage 2 of 4 for a total of 16.
    /// 4 FluidUIIndexText items can be changed in one QuadValue.
    ///
    /// FluidUIIndexText stores an unchangeable index, a DateTime
    /// and a changeable text property.
    /// </summary>
    public QuadValue<FluidUIIndexText> IndexTextQuadValue2 { get; set; }

    /// <summary>
    /// Gets or sets FluidUIIndexText models.
    ///
    /// Experimental storage 3 of 4 for a total of 16.
    /// 4 FluidUIIndexText items can be changed in one QuadValue.
    ///
    /// FluidUIIndexText stores an unchangeable index, a DateTime
    /// and a changeable text property.
    /// </summary>
    public QuadValue<FluidUIIndexText> IndexTextQuadValue3 { get; set; }

    /// <summary>
    /// Gets or sets FluidUIIndexText models.
    ///
    /// Experimental storage 4 of 4 for a total of 16.
    /// 4 FluidUIIndexText items can be changed in one QuadValue.
    ///
    /// FluidUIIndexText stores an unchangeable index, a DateTime
    /// and a changeable text property.
    /// </summary>
    public QuadValue<FluidUIIndexText> IndexTextQuadValue4 { get; set; }

    /// <summary>
    /// Gets or sets FluidUIKeyText models.
    ///
    /// Experimental storage 1 of 4 for a total of 16.
    /// 4 FluidUIKeyText items can be changed in one QuadValue.
    ///
    /// FluidUIKeyText stores a DateTime, a changeable key string property
    /// and a changeable text property.
    /// </summary>
    public QuadValue<FluidUIKeyText> KeyTextQuadValueA { get; set; }

    /// <summary>
    /// Gets or sets FluidUIKeyText models.
    ///
    /// Experimental storage 2 of 4 for a total of 16.
    /// 4 FluidUIKeyText items can be changed in one QuadValue.
    ///
    /// FluidUIKeyText stores a DateTime, a changeable key string property
    /// and a changeable text property.
    /// </summary>
    public QuadValue<FluidUIKeyText> KeyTextQuadValueB { get; set; }

    /// <summary>
    /// Gets or sets FluidUIKeyText models.
    ///
    /// Experimental storage 3 of 4 for a total of 16.
    /// 4 FluidUIKeyText items can be changed in one QuadValue.
    ///
    /// FluidUIKeyText stores a DateTime, a changeable key string property
    /// and a changeable text property.
    /// </summary>
    public QuadValue<FluidUIKeyText> KeyTextQuadValueC { get; set; }

    /// <summary>
    /// Gets or sets FluidUIKeyText models.
    ///
    /// Experimental storage 4 of 4 for a total of 16.
    /// 4 FluidUIKeyText items can be changed in one QuadValue.
    ///
    /// FluidUIKeyText stores a DateTime, a changeable key string property
    /// and a changeable text property.
    /// </summary>
    public QuadValue<FluidUIKeyText> KeyTextQuadValueD { get; set; }
}

// EOF