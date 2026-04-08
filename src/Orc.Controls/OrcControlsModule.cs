namespace Orc;

using Catel.Services;
using Catel.ThirdPartyNotices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Orc.Controls;
using Orc.Controls.Services;
using Orc.Controls.Tools;

/// <summary>
/// Core module which allows the registration of default services in the service collection.
/// </summary>
public static class OrcControlsModule
{
    public static IServiceCollection AddOrcControls(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<IApplicationLogFilterGroupService, ApplicationLogFilterGroupService>();
        serviceCollection.TryAddSingleton<ICalloutManager, CalloutManager>();
        serviceCollection.TryAddSingleton<ISuggestionListService, SuggestionListService>();
        serviceCollection.TryAddSingleton<IValidationNamesService, ValidationNamesService>();
        serviceCollection.TryAddSingleton<ITextInputWindowService, TextInputWindowService>();
        serviceCollection.TryAddSingleton<IControlToolManagerFactory, ControlToolManagerFactory>();

        serviceCollection.TryAddSingleton<ITimeAdjustmentProvider, TimeAdjustmentProvider>();

        serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Controls", "Orc.Controls.Properties", "Resources"));

        serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orc.Controls", "https://github.com/wildgums/orc.controls"));

        return serviceCollection;
    }
}
