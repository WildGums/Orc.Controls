namespace Orc.Controls.Settings;

using System.Threading.Tasks;
using System.Collections.Generic;

public interface ISettingsFolderProvider
{
    // Existing methods
    Task<SettingsFoldersCollection> GetSettingsFoldersAsync();
    Task SaveSettingsFoldersAsync(SettingsFoldersCollection settingsFolders);

    // New methods for individual folder management
    Task<SettingsFolderItem?> GetSettingsFolderAsync(string name);
    Task<bool> SettingsFolderExistsAsync(string name);
    Task AddOrUpdateSettingsFolderAsync(SettingsFolderItem folder);
    Task RemoveSettingsFolderAsync(string name);
    Task<IEnumerable<string>> GetSettingsFolderNamesAsync();

    // Initialization method
    Task InitializeWithDefaultsAsync(SettingsFoldersCollection defaultSettings, bool overrideExisting = false);

    // File management
    Task<bool> SettingsFileExistsAsync();
}
