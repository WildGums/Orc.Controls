namespace Orc.Controls
{
    using System.Collections.Generic;
    using Catel;

    public class CalloutManager : ICalloutManager
    {
        public CalloutManager()
        {
            Callouts = new List<Callout>();
        }

        public IList<Callout> Callouts { get; }

        public void Register(Callout callout)
        {
            Argument.IsNotNull(() => callout);

            Callouts.Add(callout);
        }

        public void Unregister(Callout callout)
        {
            Callouts.Remove(callout);
        }

        public void RemoveAllCallouts()
        {
            Callouts.Clear();
        }
    }
}
