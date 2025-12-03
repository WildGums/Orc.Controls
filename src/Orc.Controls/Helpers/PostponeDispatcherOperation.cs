namespace Orc.Controls;

using System;
using System.Timers;
using Catel.IoC;
using Catel.Services;
using Helpers;

[ObsoleteEx(Message = "Use PostponeDispatcherTimerAction instead", 
    TreatAsErrorFromVersion = "5.4.0", RemoveInVersion = "6.0.0")]
public sealed class PostponeDispatcherOperation
{
    private readonly Action _action;
    private readonly IDispatcherService _dispatcherService;

#pragma warning disable IDISP006 // Implement IDisposable
    private Timer? _timer;
#pragma warning restore IDISP006 // Implement IDisposable

    public PostponeDispatcherOperation(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        _action = action;

#pragma warning disable IDISP004 // Don't ignore created IDisposable
        _dispatcherService = this.GetServiceLocator().ResolveRequiredType<IDispatcherService>();
#pragma warning restore IDISP004 // Don't ignore created IDisposable
    }

    public void Stop()
    {
        var timer = _timer;
        if (timer is not null)
        {
            timer.Stop();
            timer.Elapsed -= OnCompareByColumnTimerElapsed;
        }
    }

    public void Execute(int delay)
    {
        if (delay <= 0)
        {
            _action();
            return;
        }

        var timer = _timer;
        if (timer is null)
        {
#pragma warning disable IDISP001 // Dispose created
            timer = new Timer();
#pragma warning restore IDISP001 // Dispose created

            timer.Elapsed += OnCompareByColumnTimerElapsed;

#pragma warning disable IDISP003 // Dispose previous before re-assigning
            _timer = timer;
#pragma warning restore IDISP003 // Dispose previous before re-assigning
        }

        timer.Interval = delay;
        timer.Start();
    }

    private void OnCompareByColumnTimerElapsed(object? _, ElapsedEventArgs e)
    {
        Stop();

        _dispatcherService.Invoke(_action);

        _timer?.Dispose();
        _timer = null;
    }
}
