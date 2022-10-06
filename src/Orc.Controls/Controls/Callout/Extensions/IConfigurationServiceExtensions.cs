namespace Orc.Controls
{
    using System;
    using System.Threading.Tasks;
    using Catel.Configuration;

    public static class IConfigurationServiceExtensions
    {
        public static Task<bool> IsCalloutMarkedAsShownAsync(this IConfigurationService configurationService, ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.Shown";
            return configurationService.GetRoamingValueAsync(configurationKey, false);
        }

        public static Task SetCalloutLastShownAsync(this IConfigurationService configurationService, ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            return SetCalloutLastShownAsync(configurationService, callout, DateTime.UtcNow);
        }

        public static Task SetCalloutLastShownAsync(this IConfigurationService configurationService, ICallout callout, DateTime? lastShown)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.LastShown";
            return configurationService.SetRoamingValueAsync(configurationKey, lastShown);
        }

        public static Task<DateTime?> GetCalloutLastShownAsync(this IConfigurationService configurationService, ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.LastShown";
            return configurationService.GetRoamingValueAsync<DateTime?>(configurationKey, null);
        }

        public static Task MarkCalloutAsNotShownAsync(this IConfigurationService configurationService, ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.Shown";
            return configurationService.SetRoamingValueAsync(configurationKey, false);
        }

        public static async Task MarkCalloutAsShownAsync(this IConfigurationService configurationService, ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.Shown";
            await configurationService.SetRoamingValueAsync(configurationKey, true);

            await configurationService.SetCalloutLastShownAsync(callout, DateTime.UtcNow);
        }
    }
}
