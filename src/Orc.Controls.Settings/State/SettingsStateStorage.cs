namespace Orc.Controls.Settings;

using System;
using System.Collections.Generic;
using Catel.Logging;

public class SettingsStateStorage : ISettingsStateStorage
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();
    private readonly Dictionary<string, object> _storedSettings = new();
    private readonly object _lock = new object(); // Add synchronization lock

    public event EventHandler<SettingsKeyEventArgs>? SettingsStored;
    public event EventHandler<SettingsKeyEventArgs>? SettingsRemoved;

    public void StoreCurrentSettings<T>(string settingsKey, T settings) where T : class
    {
        if (string.IsNullOrWhiteSpace(settingsKey))
        {
            return;
        }

        lock (_lock)
        {
            _storedSettings[settingsKey] = settings;
        }

        SettingsStored?.Invoke(this, new(settingsKey));
        Log.Debug($"Stored settings for key '{settingsKey}' ({typeof(T).Name})");
    }

    public T? GetStoredSettings<T>(string settingsKey) where T : class
    {
        if (string.IsNullOrWhiteSpace(settingsKey))
        {
            return null;
        }

        lock (_lock)
        {
            if (!_storedSettings.TryGetValue(settingsKey, out var settings)
                || settings is not T typedSettings)
            {
                return null;
            }

            Log.Debug($"Retrieved stored settings for key '{settingsKey}' ({typeof(T).Name})");
            return typedSettings;
        }
    }

    public void Rename(string oldKey, string newKey)
    {
        lock (_lock)
        {
            if (_storedSettings.TryGetValue(oldKey, out var settings))
            {
                _storedSettings[newKey] = settings;
                _storedSettings.Remove(oldKey);
            }
        }
    }

    public void RemoveStoredSettings(string settingsKey)
    {
        if (string.IsNullOrWhiteSpace(settingsKey))
        {
            return;
        }

        bool removed;
        lock (_lock)
        {
            removed = _storedSettings.Remove(settingsKey);
        }

        if (removed)
        {
            SettingsRemoved?.Invoke(this, new(settingsKey));
            Log.Debug($"Removed stored settings and dirty state for key '{settingsKey}'");
        }
    }
}
