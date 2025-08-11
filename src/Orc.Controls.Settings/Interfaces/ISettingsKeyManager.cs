namespace Orc.Controls.Settings;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catel;

/// <summary>
/// Simplified settings key manager - only handles key operations and state
/// </summary>
public interface ISettingsKeyManager
{
    /// <summary>
    /// Get all currently tracked settings keys
    /// </summary>
    string[] GetKeys();

    /// <summary>
    /// Get all keys that have unsaved changes
    /// </summary>
    string[] GetDirtyKeys();

    /// <summary>
    /// Check if a key is dirty
    /// </summary>
    bool IsKeyDirty(string settingsKey);

    /// <summary>
    /// Request to load settings for a key
    /// </summary>
    Task LoadAsync(string settingsKey);

    /// <summary>
    /// Request to save settings for a key
    /// </summary>
    Task SaveAsync(string settingsKey);

    /// <summary>
    /// Request to refresh/reload settings for a key
    /// </summary>
    Task RefreshAsync(string settingsKey);

    /// <summary>
    /// Set dirty state for a key
    /// </summary>
    void SetDirty(string settingsKey, bool isDirty);

    /// <summary>
    /// Request to remove/delete settings for a key
    /// </summary>
    void Remove(string settingsKey);

    /// <summary>
    /// Request to rename a settings key
    /// </summary>
    void Rename(string oldKey, string newKey);

    Task<IReadOnlyCollection<ISettingsElement>> GetElementsAsync(string settingsKey);

    // Events
    event AsyncEventHandler<SettingsKeyEventArgs>? LoadRequested;
    event AsyncEventHandler<SettingsKeyEventArgs>? SaveRequested;
    event AsyncEventHandler<SettingsKeyEventArgs>? RefreshRequested;
    event AsyncEventHandler<SettingsKeyDirtyEventArgs>? DirtyStateChanged;
    event AsyncEventHandler<SettingsKeyEventArgs>? RemoveRequested;
    event AsyncEventHandler<SettingsKeyRenameEventArgs>? RenameRequested;
    event EventHandler<SettingsKeyEventArgs>? KeyLoaded;
    event EventHandler<SettingsKeyEventArgs>? KeySaved;
    event EventHandler<SettingsKeyEventArgs>? KeyRefreshed;
    event EventHandler<SettingsKeyEventArgs>? KeyRemoved;
    event EventHandler<SettingsKeyRenameEventArgs>? KeyRenamed;

    event AsyncEventHandler<ControlRequestedEventArgs>? RequestControls;
}
