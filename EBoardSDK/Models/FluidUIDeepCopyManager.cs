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
using EBoardSDK.Interfaces.FluidUIFont;
using EBoardSDK.Interfaces.FluidUISize;
using EBoardSDK.Interfaces.FluidUIStand;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Models.FluidUIFont;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities.Factories;
using System.Windows;
using System.Windows.Media;

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
            if (context.DataBlock != null)
            {
                var data = this.DeepCopyDataBlockModel(context.DataBlock);
                fluidUIContext.DataBlock = (FluidUIDataBlockModel?)data;
            }

            if (context.Design != null)
            {
                var design = this.DeepCopyDesignModel(context.Design);
                fluidUIContext.Design = (FluidUIDesignModel?)design.Result;
            }

            if (context.Font != null)
            {
                var font = this.DeepCopyFontModel(context.Font);
                fluidUIContext.Font = (FluidUIFontModel?)font;
            }

            if (context.Size != null)
            {
                var size = this.DeepCopySizeModel(context.Size);
                fluidUIContext.Size = (FluidUISizeModel?)size;
            }

            if (context.Stand != null)
            {
                var stand = this.DeepCopyStandModel(context.Stand);
                fluidUIContext.Stand = (FluidUIStandModel?)stand;
            }
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

            fluidUIDataBlockModel.IndexTextQuadValue1 = this.GetIndexTextQuadValue(model.IndexTextQuadValue1);
            fluidUIDataBlockModel.IndexTextQuadValue2 = this.GetIndexTextQuadValue(model.IndexTextQuadValue2);
            fluidUIDataBlockModel.IndexTextQuadValue3 = this.GetIndexTextQuadValue(model.IndexTextQuadValue3);
            fluidUIDataBlockModel.IndexTextQuadValue4 = this.GetIndexTextQuadValue(model.IndexTextQuadValue4);

            fluidUIDataBlockModel.KeyTextQuadValueA = this.GetKeyTextQuadValue(model.KeyTextQuadValueA);
            fluidUIDataBlockModel.KeyTextQuadValueB = this.GetKeyTextQuadValue(model.KeyTextQuadValueB);
            fluidUIDataBlockModel.KeyTextQuadValueC = this.GetKeyTextQuadValue(model.KeyTextQuadValueC);
            fluidUIDataBlockModel.KeyTextQuadValueD = this.GetKeyTextQuadValue(model.KeyTextQuadValueD);
        }

        return fluidUIDataBlockModel;
    }

    internal async Task<IFluidUIDesignModel> DeepCopyDesignModel(IFluidUIDesignModel model)
    {
        var fluidUIDesignModel = new FluidUIDesignModel();

        if (model != null)
        {
            fluidUIDesignModel.Background = await new FluidUIBrushManager(model.BackgroundColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;
            fluidUIDesignModel.Foreground = await new FluidUIBrushManager(model.ForegroundColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.ForegroundDefaultSolidColorBrush;
            fluidUIDesignModel.Border = await new FluidUIBrushManager(model.BorderColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.BorderDefaultSolidColorBrush;
            fluidUIDesignModel.Highlight = await new FluidUIBrushManager(model.HighlightColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.HighlightDefaultSolidColorBrush;
            fluidUIDesignModel.SelectionFallbackBrush = await new FluidUIBrushManager(model.SelectionFallbackColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.SelectionFallbackDefaultSolidColorBrush;

            fluidUIDesignModel.BackgroundColor = model.BackgroundColor;
            fluidUIDesignModel.ForegroundColor = model.ForegroundColor;
            fluidUIDesignModel.BorderColor = model.BorderColor;
            fluidUIDesignModel.HighlightColor = model.HighlightColor;
            fluidUIDesignModel.SelectionFallbackColor = model.SelectionFallbackColor;

            fluidUIDesignModel.BackgroundColor.ImagePath = model.BackgroundColor.ImagePath?.ToString() ?? string.Empty;
            fluidUIDesignModel.ForegroundColor.ImagePath = model.ForegroundColor.ImagePath?.ToString() ?? string.Empty;
            fluidUIDesignModel.BorderColor.ImagePath = model.BorderColor.ImagePath?.ToString() ?? string.Empty;
            fluidUIDesignModel.HighlightColor.ImagePath = model.HighlightColor.ImagePath?.ToString() ?? string.Empty;

            fluidUIDesignModel.Opacity = model.Opacity;
        }

        return fluidUIDesignModel;
    }

    internal IFluidUIFontModel DeepCopyFontModel(IFluidUIFontModel model)
    {
        var fluidUIFontModel = new FluidUIFontModel();

        if (model != null)
        {
            fluidUIFontModel.FontWeight = model.FontWeight;
            fluidUIFontModel.FontWeightValue = model.FontWeightValue;
            fluidUIFontModel.FontSize = model.FontSize;
            fluidUIFontModel.FontFamily = new System.Windows.Media.FontFamily(model.FontFamilyName);
            fluidUIFontModel.FontFamilyName = model.FontFamilyName;
        }

        return fluidUIFontModel;
    }

    internal IFluidUISizeModel DeepCopySizeModel(IFluidUISizeModel model)
    {
        var fluidUISizeModel = new FluidUISizeModel();

        if (model != null)
        {
            int width = new SharedMethod_UI().TransformDoubleNaNToInt(model.Width);
            int height = new SharedMethod_UI().TransformDoubleNaNToInt(model.Height);

            fluidUISizeModel.Width = width;
            fluidUISizeModel.Height = height;

            fluidUISizeModel.ScaleX = model.ScaleX;
            fluidUISizeModel.ScaleY = model.ScaleY;

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
            fluidUIStandModel.TransformOriginPoint = new System.Windows.Point(model.TransformOriginPoint.X, model.TransformOriginPoint.Y);

            fluidUIStandModel.SkewAngleX = model.SkewAngleX;
            fluidUIStandModel.SkewAngleY = model.SkewAngleY;
            fluidUIStandModel.SkewCenterPoint = new System.Windows.Point(model.SkewCenterPoint.X, model.SkewCenterPoint.Y);

            fluidUIStandModel.Position = new System.Windows.Point(model.Position.X, model.Position.Y);
            fluidUIStandModel.Xmax = model.Xmax;
            fluidUIStandModel.Xmin = model.Xmin;
            fluidUIStandModel.Ymax = model.Ymax;
            fluidUIStandModel.Ymin = model.Ymin;

            fluidUIStandModel.Z = model.Z;
            fluidUIStandModel.Zmaximum = model.Zmaximum;
            fluidUIStandModel.Zminimum = model.Zminimum;
        }

        return fluidUIStandModel;
    }

    private QuadValue<FluidUIIndexText> GetIndexTextQuadValue(QuadValue<FluidUIIndexText> quadValue)
    {
        var quadindex = new QuadValue<FluidUIIndexText>(
            new FluidUIIndexText()
            {
                Index = quadValue.Value1.Index,
                Time = new DateTime(quadValue.Value1.Time.Ticks),
                Text = quadValue.Value1.Text,
            },
            new FluidUIIndexText()
            {
                Index = quadValue.Value2.Index,
                Time = new DateTime(quadValue.Value2.Time.Ticks),
                Text = quadValue.Value2.Text,
            },
            new FluidUIIndexText()
            {
                Index = quadValue.Value3.Index,
                Time = new DateTime(quadValue.Value3.Time.Ticks),
                Text = quadValue.Value3.Text,
            },
            new FluidUIIndexText()
            {
                Index = quadValue.Value4.Index,
                Time = new DateTime(quadValue.Value4.Time.Ticks),
                Text = quadValue.Value4.Text,
            });

        return quadindex;
    }

    private QuadValue<FluidUIKeyText> GetKeyTextQuadValue(QuadValue<FluidUIKeyText> quadValue)
    {
        var quadindex = new QuadValue<FluidUIKeyText>(
            new FluidUIKeyText()
            {
                Key = quadValue.Value1.Key,
                Time = new DateTime(quadValue.Value1.Time.Ticks),
                Text = quadValue.Value1.Text,
            },
            new FluidUIKeyText()
            {
                Key = quadValue.Value2.Key,
                Time = new DateTime(quadValue.Value2.Time.Ticks),
                Text = quadValue.Value2.Text,
            },
            new FluidUIKeyText()
            {
                Key = quadValue.Value3.Key,
                Time = new DateTime(quadValue.Value3.Time.Ticks),
                Text = quadValue.Value3.Text,
            },
            new FluidUIKeyText()
            {
                Key = quadValue.Value4.Key,
                Time = new DateTime(quadValue.Value4.Time.Ticks),
                Text = quadValue.Value4.Text,
            });

        return quadindex;
    }
}

// EOF