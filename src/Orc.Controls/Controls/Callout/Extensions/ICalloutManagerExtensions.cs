namespace Orc.Controls;

using System;
using System.Linq;
using Catel;
using Catel.Collections;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public static class ICalloutManagerExtensions
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(ICalloutManagerExtensions));

    public static IDisposable SuspendInScope(this ICalloutManager calloutManager)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        return new DisposableToken<ICalloutManager>(calloutManager,
            x => x.Instance.Suspend(),
            x => x.Instance.Resume());
    }

    public static bool IsAnyCalloutOpen(this ICalloutManager calloutManager)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        return calloutManager.Callouts.Any(x => x.IsOpen);
    }

    public static void ShowAllCallouts(this ICalloutManager calloutManager)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        if (calloutManager.IsSuspended)
        {
            Logger.LogDebug("Callout manager is currently suspended, cannot show all callouts");
            return;
        }

        Logger.LogDebug("Showing all callouts");

        calloutManager.Callouts.Where(x => !x.HasShown).ForEach(x => x.Show());
    }

    public static void ShowCallout(this ICalloutManager calloutManager, Guid id, Func<ICallout, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        if (calloutManager.IsSuspended)
        {
            Logger.LogDebug("Callout manager is currently suspended, cannot show callout with id '{CalloutId}'", id);
            return;
        }

        var callout = FindCallout(calloutManager, id);
        if (callout is null)
        {
            Logger.LogDebug("Callout with id '{CalloutId}' is not found", id);
            return;
        }

        if (predicate is not null)
        {
            if (!predicate(callout))
            {
                return;
            }
        }

        Logger.LogDebug("Showing callout '{Callout}'", callout);

        callout.Show();
    }

    public static void ShowCallout(this ICalloutManager calloutManager, string name, Func<ICallout, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        if (calloutManager.IsSuspended)
        {
            Logger.LogDebug("Callout manager is currently suspended, cannot show callout with name '{CalloutName}'", name);
            return;
        }

        var callout = FindCallout(calloutManager, name);
        if (callout is null)
        {
            Logger.LogDebug("Callout with name '{CalloutName}' is not found", name);
            return;
        }

        if (predicate is not null)
        {
            if (!predicate(callout))
            {
                return;
            }
        }

        Logger.LogDebug("Showing callout '{Callout}'", callout);

        callout.Show();
    }

    public static void HideAllCallouts(this ICalloutManager calloutManager)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        Logger.LogDebug("Hiding all callouts");

        calloutManager.Callouts.ForEach(x => x.Hide());
    }

    public static void HideCallout(this ICalloutManager calloutManager, Guid id)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        var callout = FindCallout(calloutManager, id);
        if (callout is null)
        {
            Logger.LogDebug("Callout with id '{CalloutId}' is not found", id);
            return;
        }

        Logger.LogDebug("Hiding callout '{Callout}'", callout);

        callout.Hide();
    }

    public static void HideCallout(this ICalloutManager calloutManager, string name)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        var callout = FindCallout(calloutManager, name);
        if (callout is null)
        {
            Logger.LogDebug("Callout with name '{CalloutName}' is not found", name);
            return;
        }

        Logger.LogDebug("Hiding callout '{Callout}'", callout);

        callout.Hide();
    }

    public static ICallout? FindCallout(this ICalloutManager calloutManager, Guid id)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        return calloutManager.Callouts.FirstOrDefault(x => x.Id == id);
    }

    public static ICallout? FindCallout(this ICalloutManager calloutManager, string name)
    {
        ArgumentNullException.ThrowIfNull(calloutManager);

        return calloutManager.Callouts.FirstOrDefault(x => x.Name == name);
    }
}
