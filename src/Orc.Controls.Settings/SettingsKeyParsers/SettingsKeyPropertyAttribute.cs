namespace Orc.Controls.Settings;

using System;

// Attribute for property-level serialization control
[AttributeUsage(AttributeTargets.Property)]
public class SettingsKeyPropertyAttribute : Attribute
{
    public SettingsKeyPropertyAttribute(string? displayName = null) => DisplayName = displayName;

    public SettingsKeyPropertyAttribute(Type serializerType)
    {
        if (!typeof(ISettingsKeyTypeSerializer).IsAssignableFrom(serializerType))
        {
            throw new ArgumentException($"Type must implement {nameof(ISettingsKeyTypeSerializer)}",
                nameof(serializerType));
        }

        SerializerType = serializerType;
    }

    public SettingsKeyPropertyAttribute(string displayName, Type serializerType) : this(displayName)
    {
        if (!typeof(ISettingsKeyTypeSerializer).IsAssignableFrom(serializerType))
        {
            throw new ArgumentException($"Type must implement {nameof(ISettingsKeyTypeSerializer)}",
                nameof(serializerType));
        }

        SerializerType = serializerType;
    }

    public string? DisplayName { get; set; }
    public bool Ignore { get; set; }
    public Type? SerializerType { get; set; }
}

// Interface for custom type serializers

// Default serializers for common scenarios

// Generic parser implementation
