namespace Orc.Controls.Settings;

using System;
using System.Reflection;
using Catel.IoC;

public class ControlSettingsModule
{
    private readonly IServiceLocator _serviceLocator;

    public ControlSettingsModule(IServiceLocator serviceLocator)
    {
        ArgumentNullException.ThrowIfNull(serviceLocator);

        _serviceLocator = serviceLocator;
    }

    public void Initialize()
    {
        //Register IControlSettingsAdapter
        //DO IT IN APPLICATION

        //Register IEmbeddedResourceProvider
        //DO IT IN APPLICATION

        //Register ISettingsDataStorage
        //DO IT IN APPLICATION

        //Register Default SettingsFolderProvider
        _serviceLocator.RegisterType<ISettingsFolderProvider, FileBasedSettingsFolderProvider>();
        

        _serviceLocator.RegisterType<ISettingsKeyManager, SettingsKeyManager>();



        _serviceLocator.RegisterType<ISettingsLocationProvider, FileSystemLocationProvider>();
        _serviceLocator.RegisterType(typeof(ISettingsSerializer<>), typeof(JsonSettingsSerializer<>));

        _serviceLocator.RegisterType(typeof(ISettingsStorage<>), typeof(SettingsStorage<>));

        _serviceLocator.RegisterType<ISettingsStateStorage, SettingsStateStorage>();

        //_serviceLocator.RegisterRequiredTypeAndInstantiate<ISettingsFolderProvider), typeof(FileBasedSettingsFolderProvider));
    }
}
