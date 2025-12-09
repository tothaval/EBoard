namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text;

public partial class SplashScreenViewModel : ObservableObject
{
    [ObservableProperty]
    private string logMessage;

    [ObservableProperty]
    private string titleMessage;

    private StringBuilder stringBuilder = new StringBuilder();

    public SplashScreenViewModel()
    {
        this.TitleMessage = "eboard startup screen";

        this.WriteLog($"eboard starts...");
    }

    public bool WriteLog(string logMessage)
    {
        if (string.IsNullOrWhiteSpace(logMessage))
        {
            return false;
        }

        var time = DateTime.Now;

        this.stringBuilder.AppendLine($"{time.Year}|{time.Month:#00}|{time.Day:00}:{time.Hour:#.##}:{time.Minute:##}:{time.Second:##}:::\t{logMessage}");

        this.LogMessage = this.stringBuilder.ToString();

        return true;
    }
}
