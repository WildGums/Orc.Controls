namespace Orc.Controls.Settings;

using System;

public interface ISettingsStateStorage
{
    /// <summary>
    /// Stores current settings for a key (for later restoration)
    /// </summary>
    void StoreCurrentSettings<T>(string settingsKey, T settings) where T : class;

    /// <summary>
    /// Gets stored settings for a key
    /// </summary>
    T? GetStoredSettings<T>(string settingsKey) where T : class;

    void Rename(string oldKey, string newKey);

    /// <summary>
    /// Removes stored settings for a key
    /// </summary>
    void RemoveStoredSettings(string settingsKey);

    /// <summary>
    /// Event fired when dirty state changes
    /// </summary>
    event EventHandler<SettingsKeyEventArgs> SettingsStored;
    event EventHandler<SettingsKeyEventArgs> SettingsRemoved;
}
