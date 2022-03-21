namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using System.Windows.Input;
    using Catel;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class TimeSpanPicker : FrameworkElement<TimeSpanPickerModel, TimeSpanPickerMap>
    {
        private const string DaysTimePart = "Days";
        private const string HoursTimePart = "Hours";
        private const string MinutesTimePart = "Minutes";
        private const string SecondsTimePart = "Seconds";

        public TimeSpanPicker(AutomationElement element)
            : base(element)
        {
        }

        public double? EditorValue
        {
            get => Map.EditorNumericTextBox?.Current.Value;
        }

        public double? TotalDays
        {
            get => GetEditorValue(DaysTimePart);
            set => SetEditorValue(DaysTimePart, value);
        }

        public double? TotalHours
        {
            get => GetEditorValue(HoursTimePart);
            set => SetEditorValue(HoursTimePart, value);
        }

        public double? TotalMinutes
        {
            get => GetEditorValue(MinutesTimePart);
            set => SetEditorValue(MinutesTimePart, value);
        }

        public double? TotalSeconds
        {
            get => GetEditorValue(SecondsTimePart);
            set => SetEditorValue(SecondsTimePart, value);
        }

        public TimeSpan? Value
        {
            get
            {
                var map = Map;

                var day = map.DaysNumericTextBox?.Current.Value;
                var hours = map.HoursNumericTextBox?.Current.Value;
                var minutes = map.MinutesNumericTextBox?.Current.Value;
                var seconds = map.SecondsNumericTextBox?.Current.Value;

                if (day is null || hours is null || minutes is null || seconds is null)
                {
                    return null;
                }

                return new TimeSpan((int)day.Value, (int)hours.Value, (int)minutes.Value, (int)seconds.Value);
            }

            set
            {
                var map = Map;

                var daysNumericTextBox = map.DaysNumericTextBox;
                var hoursTextBox = map.HoursNumericTextBox;
                var minutesTextBox = map.MinutesNumericTextBox;
                var secondsTextBox = map.SecondsNumericTextBox;

                if (daysNumericTextBox is null || hoursTextBox is null || minutesTextBox is null || secondsTextBox is null)
                {
                    return;
                }

                daysNumericTextBox.Current.Value = value?.Days;
                hoursTextBox.Current.Value = value?.Hours;
                minutesTextBox.Current.Value = value?.Minutes;
                secondsTextBox.Current.Value = value?.Seconds;
            }
        }

        private double? GetEditorValue(string timePart)
        {
            using (StartEditorScope(timePart))
            {
                return Map.EditorNumericTextBox?.Current.Value;
            }
        }

        private void SetEditorValue(string timePart, double? value)
        {
            using (StartEditorScope(timePart))
            {
                Map.EditorNumericTextBox.Current.Value = value;
            }
        }

        private IDisposable StartEditorScope(string timePart)
        {
            var textBlock = GetAbbreviationSeparator(timePart);
            if (textBlock is null)
            {
                return null;
            }

            return new DisposableToken(null,
                x =>
                {
                    Map.DaysNumericTextBox.MouseHover();
                    Wait.UntilResponsive();

                    textBlock.MouseClick();
                    textBlock.MouseClick();
                },

                x => KeyboardInput.PressRelease(Key.Enter));
        }

        private Text GetAbbreviationSeparator(string timePart)
        {
            if (timePart == DaysTimePart)
            {
                return Map.DaysAbbreviationTextBlock;
            }

            if (timePart == HoursTimePart)
            {
                return Map.HoursAbbreviationTextBlock;
            }

            if (timePart == MinutesTimePart)
            {
                return Map.MinutesAbbreviationTextBlock;
            }

            if (timePart == SecondsTimePart)
            {
                return Map.SecondsAbbreviationTextBlock;
            }

            return null;
        }
    }
}
