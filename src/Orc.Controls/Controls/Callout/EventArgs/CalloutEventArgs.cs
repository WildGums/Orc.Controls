namespace Orc.Controls
{
    using System;
    using Catel;

    public class CalloutEventArgs : EventArgs
    {
        public CalloutEventArgs(ICallout callout)
        {
            Argument.IsNotNull(() => callout);

            Callout = callout;
        }

        public ICallout Callout { get; }
    }
}
