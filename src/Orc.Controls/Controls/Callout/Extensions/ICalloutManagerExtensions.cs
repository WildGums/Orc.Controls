namespace Orc.Controls
{
    using System;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.Logging;

    public static class ICalloutManagerExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static IDisposable SuspendInScope(this ICalloutManager calloutManager)
        {
            Argument.IsNotNull(() => calloutManager);

            return new DisposableToken<ICalloutManager>(calloutManager,
                x => x.Instance.Suspend(),
                x => x.Instance.Resume());
        }

        public static bool IsAnyCalloutOpen(this ICalloutManager calloutManager)
        {
            Argument.IsNotNull(() => calloutManager);

            return calloutManager.Callouts.Any(x => x.IsOpen);
        }

        public static void ShowAllCallouts(this ICalloutManager calloutManager)
        {
            Argument.IsNotNull(() => calloutManager);

            if (calloutManager.IsSuspended)
            {
                Log.Debug($"Callout manager is currently suspended, cannot show all callouts");
                return;
            }

            Log.Debug("Showing all callouts");

            calloutManager.Callouts.Where(x => !x.HasShown).ForEach(x => x.Show());
        }

        public static void ShowCallout(this ICalloutManager calloutManager, Guid id, Func<ICallout, bool> predicate = null)
        {
            Argument.IsNotNull(() => calloutManager);

            if (calloutManager.IsSuspended)
            {
                Log.Debug($"Callout manager is currently suspended, cannot show callout with id '{id}'");
                return;
            }

            var callout = FindCallout(calloutManager, id);
            if (callout is null)
            {
                Log.Debug($"Callout with id '{id}' is not found");
                return;
            }

            if (predicate is not null)
            {
                if (!predicate(callout))
                {
                    return;
                }
            }

            Log.Debug($"Showing callout '{callout}'");

            callout.Show();
        }

        public static void ShowCallout(this ICalloutManager calloutManager, string name, Func<ICallout, bool> predicate = null)
        {
            Argument.IsNotNull(() => calloutManager);

            if (calloutManager.IsSuspended)
            {
                Log.Debug($"Callout manager is currently suspended, cannot show callout with name '{name}'");
                return;
            }

            var callout = FindCallout(calloutManager, name);
            if (callout is null)
            {
                Log.Debug($"Callout with name '{name}' is not found");
                return;
            }

            if (predicate is not null)
            {
                if (!predicate(callout))
                {
                    return;
                }
            }

            Log.Debug($"Showing callout '{callout}'");

            callout.Show();
        }

        public static void HideAllCallouts(this ICalloutManager calloutManager)
        {
            Argument.IsNotNull(() => calloutManager);

            Log.Debug("Hiding all callouts");

            calloutManager.Callouts.ForEach(x => x.Hide());
        }

        public static void HideCallout(this ICalloutManager calloutManager, Guid id)
        {
            Argument.IsNotNull(() => calloutManager);

            var callout = FindCallout(calloutManager, id);
            if (callout is null)
            {
                Log.Debug($"Callout with id '{id}' is not found");
                return;
            }

            Log.Debug($"Hiding callout '{callout}'");

            callout.Hide();
        }

        public static void HideCallout(this ICalloutManager calloutManager, string name)
        {
            Argument.IsNotNull(() => calloutManager);

            var callout = FindCallout(calloutManager, name);
            if (callout is null)
            {
                Log.Debug($"Callout with name '{name}' is not found");
                return;
            }

            Log.Debug($"Hiding callout '{callout}'");

            callout.Hide();
        }

        public static ICallout FindCallout(this ICalloutManager calloutManager, Guid id)
        {
            Argument.IsNotNull(() => calloutManager);

            return calloutManager.Callouts.FirstOrDefault(x => x.Id == id);
        }

        public static ICallout FindCallout(this ICalloutManager calloutManager, string name)
        {
            Argument.IsNotNull(() => calloutManager);

            return calloutManager.Callouts.FirstOrDefault(x => x.Name == name);
        }
    }
}
