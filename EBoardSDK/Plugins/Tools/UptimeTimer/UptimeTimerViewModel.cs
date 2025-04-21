namespace EBoardSDK.Plugins.Tools.Uptime;

using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.StandardText;

using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

public partial class UptimeViewModel : ObservableObject, IPlugin
{
    [ObservableProperty]
    private string clock;

    [ObservableProperty]
    private string uptimeValue;


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
    private string pluginHeader = "Uptime";


    [ObservableProperty]
    private string pluginName = "Uptime";

    #endregion


    private DispatcherTimer _timer;


    public UptimeViewModel() => InstantiateProperties();


    private void InstantiateProperties()
    {
        BorderManagement = new BorderManagement();
        BrushManagement = new BrushManagement();

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(200);
        _timer.Tick += _timer_Tick; ;

        _timer.Start();
    }


    private void UpdateOutput()
    {
        long tickCountMs = Environment.TickCount64;

        var uptime = TimeSpan.FromMilliseconds(tickCountMs);
        DateTime currentTime = DateTime.Now;
        Clock = $"{DateTime.Now.ToLocalTime()}";
        UptimeValue = $"{uptime.Hours:D2}:{uptime.Minutes:D2}:{uptime.Seconds:D2}";
    }


    private void _timer_Tick(object? sender, EventArgs e)
    {
        UpdateOutput();
    }

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