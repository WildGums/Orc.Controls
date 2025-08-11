namespace Orc.Controls.Settings;

using System;
using System.Collections.Generic;
using System.Linq;

public class EnabledKeyEventArgs : EventArgs
{
    private readonly HashSet<string> _enabledKeys = [];

    public string[] EnabledKeys => _enabledKeys.ToArray();

    public void Add(string enabledKey)
    {
        _enabledKeys.Add(enabledKey);
    }
}
