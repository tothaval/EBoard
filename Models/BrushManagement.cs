using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EBoard.Models
{
    public class BrushManagement : IElementBrushes
    {
        // background brush related properties, background is used on content border or as shape fill
        public Brush ElementBackground { get; set; }
        public string ElementImagePath { get; set; }



        // foreground brush related properties, foreground is used for text color
        public Brush ElementForeground { get; set; }



        // border brush related properties, border is used on content border or as shape stroke
        public Brush ElementBorder { get; set; }
        public Thickness ElementBorderThickness { get; set; }
        


        // higlight brush related properties, highlight is used on element selection
        public Brush ElementHighlight { get; set; }


        public BrushManagement()
        {
            ElementBackground = new SolidColorBrush(Color.FromArgb(205, 0, 0, 0));
            ElementBorder = new SolidColorBrush(Colors.Goldenrod);
            ElementForeground = new SolidColorBrush(Colors.DarkGoldenrod);
            ElementHighlight = new SolidColorBrush(Colors.YellowGreen);

            ElementBorderThickness = new Thickness(2, 2, 2, 2);

            ElementImagePath = string.Empty;
        }

        public BrushManagement(BrushDataSet brushDataSet)
        {

            LoadBrushDataSet(brushDataSet);
        }


        private async void LoadBrushDataSet(BrushDataSet brushDataSet)
        {
            ElementBackground = await brushDataSet.BackgroundColor.GetBrush();
            ElementForeground = await brushDataSet.ForegroundColor.GetBrush();
            ElementBorder = await brushDataSet.BorderColor.GetBrush();
            ElementHighlight = await brushDataSet.HighlightColor.GetBrush();

            ElementBorderThickness = brushDataSet.BorderThickness;

            ElementImagePath = brushDataSet.BackgroundColor.ImagePath;
        }

    }
}
