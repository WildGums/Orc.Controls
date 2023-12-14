namespace Orc.Controls;

using System;
using System.Windows.Threading;

internal static class PendingMethod
{
    public static void InvokeDispatcher(Action action, int delay)
    {
        if (delay is 0)
        {
            action.Invoke();

            return;
        }

        var timer = new DispatcherTimer
        {
            Interval = new TimeSpan(0, 0, 0, 0, delay), 
            Tag = action
        };

        timer.Tick += OnTimerTick;

        timer.Stop();
        timer.Start();
    }

    private static void OnTimerTick(object? sender, EventArgs e)
    {
        if (sender is not DispatcherTimer timer)
        {
            return;
        }

        timer.Stop();
        timer.Tick -= OnTimerTick;

        var action = timer.Tag as Action;
        action?.Invoke();
    }
}
