using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Orc.Controls;
using Orc.Controls.Services;
using Orc.Controls.ViewModels;

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
        serviceLocator.RegisterType<ISuggestionListService, SuggestionListService>();

        // Override Catel.SelectDirectoryService with Orchestra.Services.SelectDirectoryService
        serviceLocator.RegisterType<ISelectDirectoryService, MicrosoftApiSelectDirectoryService>();

        var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
        viewModelLocator.Register(typeof(DateTimePickerControl), typeof(DateTimePickerViewModel));
        viewModelLocator.Register(typeof(TimeSpanControl), typeof(TimeSpanViewModel));
    }
}