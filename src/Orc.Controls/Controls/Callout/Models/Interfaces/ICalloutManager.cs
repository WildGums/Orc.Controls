namespace Orc.Controls
{
    using System.Collections.Generic;

    public interface ICalloutManager
    {
        IList<CalloutViewModel> Callouts { get; }
        void Register(CalloutViewModel calloutViewModel, Callout callout);
        void UnRegister(CalloutViewModel callout);
    }
}
