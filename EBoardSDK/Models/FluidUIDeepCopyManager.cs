// <copyright file="FluidUIDeepCopyManager.cs" company=".">
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
using System.Windows;

internal class FluidUIDeepCopyManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIDeepCopyManager"/> class.
    /// </summary>
    internal FluidUIDeepCopyManager()
    {
    }

    internal IFluidUIContext DeepCopyIFluidUIContext(IFluidUIContext? context = null)
    {
        var fluidUIContext = new FluidUIContext();

        if (context != null)
        {
            var data = this.DeepCopyDataBlockModel(context.DataBlock);
            var design = this.DeepCopyDesignModel(context.Design);
            var font = this.DeepCopyFontModel(context.Font);
            var size = this.DeepCopySizeModel(context.Size);
            var stand = this.DeepCopyStandModel(context.Stand);

            fluidUIContext.DataBlock = (FluidUIDataBlockModel?)data;
            fluidUIContext.Design = (FluidUIDesignModel?)design;
            fluidUIContext.Font = (FluidUIFontModel?)font;
            fluidUIContext.Size = (FluidUISizeModel?)size;
            fluidUIContext.Stand = (FluidUIStandModel?)stand;

            return fluidUIContext;
        }

        return fluidUIContext;
    }

    internal IFluidUIDataBlockModel DeepCopyDataBlockModel(IFluidUIDataBlockModel model)
    {
        var fluidUIDataBlockModel = new FluidUIDataBlockModel();

        if (model != null)
        {
            fluidUIDataBlockModel.Text = model.Text.ToString();
            fluidUIDataBlockModel.Title = model.Title.ToString();

            foreach (var item in model.IndexTextList)
            {
                fluidUIDataBlockModel.IndexTextList.Add(new FluidUIIndexText() { Index = item.Index, Text = item.Text, Time = item.Time });
            }

            foreach (var item in model.KeyTextList)
            {
                fluidUIDataBlockModel.KeyTextList.Add(new FluidUIKeyText() { Key = item.Key, Text = item.Text, Time = item.Time });
            }
        }

        return fluidUIDataBlockModel;
    }

    internal IFluidUIDesignModel DeepCopyDesignModel(IFluidUIDesignModel model)
    {
        var fluidUIDesignModel = new FluidUIDesignModel();

        fluidUIDesignModel.LoadBrushesFromColorData();

        if (model != null)
        {
            fluidUIDesignModel.Background = model.BackgroundColor.GetBrush().Result.Clone();
            fluidUIDesignModel.Foreground = model.ForegroundColor.GetBrush().Result.Clone();
            fluidUIDesignModel.Border = model.BorderColor.GetBrush().Result.Clone();
            fluidUIDesignModel.Highlight = model.HighlightColor.GetBrush().Result.Clone();
            fluidUIDesignModel.SelectionFallbackBrush = model.SelectionFallbackColor.GetBrush().Result.Clone();

            fluidUIDesignModel.ImagePath = model.ImagePath?.ToString() ?? string.Empty;
            fluidUIDesignModel.ImageForegroundPath = model.ImageForegroundPath?.ToString() ?? string.Empty;
            fluidUIDesignModel.ImageBorderPath = model.ImageBorderPath?.ToString() ?? string.Empty;
            fluidUIDesignModel.ImageHighlightPath = model.ImageHighlightPath?.ToString() ?? string.Empty;
        }

        return fluidUIDesignModel;
    }

    internal IFluidUIFontModel DeepCopyFontModel(IFluidUIFontModel model)
    {
        var fluidUIFontModel = new FluidUIFontModel();

        if (model != null)
        {
            fluidUIFontModel.FontWeight = model.FontWeight;
            fluidUIFontModel.FontWeightValue = model.FontWeightValue + 0;
            fluidUIFontModel.FontSize = model.FontSize + 0.0;
            fluidUIFontModel.FontFamily = new System.Windows.Media.FontFamily(model.FontFamilyName);
            fluidUIFontModel.FontFamilyName = model.FontFamilyName + string.Empty;
        }

        return fluidUIFontModel;
    }

    internal IFluidUISizeModel DeepCopySizeModel(IFluidUISizeModel model)
    {
        var fluidUISizeModel = new FluidUISizeModel();

        if (model != null)
        {
            int width = (int)model.Width;
            int height = (int)model.Height;

            fluidUISizeModel.Width = width;
            fluidUISizeModel.Height = height;

            int value1 = (int)model.BorderThickness.Left;
            int value2 = (int)model.BorderThickness.Top;
            int value3 = (int)model.BorderThickness.Right;
            int value4 = (int)model.BorderThickness.Bottom;

            var borderthickness = new Thickness(value1, value2, value3, value4);

            fluidUISizeModel.BorderThickness = borderthickness;

            value1 = (int)model.Margin.Left;
            value2 = (int)model.Margin.Top;
            value3 = (int)model.Margin.Right;
            value4 = (int)model.Margin.Bottom;

            var margin = new Thickness(value1, value2, value3, value4);

            fluidUISizeModel.Margin = margin;

            value1 = (int)model.Padding.Left;
            value2 = (int)model.Padding.Top;
            value3 = (int)model.Padding.Right;
            value4 = (int)model.Padding.Bottom;

            var padding = new Thickness(value1, value2, value3, value4);

            fluidUISizeModel.Padding = padding;

            value1 = (int)model.CornerRadius.TopLeft;
            value2 = (int)model.CornerRadius.TopRight;
            value3 = (int)model.CornerRadius.BottomRight;
            value4 = (int)model.CornerRadius.BottomLeft;

            var cornerRadius = new CornerRadius(value1, value2, value3, value4);

            fluidUISizeModel.CornerRadius = cornerRadius;
        }

        return fluidUISizeModel;
    }

    internal IFluidUIStandModel DeepCopyStandModel(IFluidUIStandModel model)
    {
        var fluidUIStandModel = new FluidUIStandModel();

        if (model != null)
        {
            fluidUIStandModel.Angle = model.Angle;
            fluidUIStandModel.Position = new System.Windows.Point(model.Position.X, model.Position.Y);
            fluidUIStandModel.Z = model.Z;
        }

        return fluidUIStandModel;
    }
}

// EOF