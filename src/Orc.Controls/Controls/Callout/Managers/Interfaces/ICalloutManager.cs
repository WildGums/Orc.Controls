namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using Catel.MVVM;

    public interface ICalloutManager
    {
        List<ICallout> Callouts { get; }

        bool IsSuspended { get; }

        event EventHandler<CalloutEventArgs> Registered;
        event EventHandler<CalloutEventArgs> Unregistered;

        event EventHandler<CalloutEventArgs> Showing;
        event EventHandler<CalloutEventArgs> Hiding;

        void Suspend();
        void Resume();

        void Register(ICallout callout);
        void Unregister(ICallout callout);

        void Clear();
    }
}
