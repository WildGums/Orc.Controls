namespace Orc.Controls
{
    using System;
    using System.Windows.Media;
    using Catel.Logging;
    using Catel.Runtime.Serialization;

    public class ColorLegendItemSerializerModifier : SerializerModifierBase<ColorLegendItem>
    {
        private const char ArgbSeparator = ';';

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public override void SerializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(nameof(ColorLegendItem.Color), memberValue.Name))
            {
                var color = (Color?)memberValue.Value;
                if (color is not null)
                {
                    memberValue.Value = $"{color.Value.A}{ArgbSeparator}{color.Value.R}{ArgbSeparator}{color.Value.G}{ArgbSeparator}{color.Value.B}";
                }
            }

            base.SerializeMember(context, memberValue);
        }

        public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals(nameof(ColorLegendItem.Color), memberValue.Name))
            {
                var color = memberValue.Value as string;

                try
                {
                    if (!string.IsNullOrEmpty(color))
                    {
                        var colorParts = color.Split(ArgbSeparator);

                        memberValue.Value = Color.FromArgb(Convert.ToByte(colorParts[0]), Convert.ToByte(colorParts[1]),
                            Convert.ToByte(colorParts[2]), Convert.ToByte(colorParts[3]));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Cannot modify '{0}' to a color", color);
                }
            }

            base.DeserializeMember(context, memberValue);
        }
    }
}
