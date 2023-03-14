namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using Catel;
using Catel.Logging;

public class CalloutManager : ICalloutManager
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly List<ICallout> _callouts;
    private int _suspendCount;

    public CalloutManager()
    {
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

        Log.Debug($"Suspended callouts, count = '{_suspendCount}'");
    }

    public void Resume()
    {
        _suspendCount = Math.Max(0, _suspendCount - 1);

        Log.Debug($"Resumed callouts, count = '{_suspendCount}'");
    }

    public void Register(ICallout callout)
    {
        ArgumentNullException.ThrowIfNull(callout);

        Log.Debug($"Registering callout '{callout}'");

        _callouts.Add(callout);

        SubscribeToCallout(callout);

        Registered?.Invoke(this, new CalloutEventArgs(callout));
    }

    public void Unregister(ICallout callout)
    {
        ArgumentNullException.ThrowIfNull(callout);

        Log.Debug($"Unregistering callout '{callout}'");

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
