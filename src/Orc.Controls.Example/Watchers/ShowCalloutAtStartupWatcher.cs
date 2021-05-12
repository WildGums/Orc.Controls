namespace Orc.Controls.Example.Watchers
{
    using System;
    using Catel.Configuration;

    public class ShowCalloutAtStartupWatcher : TimeBasedCalloutWatcherBase
    {
        public ShowCalloutAtStartupWatcher(ICalloutManager calloutManager, IConfigurationService configurationService) 
            : base(calloutManager, configurationService)
        {
            Name = "ExampleCallout";
            IsOneTimeCallout = false;
        }

        public override TimeSpan Delay => TimeSpan.FromSeconds(10);
    }
}
