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
        public Brush Background { get; set; }


        public string ImagePath { get; set; }


        // foreground brush related properties, foreground is used for text color
        public Brush Foreground { get; set; }



        // border brush related properties, border is used on content border or as shape stroke
        public Brush Border { get; set; }
        


        // higlight brush related properties, highlight is used on element selection
        public Brush Highlight { get; set; }


        public BrushManagement()
        {
            SetInitialValues();
        }


        public BrushManagement(BrushDataSet brushDataSet)
        {
            LoadBrushDataSet(brushDataSet);
        }


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

        private void SetInitialValues()
        {
            Background = new SolidColorBrush(Color.FromArgb(205, 0, 0, 0));
            Border = new SolidColorBrush(Colors.Goldenrod);
            Foreground = new SolidColorBrush(Colors.DarkGoldenrod);
            Highlight = new SolidColorBrush(Colors.YellowGreen);

            ImagePath = string.Empty;
        }

    }
}
