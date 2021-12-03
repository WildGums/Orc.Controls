namespace Orc.Automation
{
    using System.Windows.Media;

    public class SolidColorBrushSerializationConverter : SerializationValueConverterBase<SolidColorBrush, SerializableColor>
    {
        private readonly ColorSerializationConverter _colorSerializationConverter;

        public SolidColorBrushSerializationConverter()
        {
            _colorSerializationConverter = new ColorSerializationConverter();
        }

        public override object ConvertFrom(SolidColorBrush value)
        {
            return _colorSerializationConverter.ConvertFrom(value.Color);
        }

        public override object ConvertTo(SerializableColor value)
        {
            var color = (Color) _colorSerializationConverter.ConvertTo(value);
            return new SolidColorBrush(color);
        }
    }
}
