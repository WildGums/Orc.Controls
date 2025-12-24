namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public class CalloutManager : ICalloutManager
{
    private readonly ILogger<CalloutManager> _logger;

    private readonly List<ICallout> _callouts;

    private int _suspendCount;

    public CalloutManager(ILogger<CalloutManager> logger)
    {
        _logger = logger;

        _callouts = new List<ICallout>();
    }

    public bool IsSuspended
    {
        get { return _suspendCount > 0; }
    }

    public List<ICallout> Callouts
    {
        get
        {
            var calloutsCopy = _callouts.ToList();
            return calloutsCopy;
        }
    }

    public event EventHandler<CalloutEventArgs>? Registered;
    public event EventHandler<CalloutEventArgs>? Unregistered;

    public event EventHandler<CalloutEventArgs>? Showing;
    public event EventHandler<CalloutEventArgs>? Hiding;

    public void Suspend()
    {
        _suspendCount++;

        _logger.LogDebug($"Suspended callouts, count = '{_suspendCount}'");
    }

    public void Resume()
    {
        _suspendCount = Math.Max(0, _suspendCount - 1);

        _logger.LogDebug($"Resumed callouts, count = '{_suspendCount}'");
    }

    public void Register(ICallout callout)
    {
        ArgumentNullException.ThrowIfNull(callout);

        _logger.LogDebug($"Registering callout '{callout}'");

        _callouts.Add(callout);

        SubscribeToCallout(callout);

        Registered?.Invoke(this, new CalloutEventArgs(callout));
    }

    public void Unregister(ICallout callout)
    {
        ArgumentNullException.ThrowIfNull(callout);

        _logger.LogDebug($"Unregistering callout '{callout}'");

        UnsubscribeFromCallout(callout);

        // Make sure to hide
        callout.Hide();

        _callouts.Remove(callout);

        Unregistered?.Invoke(this, new CalloutEventArgs(callout));
    }

    public void Clear()
    {
        _callouts.ForEach(x => UnsubscribeFromCallout(x));
        _callouts.Clear();
    }

    private void SubscribeToCallout(ICallout callout)
    {
        callout.Showing += OnCalloutShowing;
        callout.Hiding += OnCalloutHiding;
    }

    private void UnsubscribeFromCallout(ICallout callout)
    {
        callout.Showing -= OnCalloutShowing;
        callout.Hiding -= OnCalloutHiding;
    }

    private void OnCalloutShowing(object? sender, CalloutEventArgs e)
    {
        Showing?.Invoke(this, e);
    }

    private void OnCalloutHiding(object? sender, CalloutEventArgs e)
    {
        Hiding?.Invoke(this, e);
    }
}
