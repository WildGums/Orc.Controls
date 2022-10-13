namespace Orc.Controls.Tools.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ToolSettingsAttribute : Attribute
    {
        public ToolSettingsAttribute()
        {
            Storage = string.Empty;
        }

        public string Storage { get; set; }
    }
}
