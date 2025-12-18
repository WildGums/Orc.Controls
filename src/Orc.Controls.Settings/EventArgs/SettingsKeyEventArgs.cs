namespace Orc.Controls.Settings;

using System;

/// <summary>
/// Event arguments for settings key operations with success tracking
/// </summary>
public class SettingsKeyEventArgs : EventArgs
{
    public string SettingsKey { get; }
    public bool Success { get; set; } = false;

    public SettingsKeyEventArgs(string settingsKey)
    {
        SettingsKey = settingsKey;
    }
}
