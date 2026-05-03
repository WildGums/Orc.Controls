namespace Orc.Controls.Settings;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Media;

public class ColorConverter : JsonConverter<Color?>
{
    private static readonly System.Windows.Media.ColorConverter _wpfColorConverter = new();

    public override Color? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var colorString = reader.GetString();
        if (string.IsNullOrEmpty(colorString))
        {
            return null;
        }

        try
        {
            // Handle hex format (#AARRGGBB or #RRGGBB)
            if (colorString.StartsWith("#"))
            {
                return (Color?)((System.ComponentModel.TypeConverter)_wpfColorConverter).ConvertFromString(colorString);
            }

            // Handle named colors
            var color = (Color?)((System.ComponentModel.TypeConverter)_wpfColorConverter).ConvertFromString(colorString);
            return color;
        }
        catch
        {
            return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, Color? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var color = value.Value;
        // Write as hex string with alpha channel
        var hexString = _wpfColorConverter.ConvertToString(color); /*$"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}"*/;
        writer.WriteStringValue(hexString);
    }
}
