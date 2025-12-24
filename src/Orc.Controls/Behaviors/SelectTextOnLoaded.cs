namespace Orc.Controls;

using System;
using System.Timers;
using Catel.Services;
using Catel.Windows.Interactivity;
using System.Windows.Controls;

public partial class SelectTextOnLoaded : BehaviorBase<TextBox>
{
    private const double DelayBeforeTextSelected = 10d;

    private readonly IDispatcherService _dispatcherService;
#pragma warning disable IDISP006 // Implement IDisposable.
    private readonly Timer _textSelectTimer = new Timer(DelayBeforeTextSelected);
#pragma warning restore IDISP006 // Implement IDisposable.

    public SelectTextOnLoaded(IDispatcherService dispatcherService)
    {
        _dispatcherService = dispatcherService;
    }

    protected override void OnAssociatedObjectLoaded()
    {
        base.OnAssociatedObjectLoaded();

        _textSelectTimer.Elapsed += OnSearchTimerElapsed;
        _textSelectTimer.Start();
    }

    protected override void OnAssociatedObjectUnloaded()
    {
        _textSelectTimer.Stop();
        _textSelectTimer.Elapsed -= OnSearchTimerElapsed;

        base.OnAssociatedObjectUnloaded();
    }

    private void OnSearchTimerElapsed(object? sender, EventArgs e)
    {
        _textSelectTimer.Stop();

        _dispatcherService.Invoke(() => AssociatedObject?.SelectAll());
    }
}
