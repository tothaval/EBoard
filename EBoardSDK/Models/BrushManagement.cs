﻿/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BrushManagement 
 * 
 *  model for brush property changes
 */
using EBoardSDK.Interfaces;
using EBoardSDK.Models.DataSets;
using System.Windows.Media;

namespace EBoardSDK.Models;

public class BrushManagement : IElementBrushes
{
    // background brush related properties, background is used on content border or as shape fill
    public Brush Background { get; set; }

    // store background brush while user control object is highlighted due to selection or due to having focus
    public Brush SelectionFallbackBrush { get; set; } = new SolidColorBrush(Colors.Black);


    public string ImagePath { get; set; }


    // foreground brush related properties, foreground is used for text color
    public Brush Foreground { get; set; }



    // border brush related properties, border is used on content border or as shape stroke
    public Brush Border { get; set; }



    // higlight brush related properties, highlight is used on element selection
    public Brush Highlight { get; set; }


    public BrushManagement() => SetInitialValues();


    public BrushManagement(BrushDataSet brushDataSet) => LoadBrushDataSet(brushDataSet);


    private async void LoadBrushDataSet(BrushDataSet brushDataSet)
    {
        if (brushDataSet != null)
        {
            Background = await brushDataSet.BackgroundColor.GetBrush();
            Foreground = await brushDataSet.ForegroundColor.GetBrush();
            Border = await brushDataSet.BorderColor.GetBrush();
            Highlight = await brushDataSet.HighlightColor.GetBrush();

            ImagePath = brushDataSet.BackgroundColor.ImagePath;

            await Task.CompletedTask;

            return;
        }

        SetInitialValues();

        await Task.CompletedTask;
    }


    public void SwitchBorderToHighlight()
    {
        SelectionFallbackBrush = Border;
        Border = Highlight;
    }


    public void SwitchBorderToBorder()
    {
        Border = SelectionFallbackBrush;
    }


    private void SetInitialValues()
    {
        Background = new SolidColorBrush(Color.FromArgb(205, 0, 0, 0));
        Border = new SolidColorBrush(Colors.Goldenrod);
        Foreground = new SolidColorBrush(Colors.DarkGoldenrod);
        Highlight = new SolidColorBrush(Colors.YellowGreen);

        ImagePath = string.Empty;
    }

}
// EOF