namespace Orc.Controls.Settings;

using System.Threading.Tasks;

public interface ISettingsLocationProvider
{
    /// <summary>
    /// Gets the file path for a given settings key
    /// </summary>
    Task<string> GetLocationAsync(string settingsKey);
}
