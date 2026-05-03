namespace Orc.Controls.Settings;

using System.Windows.Media;

public class SettingsGroup
{
    public string? Name { get; set; }
    public bool CanEdit { get; set; }
    public bool CanRemove { get; set; }
    public Color? Color { get; set; }

    protected bool Equals(SettingsGroup other) => Name == other.Name;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((SettingsGroup)obj);
    }

    public override int GetHashCode() => (Name is not null ? Name.GetHashCode() : 0);
}
