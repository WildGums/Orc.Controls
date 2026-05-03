namespace Orc.Controls.Settings;

using System;

/// <summary>
/// Event arguments for settings key dirty state changes
/// </summary>
public class SettingsKeyDirtyEventArgs : EventArgs
{
    public string SettingsKey { get; }
    public bool IsDirty { get; }

    public SettingsKeyDirtyEventArgs(string settingsKey, bool isDirty)
    {
        SettingsKey = settingsKey;
        IsDirty = isDirty;
    }
}
