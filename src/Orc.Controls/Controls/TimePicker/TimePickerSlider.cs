namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    public class TimePickerSlider : Control
    {
        static TimePickerSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePickerSlider),
                new FrameworkPropertyMetadata(typeof(TimePickerSlider)));
        }

        public DigitalTime Time
        {
            get { return (DigitalTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register(nameof(Time), typeof(DigitalTime), typeof(TimePickerSlider), new PropertyMetadata(new DigitalTime(12, 0), new PropertyChangedCallback(TimeChanged)));

        public DigitalTime MinTime
        {
            get { return (DigitalTime)GetValue(MinTimeProperty); }
            set { SetValue(MinTimeProperty, value); }
        }

        public static readonly DependencyProperty MinTimeProperty =
            DependencyProperty.Register(nameof(MinTime), typeof(DigitalTime), typeof(TimePickerSlider), new PropertyMetadata(new DigitalTime(9, 0), new PropertyChangedCallback(TimeChanged)));

        public DigitalTime MaxTime
        {
            get { return (DigitalTime)GetValue(MaxTimeProperty); }
            set { SetValue(MaxTimeProperty, value); }
        }

        public static readonly DependencyProperty MaxTimeProperty =
            DependencyProperty.Register(nameof(MaxTime), typeof(DigitalTime), typeof(TimePickerSlider), new PropertyMetadata(new DigitalTime(21, 0), new PropertyChangedCallback(TimeChanged)));


        private static void TimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var slider = d as TimePickerSlider;
            if (slider != null)
            {

                if (slider.MaxTime < slider.MinTime)
                {
                    slider.MaxTime = slider.MinTime;
                }

                if (slider.Time > slider.MaxTime)
                {
                    slider.Time = slider.MaxTime;
                }

                if (slider.Time < slider.MinTime)
                {
                    slider.Time = slider.MinTime;
                }
            }
        }
    }
}
