namespace EboardElementPluginCountDownTimer.Interfaces;

using System.Windows.Threading;

public interface ICountDownTimer
{
    public DispatcherTimer Timer { get; }

    public void StartTimer();

    public void StopTimer();
}
