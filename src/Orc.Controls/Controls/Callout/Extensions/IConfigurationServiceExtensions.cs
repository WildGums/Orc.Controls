namespace Orc.Controls
{
    using System;
    using System.Threading.Tasks;
    using Catel.Configuration;

    public static class IConfigurationServiceExtensions
    {
        public static async Task<bool> IsCalloutMarkedAsShownAsync(this IConfigurationService configurationService, ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.Shown";
            return configurationService.GetRoamingValue(configurationKey, false);
        }

        public static Task SetCalloutLastShownAsync(this IConfigurationService configurationService, ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            return SetCalloutLastShownAsync(configurationService, callout, DateTime.UtcNow);
        }

        public static async Task SetCalloutLastShownAsync(this IConfigurationService configurationService, ICallout callout, DateTime? lastShown)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.LastShown";
            configurationService.SetRoamingValue(configurationKey, lastShown);
        }

        public static async Task<DateTime?> GetCalloutLastShownAsync(this IConfigurationService configurationService, ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.LastShown";
            return configurationService.GetRoamingValue<DateTime?>(configurationKey, null);
        }

        public static async Task MarkCalloutAsNotShownAsync(this IConfigurationService configurationService, ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.Shown";
            configurationService.SetRoamingValue(configurationKey, false);
        }

        public static async Task MarkCalloutAsShownAsync(this IConfigurationService configurationService, ICallout callout)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(callout);

            var configurationKey = $"{callout.GetCalloutConfigurationKeyPrefix()}.Shown";
            configurationService.SetRoamingValue(configurationKey, true);

            await configurationService.SetCalloutLastShownAsync(callout, DateTime.UtcNow);
        }
    }
}
