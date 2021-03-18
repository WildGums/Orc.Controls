namespace Orc.Controls.Controls.Callout.Models
{
    using System;
    using System.Collections.Generic;
    using Orc.Controls.Controls.Callout.ViewModels;

    public class CalloutManager : ICalloutManager
    {

        public CalloutManager()
        {
            if(Callouts == null)
                Callouts = new List<CalloutViewModel>();
        }

        public IList<CalloutViewModel> Callouts { get; set; }

        public void Register(CalloutViewModel callout)
        {
            Callouts.Add(callout);
        }

        public void UnRegister(CalloutViewModel callout)
        {
            Callouts.Remove(callout);
        }
    }
}
