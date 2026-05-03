namespace Orc.Controls;

using System;
using System.Windows.Threading;

public sealed class DispatcherTimerWrapper : IPostponeActionTimer
{
    private readonly DispatcherTimer _timer;

    public DispatcherTimerWrapper()
    {
        _timer = new();
        _timer.Tick += (sender, e) => Tick?.Invoke(sender, e);
    }

    public TimeSpan Interval
    {
        get => _timer.Interval;
        set => _timer.Interval = value;
    }

    public bool IsEnabled => _timer.IsEnabled;

    public event EventHandler? Tick;

    public void Start() => _timer.Start();
    public void Stop() => _timer.Stop();
}