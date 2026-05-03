namespace Orc.Controls.Settings;

using System;

public interface ISettingsKeyTypeSerializer
{
    string Serialize(object? value);
    object? Deserialize(string value, Type targetType);
}