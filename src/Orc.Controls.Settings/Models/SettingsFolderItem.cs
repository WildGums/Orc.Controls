namespace Orc.Controls.Settings;

using System.Windows.Media;

public class SettingsFolderItem
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public bool CanEdit { get; set; }
    public bool CanRemove { get; set; }
    public string? ResolvedPath { get; set; }
    public Color? Color { get; set; } = Colors.Black;
}
