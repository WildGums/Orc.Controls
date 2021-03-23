namespace Orc.Controls.Controls.Callout.Models
{
    using System;
    using System.Collections.Generic;
    using Orc.Controls.Controls.Callout.ViewModels;
    using Orc.Controls.Controls.Callout.Views;

    public interface ICalloutManager
    {
        IList<CalloutViewModel> Callouts { get; set; }
        void Register(CalloutViewModel calloutViewModel, Callout callout);
        void UnRegister(CalloutViewModel callout);
    }
}
