// <copyright file="FluidUIContextManager.cs" company=".">
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
using EBoardSDK.ViewModels;
using System.Windows;
using System.Windows.Media;

internal class FluidUIContextManager : IFluidUIManager
{
    private EboardFluidUIBaseViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIContextManager"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    internal FluidUIContextManager(EboardFluidUIBaseViewModel viewModel)
    {
        this.viewModel = viewModel;
    }

    public void Reset()
    {
        new FluidUIDataBlockManager(this.viewModel).Reset();
        new FluidUIDesignManager(this.viewModel).Reset();
        new FluidUIFontManager(this.viewModel).Reset();
        new FluidUISizeManager(this.viewModel).Reset();
        new FluidUIStandManager(this.viewModel).Reset();
    }

    internal void Apply_FluidUI(
         IFluidUIDataBlockModel? dataBlock = null,
         IFluidUIDesignModel? design = null,
         IFluidUIFontModel? font = null,
         IFluidUISizeModel? size = null,
         IFluidUIStandModel? stand = null)
    {
        this.Apply_FluidUIDataBlock(dataBlock);
        this.Apply_FluidUIDesign(design);
        this.Apply_FluidUIFont(font);
        this.Apply_FluidUISize(size);
        this.Apply_FluidUIStand(stand);
    }

    internal void Apply_FluidUIDataBlock(IFluidUIDataBlockModel? dataBlock = null)
    {
        if (dataBlock != null)
        {
            this.viewModel.FluidUI.DataBlock = (FluidUIDataBlockModel)dataBlock;

            return;
        }

        this.viewModel.FluidUI.DataBlock = new FluidUIDataBlockModel()
        {
            Title = "title",
            Text = "text",
        };
    }

    internal void Apply_FluidUIDesign(IFluidUIDesignModel? design = null)
    {
        if (design != null)
        {
            this.viewModel.FluidUI.Design = (FluidUIDesignModel)design;

            return;
        }

        this.viewModel.FluidUI.Design = new FluidUIDesignModel()
        {
            Background = new SolidColorBrush(Colors.White),
            Foreground = new SolidColorBrush(Colors.DarkGray),
            Border = new SolidColorBrush(Colors.Black),
            Highlight = new SolidColorBrush(Colors.DarkGoldenrod),
        };
    }

    internal void Apply_FluidUIFont(IFluidUIFontModel? font = null)
    {
        if (font != null)
        {
            this.viewModel.FluidUI.Font = (FluidUIFontModel)font;

            return;
        }

        this.viewModel.FluidUI.Font = new FluidUIFontModel()
        {
            FontSize = 15.0,
            FontFamily = new System.Windows.Media.FontFamily("Verdana"),
            FontWeight = FontWeights.Normal,
        };
    }

    internal void Apply_FluidUISize(IFluidUISizeModel? size = null)
    {
        if (size != null)
        {
            this.viewModel.FluidUI.Size = (FluidUISizeModel)size;

            return;
        }

        this.viewModel.FluidUI.Size = new FluidUISizeModel()
        {
            Margin = new Thickness(5),
            CornerRadius = new CornerRadius(0),
            BorderThickness = new Thickness(2),
            Padding = new Thickness(10, 5, 10, 5),
            Height = double.NaN,
            Width = double.NaN,
        };
    }

    internal void Apply_FluidUIStand(IFluidUIStandModel? stand = null)
    {
        if (stand != null)
        {
            this.viewModel.FluidUI.Stand = (FluidUIStandModel)stand;

            return;
        }

        this.viewModel.FluidUI.Stand = new FluidUIStandModel()
        {
            Angle = 0,
            Z = 0,
            Position = new System.Windows.Point(5, 5),
        };
    }
}

// EOF