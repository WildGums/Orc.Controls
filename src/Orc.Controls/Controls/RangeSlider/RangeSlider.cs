namespace Orc.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using Catel.Windows;
    using Catel.Windows.Threading;

    [TemplatePart(Name = "PART_TrackBackground", Type = typeof(Border))]
    [TemplatePart(Name = "PART_SelectedRange", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_LowerSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_UpperSlider", Type = typeof(Slider))]
    public class RangeSlider : RangeBase
    {
        private static readonly Point LeftTop = new Point(0, 0);

        private readonly DispatcherTimer _dispatcherTimer;

        private bool _ignoreSliderValueChanging;
        private Border _trackBackgroundBorder;
        private Rectangle _selectedRangeRectangle;
        private Slider _lowerSlider;
        private Slider _upperSlider;
        private Track _lowerTrack;
        private Track _upperTrack;
        private Thumb _lowerThumb;
        private Thumb _upperThumb;

        static RangeSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(typeof(RangeSlider)));
        }

        public RangeSlider()
        {
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Input)
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };

            _dispatcherTimer.Tick += OnDispatcherTimerTick;
        }

        [Category("Behavior"), Bindable(true)]
        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        public static readonly DependencyProperty LowerValueProperty = DependencyProperty.Register(nameof(LowerValue), typeof(double),
            typeof(RangeSlider), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((RangeSlider)sender).OnLowerValueChanged(e)));


        [Category("Behavior"), Bindable(true)]
        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }

        public static readonly DependencyProperty UpperValueProperty = DependencyProperty.Register(nameof(UpperValue), typeof(double),
            typeof(RangeSlider), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((RangeSlider)sender).OnUpperValueChanged(e)));


        [Category("Behavior"), Bindable(true)]
        public bool HighlightSelectedRange
        {
            get { return (bool)GetValue(HighlightSelectedRangeProperty); }
            set { SetValue(HighlightSelectedRangeProperty, value); }
        }

        public static readonly DependencyProperty HighlightSelectedRangeProperty = DependencyProperty.Register(nameof(HighlightSelectedRange),
            typeof(bool), typeof(RangeSlider), new PropertyMetadata(true));


        [Category("Behavior"), Bindable(true)]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation),
            typeof(RangeSlider), new PropertyMetadata(Orientation.Horizontal, (sender, e) => ((RangeSlider)sender).OnOrientationChanged()));


        [Category("Behavior")]
        public event RoutedPropertyChangedEventHandler<double> LowerValueChanged
        {
            add => AddHandler(LowerValueChangedEvent, (value));
            remove => RemoveHandler(LowerValueChangedEvent, (value));
        }

        public static readonly RoutedEvent LowerValueChangedEvent = EventManager.RegisterRoutedEvent(nameof(LowerValueChanged), 
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(RangeSlider));


        [Category("Behavior")]
        public event RoutedPropertyChangedEventHandler<double> UpperValueChanged
        {
            add => AddHandler(UpperValueChangedEvent, value);
            remove => RemoveHandler(UpperValueChangedEvent, value);
        }

        public static readonly RoutedEvent UpperValueChangedEvent = EventManager.RegisterRoutedEvent(nameof(UpperValueChanged), 
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(RangeSlider));

        private void OnOrientationChanged()
        {
            UpdateState();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            var propertyName = e.Property.Name;
            if (propertyName == nameof(Minimum))
            {
                if (Minimum > LowerValue)
                {
                    LowerValue = Minimum;
                }
                else
                {
                    StartUpdate();
                }
            }

            if (propertyName != nameof(Maximum))
            {
                return;
            }

            if (Maximum < UpperValue)
            {
                UpperValue = Maximum;
            }
            else
            {
                StartUpdate();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _lowerSlider = GetTemplateChild("PART_LowerSlider") as Slider;
            if (_lowerSlider != null)
            {
                _lowerSlider.ValueChanged += OnSliderValueChanged;
            }

            _upperSlider = GetTemplateChild("PART_UpperSlider") as Slider;
            if (_upperSlider != null)
            {
                _upperSlider.ValueChanged += OnSliderValueChanged;
            }

            _trackBackgroundBorder = GetTemplateChild("PART_TrackBackground") as Border;
            _selectedRangeRectangle = GetTemplateChild("PART_SelectedRange") as Rectangle;

            Dispatcher.BeginInvoke(UpdateState, false);
        }

        private void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_ignoreSliderValueChanging)
            {
                return;
            }

            try
            {
                _ignoreSliderValueChanging = true;

                if (ReferenceEquals(sender, _lowerSlider))
                {
                    _upperSlider.Value = Math.Max(_upperSlider.Value, _lowerSlider.Value);
                }
                else if (ReferenceEquals(sender, _upperSlider))
                {
                    _lowerSlider.Value = Math.Min(_upperSlider.Value, _lowerSlider.Value);
                }
            }
            finally
            {
                _ignoreSliderValueChanging = false;
            }

            // In case the thumbs already moved
            StartUpdate(false);

            // In case the thumbs not moved yet
            StartUpdate();
        }

        private void OnUpperSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _lowerSlider.Value = Math.Min(_upperSlider.Value, _lowerSlider.Value);

            StartUpdate(false);
        }

        private void OnLowerValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var changedEventArgs = new RoutedPropertyChangedEventArgs<double>((double)e.OldValue, (double)e.NewValue)
            {
                RoutedEvent = LowerValueChangedEvent
            };

            RaiseEvent(changedEventArgs);
        }
        private void OnUpperValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var changedEventArgs = new RoutedPropertyChangedEventArgs<double>((double)e.OldValue, (double)e.NewValue)
            {
                RoutedEvent = UpperValueChangedEvent
            };

            RaiseEvent(changedEventArgs);
        }

        private void UpdateState()
        {
            if (!IsLoaded)
            {
                return;
            }

            _lowerSlider?.ApplyTemplate();
            _lowerTrack = _lowerSlider?.Template?.FindName("PART_Track", _lowerSlider) as Track;
            _lowerThumb = _lowerTrack?.FindVisualDescendantByType<Thumb>();

            _upperSlider?.ApplyTemplate();
            _upperTrack = _upperSlider?.Template?.FindName("PART_Track", _upperSlider) as Track;
            _upperThumb = _upperTrack?.FindVisualDescendantByType<Thumb>();

            StartUpdate();
        }

        private void StartUpdate(bool dispatch = true)
        {
            if (!dispatch)
            {
                UpdateRelatedValues();
                return;
            }

            // We need to delay a bit in order to allow values to propagate
            _dispatcherTimer.Stop();
            _dispatcherTimer.Start();
        }

        private void OnDispatcherTimerTick(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();

            UpdateRelatedValues();
        }

        private void UpdateRelatedValues()
        {
            if (!IsLoaded)
            {
                return;
            }

            if (_lowerSlider == null || _upperSlider == null)
            {
                return;
            }

            // As a bonus, the value will show the average
            Value = (_lowerSlider.Value + _upperSlider.Value) / 2;

            var lowerThumb = _lowerThumb;
            var upperThumb = _upperThumb;
            var trackBackgroundBorder = _trackBackgroundBorder;
            var selectedRangeRectangle = _selectedRangeRectangle;

            if (lowerThumb == null || upperThumb == null || trackBackgroundBorder == null || selectedRangeRectangle == null)
            {
                return;
            }

            // When unloaded, item becomes IsVisible = false; If this is the case, we can't call PointToScreen,
            // it will throw an exception
            if (!lowerThumb.IsVisible || !upperThumb.IsVisible)
            {
                return;
            }

            var lowerThumbPosition = lowerThumb.PointToScreen(LeftTop);
            var upperThumbPosition = upperThumb.PointToScreen(LeftTop);
            var leftTop = PointToScreen(LeftTop);

            switch (Orientation)
            {
                case Orientation.Horizontal:
                    // Draw left => right
                    var lowerThumbCenterX = lowerThumb.Width / 2;
                    var upperThumbCenterX = upperThumb.Width / 2;
                    var containerWidth = trackBackgroundBorder.ActualWidth;
                    var width = ActualWidth;
                    var widthRatio = (containerWidth * 100) / width;
                    var left = (lowerThumbPosition.X - leftTop.X) + lowerThumbCenterX;
                    var finalLeft = (left / 100) * widthRatio;

                    var right = (upperThumbPosition.X - leftTop.X) + upperThumbCenterX;

                    selectedRangeRectangle.Height = double.NaN;
                    selectedRangeRectangle.Width = right - left;

                    Canvas.SetTop(selectedRangeRectangle, double.NaN);
                    Canvas.SetLeft(selectedRangeRectangle, finalLeft);

                    break;

                case Orientation.Vertical:
                    // Draw bottom => top
                    var lowerThumbCenterY = lowerThumb.Height / 2;
                    var upperThumbCenterY = upperThumb.Height / 2;
                    var containerHeight = trackBackgroundBorder.ActualHeight;
                    var height = ActualHeight;
                    var heightRatio = (containerHeight * 100) / height;
                    var bottom = (lowerThumbPosition.Y - leftTop.Y) + lowerThumbCenterY;
                    var top = (upperThumbPosition.Y - leftTop.Y) + upperThumbCenterY;
                    var finalTop = (top / 100) * heightRatio;

                    selectedRangeRectangle.Width = double.NaN;
                    selectedRangeRectangle.Height = bottom - top;

                    Canvas.SetLeft(selectedRangeRectangle, double.NaN);
                    Canvas.SetTop(selectedRangeRectangle, finalTop);

                    break;
            }
        }
    }
}
