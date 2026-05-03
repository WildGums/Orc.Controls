namespace Orc.Controls;

using System;
using System.Timers;
using Catel.Services;
using Catel.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Threading;

public partial class SelectTextOnLoaded : BehaviorBase<TextBox>
{
    private const double DelayBeforeTextSelected = 10d;

#pragma warning disable IDISP006 // Implement IDisposable.
    private readonly DispatcherTimer _textSelectTimer = new()
    {
        Interval = TimeSpan.FromMilliseconds(DelayBeforeTextSelected)
    };
#pragma warning restore IDISP006 // Implement IDisposable.

    protected override void OnAssociatedObjectLoaded()
    {
        base.OnAssociatedObjectLoaded();

        _textSelectTimer.Tick += OnSearchTimerElapsed;
        _textSelectTimer.Start();
    }

    protected override void OnAssociatedObjectUnloaded()
    {
        _textSelectTimer.Stop();
        _textSelectTimer.Tick -= OnSearchTimerElapsed;

        base.OnAssociatedObjectUnloaded();
    }

    private void OnSearchTimerElapsed(object? sender, EventArgs e)
    {
        _textSelectTimer.Stop();

        AssociatedObject?.SelectAll();
    }
}
