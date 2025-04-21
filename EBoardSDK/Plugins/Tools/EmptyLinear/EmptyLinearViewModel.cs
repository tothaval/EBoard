namespace EBoardSDK.Plugins.Tools.EmptyLinear;

using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.StandardText;

using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class EmptyLinearViewModel : ObservableObject, IPlugin
{
    [ObservableProperty]
    private string content = "\t\t\t\n\n\n";

    [ObservableProperty]
    private LinearGradientBrush background = new LinearGradientBrush(
        [new GradientStop(Colors.AliceBlue, 0.0), new GradientStop(Colors.Navy, 0.5)],
        new Point(0, 0),
        new Point(0.5, 1)
    );

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
    [NotifyPropertyChangedFor(nameof(BorderManagement))]
    private int cornerRadiusValue;

    partial void OnCornerRadiusValueChanged(int value)
    {
        BorderManagement.CornerRadius = new CornerRadius(value);
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

    [ObservableProperty]
    private string pluginHeader = "Empty Linear";


    [ObservableProperty]
    private string pluginName = "Empty Linear";

    #endregion


    public EmptyLinearViewModel() => InstantiateProperties();

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