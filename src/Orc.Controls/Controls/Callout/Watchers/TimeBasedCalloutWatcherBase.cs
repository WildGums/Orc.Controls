namespace Orc.Controls;

using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Catel.Configuration;
using Catel.Logging;

public abstract class TimeBasedCalloutWatcherBase : CalloutWatcherBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly DispatcherTimer _dispatcherTimer = new();

    public TimeBasedCalloutWatcherBase(ICalloutManager calloutManager, IConfigurationService configurationService)
        : base(calloutManager, configurationService)
    {
        Start = DateTime.MaxValue;
        _dispatcherTimer.Tick += OnDispatcherTimerTick;

        Subscribe(_calloutManager);
    }

    public DateTime Start { get; protected set; }

    public DateTime End
    {
        get { return Start + Delay; }
    }

    public abstract TimeSpan Delay { get; }

    protected virtual void Subscribe(ICalloutManager calloutManager)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        var callout = Callout;
        if (callout is not null)
        {
            Start = DateTime.Now;
            return;
        }

        Log.Debug($"Callout is not yet registered, subscribing to ICalloutManager.Registered event");

        calloutManager.Registered += OnCalloutManagerRegistered;
    }

    private async void OnCalloutManagerRegistered(object? sender, CalloutEventArgs e)
    {
        if (Id.HasValue &&
            e.Callout.Id == Id.Value)
        {
            Start = DateTime.Now;
        }
        else if (!string.IsNullOrEmpty(Name) &&
                 e.Callout.Name == Name)
        {
            Start = DateTime.Now;
        }

        if (Start != DateTime.MaxValue)
        {
            await ScheduleShowAsync();
        }
    }

    private async Task ScheduleShowAsync()
    {
        if (HasShown)
        {
            return;
        }

        var end = End;
        if (end < DateTime.Now)
        {
            await ShowAsync();
            return;
        }

        var interval = end - DateTime.Now;

        _dispatcherTimer.Interval = interval;
        _dispatcherTimer.Start();
    }

    private async void OnDispatcherTimerTick(object? sender, EventArgs e)
    {
        _dispatcherTimer.Stop();

        await ShowAsync();
    }
}
