namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Media;
    using Enums;
    using Orc.Automation;

    [AutomationAccessType]
    public class TimePickerModel : FrameworkElementModel
    {
        public TimePickerModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        #region Dependency properties
        public TimeSpan TimeValue { get; set; }
        public Meridiem AmPmValue { get; set; }
        public Brush HourBrush { get; set; }
        public double HourThickness { get; set; }
        public Brush MinuteBrush { get; set; }
        public double MinuteThickness { get; set; }
        public Brush HourTickBrush { get; set; }
        public double HourTickThickness { get; set; }
        public Brush MinuteTickBrush { get; set; }
        public double MinuteTickThickness { get; set; }
        public double ClockBorderThickness { get; set; }
        public bool ShowNumbers { get; set; }
        public bool Is24HourFormat { get; set; }
        #endregion
    }
}
