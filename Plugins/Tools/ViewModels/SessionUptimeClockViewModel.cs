using CommunityToolkit.Mvvm.ComponentModel;
using EBoard.Interfaces;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EBoard.Plugins.Tools.ViewModels;

public partial class SessionUptimeClockViewModel : ObservableObject
{

    public IElementContent Content { get; }


    private DispatcherTimer _timer;

    public DateTime CurrentDate => DateTime.Now.Date;


    [ObservableProperty]
    private string _CurrentTime;


    [ObservableProperty]
    private string _SessionUptime;


    public SessionUptimeClockViewModel()
    {
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(200);
        _timer.Tick += _timer_Tick;

        _timer.Start();
    }

    private void _timer_Tick(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(CurrentDate));

        long tickCountMs = Environment.TickCount64;

        var uptime = TimeSpan.FromMilliseconds(tickCountMs);
        DateTime currentTime = DateTime.Now;

        CurrentTime = $"{currentTime.Hour:D2}:{currentTime.Minute:D2}:{currentTime.Second:D2}";
        SessionUptime = $"{uptime.Hours:D2}:{uptime.Minutes:D2}:{uptime.Seconds:D2}";
    }



}
