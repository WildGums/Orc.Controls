namespace Orc.Controls.Settings;

using System;
using System.Threading.Tasks;

/// <summary>
/// Generic settings storage that combines data storage and serialization
/// </summary>
public class SettingsStorage<TSettings> : ISettingsStorage<TSettings> 
    where TSettings : class
{
    private readonly ISettingsDataStorage _dataStorage;
    private readonly ISettingsSerializer<TSettings> _serializer;

    public SettingsStorage(ISettingsDataStorage dataStorage, ISettingsSerializer<TSettings> serializer)
    {
        ArgumentNullException.ThrowIfNull(dataStorage);
        ArgumentNullException.ThrowIfNull(serializer);

        _dataStorage = dataStorage;
        _serializer = serializer;
    }

    public async Task<TSettings?> LoadAsync(string settingsKey)
    {
        var data = await _dataStorage.LoadStringAsync(settingsKey);
        if (string.IsNullOrEmpty(data))
        {
            return null;
        }

        try
        {
            var result = await _serializer.DeserializeAsync(data);
            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task SaveAsync(string settingsKey, TSettings settings)
    {
        var data = await _serializer.SerializeAsync(settings);
        await _dataStorage.SaveStringAsync(settingsKey, data);
    }

    public async Task DeleteAsync(string settingsKey)
    {
        await _dataStorage.DeleteAsync(settingsKey);
    }

    public async Task RenameAsync(string oldKey, string newKey)
    {
        await _dataStorage.RenameAsync(oldKey, newKey);
    }

    public async Task<bool> ExistsAsync(string settingsKey)
    {
        return await _dataStorage.ExistsAsync(settingsKey);
    }
}
