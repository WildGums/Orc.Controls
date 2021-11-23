namespace Orc.Automation.Converters
{
    using System.Windows;

    public class SerializableRect
    {
        public Rect Rect { get; set; }
    }

    public class RectSerializationConverter : SerializationValueConverterBase<Rect, SerializableRect>
    {
        public override object ConvertFrom(Rect value)
        {
            return new SerializableRect
            {
                Rect = value
            };
        }

        public override object ConvertTo(SerializableRect value)
        {
            return value.Rect;
        }
    }
}
