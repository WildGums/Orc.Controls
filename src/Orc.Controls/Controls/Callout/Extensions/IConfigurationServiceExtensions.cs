namespace Orc.Controls
{
    using System;
    using Catel;
    using Catel.Configuration;

    public static class IConfigurationServiceExtensions
    {
        public static bool IsCalloutMarkedAsShown(this IConfigurationService configurationService, ICallout callout)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.Shown";
            return configurationService.GetRoamingValue(configurationKey, false);
        }

        public static void SetCalloutLastShown(this IConfigurationService configurationService, ICallout callout)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => callout);

            SetCalloutLastShown(configurationService, callout, DateTime.UtcNow);
        }

        public static void SetCalloutLastShown(this IConfigurationService configurationService, ICallout callout, DateTime? lastShown)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.LastShown";
            configurationService.SetRoamingValue(configurationKey, lastShown);
        }

        public static DateTime? GetCalloutLastShown(this IConfigurationService configurationService, ICallout callout)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.LastShown";
            return configurationService.GetRoamingValue<DateTime?>(configurationKey, null);
        }

        public static void MarkCalloutAsNotShown(this IConfigurationService configurationService, ICallout callout)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.Shown";
            configurationService.SetRoamingValue(configurationKey, false);
        }

        public static void MarkCalloutAsShown(this IConfigurationService configurationService, ICallout callout)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.Shown";
            configurationService.SetRoamingValue(configurationKey, true);

            configurationService.SetCalloutLastShown(callout, DateTime.UtcNow);
        }
    }
}
