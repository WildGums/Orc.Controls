namespace Orc.Controls;

using System;
using System.Collections.Generic;

public interface ICalloutManager
{
    List<ICallout> Callouts { get; }

    bool IsSuspended { get; }

    void Suspend();
    void Resume();

    void Register(ICallout callout);
    void Unregister(ICallout callout);

    void Clear();

    event EventHandler<CalloutEventArgs>? Registered;
    event EventHandler<CalloutEventArgs>? Unregistered;

    event EventHandler<CalloutEventArgs>? Showing;
    event EventHandler<CalloutEventArgs>? Hiding;
}
