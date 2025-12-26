// <copyright file="BrushManagement.cs" company=".">
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
/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  BrushManagement
 *
 *  model for brush property changes
 */
namespace EBoardSDK.Models.FluidUIDesign;

using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.Models;
using System;
using System.Text.Json.Serialization;
using System.Windows.Media;

public class BrushManagement : IFluidUIDesignModel
{
    private Brush background;
    private Brush border;
    private Brush foreground;
    private Brush highlight;
    private Brush selectionFallbackBrush;

    public BrushManagement()
    {
    }

    public event Action PropertyChangedEvent;

    // background brush related properties, background is used on content border or as shape fill
    [JsonIgnore]
    public Brush Background
    {
        get
        {
            return this.background;
        }

        set
        {
            this.background = value;
            this.BackgroundColor = new ColorDataModel(value);
            //this.PropertyChangedEvent?.Invoke();
        }
    }

    // store background brush while user control object is highlighted due to selection or due to having focus
    [JsonIgnore]
    public Brush SelectionFallbackBrush
    {
        get
        {
            return this.selectionFallbackBrush;
        }

        set
        {
            this.selectionFallbackBrush = value;
            this.SelectionFallbackColor = new ColorDataModel(value);
            //this.PropertyChangedEvent?.Invoke();
        }
    }

    // foreground brush related properties, foreground is used for text color
    [JsonIgnore]
    public Brush Foreground
    {
        get
        {
            return this.foreground;
        }

        set
        {
            this.foreground = value;
            this.ForegroundColor = new ColorDataModel(value);
            //this.PropertyChangedEvent?.Invoke();
        }
    }

    // border brush related properties, border is used on content border or as shape stroke
    [JsonIgnore]
    public Brush Border
    {
        get
        {
            return this.border;
        }

        set
        {
            this.border = value;
            this.BorderColor = new ColorDataModel(value);
            //this.PropertyChangedEvent?.Invoke();
        }
    }

    // higlight brush related properties, highlight is used on element selection
    [JsonIgnore]
    public Brush Highlight
    {
        get
        {
            return this.highlight;
        }

        set
        {
            this.highlight = value;
            this.HighlightColor = new ColorDataModel(value);
            //this.PropertyChangedEvent?.Invoke();
        }
    }

    public ColorDataModel BackgroundColor { get; set; }

    public ColorDataModel BorderColor { get; set; }

    public ColorDataModel ForegroundColor { get; set; }

    public ColorDataModel HighlightColor { get; set; }

    public ColorDataModel SelectionFallbackColor { get; set; }

    public string ImagePath { get; set; }

    public string ImageForegroundPath { get; set; }

    public string ImageBorderPath { get; set; }

    public string ImageHighlightPath { get; set; }

    public void Dispose()
    {
    }

    public void LoadBrushesFromColorData()
    {
        this.Background = this.BackgroundColor.GetBrush().Result;
        this.Foreground = this.ForegroundColor.GetBrush().Result;
        this.Border = this.BorderColor.GetBrush().Result;
        this.Highlight = this.HighlightColor.GetBrush().Result;
        this.SelectionFallbackBrush = this.SelectionFallbackColor?.GetBrush().Result ?? new SolidColorBrush();
    }

    public void SetInitialValues()
    {
        this.Background = new SolidColorBrush(Color.FromArgb(205, 0, 0, 0));
        this.Border = new SolidColorBrush(Colors.Goldenrod);
        this.Foreground = new SolidColorBrush(Colors.DarkGoldenrod);
        this.Highlight = new SolidColorBrush(Colors.YellowGreen);

        this.ImagePath = string.Empty;
        this.ImageForegroundPath = string.Empty;
        this.ImageBorderPath = string.Empty;
        this.ImageHighlightPath = string.Empty;
    }
}

// EOF