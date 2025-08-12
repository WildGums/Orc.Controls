namespace Orc.Controls.Settings;

using System.Threading.Tasks;

public interface ISettingsSerializer<TSettings> where TSettings : class
{
    Task<TSettings?> DeserializeAsync(string data);
    Task<string> SerializeAsync(TSettings settings);
}
