namespace Orc.Controls
{
    using System.Collections.Generic;

    public interface ICalloutManager
    {
        IList<Callout> Callouts { get; }
        void Register(Callout callout);
        void Unregister(Callout callout);
    }
}
