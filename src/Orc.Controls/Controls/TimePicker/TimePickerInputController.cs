namespace Orc.Controls
{
    using System;
    using System.Windows;
    using static Orc.Controls.ClockMath;
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
        public TimePickerInputController(TimePicker timePicker)
        {
            _timePicker = timePicker;

            _timePicker.PreviewMouseLeftButtonDown += _timePicker_PreviewMouseLeftButtonDown;
            _timePicker.PreviewMouseMove += _timePicker_PreviewMouseMove;
            _timePicker.MouseLeave += _timePicker_MouseLeave; ;
            _timePicker.PreviewMouseLeftButtonUp += _timePicker_PreviewMouseLeftButtonUp; ;
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

        private void _timePicker_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isDragging)
            {
                var width = _timePicker.ActualWidth;
                var height = _timePicker.ActualHeight;
                var radius = (Math.Min(width, height) - _timePicker.BorderThickness.Left) / 2.0;
                var center = new Point(width / 2.0, height / 2.0);
                var mouse = e.GetPosition(_timePicker);

                var time = _timePicker.TimeValue;

                if (_indicator == Indicator.HourIndicator)
                {
                    var hour = AngleToHour(center, mouse);
                    _timePicker.SetCurrentValue(TimeValueProperty, new TimeSpan(hour, time.Minutes, time.Seconds));
                }

                if (_indicator == Indicator.MinuteIndicator)
                {
                    var minutes = AngleToMinutes(center, mouse);
                    _timePicker.SetCurrentValue(TimeValueProperty, new TimeSpan(time.Hours, minutes, time.Seconds));
                }
            }
        }

        private void FindIndicator(double width, double height, double radius, Point center, Point mouse)
        {
            var minuteTip = LineOnCircle((Math.PI * 2 * _timePicker.TimeValue.Minutes / 60) - Math.PI / 2.0, center, 0, radius * MinuteIndicatorRatio)[1];
            var hourTip = LineOnCircle((Math.PI * 2 * _timePicker.TimeValue.Hours / 12) - Math.PI / 2.0, center, 0, radius * HourIndicatorRatio)[1];

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

        private void _timePicker_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StartDragging(e.GetPosition(_timePicker));
        }

        private void _timePicker_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StopDragging();
        }

        private void _timePicker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
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
