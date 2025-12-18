namespace Orc.Controls.Settings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catel.Logging;

public class HybridSettingsDataStorage : ISettingsDataStorage
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly ISettingsDataStorage[] _storages;

    public HybridSettingsDataStorage(IEnumerable<ISettingsDataStorage> storages)
    {
        ArgumentNullException.ThrowIfNull(storages);

        _storages = storages.ToArray();
    }

    public async Task<bool> IsReadOnlyAsync(string settingsKey)
    {
        var storage = await GetDataStorageAsync(settingsKey);
        if (storage is null)
        {
            return false;
        }

        return await storage.IsReadOnlyAsync(settingsKey);
    }

    public async Task<bool> CanHandleAsync(string settingsKey)
    {
        return await GetDataStorageAsync(settingsKey) is not null;
    }

    private async Task<ISettingsDataStorage?> GetDataStorageAsync(string settingsKey)
    {
        foreach (var settingsDataStorage in _storages)
        {
            if (await settingsDataStorage.CanHandleAsync(settingsKey))
            {
                return settingsDataStorage;
            }
        }

        return null;
    } 

    public async Task<string?> LoadStringAsync(string settingsKey)
    {
        Log.Debug($"Loading settings for key: {settingsKey}");

        var storage = await GetDataStorageAsync(settingsKey);
        if (storage is null)
        {
            return null;
        }

        try
        {
            var data = await storage.LoadStringAsync(settingsKey);
            if (!string.IsNullOrEmpty(data))
            {
                return data;
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, $"Error loading from provider for key: {settingsKey}");
            // Continue to next provider
        }

        Log.Debug($"No settings found for key: {settingsKey}");
        return null;
    }

    public async Task SaveStringAsync(string settingsKey, string data)
    {
        var storage = await GetDataStorageAsync(settingsKey);
        if (storage is null)
        {
            return;
        }

        if (await storage.IsReadOnlyAsync(settingsKey))
        {
            return;
        }

        try
        {
            await storage.SaveStringAsync(settingsKey, data);
            Log.Debug($"Saved settings to provider for key: {settingsKey}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error saving to provider for key: {settingsKey}");
            throw;
        }
    }

    public async Task DeleteAsync(string settingsKey)
    {
        var storage = await GetDataStorageAsync(settingsKey);
        if (storage is null)
        {
            return;
        }

        if (await storage.IsReadOnlyAsync(settingsKey))
        {
            return;
        }

        try
        {
            await storage.DeleteAsync(settingsKey);
            Log.Debug($"Error loading from provider for key: {settingsKey}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error loading from provider for key: {settingsKey}");
            throw;
        }
    }

    public async Task RenameAsync(string oldKey, string newKey)
    {
        var oldStorage = await GetDataStorageAsync(oldKey);
        if (oldStorage is null)
        {
            return;
        }

        var newStorage = await GetDataStorageAsync(newKey);
        if (newStorage is null)
        {
            return;
        }

        if (!Equals(newStorage, oldStorage))
        {
            return;
        }

        if (await oldStorage.IsReadOnlyAsync(oldKey))
        {
            return;
        }

        if (await newStorage.IsReadOnlyAsync(newKey))
        {
            return;
        }

        try
        {
            await oldStorage.RenameAsync(oldKey, newKey);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error renaming in provider from '{oldKey}' to '{newKey}'");
            throw;
        }
    }

    public async Task<bool> ExistsAsync(string settingsKey)
    {
        var storage = await GetDataStorageAsync(settingsKey);
        if (storage is null)
        {
            return false;
        }

        // Check all providers (read and write enabled) in priority order
        try
        {
            var exists = await storage.ExistsAsync(settingsKey);
            if (exists)
            {
                Log.Debug($"Settings found in provider for key: {settingsKey}");
                return true;
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, $"Error checking existence in provider for key: {settingsKey}");
            // Continue to next provider
        }

        return false;
    }
}
