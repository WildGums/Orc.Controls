namespace Orc.Controls.Settings;

using System;
using System.Timers;
using Catel.IoC;
using Catel.Services;

internal sealed class PostponeDispatcherOperation : IDisposable
{
    private readonly Action _action;
    private readonly IDispatcherService _dispatcherService;
    private readonly Timer _timer;

    public PostponeDispatcherOperation(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        _action = action;

        _dispatcherService = this.GetServiceLocator().ResolveRequiredType<IDispatcherService>();

        _timer = new();
        _timer.Elapsed += OnCompareByColumnTimerElapsed;
    }

    public void Dispose()
    {
        _timer.Dispose();
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

        _timer.Interval = delay;
        _timer.Start();
    }

    private void OnCompareByColumnTimerElapsed(object? _, ElapsedEventArgs e)
    {
        _timer.Stop();

        _dispatcherService.Invoke(_action);
    }
}
