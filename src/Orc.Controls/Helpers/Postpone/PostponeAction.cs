namespace Orc.Controls;

using System;

public sealed class PostponeAction
{
    private readonly Action _action;
    private readonly IPostponeActionTimer _timer;

    private bool _disposed;

    public PostponeAction(Action action, IPostponeActionTimer timer)
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(timer);

        _action = action;
        _timer = timer;
        _timer.Tick += OnTimerTick;
    }

    public PostponeAction(Action action)
        : this(action, new DispatcherTimerWrapper())
    {
    }

    public static void Execute(Action action, int delay)
    {
        ArgumentNullException.ThrowIfNull(action);

        new PostponeAction(action)
            .Execute(delay);
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void Execute(int delay)
    {
        if (delay <= 0)
        {
            _action();
            return;
        }

        _timer.Interval = TimeSpan.FromMilliseconds(delay);
        _timer.Start();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        DisposeTimer();

        _action.Invoke();
    }

    private void DisposeTimer()
    {
        if (_disposed)
        {
            return;
        }

        _timer.Stop();
        _timer.Tick -= OnTimerTick;

        if (_timer is IDisposable disposableTimer)
        {
            disposableTimer.Dispose();
        }

        _disposed = true;
    }
}
