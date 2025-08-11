namespace Orc.Controls.Settings;

public interface ISettingsKeyInteractionHub
{
    bool CanSave(ISettingsElement element, string key);
}
