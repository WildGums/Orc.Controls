using Catel.IoC;
using Catel.MVVM;
using Orc.Controls;

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

        var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
        viewModelLocator.Register(typeof(DateTimePickerControl), typeof(DateTimePickerViewModel));
        viewModelLocator.Register(typeof(TimeSpanControl), typeof(TimeSpanViewModel));
    }
}