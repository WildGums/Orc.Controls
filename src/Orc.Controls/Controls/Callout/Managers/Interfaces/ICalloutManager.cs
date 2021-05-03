namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using Catel.MVVM;

    public interface ICalloutManager
    {
        List<ICallout> Callouts { get; }

        event EventHandler<CalloutEventArgs> Registered;
        event EventHandler<CalloutEventArgs> Unregistered;

        event EventHandler<CalloutEventArgs> Showing;
        event EventHandler<CalloutEventArgs> Hiding;

        void Register(ICallout callout);
        void Unregister(ICallout callout);

        void Clear();
    }
}
