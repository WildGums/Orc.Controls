namespace Orc.Controls
{
    using System;
    using Catel;

    public class CalloutEventArgs : EventArgs
    {
        public CalloutEventArgs(ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(callout);

            Callout = callout;
        }

        public ICallout Callout { get; }
    }
}
