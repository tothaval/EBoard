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
using System;
using System.Windows;
using System.Windows.Media;

public class FluidUIContext : IFluidUIContext
{
    public event Action? PropertyChangedEvent;

    public DataBlockManagement DataBlock { get; set; } = new DataBlockManagement();

    public BrushManagement Design { get; set; } = new BrushManagement();

    public FontManagement Font { get; set; } = new FontManagement();

    public BorderManagement Size { get; set; } = new BorderManagement();

    public PlacementManagement Stand { get; set; } = new PlacementManagement();

    public FluidUIContext()
    {
        this.Apply_FluidUI();
    }

    public void Apply_FluidUI(
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

        //this.DataBlock.PropertyChangedEvent += this.DataBlock_PropertyChangedEvent;
        //this.Design.PropertyChangedEvent += this.Design_PropertyChangedEvent;
        //this.Font.PropertyChangedEvent += this.Font_PropertyChangedEvent;
        //this.Size.PropertyChangedEvent += this.Size_PropertyChangedEvent;
        //this.Stand.PropertyChangedEvent += this.Stand_PropertyChangedEvent;
    }

    public void Apply_FluidUIDataBlock(IFluidUIDataBlockModel? dataBlock = null)
    {
        if (dataBlock != null)
        {
            this.DataBlock = (DataBlockManagement)dataBlock;
        }
        else if (this.DataBlock == null)
        {
            this.DataBlock = new DataBlockManagement()
            {
                Title = "title",
                Text = "text",
            };
        }
    }

    public void Apply_FluidUIDesign(IFluidUIDesignModel? design = null)
    {
        if (design != null)
        {
            this.Design = (BrushManagement)design;
        }
        else if (this.Design == null)
        {
            this.Design = new BrushManagement()
            {
                Background = new SolidColorBrush(Colors.White),
                Foreground = new SolidColorBrush(Colors.DarkGray),
                Border = new SolidColorBrush(Colors.Black),
                Highlight = new SolidColorBrush(Colors.DarkGoldenrod),
            };
        }
    }

    public void Apply_FluidUIFont(IFluidUIFontModel? font = null)
    {
        if (font != null)
        {
            this.Font = (FontManagement)font;
        }
        else if (this.Font == null)
        {
            this.Font = new FontManagement()
            {
                FontSize = 15.0,
                FontFamily = new System.Windows.Media.FontFamily("Verdana"),
                FontWeight = FontWeights.Normal,
            };
        }
    }

    public void Apply_FluidUISize(IFluidUISizeModel? size = null)
    {
        if (size != null)
        {
            this.Size = (BorderManagement)size;
        }
        else if (this.Size == null)
        {
            this.Size = new BorderManagement()
            {
                Margin = new Thickness(5),
                CornerRadius = new CornerRadius(0),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(10, 5, 10, 5),
                Height = double.NaN,
                Width = double.NaN,
            };
        }
    }

    public void Apply_FluidUIStand(IFluidUIStandModel? stand = null)
    {
        if (stand != null)
        {
            this.Stand = (PlacementManagement)stand;
        }
        else if (this.Stand == null)
        {
            this.Stand = new PlacementManagement()
            {
                Angle = 0,
                Z = 0,
                Position = new System.Windows.Point(5, 5),
            };
        }
    }

    public void Dispose()
    {
        //this.DataBlock.PropertyChangedEvent -= this.DataBlock_PropertyChangedEvent;
        //this.Design.PropertyChangedEvent -= this.Design_PropertyChangedEvent;
        //this.Font.PropertyChangedEvent -= this.Font_PropertyChangedEvent;
        //this.Size.PropertyChangedEvent -= this.Size_PropertyChangedEvent;
        //this.Stand.PropertyChangedEvent -= this.Stand_PropertyChangedEvent;

        this.DataBlock.Dispose();
        this.Design.Dispose();
        this.Font.Dispose();
        this.Size.Dispose();
        this.Stand.Dispose();
    }

    public void SetInitialValues()
    {
        this.Apply_FluidUI();

        this.DataBlock?.SetInitialValues();
        this.Design?.SetInitialValues();
        this.Font?.SetInitialValues();
        this.Size?.SetInitialValues();
        this.Stand?.SetInitialValues();
    }

    //private void DataBlock_PropertyChangedEvent()
    //{
    //    this.PropertyChangedEvent?.Invoke();
    //}

    //private void Design_PropertyChangedEvent()
    //{
    //    this.PropertyChangedEvent?.Invoke();
    //}

    //private void Font_PropertyChangedEvent()
    //{
    //    this.PropertyChangedEvent?.Invoke();
    //}

    //private void Size_PropertyChangedEvent()
    //{
    //    this.PropertyChangedEvent?.Invoke();
    //}

    //private void Stand_PropertyChangedEvent()
    //{
    //    this.PropertyChangedEvent?.Invoke();
    //}
}

// EOF