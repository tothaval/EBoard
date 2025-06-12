// <copyright file="BrushManagement.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  BrushManagement
 *
 *  model for brush property changes
 */
namespace EBoardSDK.Models;

using EBoardSDK.Interfaces;
using EBoardSDK.Models.DataSets;
using System.Windows.Media;

public class BrushManagement : IElementBrushes
{
    private Brush background;
    private Brush border;
    private Brush foreground;
    private Brush highlight;
    private Brush selectionFallbackBrush;

    // background brush related properties, background is used on content border or as shape fill
    public Brush Background
    {
        get
        {
            return this.background;
        }

        set
        {
            this.background = value;
            this.PropertyChangedEvent?.Invoke();
        }
    }

    // store background brush while user control object is highlighted due to selection or due to having focus

    public Brush SelectionFallbackBrush
    {
        get
        {
            return this.selectionFallbackBrush;
        }

        set
        {
            this.selectionFallbackBrush = value;
            this.PropertyChangedEvent?.Invoke();
        }
    }

    public string ImagePath { get; set; }

    // foreground brush related properties, foreground is used for text color
    public Brush Foreground
    {
        get
        {
            return this.foreground;
        }

        set
        {
            this.foreground = value;
            this.PropertyChangedEvent?.Invoke();
        }
    }

    // border brush related properties, border is used on content border or as shape stroke
    public Brush Border
    {
        get
        {
            return this.border;
        }

        set
        {
            this.border = value;
            this.PropertyChangedEvent?.Invoke();
        }
    }

    // higlight brush related properties, highlight is used on element selection
    public Brush Highlight
    {
        get
        {
            return this.highlight;
        }

        set
        {
            this.highlight = value;
            this.PropertyChangedEvent?.Invoke();
        }
    }

    public BrushManagement() => this.SetInitialValues();

    public BrushManagement(BrushDataSet brushDataSet) => this.LoadBrushDataSet(brushDataSet);

    public event Action PropertyChangedEvent;

    private async void LoadBrushDataSet(BrushDataSet brushDataSet)
    {
        if (brushDataSet != null)
        {
            this.Background = await brushDataSet.BackgroundColor.GetBrush();
            this.Foreground = await brushDataSet.ForegroundColor.GetBrush();
            this.Border = await brushDataSet.BorderColor.GetBrush();
            this.Highlight = await brushDataSet.HighlightColor.GetBrush();

            this.ImagePath = brushDataSet.BackgroundColor.ImagePath;

            await Task.CompletedTask;

            return;
        }

        this.SetInitialValues();

        await Task.CompletedTask;
    }

    public void SwitchBorderToHighlight()
    {
        this.SelectionFallbackBrush = this.Border;
        this.Border = this.Highlight;

        this.PropertyChangedEvent?.Invoke();
    }

    public void SwitchBorderToBorder()
    {
        this.Border = this.SelectionFallbackBrush;

        this.PropertyChangedEvent?.Invoke();
    }

    private void SetInitialValues()
    {
        this.Background = new SolidColorBrush(Color.FromArgb(205, 0, 0, 0));
        this.Border = new SolidColorBrush(Colors.Goldenrod);
        this.Foreground = new SolidColorBrush(Colors.DarkGoldenrod);
        this.Highlight = new SolidColorBrush(Colors.YellowGreen);

        this.ImagePath = string.Empty;
    }
}