namespace Orc.Controls.Settings;

using System;

/// <summary>
/// Event arguments for settings key rename operations with success tracking
/// </summary>
public class SettingsKeyRenameEventArgs : EventArgs
{
    public string OldKey { get; }
    public string NewKey { get; }
    public bool Success { get; set; } = false;

    public SettingsKeyRenameEventArgs(string oldKey, string newKey)
    {
        OldKey = oldKey;
        NewKey = newKey;
    }
}
