namespace Orc.Controls
{
    using Catel.MVVM;
    using Catel.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Orc.Controls;
    using Orc.Controls.Controls.LogViewer.Logging;
    using Orc.Controls.Services;
    using Orc.Controls.Tools;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcControlsModule
    {
        public static IServiceCollection AddOrcControls(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILogger, InMemoryLogger>();

            serviceCollection.TryAddSingleton<IApplicationLogFilterGroupService, ApplicationLogFilterGroupService>();
            serviceCollection.TryAddSingleton<ICalloutManager, CalloutManager>();
            serviceCollection.TryAddSingleton<ISuggestionListService, SuggestionListService>();
            serviceCollection.TryAddSingleton<IValidationNamesService, ValidationNamesService>();
            serviceCollection.TryAddSingleton<ITextInputWindowService, TextInputWindowService>();
            serviceCollection.TryAddSingleton<IControlToolManagerFactory, ControlToolManagerFactory>();

            serviceCollection.TryAddSingleton<ITimeAdjustmentProvider, TimeAdjustmentProvider>();

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Controls", "Orc.Controls.Properties", "Resources"));

            return serviceCollection;
        }
    }
}
