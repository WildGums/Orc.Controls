namespace Orc.Controls.Controls.TimePicker
{
    using System;
    using System.Windows;
    using static Orc.Controls.TimePicker.ClockMath;
    using static Orc.Controls.TimePicker;
    public class TimePickerInputController
    {
        private readonly TimePicker _timePicker;

        // TimePicker.ActualHeight * MinDistanceRatio is the max
        // distance away from the tip of the indicator you can 
        // click to still start dragging it
        private const double MinDistanceRatio = 0.2;

        private Indicator _indicator;
        private bool _isDragging;
        private Orc.Controls.TimePicker _timePicker1;

        public TimePickerInputController(TimePicker timePicker)
        {
            _timePicker = timePicker;

            _timePicker.PreviewMouseLeftButtonDown += TimePicker_MouseLeftButtonDown;
            _timePicker.PreviewMouseMove += TimePicker_MouseMove;
            _timePicker.MouseLeave += TimePicker_MouseLeave;
            _timePicker.PreviewMouseLeftButtonUp += TimePicker_MouseLeftButtonUp;
        }

        public TimePickerInputController(Orc.Controls.TimePicker timePicker1)
        {
            _timePicker1 = timePicker1;
        }

        private void StartDragging(Point mouse)
        {
            var width = _timePicker.ActualWidth;
            var height = _timePicker.ActualHeight;
            var radius = (Math.Min(width, height) - _timePicker.BorderThickness.Left) / 2.0;
            var center = new Point(width / 2.0, height / 2.0);

            // TODO: highlight indicator that you're dragging
            FindIndicator(width, height, radius, center, mouse);

            _isDragging = true;
        }

        private void StopDragging()
        {
            _indicator = Indicator.None;
            _isDragging = false;
        }

        private void TimePicker_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isDragging)
            {
                var width = _timePicker.ActualWidth;
                var height = _timePicker.ActualHeight;
                var radius = (Math.Min(width, height) - _timePicker.BorderThickness.Left) / 2.0;
                var center = new Point(width / 2.0, height / 2.0);
                var mouse = e.GetPosition(_timePicker);

                var time = _timePicker.Time;

                if (_indicator == Indicator.HourIndicator)
                {
                    var hour = AngleToHour(center, mouse);
                    _timePicker.SetCurrentValue(TimeProperty, new AnalogueTime(hour, time.Minute, time.Meridiem));
                }

                if (_indicator == Indicator.MinuteIndicator)
                {
                    var minutes = AngleToMinutes(center, mouse);
                    _timePicker.SetCurrentValue(TimeProperty, new AnalogueTime(time.Hour, minutes, time.Meridiem));
                }
            }
        }

        private void FindIndicator(double width, double height, double radius, Point center, Point mouse)
        {
            var minuteTip = LineOnCircle((Math.PI * 2 * _timePicker.Time.Minute / 60) - Math.PI / 2.0, center, 0, radius * MinuteIndicatorRatio)[1];
            var hourTip = LineOnCircle((Math.PI * 2 * _timePicker.Time.Hour / 12) - Math.PI / 2.0, center, 0, radius * HourIndicatorRatio)[1];

            var maxDistance = width * MinDistanceRatio;

            var minuteDistance = Distance(mouse, minuteTip);
            var hourDistance = Distance(mouse, hourTip);

            if (minuteDistance < hourDistance && minuteDistance < maxDistance)
            {
                _indicator = Indicator.MinuteIndicator;
            }
            else if (hourDistance <= minuteDistance && hourDistance < maxDistance)
            {
                _indicator = Indicator.HourIndicator;
            }
        }

        private void TimePicker_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StartDragging(e.GetPosition(_timePicker));
        }

        private void TimePicker_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StopDragging();
        }

        private void TimePicker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StopDragging();
        }

        private enum Indicator
        {
            None,
            HourIndicator,
            MinuteIndicator
        }

    }
}
