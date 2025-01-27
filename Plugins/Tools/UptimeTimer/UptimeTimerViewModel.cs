using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Plugins.Elements.StandardText;
using EBoard.Utilities.Factories;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System.Data;
using System.Windows.Media.Animation;

namespace EBoard.Plugins.Tools.Uptime;

public partial class UptimeViewModel : PluginViewModel
{
    [ObservableProperty]
    private string clock;

    [ObservableProperty]
    private string uptime;


    private DispatcherTimer _timer;


    public UptimeViewModel()
    {
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
        Uptime = $"{uptime.Hours:D2}:{uptime.Minutes:D2}:{uptime.Seconds:D2}";
    }


    private void _timer_Tick(object? sender, EventArgs e)
    {
        UpdateOutput();
    }


}// EOF