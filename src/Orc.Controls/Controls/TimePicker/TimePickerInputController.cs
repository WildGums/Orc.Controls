namespace Orc.Controls
{
    using System;
    using System.Windows;
    using static ClockMath;
    using static TimePicker;
    internal class TimePickerInputController
    {
        #region Fields
        // TimePicker.ActualHeight * MinDistanceRatio is the max
        // distance away from the tip of the indicator you can 
        // click to still start dragging it
        private const double MinDistanceRatio = 0.2;

        private readonly TimePicker _timePicker;

        private Indicator _indicator;
        private bool _isDragging;
        #endregion

        #region Constructors
        public TimePickerInputController(TimePicker timePicker)
        {
            _timePicker = timePicker;

            _timePicker.PreviewMouseLeftButtonDown += OnTimePickerPreviewMouseLeftButtonDown;
            _timePicker.PreviewMouseMove += OnTimePickerPreviewMouseMove;
            _timePicker.MouseLeave += OnTimePickerMouseLeave;
            _timePicker.PreviewMouseLeftButtonUp += OnTimePickerPreviewMouseLeftButtonUp;
        }
        #endregion

        #region Enums
        private enum Indicator
        {
            None,
            HourIndicator,
            MinuteIndicator
        }
        #endregion
        
        #region Methods
        private void StartDragging(Point mouse)
        {
            var width = _timePicker.ActualWidth;
            var height = _timePicker.ActualHeight;
            var radius = (Math.Min(width, height) - _timePicker.ClockBorderThickness) / 2.0;
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

        private void OnTimePickerPreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_isDragging)
            {
                return;
            }

            var width = _timePicker.ActualWidth;
            var height = _timePicker.ActualHeight;
            var center = new Point(width / 2.0, height / 2.0);
            var mouse = e.GetPosition(_timePicker);

            var time = _timePicker.TimeValue;

            switch (_indicator)
            {
                case Indicator.HourIndicator:
                {
                    var hour = AngleToHour(center, mouse);
                    _timePicker.SetCurrentValue(TimeValueProperty, new TimeSpan(hour, time.Minutes, time.Seconds));
                    break;
                }

                case Indicator.MinuteIndicator:
                {
                    var minutes = AngleToMinutes(center, mouse);
                    _timePicker.SetCurrentValue(TimeValueProperty, new TimeSpan(time.Hours, minutes, time.Seconds));
                    break;
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

            if (minuteDistance < hourDistance)
            {
                if (minuteDistance < maxDistance)
                {
                    _indicator = Indicator.MinuteIndicator;
                }
            }
            else
            { 
                if (hourDistance < maxDistance)
                {
                    _indicator = Indicator.HourIndicator;
                } 
            }
        }

        private void OnTimePickerPreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StartDragging(e.GetPosition(_timePicker));
        }

        private void OnTimePickerPreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StopDragging();
        }

        private void OnTimePickerMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StopDragging();
        }
        #endregion
    }
}
