namespace Orc.Controls
{
    using System.Collections.Generic;

    public interface ICalloutManager
    {
        IList<Callout> Callouts;
        void Register(Callout callout);
        void Unregister(Callout callout);
    }
}
