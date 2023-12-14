namespace Orc.Controls;

using System;
using System.Windows.Threading;

/// <summary>
/// A timer used to open and close tooltips.
/// </summary>
internal sealed class ToolTipTimer : DispatcherTimer
{
    /// <summary>
    /// The timer interval.
    /// </summary>
    private const int TimerInterval = 100;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToolTipTimer" /> class.
    /// </summary>
    /// <param name="maximumTicks">The maximum ticks.</param>
    /// <param name="initialDelay">The initial delay.</param>
    public ToolTipTimer(TimeSpan maximumTicks, TimeSpan initialDelay)
    {
        InitialDelay = initialDelay;
        MaximumTicks = maximumTicks;
        Interval = TimeSpan.FromMilliseconds(TimerInterval);
        Tick += OnTick;
    }
    
    /// <summary>
    /// This event occurs when the timer has stopped.
    /// </summary>
    public event EventHandler<EventArgs>? Stopped;

    private void OnTick(object? _, EventArgs e)
    {
        CurrentTick += TimerInterval;

        if (CurrentTick >= (MaximumTicks.TotalMilliseconds + InitialDelay.TotalMilliseconds))
        {
            Stop();
        }
    }

    /// <summary>
    /// Gets the current tick of the ToolTipTimer.
    /// </summary>
    public int CurrentTick { get; private set; }

    /// <summary>
    /// Gets the initial delay for this timer in seconds.
    /// When the maximum number of ticks is hit, the timer will stop itself.
    /// </summary>
    /// <remarks>The default delay is 0 seconds.</remarks>
    public TimeSpan InitialDelay { get; set; }

    /// <summary>
    /// Gets the maximum number of seconds for this timer.
    /// When the maximum number of ticks is hit, the timer will stop itself.
    /// </summary>
    /// <remarks>The minimum number of seconds is 1.</remarks>
    public TimeSpan MaximumTicks { get; set; }

    /// <summary>
    /// Resets the ToolTipTimer and starts it.
    /// </summary>
    public void StartAndReset()
    {
        CurrentTick = 0;
        Start();
    }

    /// <summary>
    /// Stops the ToolTipTimer.
    /// </summary>
    public new void Stop()
    {
        base.Stop();

        Stopped?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Stops the ToolTipTimer and resets its tick count.
    /// </summary>
    public void StopAndReset()
    {
        Stop();
        CurrentTick = 0;
    }
}
