namespace Orc.Controls.Settings;

using System;

public class SettingsChangedEventArgs : EventArgs
{
    public string SettingsKey { get; }
    public object Settings { get; }
    public Type SettingsType { get; }

    public SettingsChangedEventArgs(string settingsKey, object settings)
    {
        SettingsKey = settingsKey;
        Settings = settings;
        SettingsType = settings?.GetType() ?? typeof(object);
    }
}
