namespace Orc.Controls
{
    using System;
    using System.Windows;
    using static Orc.Controls.ClockMath;
    using static Orc.Controls.ClockFace;
    public class ClockFaceInputController
    {
        private readonly ClockFace _clockFace;

        // TimePicker.ActualHeight * MinDistanceRatio is the max
        // distance away from the tip of the indicator you can 
        // click to still start dragging it
        private const double MinDistanceRatio = 0.2;

        private Indicator _indicator;
        private bool _isDragging;
        public ClockFaceInputController(ClockFace clockFace)
        {
            _clockFace = clockFace;

            _clockFace.PreviewMouseLeftButtonDown += _clockFace_PreviewMouseLeftButtonDown;
            _clockFace.PreviewMouseMove += _clockFace_PreviewMouseMove;
            _clockFace.MouseLeave += _clockFace_MouseLeave; ;
            _clockFace.PreviewMouseLeftButtonUp += _clockFace_PreviewMouseLeftButtonUp; ;
        }


        private void StartDragging(Point mouse)
        {
            var width = _clockFace.ActualWidth;
            var height = _clockFace.ActualHeight;
            var radius = (Math.Min(width, height) - _clockFace.BorderThickness.Left) / 2.0;
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

        private void _clockFace_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isDragging)
            {
                var width = _clockFace.ActualWidth;
                var height = _clockFace.ActualHeight;
                var radius = (Math.Min(width, height) - _clockFace.BorderThickness.Left) / 2.0;
                var center = new Point(width / 2.0, height / 2.0);
                var mouse = e.GetPosition(_clockFace);

                var time = _clockFace.TimeValue;

                if (_indicator == Indicator.HourIndicator)
                {
                    var hour = AngleToHour(center, mouse);
                    _clockFace.SetCurrentValue(TimeValueProperty, new TimeSpan(hour, time.Minutes, time.Seconds));
                }

                if (_indicator == Indicator.MinuteIndicator)
                {
                    var minutes = AngleToMinutes(center, mouse);
                    _clockFace.SetCurrentValue(TimeValueProperty, new TimeSpan(time.Hours, minutes, time.Seconds));
                }
            }
        }

        private void FindIndicator(double width, double height, double radius, Point center, Point mouse)
        {
            var minuteTip = LineOnCircle((Math.PI * 2 * _clockFace.TimeValue.Minutes / 60) - Math.PI / 2.0, center, 0, radius * MinuteIndicatorRatio)[1];
            var hourTip = LineOnCircle((Math.PI * 2 * _clockFace.TimeValue.Hours / 12) - Math.PI / 2.0, center, 0, radius * HourIndicatorRatio)[1];

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

        private void _clockFace_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StartDragging(e.GetPosition(_clockFace));
        }

        private void _clockFace_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StopDragging();
        }

        private void _clockFace_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
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
