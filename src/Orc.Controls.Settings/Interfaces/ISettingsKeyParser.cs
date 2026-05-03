namespace Orc.Controls.Settings;

public interface ISettingsKeyParser<T> where T : class, new()
{
    string ToString(T keyInfo);
    T Parse(string settingsKey);
}
