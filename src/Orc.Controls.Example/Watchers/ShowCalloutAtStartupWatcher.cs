namespace Orc.Controls.Example.Watchers;

using System;
using Catel.Configuration;
using Catel.IoC;
using Catel.Services;
using Microsoft.Extensions.Logging;

public class ShowCalloutAtStartupWatcher : TimeBasedCalloutWatcherBase, IConstructAtStartup
{
    public ShowCalloutAtStartupWatcher(ILogger<ShowCalloutAtStartupWatcher> logger, 
        ICalloutManager calloutManager, 
        IConfigurationService configurationService,
        IDispatcherService dispatcherService) 
        : base(logger, calloutManager, configurationService, dispatcherService)
    {
        Name = "ExampleCallout";
        IsOneTimeCallout = false;
    }

    public override TimeSpan Delay => TimeSpan.FromSeconds(10);
}
