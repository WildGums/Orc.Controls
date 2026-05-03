namespace Orc.Controls.Settings;

using System.Threading.Tasks;

public interface ISettingsStorage<TSettings> where TSettings : class
{
    Task<TSettings?> LoadAsync(string settingsKey);
    Task SaveAsync(string settingsKey, TSettings settings);
    Task DeleteAsync(string settingsKey);
    Task RenameAsync(string oldKey, string newKey);
    Task<bool> ExistsAsync(string settingsKey);
}
