namespace Orc.Controls.Controls.Callout.Models
{
    using System;
    using System.Collections.Generic;
    using Orc.Controls.Controls.Callout.ViewModels;

    public interface ICalloutManager
    {
        IList<CalloutViewModel> Callouts { get; set; }
        void Register(CalloutViewModel callout);
        void UnRegister(CalloutViewModel callout);
    }
}
