namespace Orc.Controls
{
    using System.Collections.Generic;
    using Catel;

    public class CalloutManager : ICalloutManager
    {
        public CalloutManager()
        {
        }

        public IList<CalloutViewModel> Callouts { get; set; } = new List<CalloutViewModel>();

        public void Register(CalloutViewModel calloutViewModel, Callout callout)
        {
            Argument.IsNotNull(() => calloutViewModel);
            Argument.IsNotNull(() => callout);
            Callouts.Add(calloutViewModel);
            callout.DataContext = calloutViewModel;
            
        }

        public void Unregister(CalloutViewModel callout)
        {
            Callouts.Remove(callout);
        }

        public void RemoveAllCallouts()
        {
            Callouts.Clear();
        }
    }
}
