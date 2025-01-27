using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace EBoard.Plugins.Tools.EmptyRadial;

public partial class EmptyRadialViewModel : PluginViewModel
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
    }


}// EOF