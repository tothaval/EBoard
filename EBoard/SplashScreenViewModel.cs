namespace EBoard;

using CommunityToolkit.Mvvm.ComponentModel;
using System;

public partial class SplashScreenViewModel : ObservableObject
{
    public string LogMessage { get; set; }

    public SplashScreenViewModel()
    {
        this.LogMessage = $"{DateTime.Now} eboard starts...";
    }
}
