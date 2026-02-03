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
namespace EBoardSDK.Models.FluidUIDesign;

using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.Utilities.Factories;
using System;
using System.Text.Json.Serialization;
using System.Windows.Media;

/// <summary>
/// Provides properties for FluidUI-Design logic
/// and is the model class for state serialization
/// or deserialization.
/// </summary>
public class FluidUIDesignModel : IFluidUIDesignModel
{
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

    private FluidUIBrushModel backgroundColor = new(FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush);
    private FluidUIBrushModel foregroundColor = new(FluidUIDesignDefaultPropertyFactory.ForegroundDefaultSolidColorBrush);
    private FluidUIBrushModel borderColor = new(FluidUIDesignDefaultPropertyFactory.BorderDefaultSolidColorBrush);
    private FluidUIBrushModel highlightColor = new(FluidUIDesignDefaultPropertyFactory.HighlightDefaultSolidColorBrush);
    private FluidUIBrushModel selectionFallbackColor = new(FluidUIDesignDefaultPropertyFactory.SelectionFallbackDefaultSolidColorBrush);

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIDesignModel"/> class.
    /// </summary>
    public FluidUIDesignModel()
    {
    }

    /// <inheritdoc/>
    public event Action? PropertyChangedEvent;

    /// <summary>
    /// Gets or sets Background brush property, that is mostly used on FluidUI CAs as
    /// value for Background properties or as shape Fill.
    ///
    /// It is sometimes used as Foreground brush, when Foreground or Highlight brushes
    /// are used as Background. This allows to communicate something of importance or
    /// relevance to the user, without breaking the feeling of the design.
    /// </summary>
    [JsonIgnore]
    public Brush Background
    {
        get
        {
            return this.background;
        }

        set
        {
            if (this.background != value)
            {
                this.background = value;

                this.InvokePropertyChangedEvent();
            }

            if (!this.onBackgroundLoad)
            {
                this.BackgroundColor = new FluidUIBrushModel(value);
            }
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
            if (this.foreground != value)
            {
                this.foreground = value;

                this.InvokePropertyChangedEvent();
            }

            if (!this.onForegroundLoad)
            {
                this.ForegroundColor = new FluidUIBrushModel(value);
            }
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
            if (this.border != value)
            {
                this.border = value;

                this.InvokePropertyChangedEvent();
            }

            if (!this.onBorderLoad)
            {
                this.BorderColor = new FluidUIBrushModel(value);
            }
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
            if (this.highlight != value)
            {
                this.highlight = value;

                this.InvokePropertyChangedEvent();
            }

            if (!this.onHighlightLoad)
            {
                this.HighlightColor = new FluidUIBrushModel(value);
            }
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
            if (this.selectionFallbackBrush != value)
            {
                this.selectionFallbackBrush = value;

                this.InvokePropertyChangedEvent();
            }

            if (!this.onSelectionFallbackLoad)
            {
                this.SelectionFallbackColor = new FluidUIBrushModel(value);
            }
        }
    }

    public FluidUIBrushModel BackgroundColor
    {
        get
        {
            return this.backgroundColor;
        }

        set
        {
            this.backgroundColor = value;

            if (this.onBackgroundLoad)
            {
                this.BuildBrushesFromFluidUIBrushModel(BrushTargets.Background).Wait();
            }
        }
    }

    public FluidUIBrushModel ForegroundColor
    {
        get
        {
            return this.foregroundColor;
        }

        set
        {
            this.foregroundColor = value;

            if (this.onForegroundLoad)
            {
                this.BuildBrushesFromFluidUIBrushModel(BrushTargets.Foreground).Wait();
            }
        }
    }

    public FluidUIBrushModel BorderColor
    {
        get
        {
            return this.borderColor;
        }

        set
        {
            this.borderColor = value;

            if (this.onBorderLoad)
            {
                this.BuildBrushesFromFluidUIBrushModel(BrushTargets.Border).Wait();
            }
        }
    }

    public FluidUIBrushModel HighlightColor
    {
        get
        {
            return this.highlightColor;
        }

        set
        {
            this.highlightColor = value;

            if (this.onHighlightLoad)
            {
                this.BuildBrushesFromFluidUIBrushModel(BrushTargets.Highlight).Wait();
            }
        }
    }

    public FluidUIBrushModel SelectionFallbackColor
    {
        get
        {
            return this.selectionFallbackColor;
        }

        set
        {
            this.selectionFallbackColor = value;

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

        set
        {
            if (this.opacity != value)
            {
                this.opacity = value;

                this.InvokePropertyChangedEvent();
            }
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    public void ResetValuesToInitial(bool elementIsSelected)
    {
        this.SetInitialValues();

        if (elementIsSelected)
        {
            this.Border = FluidUIDesignDefaultPropertyFactory.HighlightDefaultSolidColorBrush;
            this.SelectionFallbackBrush = FluidUIDesignDefaultPropertyFactory.BorderDefaultSolidColorBrush;
        }
    }

    /// <inheritdoc/>
    public void SetInitialValues()
    {
        this.Opacity = FluidUIDesignDefaultPropertyFactory.DefaultOpacity;

        this.Background = FluidUIDesignDefaultPropertyFactory.BackgroundDefaultSolidColorBrush;
        this.Foreground = FluidUIDesignDefaultPropertyFactory.ForegroundDefaultSolidColorBrush;
        this.Border = FluidUIDesignDefaultPropertyFactory.BorderDefaultSolidColorBrush;
        this.Highlight = FluidUIDesignDefaultPropertyFactory.HighlightDefaultSolidColorBrush;
        this.SelectionFallbackBrush = FluidUIDesignDefaultPropertyFactory.SelectionFallbackDefaultSolidColorBrush;

        this.BackgroundColor = new FluidUIBrushModel(this.Background);
        this.ForegroundColor = new FluidUIBrushModel(this.Foreground);
        this.BorderColor = new FluidUIBrushModel(this.Border);
        this.HighlightColor = new FluidUIBrushModel(this.Highlight);
        this.SelectionFallbackColor = new FluidUIBrushModel(this.SelectionFallbackBrush);
    }

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
    }
}

// EOF