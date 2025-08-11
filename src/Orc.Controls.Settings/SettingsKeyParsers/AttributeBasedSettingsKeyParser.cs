namespace Orc.Controls.Settings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

public class AttributeBasedSettingsKeyParser<T> : ISettingsKeyParser<T> where T : class, new()
{
    private static readonly Regex KeyValueRegex = new(@"\[(\w+)\s*=\s*([^\]]+)\]", RegexOptions.Compiled);
    private readonly Dictionary<Type, ISettingsKeyTypeSerializer> _serializerCache = new();

    public string ToString(T keyInfo)
    {
        ArgumentNullException.ThrowIfNull(keyInfo);
        var properties = GetSettingsProperties();
        var keyParts = new List<string>();
        foreach (var (property, attribute) in properties)
        {
            if (attribute?.Ignore == true)
            {
                continue;
            }

            var value = property.GetValue(keyInfo);
            if (value is null)
            {
                continue;
            }

            var displayName = attribute?.DisplayName ?? property.Name;
            var serializedValue = SerializeValue(value, property.PropertyType, attribute?.SerializerType);
            keyParts.Add($"[{displayName} = {serializedValue}]");
        }

        return string.Join(string.Empty, keyParts);
    }

    public T Parse(string settingsKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(settingsKey);

        var result = new T();
        var properties = GetSettingsProperties().ToDictionary(
            pair => pair.attribute?.DisplayName ?? pair.property.Name,
            pair => pair,
            StringComparer.OrdinalIgnoreCase);

        var matches = KeyValueRegex.Matches(settingsKey);

        foreach (Match match in matches)
        {
            var key = match.Groups[1].Value.Trim();
            var value = match.Groups[2].Value.Trim();

            if (!properties.TryGetValue(key, out var propertyInfo))
            {
                continue;
            }

            var (property, attribute) = propertyInfo;
            if (attribute?.Ignore == true)
            {
                continue;
            }

            var deserializedValue = DeserializeValue(value, property.PropertyType, attribute?.SerializerType);
            property.SetValue(result, deserializedValue);
        }

        return result;
    }

    private List<(PropertyInfo property, SettingsKeyPropertyAttribute? attribute)> GetSettingsProperties()
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite)
            .Select(p => (p, p.GetCustomAttribute<SettingsKeyPropertyAttribute>()))
            .ToList();
    }

    private string SerializeValue(object value, Type propertyType, Type? serializerType)
    {
        var serializer = GetSerializer(propertyType, serializerType);
        return serializer?.Serialize(value) ?? value?.ToString() ?? string.Empty;
    }

    private object? DeserializeValue(string value, Type propertyType, Type? serializerType)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var serializer = GetSerializer(propertyType, serializerType);
        return serializer?.Deserialize(value, propertyType) ?? Convert.ChangeType(value, propertyType);
    }

    private ISettingsKeyTypeSerializer? GetSerializer(Type propertyType, Type? serializerType)
    {
        // Check for explicit serializer first
        if (serializerType is not null)
        {
            return GetOrCreateSerializer(serializerType);
        }

        // Default serializers for common types
        if (propertyType.IsEnum)
        {
            return GetOrCreateSerializer(typeof(EnumSettingsSerializer));
        }

        return null;
    }

    private ISettingsKeyTypeSerializer GetOrCreateSerializer(Type serializerType)
    {
        if (!_serializerCache.TryGetValue(serializerType, out var serializer))
        {
            serializer = (ISettingsKeyTypeSerializer)Activator.CreateInstance(serializerType)!;
            _serializerCache[serializerType] = serializer;
        }

        return serializer;
    }
}
