namespace EboardElementPluginCountDownTimer.Components;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaktionslogik für DateTimePicker.xaml
/// </summary>
public partial class DateTimePicker : UserControl
{
    public int Year
    {
        get { return (int)GetValue(YearProperty); }
        set { SetValue(YearProperty, value); }
    }
    public static readonly DependencyProperty YearProperty =
        DependencyProperty.Register("Year", typeof(int), typeof(DateTimePicker), new PropertyMetadata(2024));

    public int Month
    {
        get { return (int)GetValue(MonthProperty); }
        set { SetValue(MonthProperty, value); Test(); }
    }
    public static readonly DependencyProperty MonthProperty =
        DependencyProperty.Register("Month", typeof(int), typeof(DateTimePicker), new PropertyMetadata(1));
    
    public void Test()
    {
        MessageBox.Show("Test");
    }

    public int Day
    {
        get { return (int)GetValue(DayProperty); }
        set { SetValue(DayProperty, value); }
    }
    public static readonly DependencyProperty DayProperty =
        DependencyProperty.Register("Day", typeof(int), typeof(DateTimePicker), new PropertyMetadata(1));

    public int Hour
    {
        get { return (int)GetValue(HourProperty); }
        set { SetValue(HourProperty, value); }
    }
    public static readonly DependencyProperty HourProperty =
        DependencyProperty.Register("Hour", typeof(int), typeof(DateTimePicker), new PropertyMetadata(1));

    public int Minute
    {
        get { return (int)GetValue(MinuteProperty); }
        set { SetValue(MinuteProperty, value); }
    }
    public static readonly DependencyProperty MinuteProperty =
        DependencyProperty.Register("Minute", typeof(int), typeof(DateTimePicker), new PropertyMetadata(1));
       

    public ObservableCollection<int> Months
    {
        get { return (ObservableCollection<int>)GetValue(MonthsProperty); }
        set { SetValue(MonthsProperty, value); }
    }
    public static readonly DependencyProperty MonthsProperty =
        DependencyProperty.Register("Months", typeof(ObservableCollection<int>), typeof(DateTimePicker), new PropertyMetadata(null));

    public ObservableCollection<int> Days
    {
        get { return (ObservableCollection<int>)GetValue(DaysProperty); }
        set { SetValue(DaysProperty, value); }
    }
    public static readonly DependencyProperty DaysProperty =
        DependencyProperty.Register("Days", typeof(ObservableCollection<int>), typeof(DateTimePicker), new PropertyMetadata(null));

    public ObservableCollection<int> Hours
    {
        get { return (ObservableCollection<int>)GetValue(HoursProperty); }
        set { SetValue(HoursProperty, value); }
    }
    public static readonly DependencyProperty HoursProperty =
        DependencyProperty.Register("Hours", typeof(ObservableCollection<int>), typeof(DateTimePicker), new PropertyMetadata(null));

    public ObservableCollection<int> Minutes
    {
        get { return (ObservableCollection<int>)GetValue(MinutesProperty); }
        set { SetValue(MinutesProperty, value); }
    }
    public static readonly DependencyProperty MinutesProperty =
        DependencyProperty.Register("Minutes", typeof(ObservableCollection<int>), typeof(DateTimePicker), new PropertyMetadata(null));

    public DateTimePicker()
    {
        InitializeComponent();
    }

}
