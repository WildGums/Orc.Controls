namespace Orc.Controls.Settings;

using System.Threading.Tasks;

public interface ISettingsDataStorage
{
    Task<bool> IsReadOnlyAsync(string settingsKey);
    Task<bool> CanHandleAsync(string settingsKey);
    Task<string?> LoadStringAsync(string settingsKey);
    Task SaveStringAsync(string settingsKey, string data);
    Task DeleteAsync(string settingsKey);
    Task RenameAsync(string oldKey, string newKey);
    Task<bool> ExistsAsync(string settingsKey);
}
