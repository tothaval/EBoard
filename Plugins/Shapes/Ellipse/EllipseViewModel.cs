using CommunityToolkit.Mvvm.ComponentModel;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Plugins.Elements.StandardText;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EBoard.Plugins.Shapes.Ellipse;

public partial class EllipseViewModel : ObservableObject, IPlugin
{

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
    private string pluginHeader = "Ellipse";


    [ObservableProperty]
    private string pluginName = "Ellipse";

    #endregion


    public EllipseViewModel() => InstantiateProperties();


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


    public Task Load(string path, ElementDataSet elementDataSet)
    {
        return Task.CompletedTask;
    }


    public Task Save(string path, ElementDataSet elementDataSet)
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