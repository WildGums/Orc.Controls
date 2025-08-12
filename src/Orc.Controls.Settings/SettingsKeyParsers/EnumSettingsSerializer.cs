namespace Orc.Controls.Settings;

using System;

public class EnumSettingsSerializer : ISettingsKeyTypeSerializer
{
    public string Serialize(object? value) => value?.ToString() ?? string.Empty;

    public object? Deserialize(string value, Type targetType)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return Enum.Parse(targetType, value, true);
    }
}