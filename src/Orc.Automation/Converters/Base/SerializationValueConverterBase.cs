namespace Orc.Automation
{
    using System;

    public abstract class SerializationValueConverterBase<TFrom, TTo> : ISerializationValueConverter<TFrom, TTo>
    {
        public abstract object ConvertFrom(TFrom value);
        public abstract object ConvertTo(TTo value);

        public Type FromType => typeof(TFrom);
        public Type ToType => typeof(TTo);

        public object ConvertFrom(object value)
        {
            return ConvertFrom((TFrom)value);
        }

        public object ConvertTo(object value)
        {
            return ConvertTo((TTo)value);
        }
    }
}
