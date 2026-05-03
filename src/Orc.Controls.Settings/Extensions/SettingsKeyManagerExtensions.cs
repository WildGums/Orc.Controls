namespace Orc.Controls.Settings;

using System;
using System.Runtime.CompilerServices;

// Extension to your existing ISettingsKeyManager interface
public static class SettingsKeyManagerExtensions
{
    private static readonly ConditionalWeakTable<ISettingsKeyManager, SettingsSynchronizationSupport> _syncSupport =
        new();

    // Add synchronization support to existing manager
    public static void EnableSynchronization(this ISettingsKeyManager manager)
    {
        if (!_syncSupport.TryGetValue(manager, out _))
        {
            _syncSupport.Add(manager, new());
        }
    }

    // Subscribe to settings changes
    public static void SubscribeToSettingsChanges(this ISettingsKeyManager manager,
        EventHandler<SettingsChangedEventArgs> handler)
    {
        if (_syncSupport.TryGetValue(manager, out var support))
        {
            support.SettingsChanged += handler;
        }
    }

    // Unsubscribe from settings changes
    public static void UnsubscribeFromSettingsChanges(this ISettingsKeyManager manager,
        EventHandler<SettingsChangedEventArgs> handler)
    {
        if (_syncSupport.TryGetValue(manager, out var support))
        {
            support.SettingsChanged -= handler;
        }
    }

    // Notify about settings changes
    public static void NotifySettingsChanged(this ISettingsKeyManager manager, string settingsKey, object settings)
    {
        if (_syncSupport.TryGetValue(manager, out var support))
        {
            support.NotifySettingsChanged(settingsKey, settings);
        }
    }

    private class SettingsSynchronizationSupport
    {
        public event EventHandler<SettingsChangedEventArgs>? SettingsChanged;

        public void NotifySettingsChanged(string settingsKey, object settings)
        {
            var args = new SettingsChangedEventArgs(settingsKey, settings);
            SettingsChanged?.Invoke(this, args);
        }
    }
}
