namespace Orc.Automation.Converters
{
    using System.Windows;

    public class SerializablePoint
    {
        public Point Point { get; set; }
    }

    public class PointSerializationConverter : SerializationValueConverterBase<Point, SerializablePoint>
    {
        public override object ConvertFrom(Point value)
        {
            return new SerializablePoint
            {
                Point = value
            };
        }

        public override object ConvertTo(SerializablePoint value)
        {
            return value.Point;
        }
    }
}
