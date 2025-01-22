using CommunityToolkit.Mvvm.ComponentModel;
using EBoard.IOProcesses.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EBoard.Plugins.Tools.EmptyRadial
{
    public partial class EmptyRadialViewModel : ObservableObject
    {
        [ObservableProperty]
        private string content = "\t\t\t\n\n\n";

        [ObservableProperty]
        private RadialGradientBrush background = new RadialGradientBrush(
            [new GradientStop(Colors.AliceBlue, 0.0), new GradientStop(Colors.DarkSlateGray, 0.5)]
        )
        {
            Center = new Point(0.5, 0.5),
            GradientOrigin = new Point(0.25, 0.5)
        };

        public EmptyRadialViewModel()
        {

            //GradientStopCollection gradientStops = new GradientStopCollection();
            //gradientStops.Add(new GradientStop(Colors.AliceBlue, 0.0));
            //gradientStops.Add(new GradientStop(Colors.DarkSlateGray, 0.5));

            //Point end = new Point(0.5, 1);

            //RadialGradientBrush radialGradientBrush = new RadialGradientBrush(gradientStops)
            //{
            //};

            //Background = radialGradientBrush;

            //Content =  "\t\t\t\n\n\n";

        }
    }
}
