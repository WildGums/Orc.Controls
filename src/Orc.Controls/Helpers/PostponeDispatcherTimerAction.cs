namespace Orc.Controls.Helpers;

using System;
using System.Windows.Threading;

public sealed class PostponeDispatcherTimerAction
{
    private readonly Action _action;
    private readonly DispatcherTimer _timer;

    public PostponeDispatcherTimerAction(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        _action = action;

        _timer = new();
        _timer.Tick += OnTimerTick;
    }

    public static void Execute(Action action, int delay)
    {
        ArgumentNullException.ThrowIfNull(action);

        new PostponeDispatcherTimerAction(action)
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
        _timer.Stop();

        _action.Invoke();
    }
}
