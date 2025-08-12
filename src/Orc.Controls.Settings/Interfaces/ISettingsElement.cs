namespace Orc.Controls.Settings;

using System.Threading.Tasks;
using System.Windows;

public interface ISettingsElement
{
    FrameworkElement? Control { get; }
    string? SettingsKey { get; }
    Task<object?> GetSettingsAsync(string key);
    Task SaveAsync(string key);
}
