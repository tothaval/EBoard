using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using EBoardSDK.Plugins.Elements.StandardText;

using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EBoardSDK.Plugins.Tools.EmptyRadial;

public partial class EmptyRadialViewModel : ObservableObject, IPlugin
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


    #region IPlugin properties

    public BorderManagement BorderManagement { get; set; }


    public BrushManagement BrushManagement { get; set; }


    // !!!! prüfen ob sinnvoll und relevant, ggf. ersetzen
    // später ggf. per Factory oder via Singleton, falls nötig
    // ist für die option, im load des programms den view typ sauber
    // instanzieren zu können.
    private StandardTextView plugin = new StandardTextView();
    public UserControl Plugin => plugin;


    [ObservableProperty]
    private CornerRadius cornerRadius;
    [ObservableProperty]
    private int cornerRadiusValue;

    partial void OnCornerRadiusValueChanged(int value)
    {
        BorderManagement.CornerRadius = new CornerRadius(value);

        OnPropertyChanged(nameof(BorderManagement));
    }


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManagement))]
    private double height;

    partial void OnHeightChanged(double value)
    {
        BorderManagement.Height = value;
    }


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManagement))]
    private double width;

    partial void OnWidthChanged(double value)
    {
        BorderManagement.Width = value;
    }


    public PluginDataSet PluginDataSet { get; set; } = new PluginDataSet();


    [ObservableProperty]
    private string pluginHeader = "Empty Radial";


    [ObservableProperty]
    private string pluginName = "EmptyRadial";

    #endregion


    public EmptyRadialViewModel() => InstantiateProperties();


    public bool ApplyBackgroundBrush(Brush brush)
    {
        try
        {
            BrushManagement.Background = brush;

            OnPropertyChanged(nameof(BrushManagement));

            OnPropertyChanged(nameof(BrushManagement.Background));

            return true;
        }
        catch (Exception)
        {

            return false;
        }
    }


    private void InstantiateProperties()
    {
        BorderManagement = new BorderManagement();
        BrushManagement = new BrushManagement();
    }


    public Task Load(string path, IElementDataSet elementDataSet)
    {
        return Task.CompletedTask;
    }


    public Task Save(string path, IElementDataSet elementDataSet)
    {
        return Task.CompletedTask;
    }


    public bool SelectionChange(bool isSelected)
    {

        if (isSelected)
        {
            BrushManagement.SwitchBorderToHighlight();

            OnPropertyChanged(nameof(BrushManagement));

            return true;
        }

        BrushManagement.SwitchBorderToBorder();

        OnPropertyChanged(nameof(BrushManagement));

        return false;
    }


}// EOF