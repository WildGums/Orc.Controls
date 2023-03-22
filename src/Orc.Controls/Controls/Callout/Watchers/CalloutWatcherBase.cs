namespace Orc.Controls;

using System;
using System.Threading.Tasks;
using Catel.Configuration;

public abstract class CalloutWatcherBase
{
    protected readonly ICalloutManager _calloutManager;
    protected readonly IConfigurationService _configurationService;

    public CalloutWatcherBase(ICalloutManager calloutManager, IConfigurationService configurationService)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);
        ArgumentNullException.ThrowIfNull(configurationService);

        _calloutManager = calloutManager;
        _configurationService = configurationService;

        IsOneTimeCallout = true;
    }

    public Guid? Id { get; protected set; }

    public string? Name { get; protected set; }

    public virtual string Version => "1.0.0";

    public bool HasShown { get; private set; }

    public bool IsOneTimeCallout { get; protected set; }

    public TimeSpan ShowInterval { get; protected set; }

    public virtual ICallout? Callout
    {
        get
        {
            ICallout? callout = null;

            if (Id.HasValue)
            {
                callout = _calloutManager.FindCallout(Id.Value);
            }

            if (callout is null && Name is not null)
            {
                callout = _calloutManager.FindCallout(Name);
            }

            return callout;
        }
    }

    protected virtual async Task ShowAsync()
    {
        var callout = Callout;
        if (callout is null)
        {
            return;
        }

        var lastShownUtc = await GetLastShownUtcAsync();
        if (IsOneTimeCallout && lastShownUtc.HasValue)
        {
            return;
        }

        callout.Show();

        HasShown = true;

        await _configurationService.MarkCalloutAsShownAsync(callout);
    }

    protected virtual void Hide()
    {
        Callout?.Hide();
    }

    protected virtual async Task<DateTime?> GetLastShownUtcAsync()
    {
        var callout = Callout;
        if (callout is null)
        {
            return null;
        }

        var value = await _configurationService.GetCalloutLastShownAsync(callout);
        return value;
    }
}
