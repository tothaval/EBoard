using CommunityToolkit.Mvvm.ComponentModel;
using EBoard.IOProcesses.DataSets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EBoard.Plugins.Tools.EmptyLinear
{
    public partial class EmptyLinearViewModel : ObservableObject
    {
        [ObservableProperty]
        private string content = "\t\t\t\n\n\n";

        [ObservableProperty]
        private LinearGradientBrush background = new LinearGradientBrush(
            [new GradientStop(Colors.AliceBlue, 0.0), new GradientStop(Colors.Navy, 0.5)],
            new Point(0, 0),
            new Point(0.5, 1)
        );


        public EmptyLinearViewModel()
        {
            //GradientStopCollection gradientStops = new GradientStopCollection();
            //gradientStops.Add(new GradientStop(Colors.AliceBlue, 0.0));
            //gradientStops.Add(new GradientStop(Colors.Navy, 0.5));

            //Point start = new Point(0, 0);
            //Point end = new Point(0.5, 1);

            //Background = new LinearGradientBrush(gradientStops, start, end);


            //Content = "\t\t\t\n\n\n";
        }



    }
}
