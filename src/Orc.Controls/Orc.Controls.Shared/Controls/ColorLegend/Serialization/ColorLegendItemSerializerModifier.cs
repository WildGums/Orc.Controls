// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorLegendItemSerializerModifier.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows.Media;
    using Catel.Logging;
    using Catel.Runtime.Serialization;

    public class ColorLegendItemSerializerModifier : SerializerModifierBase<ColorLegendItem>
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public override void SerializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals("Color", memberValue.Name))
            {
                var color = (Color) memberValue.Value;
                memberValue.Value = string.Format("{0};{1};{2};{3}", color.A, color.R, color.G, color.B);
            }

            base.SerializeMember(context, memberValue);
        }

        public override void DeserializeMember(ISerializationContext context, MemberValue memberValue)
        {
            if (string.Equals("Color", memberValue.Name))
            {
                var color = memberValue.Value as string;

                try
                {
                    if (!string.IsNullOrEmpty(color))
                    {
                        var colorParts = color.Split(';');

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