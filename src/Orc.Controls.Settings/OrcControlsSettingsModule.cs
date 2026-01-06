namespace Orc
{
    using Catel.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Orc.Controls.Settings;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcControlsSettingsModule
    {
        public static IServiceCollection AddOrcControlsSettings(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ISettingsFolderProvider, FileBasedSettingsFolderProvider>();

            serviceCollection.TryAddSingleton<ISettingsKeyManager, SettingsKeyManager>();

            serviceCollection.TryAddSingleton<ISettingsLocationProvider, FileSystemLocationProvider>();
            serviceCollection.TryAddSingleton(typeof(ISettingsSerializer<>), typeof(JsonSettingsSerializer<>));

            serviceCollection.TryAddSingleton(typeof(ISettingsStorage<>), typeof(SettingsStorage<>));

            serviceCollection.TryAddSingleton<ISettingsStateStorage, SettingsStateStorage>();

            serviceCollection.TryAddSingleton(typeof(ISettingsKeyParser<>), typeof(AttributeBasedSettingsKeyParser<>));


            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Controls.Settings", "Orc.Controls.Settings.Properties", "Resources"));

            return serviceCollection;
        }
    }
}
