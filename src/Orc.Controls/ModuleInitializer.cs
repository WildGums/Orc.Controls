using System.Linq;
using Catel.IoC;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;
using Orc.Controls;
using Orc.Controls.Services;
using Orc.Controls.Tools;

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

        var logListener = (from x in LogManager.GetListeners()
                           where x is LogViewerLogListener
                           select (LogViewerLogListener)x).FirstOrDefault();
        if (logListener == null)
        {
            logListener = new LogViewerLogListener
            {
                IgnoreCatelLogging = true,
                IsDebugEnabled = true,
                IsInfoEnabled = true,
                IsWarningEnabled = true,
                IsErrorEnabled = true
            };

            LogManager.AddListener(logListener);
        }

        serviceLocator.RegisterInstance<LogViewerLogListener>(logListener);

        serviceLocator.RegisterType<IAccentColorService, AccentColorService>();
        serviceLocator.RegisterType<IApplicationLogFilterGroupService, ApplicationLogFilterGroupService>();

        serviceLocator.RegisterType<ISuggestionListService, SuggestionListService>();
        serviceLocator.RegisterType<IValidationNamesService, ValidationNamesService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IConnectionStringBuilderServiceInitializer, ConnectionStringBuilderServiceInitializer>();
        serviceLocator.RegisterType<IConnectionStringBuilderService, ConnectionStringBuilderService>();
        serviceLocator.RegisterTypeIfNotYetRegistered<IControlToolManagerFactory, ControlToolManagerFactory>();

        // Override Catel.SelectDirectoryService with Orchestra.Services.SelectDirectoryService, moved to orchestra
        serviceLocator.RegisterType<ISelectDirectoryService, MicrosoftApiSelectDirectoryService>();

        serviceLocator.RegisterType<ITimeAdjustmentProvider, TimeAdjustmentProvider>();

        var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
        viewModelLocator.Register(typeof(CulturePicker), typeof(CulturePickerViewModel));
        viewModelLocator.Register(typeof(ValidationContextTree), typeof(ValidationContextTreeViewModel));
        viewModelLocator.Register(typeof(ValidationContextView), typeof(ValidationContextViewModel));
        viewModelLocator.Register(typeof(ConnectionStringBuilder), typeof(ConnectionStringBuilderViewModel));
        viewModelLocator.Register(typeof(ConnectionStringEditWindow), typeof(ConnectionStringEditViewModel));
        viewModelLocator.Register(typeof(ConnectionStringAdvancedOptionsWindow), typeof(ConnectionStringAdvancedOptionsViewModel));
        viewModelLocator.Register(typeof(DbConnectionProviderListWindow), typeof(DbConnectionProviderListViewModel));

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.Controls", "Orc.Controls.Properties", "Resources"));
    }
}
