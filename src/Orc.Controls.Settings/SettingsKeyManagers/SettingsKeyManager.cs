namespace Orc.Controls.Settings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catel;
using Catel.Logging;

public class SettingsKeyManager : ISettingsKeyManager
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();
    private readonly Dictionary<string, bool> _dirtyStates = new();

    private readonly HashSet<string> _keys = new();
    private readonly object _lock = new();

    public event AsyncEventHandler<SettingsKeyEventArgs>? LoadRequested;
    public event AsyncEventHandler<SettingsKeyEventArgs>? SaveRequested;
    public event AsyncEventHandler<SettingsKeyEventArgs>? RefreshRequested;
    public event AsyncEventHandler<SettingsKeyDirtyEventArgs>? DirtyStateChanged;
    public event AsyncEventHandler<SettingsKeyEventArgs>? RemoveRequested;
    public event AsyncEventHandler<ControlRequestedEventArgs>? RequestControls;
    public event AsyncEventHandler<SettingsKeyRenameEventArgs>? RenameRequested;
    public event EventHandler<SettingsKeyEventArgs>? KeyLoaded;
    public event EventHandler<SettingsKeyEventArgs>? KeySaved;
    public event EventHandler<SettingsKeyEventArgs>? KeyRefreshed;
    public event EventHandler<SettingsKeyEventArgs>? KeyRemoved;
    public event EventHandler<SettingsKeyRenameEventArgs>? KeyRenamed;

    public string[] GetKeys()
    {
        lock (_lock)
        {
            return _keys.ToArray();
        }
    }

    public async Task LoadAsync(string settingsKey)
    {
        if (string.IsNullOrEmpty(settingsKey))
        {
            return;
        }

        Log.Debug($"Load requested for settings key: {settingsKey}");
        var eventArgs = new SettingsKeyEventArgs(settingsKey);

        if (LoadRequested is not null)
        {
            await LoadRequested.SafeInvokeAsync(this, eventArgs);
        }

        // Only make changes if the operation was successful
        if (eventArgs.Success)
        {
            lock (_lock)
            {
                _keys.Add(settingsKey);
            }

            Log.Debug($"Settings key loaded successfully: {settingsKey}");
            KeyLoaded?.Invoke(this, new(settingsKey)
            {
                Success = true
            });
        }
    }

    public async Task SaveAsync(string settingsKey)
    {
        if (string.IsNullOrEmpty(settingsKey))
        {
            return;
        }

        Log.Debug($"Save requested for settings key: {settingsKey}");
        var eventArgs = new SettingsKeyEventArgs(settingsKey);
        await SaveRequested.SafeInvokeAsync(this, eventArgs);

        // Only make changes if the operation was successful
        if (eventArgs.Success)
        {
            // Reset dirty state after successful save
            SetDirty(settingsKey, false);

            Log.Debug($"Settings key saved successfully: {settingsKey}");
            KeySaved?.Invoke(this, new(settingsKey)
            {
                Success = true
            });
        }
    }

    public void SetDirty(string settingsKey, bool isDirty)
    {
        if (string.IsNullOrEmpty(settingsKey))
        {
            return;
        }

        _keys.Add(settingsKey);

        var stateChanged = false;
        lock (_lock)
        {
            if (_dirtyStates.TryGetValue(settingsKey, out var currentState))
            {
                if (currentState != isDirty)
                {
                    _dirtyStates[settingsKey] = isDirty;
                    stateChanged = true;
                }
            }
            else
            {
                _dirtyStates[settingsKey] = isDirty;
                stateChanged = true;
            }
        }

        if (stateChanged)
        {
            Log.Debug($"Dirty state changed for settings key '{settingsKey}': {isDirty}");
            DirtyStateChanged?.Invoke(this, new(settingsKey, isDirty));
        }
    }

    public async Task RefreshAsync(string settingsKey)
    {
        if (string.IsNullOrEmpty(settingsKey))
        {
            return;
        }

        Log.Debug($"Refresh requested for settings key: {settingsKey}");
        var eventArgs = new SettingsKeyEventArgs(settingsKey);
        await RefreshRequested.SafeInvokeAsync(this, eventArgs);

        // Only make changes if the operation was successful
        if (eventArgs.Success)
        {
            Log.Debug($"Settings key refreshed successfully: {settingsKey}");
            KeyRefreshed?.Invoke(this, new(settingsKey)
            {
                Success = true
            });
        }
    }

    public void Remove(string settingsKey)
    {
        if (string.IsNullOrEmpty(settingsKey))
        {
            return;
        }

        Log.Debug($"Remove requested for settings key: {settingsKey}");
        var eventArgs = new SettingsKeyEventArgs(settingsKey);
        RemoveRequested?.Invoke(this, eventArgs);

        // Only make changes if the operation was successful
        if (eventArgs.Success)
        {
            lock (_lock)
            {
                _keys.Remove(settingsKey);
                _dirtyStates.Remove(settingsKey);
            }

            Log.Debug($"Settings key removed successfully: {settingsKey}");
            KeyRemoved?.Invoke(this, new(settingsKey)
            {
                Success = true
            });
        }
    }

    public void Rename(string oldKey, string newKey)
    {
        if (string.IsNullOrEmpty(oldKey) || string.IsNullOrEmpty(newKey))
        {
            return;
        }

        Log.Debug($"Rename requested from '{oldKey}' to '{newKey}'");
        var eventArgs = new SettingsKeyRenameEventArgs(oldKey, newKey);
        RenameRequested?.Invoke(this, eventArgs);

        // Only make changes if the operation was successful
        if (!eventArgs.Success)
        {
            return;
        }

        lock (_lock)
        {
            if (_keys.Remove(oldKey))
            {
                _keys.Add(newKey);
            }

            if (_dirtyStates.Remove(oldKey, out var dirtyState))
            {
                _dirtyStates[newKey] = dirtyState;
            }
        }

        Log.Debug($"Settings key renamed successfully from '{oldKey}' to '{newKey}'");
        KeyRenamed?.Invoke(this, new(oldKey, newKey)
        {
            Success = true
        });
    }

    public bool IsKeyDirty(string settingsKey)
    {
        if (string.IsNullOrEmpty(settingsKey))
        {
            return false;
        }

        lock (_lock)
        {
            return _dirtyStates.TryGetValue(settingsKey, out var isDirty) && isDirty;
        }
    }

    public string[] GetDirtyKeys()
    {
        lock (_lock)
        {
            return _dirtyStates.Where(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToArray();
        }
    }

    public async Task<IReadOnlyCollection<ISettingsElement>> GetElementsAsync(string settingsKey)
    {
        var requestElementEventArgs = new ControlRequestedEventArgs(settingsKey);

        await RequestControls.SafeInvokeAsync(this, requestElementEventArgs);

        return requestElementEventArgs.Elements;
    }
}
