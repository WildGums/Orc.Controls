using System.Linq;
using Catel.IoC;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;
using Orc.Automation.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<ISetupAutomationService, SetupAutomationService>();

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.Automation", "Orc.Automation.Properties", "Resources"));
    }
}
