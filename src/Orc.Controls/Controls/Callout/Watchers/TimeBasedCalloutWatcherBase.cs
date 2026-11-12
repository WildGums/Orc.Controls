namespace Orc.Controls;

using System;
using System.Threading.Tasks;
using Catel.Configuration;
using Catel.Services;
using Catel.Windows.Threading;
using Microsoft.Extensions.Logging;

public abstract class TimeBasedCalloutWatcherBase : CalloutWatcherBase
{
    private readonly ILogger _logger;
    private readonly IDispatcherService _dispatcherService;

    private readonly DispatcherTimerEx _dispatcherTimer;

    public TimeBasedCalloutWatcherBase(ILogger logger,
        ICalloutManager calloutManager, IConfigurationService configurationService,
        IDispatcherService dispatcherService)
        : base(calloutManager, configurationService)
    {
        _logger = logger;
        _dispatcherService = dispatcherService;

        _dispatcherTimer = new DispatcherTimerEx(dispatcherService);
        _dispatcherTimer.Tick += OnDispatcherTimerTick;

        Start = DateTimeOffset.MaxValue;

        Subscribe(_calloutManager);
    }

    public DateTimeOffset Start { get; protected set; }

    public DateTimeOffset End
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
            Start = DateTimeOffset.Now;
            return;
        }

        _logger.LogDebug("Callout is not yet registered, subscribing to ICalloutManager.Registered event");

        calloutManager.Registered += OnCalloutManagerRegistered;
    }

    private async void OnCalloutManagerRegistered(object? sender, CalloutEventArgs e)
    {
        if (Id.HasValue &&
            e.Callout.Id == Id.Value)
        {
            Start = DateTimeOffset.Now;
        }
        else if (!string.IsNullOrEmpty(Name) &&
                 e.Callout.Name == Name)
        {
            Start = DateTimeOffset.Now;
        }

        if (Start != DateTimeOffset.MaxValue)
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
        if (end < DateTimeOffset.Now)
        {
            await ShowAsync();
            return;
        }

        var interval = end - DateTimeOffset.Now;

        _dispatcherTimer.Interval = interval;
        _dispatcherTimer.Start();
    }

    private async void OnDispatcherTimerTick(object? sender, EventArgs e)
    {
        _dispatcherTimer.Stop();

        await ShowAsync();
    }
}
