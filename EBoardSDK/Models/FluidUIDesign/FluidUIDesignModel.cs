// <copyright file="FluidUIDesignModel.cs" company=".">
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

public class FluidUIDesignModel : IFluidUIDesignModel
{
<<<<<<< Updated upstream
    private Brush background = new SolidColorBrush(Colors.White);
    private Brush border = new SolidColorBrush(Colors.Black);
    private Brush foreground = new SolidColorBrush(Colors.DarkGray);
    private Brush highlight = new SolidColorBrush(Colors.DarkGoldenrod);
    private Brush selectionFallbackBrush = new SolidColorBrush(Colors.WhiteSmoke);
=======
    private bool onBackgroundLoad = true;
    private bool onForegroundLoad = true;
    private bool onBorderLoad = true;
    private bool onHighlightLoad = true;
    private bool onSelectionFallbackLoad = true;

    private double opacity = FluidUIDesignDefaultPropertyFactory.DefaultOpacity;

    private Brush background = FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;
    private Brush foreground = FluidUIDesignDefaultPropertyFactory.ForegroundDefaultSolidColorBrush;
    private Brush border = FluidUIDesignDefaultPropertyFactory.BorderDefaultSolidColorBrush;
    private Brush highlight = FluidUIDesignDefaultPropertyFactory.HighlightDefaultSolidColorBrush;
    private Brush selectionFallbackBrush = FluidUIDesignDefaultPropertyFactory.SelectionFallbackDefaultSolidColorBrush;

    private Brush hideElementFallbackBrush = FluidUIDesignDefaultPropertyFactory.SelectionFallbackDefaultSolidColorBrush;

    private FluidUIBrushModel backgroundColor = new(FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush);
    private FluidUIBrushModel foregroundColor = new(FluidUIDesignDefaultPropertyFactory.ForegroundDefaultSolidColorBrush);
    private FluidUIBrushModel borderColor = new(FluidUIDesignDefaultPropertyFactory.BorderDefaultSolidColorBrush);
    private FluidUIBrushModel highlightColor = new(FluidUIDesignDefaultPropertyFactory.HighlightDefaultSolidColorBrush);
    private FluidUIBrushModel selectionFallbackColor = new(FluidUIDesignDefaultPropertyFactory.SelectionFallbackDefaultSolidColorBrush);
    private FluidUIBrushModel hideElementFallbackColor = new(FluidUIDesignDefaultPropertyFactory.SelectionFallbackDefaultSolidColorBrush);
>>>>>>> Stashed changes

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIDesignModel"/> class.
    /// </summary>
    public FluidUIDesignModel()
    {
        this.SetInitialValues();
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
        }
    }

<<<<<<< Updated upstream
    // foreground brush related properties, foreground is used for text color
    [JsonIgnore]
    public Brush Foreground
=======
    // store background brush while user control object is highlighted due to selection or due to having focus
    [JsonIgnore]
    public Brush HideElementFallbackBrush
    {
        get
        {
            return this.selectionFallbackBrush;
        }

        set
        {
            if (this.hideElementFallbackBrush != value)
            {
                this.hideElementFallbackBrush = value;

                this.InvokePropertyChangedEvent();
            }

            if (!this.onSelectionFallbackLoad)
            {
                this.hideElementFallbackColor = new FluidUIBrushModel(value);
            }
        }
    }

    public FluidUIBrushModel BackgroundColor
>>>>>>> Stashed changes
    {
        get
        {
            return this.foreground;
        }

        set
        {
            this.foreground = value;

            this.ForegroundColor = new ColorDataModel(value);
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
        }
    }

    public ColorDataModel BackgroundColor { get; set; } = new ();

    public ColorDataModel BorderColor { get; set; } = new ();

    public ColorDataModel ForegroundColor { get; set; } = new ();

    public ColorDataModel HighlightColor { get; set; } = new();

    public ColorDataModel SelectionFallbackColor { get; set; } = new();

    public string ImagePath { get; set; } = string.Empty;

<<<<<<< Updated upstream
    public string ImageForegroundPath { get; set; } = string.Empty;
=======
    public FluidUIBrushModel HideElementFallbackColor
    {
        get
        {
            return this.hideElementFallbackColor;
        }

        set
        {
            this.hideElementFallbackColor = value;

            if (this.onSelectionFallbackLoad)
            {
                this.BuildBrushesFromFluidUIBrushModel(BrushTargets.SelectionFallback).Wait();
            }
        }
    }

    public double Opacity
    {
        get
        {
            return this.opacity;
        }
>>>>>>> Stashed changes

    public string ImageBorderPath { get; set; } = string.Empty;

    public string ImageHighlightPath { get; set; } = string.Empty;

    public double Opacity { get; set; } = 1.0;

    public void Dispose()
    {
    }

    public void LoadBrushesFromColorData()
    {
        this.Background = this.BackgroundColor?.GetBrush().Result ?? new SolidColorBrush(Colors.White);
        this.Foreground = this.ForegroundColor?.GetBrush().Result ?? new SolidColorBrush(Colors.DarkGray);
        this.Border = this.BorderColor?.GetBrush().Result ?? new SolidColorBrush(Colors.Black);
        this.Highlight = this.HighlightColor?.GetBrush().Result ?? new SolidColorBrush(Colors.DarkGoldenrod);
        this.SelectionFallbackBrush = this.SelectionFallbackColor?.GetBrush().Result ?? new SolidColorBrush(Colors.WhiteSmoke);
    }

    public void ResetValuesToInitial(bool elementIsSelected)
    {
        this.SetInitialValues();

        if (elementIsSelected)
        {
            this.Border = new SolidColorBrush(Colors.DarkGoldenrod);
            this.SelectionFallbackBrush = new SolidColorBrush(Colors.Black);
        }
    }

    public void SetInitialValues()
    {
        this.ImagePath = string.Empty;
        this.ImageForegroundPath = string.Empty;
        this.ImageBorderPath = string.Empty;
        this.ImageHighlightPath = string.Empty;

        this.Opacity = 1.0;

        this.Background = new SolidColorBrush(Colors.White);
        this.Foreground = new SolidColorBrush(Colors.DarkGray);
        this.Highlight = new SolidColorBrush(Colors.DarkGoldenrod);
        this.Border = new SolidColorBrush(Colors.Black);
        this.SelectionFallbackBrush = new SolidColorBrush(Colors.WhiteSmoke);

<<<<<<< Updated upstream
        this.BackgroundColor = new ColorDataModel(this.Background);
        this.BorderColor = new ColorDataModel(this.Border);
        this.ForegroundColor = new ColorDataModel(this.Foreground);
        this.HighlightColor = new ColorDataModel(this.Highlight);
        this.SelectionFallbackColor = new ColorDataModel(this.SelectionFallbackBrush);
=======
    internal async Task BuildBrushesFromFluidUIBrushModel(BrushTargets brushTargets)
    {
        switch (brushTargets)
        {
            case BrushTargets.Background:
                this.background = await new FluidUIBrushManager(this.backgroundColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;
                this.onBackgroundLoad = false;
                break;
            case BrushTargets.Foreground:
                this.foreground = await new FluidUIBrushManager(this.foregroundColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.ForegroundDefaultSolidColorBrush;
                this.onForegroundLoad = false;
                break;
            case BrushTargets.Border:
                this.border = await new FluidUIBrushManager(this.borderColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.BorderDefaultSolidColorBrush;
                this.onBorderLoad = false;
                break;
            case BrushTargets.Highlight:
                this.highlight = await new FluidUIBrushManager(this.highlightColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.HighlightDefaultSolidColorBrush;
                this.onHighlightLoad = false;
                break;
            case BrushTargets.SelectionFallback:
                this.selectionFallbackBrush = await new FluidUIBrushManager(this.selectionFallbackColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.SelectionFallbackDefaultSolidColorBrush;
                this.onSelectionFallbackLoad = false;
                break;
            case BrushTargets.HideElementFallback:
                this.hideElementFallbackBrush = await new FluidUIBrushManager(this.hideElementFallbackColor).GetBrush() ?? FluidUIDesignDefaultPropertyFactory.SelectionFallbackDefaultSolidColorBrush;
                this.onSelectionFallbackLoad = false;
                break;
            default:
                break;
        }
    }

    /// <inheritdoc/>
    public void UpdateValues()
    {
        this.InvokePropertyChangedEvent();
    }

    private void InvokePropertyChangedEvent()
    {
        this.PropertyChangedEvent?.Invoke();
>>>>>>> Stashed changes
    }
}

// EOF