namespace Orc.Controls.Settings;

using System.Collections.Generic;

public class SettingsFoldersCollection
{
    public List<SettingsFolderItem> SettingsFolders { get; set; } = [];
    public bool CanAddFolder { get; set; } = true;
}
