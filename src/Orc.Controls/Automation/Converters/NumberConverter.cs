﻿namespace Orc.Controls.Automation.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Orc.Automation;
    using Orc.Controls;

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
}
