namespace Orc.Controls.Settings;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class TypeConverter : JsonConverter<Type>
{
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeName = reader.GetString();
        if (string.IsNullOrEmpty(typeName))
        {
            return null;
        }

        try
        {
            return Type.GetType(typeName);
        }
        catch
        {
            return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.AssemblyQualifiedName);
    }
}
