namespace Orc.Controls
{
    using System.Collections.Generic;

    public interface ICalloutManager
    {
        void Register(Callout callout);
        void Unregister(Callout callout);
    }
}
