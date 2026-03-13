// <copyright file="CountDownTimerViewModel.cs" company=".">
// Stephan Kammel
// </copyright>
/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may 
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial 
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
namespace EboardElementPluginCountDownTimer.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EboardElementPluginCountDownTimer.Views;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
<<<<<<< Updated upstream
=======
using EBoardSDK.Utilities.Factories;
>>>>>>> Stashed changes
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

public partial class CountDownTimerViewModel : EBoardElementPluginBaseViewModel
{
    private int _SelectedYear;
    private int _SelectedYear_Stop;
    private int _SelectedMonth;
    private int _SelectedMonth_Stop;
    private int _SelectedDay;
    private int _SelectedDay_Stop;
    private int _SelectedHour;
    private int _SelectedHour_Stop;
    private int _SelectedMinute;
    private int _SelectedMinute_Stop;

    [ObservableProperty]
    private string countdown;

    private string pluginHeader = "CountDownTimer Element";
    private string pluginName = "CountDownTimer";

    public CountDownTimerViewModel()
    {
        this.ElementScreenIntegrationConstraints = new EBoardSDK.Models.ElementScreenIntegrationConstraints(ElementInstantiationPolicy.OnePerScreen);

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

        Timer.Interval = TimeSpan.FromMilliseconds(16.18);
        Timer.Tick += Timer_Tick;
    }

    public override bool NoDefaultBorders { get; } = false;

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }


    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EboardElementPluginCountDownTimer;component/ElementPluginResources.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(CountDownTimerView);

    public override Type ElementPluginViewModel => typeof(CountDownTimerViewModel);

    public DateTime StartDate => GetStartDate();

    public DateTime StopDate => GetStopDate();

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

            OnPropertyChanged(nameof(Countdown));
            OnPropertyChanged(nameof(SelectedYear));
            OnPropertyChanged(nameof(StartDate));

            FillDays();
        }
    }

    public int SelectedMonth
    {
        get { return _SelectedMonth; }
        set
        {
            _SelectedMonth = value;

            OnPropertyChanged(nameof(Countdown));
            OnPropertyChanged(nameof(SelectedMonth));
            OnPropertyChanged(nameof(StartDate));

            FillDays();
        }
    }

    public int SelectedDay
    {
        get { return _SelectedDay; }
        set
        {
            _SelectedDay = value;

            OnPropertyChanged(nameof(Countdown));
            OnPropertyChanged(nameof(SelectedDay));
            OnPropertyChanged(nameof(StartDate));
        }
    }

    public int SelectedHour
    {
        get { return _SelectedHour; }
        set
        {
            _SelectedHour = value;

            OnPropertyChanged(nameof(Countdown));
            OnPropertyChanged(nameof(SelectedHour));
            OnPropertyChanged(nameof(StartDate));
        }
    }

    public int SelectedMinute
    {
        get { return _SelectedMinute; }
        set
        {
            _SelectedMinute = value;

            OnPropertyChanged(nameof(Countdown));
            OnPropertyChanged(nameof(SelectedMinute));
            OnPropertyChanged(nameof(StartDate));
        }
    }

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

            OnPropertyChanged(nameof(Countdown));
            OnPropertyChanged(nameof(SelectedYear_Stop));
            OnPropertyChanged(nameof(StopDate));

            FillDays_Stop();
        }
    }

    public int SelectedMonth_Stop
    {
        get { return _SelectedMonth_Stop; }
        set
        {
            _SelectedMonth_Stop = value;

            OnPropertyChanged(nameof(Countdown));
            OnPropertyChanged(nameof(SelectedMonth_Stop));
            OnPropertyChanged(nameof(StopDate));

            FillDays_Stop();
        }
    }

    public int SelectedDay_Stop
    {
        get { return _SelectedDay_Stop; }
        set
        {
            _SelectedDay_Stop = value;

            OnPropertyChanged(nameof(Countdown));
            OnPropertyChanged(nameof(SelectedDay_Stop));
            OnPropertyChanged(nameof(StopDate));
        }
    }

    public int SelectedHour_Stop
    {
        get { return _SelectedHour_Stop; }
        set
        {
            _SelectedHour_Stop = value;

            OnPropertyChanged(nameof(Countdown));
            OnPropertyChanged(nameof(SelectedHour_Stop));
            OnPropertyChanged(nameof(StopDate));
        }
    }

    public int SelectedMinute_Stop
    {
        get { return _SelectedMinute_Stop; }
        set
        {
            _SelectedMinute_Stop = value;

            OnPropertyChanged(nameof(Countdown));
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

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        this.Countdown = this.ProcessCountDownRequest();
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
}

// EOF