namespace Orc.Controls.Settings;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

public class ControlRequestedEventArgs : EventArgs
{
    private const string SearchPattern = @"\{[^}]*\}[^/]*";

    public ControlRequestedEventArgs(string settingsKey) => SettingsKey = settingsKey;

    public string SettingsKey { get; }
    public List<ISettingsElement> Elements { get; private set; } = [];
}
