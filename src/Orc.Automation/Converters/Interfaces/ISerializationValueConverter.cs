namespace Orc.Automation
{
    using System;
    using System.Windows.Media;
    using Catel.Logging;
    using Converters;

    public interface ISerializationValueConverter
    {
        Type FromType { get; }
        Type ToType { get; }

        object ConvertFrom(object value);
        object ConvertTo(object value);
    }

    public interface ISerializationValueConverter<in TFrom, in TTo> : ISerializationValueConverter
    {
        object ConvertFrom(TFrom value);
        object ConvertTo(TTo value);
    }
}
