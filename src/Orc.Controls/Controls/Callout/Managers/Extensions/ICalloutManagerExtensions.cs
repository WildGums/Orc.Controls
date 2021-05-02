namespace Orc.Controls
{
    using System;
    using System.Linq;
    using Catel;

    public static class ICalloutManagerExtensions
    {
        public static void ShowAllCallouts(this ICalloutManager calloutManager)
        {
            Argument.IsNotNull(() => calloutManager);

            calloutManager.Callouts.ForEach(x => x.Show());
        }

        public static void HideAllCallouts(this ICalloutManager calloutManager)
        {
            Argument.IsNotNull(() => calloutManager);

            calloutManager.Callouts.ForEach(x => x.Hide());
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
