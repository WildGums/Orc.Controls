#nullable disable
namespace Orc.Controls.Automation.Converters;

using System;
using Orc.Automation;
using Controls;
using Orc.Automation.Converters;

public class SerializableNumber
{
    public double DValue { get; set; }
    public SerializableType Type { get; set; }
}

public class NumberConverter : SerializationValueConverterBase<Number, SerializableNumber>
{
    private readonly TypeSerializationConverter _typeSerializationConverter = new ();

    public override object ConvertFrom(Number value)
    { 
        return new SerializableNumber
        {
            Type = _typeSerializationConverter.ConvertFrom(value.Type) as SerializableType,
            DValue = value.DValue
        };
    }

    public override object ConvertTo(SerializableNumber value)
    {
        var type = _typeSerializationConverter.ConvertTo(value.Type) as Type;

        return new Number(value.DValue, type);
    }
}
#nullable enable
