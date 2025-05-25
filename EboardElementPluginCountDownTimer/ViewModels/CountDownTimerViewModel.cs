namespace EboardElementPluginCountDownTimer.ViewModels;

using CommunityToolkit.Mvvm.Input;
using EboardElementPluginCountDownTimer.Interfaces;
using EBoardSDK.Plugins;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EboardElementPluginCountDownTimer.Views;
using EBoardSDK;

public partial class CountDownTimerViewModel : EBoardElementPluginBaseViewModel
{
    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    private string pluginHeader = "CountDownTimer Element";

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    private string pluginName = "CountDownTimer";

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override string ElementPluginName => "CountDownTimer";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EboardElementPluginCountDownTimer;component/ElementPluginResources.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(CountDownTimerView);

    public override Type ElementPluginViewModel => typeof(CountDownTimerViewModel);


    public string CountDown => ProcessCountDownRequest();

    public DateTime StartDate => GetStartDate();

    public DateTime StopDate => GetStopDate();

    private int _SelectedYear;
    public int SelectedYear
    {
        get { return _SelectedYear; }
        set
        {
            _SelectedYear = value;

            if (value <= 0)
            {
                _SelectedYear = 1;
            }
            else if (value > 9999)
            {
                _SelectedYear = 9999;
            }
            else
            {
                _SelectedYear = value;
            }

            OnPropertyChanged(nameof(CountDown));
            OnPropertyChanged(nameof(SelectedYear));
            OnPropertyChanged(nameof(StartDate));

            FillDays();
        }
    }


    private int _SelectedMonth;
    public int SelectedMonth
    {
        get { return _SelectedMonth; }
        set
        {
            _SelectedMonth = value;

            OnPropertyChanged(nameof(CountDown));
            OnPropertyChanged(nameof(SelectedMonth));
            OnPropertyChanged(nameof(StartDate));

            FillDays();
        }
    }


    private int _SelectedDay;
    public int SelectedDay
    {
        get { return _SelectedDay; }
        set
        {
            _SelectedDay = value;

            OnPropertyChanged(nameof(CountDown));
            OnPropertyChanged(nameof(SelectedDay));
            OnPropertyChanged(nameof(StartDate));
        }
    }


    private int _SelectedHour;
    public int SelectedHour
    {
        get { return _SelectedHour; }
        set
        {
            _SelectedHour = value;

            OnPropertyChanged(nameof(CountDown));
            OnPropertyChanged(nameof(SelectedHour));
            OnPropertyChanged(nameof(StartDate));
        }
    }


    private int _SelectedMinute;
    public int SelectedMinute
    {
        get { return _SelectedMinute; }
        set
        {
            _SelectedMinute = value;

            OnPropertyChanged(nameof(CountDown));
            OnPropertyChanged(nameof(SelectedMinute));
            OnPropertyChanged(nameof(StartDate));
        }
    }


    private int _SelectedYear_Stop;
    public int SelectedYear_Stop
    {
        get { return _SelectedYear_Stop; }
        set
        {

            if (value <= 0)
            {
                _SelectedYear_Stop = 1;
            }
            else if (value > 9999)
            {
                _SelectedYear_Stop = 9999;
            }
            else
            {
                _SelectedYear_Stop = value;
            }


            OnPropertyChanged(nameof(CountDown));
            OnPropertyChanged(nameof(SelectedYear_Stop));
            OnPropertyChanged(nameof(StopDate));

            FillDays_Stop();
        }
    }


    private int _SelectedMonth_Stop;
    public int SelectedMonth_Stop
    {
        get { return _SelectedMonth_Stop; }
        set
        {
            _SelectedMonth_Stop = value;

            OnPropertyChanged(nameof(CountDown));
            OnPropertyChanged(nameof(SelectedMonth_Stop));
            OnPropertyChanged(nameof(StopDate));

            FillDays_Stop();
        }
    }


    private int _SelectedDay_Stop;
    public int SelectedDay_Stop
    {
        get { return _SelectedDay_Stop; }
        set
        {
            _SelectedDay_Stop = value;

            OnPropertyChanged(nameof(CountDown));
            OnPropertyChanged(nameof(SelectedDay_Stop));
            OnPropertyChanged(nameof(StopDate));
        }
    }


    private int _SelectedHour_Stop;
    public int SelectedHour_Stop
    {
        get { return _SelectedHour_Stop; }
        set
        {
            _SelectedHour_Stop = value;

            OnPropertyChanged(nameof(CountDown));
            OnPropertyChanged(nameof(SelectedHour_Stop));
            OnPropertyChanged(nameof(StopDate));
        }
    }


    private int _SelectedMinute_Stop;
    public int SelectedMinute_Stop
    {
        get { return _SelectedMinute_Stop; }
        set
        {
            _SelectedMinute_Stop = value;

            OnPropertyChanged(nameof(CountDown));
            OnPropertyChanged(nameof(SelectedMinute_Stop));
            OnPropertyChanged(nameof(StopDate));
        }
    }


    public ICommand StartCommand { get; set; }
    public ICommand StopCommand { get; set; }


    public DispatcherTimer Timer { get; } = new DispatcherTimer();


    public ObservableCollection<int> Days { get; set; }
    public ObservableCollection<int> Days_Stop { get; set; }
    public ObservableCollection<int> Hours { get; set; }
    public ObservableCollection<int> Minutes { get; set; }
    public ObservableCollection<int> Months { get; set; }



    public CountDownTimerViewModel()
    {
        Timer.Interval = TimeSpan.FromMilliseconds(16.18);
        Timer.Tick += Timer_Tick;

        FillMonths();
        FillHours();
        FillMinutes();

        SelectedYear = DateTime.Now.Year;
        SelectedMonth = DateTime.Now.Month;
        SelectedDay = DateTime.Now.Day;
        SelectedHour = DateTime.Now.Hour;
        SelectedMinute = Minutes.First();

        SelectedYear_Stop = DateTime.Now.Year;
        SelectedMonth_Stop = DateTime.Now.Month;
        SelectedDay_Stop = DateTime.Now.Day + 1;
        SelectedHour_Stop = DateTime.Now.Hour;
        SelectedMinute_Stop = Minutes.First();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(CountDown));
    }

    private void FillDays()
    {
        Days = new ObservableCollection<int>();

        if (SelectedMonth > 0 && SelectedMonth < 12)
        {
            for (int i = 1; i < DateTime.DaysInMonth(SelectedYear, SelectedMonth) + 1; i++)
            {
                Days.Add(i);
            }
        }

        OnPropertyChanged(nameof(Days));
    }

    private void FillDays_Stop()
    {
        Days_Stop = new ObservableCollection<int>();

        if (SelectedMonth_Stop > 0 && SelectedMonth_Stop < 12)
        {
            for (int i = 1; i < DateTime.DaysInMonth(SelectedYear_Stop, SelectedMonth_Stop) + 1; i++)
            {
                Days_Stop.Add(i);
            }
        }

        OnPropertyChanged(nameof(Days_Stop));
    }


    private void FillHours()
    {
        Hours = new ObservableCollection<int>();

        for (int i = 0; i < 24; i++)
        {
            Hours.Add(i);
        }

        OnPropertyChanged(nameof(Hours));
    }


    private void FillMinutes()
    {
        Minutes = new ObservableCollection<int>();

        for (int i = 0; i < 60; i++)
        {
            if (i % 5 == 0)
            {
                Minutes.Add(i);
            }
        }

        OnPropertyChanged(nameof(Minutes));
    }


    private void FillMonths()
    {
        Months = new ObservableCollection<int>();

        for (int i = 1; i < 13; i++)
        {
            Months.Add(i);
        }

        OnPropertyChanged(nameof(Months));
    }


    private DateTime GetStartDate()
    {
        DateTime dateTime = new DateTime(
            SelectedYear,
            SelectedMonth,
            SelectedDay,
            SelectedHour,
            SelectedMinute,
            0
            );

        return dateTime;
    }


    private DateTime GetStopDate()
    {
        DateTime dateTime = new DateTime(
            SelectedYear_Stop,
            SelectedMonth_Stop,
            SelectedDay_Stop,
            SelectedHour_Stop,
            SelectedMinute_Stop,
            0
            );

        return dateTime;
    }

    private string ProcessCountDownRequest()
    {
        double difference = (StopDate - DateTime.Now).TotalMicroseconds;
        double diff_seconds = difference / (1000 * 1000);
        double diff_minutes = diff_seconds / 60;
        double diff_hours = diff_minutes / 60;
        double diff_days = diff_hours / 24;

        double percentageLeft = difference / (StopDate - StartDate).TotalMicroseconds * 100;


        return $"{diff_days.ToString("N0")} d\n" +
            $"{diff_hours.ToString("N0")} h\n" +
            $"{diff_minutes.ToString("N0")} '\n" +
            $"{diff_seconds.ToString("N0")} s\n" +
            $"{difference.ToString("N0")} µs\n{percentageLeft.ToString("N7")} %";
    }

    [RelayCommand]
    private void StartTimer()
    {
        Timer.Start();
    }

    [RelayCommand]
    private void StopTimer()
    {
        Timer.Stop();
    }

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }
}
