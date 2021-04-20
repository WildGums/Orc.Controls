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

    [TemplatePart(Name = "PART_TrackBackground", Type = typeof(Border))]
    [TemplatePart(Name = "PART_SelectedRange", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_LowerSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_UpperSlider", Type = typeof(Slider))]
    public class RangeSlider : RangeBase
    {
        #region Fields
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
        #endregion
        
        #region Constructors
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

            IsVisibleChanged += OnIsVisibleChanged;
        }
        #endregion

        #region Dependency properties
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
        #endregion

        #region Routed events
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
        #endregion

        #region Methods
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
                    SetCurrentValue(LowerValueProperty, Minimum);
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
                SetCurrentValue(UpperValueProperty, Maximum);
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
            if (_lowerSlider is not null)
            {
                _lowerSlider.ValueChanged += OnSliderValueChanged;
            }

            _upperSlider = GetTemplateChild("PART_UpperSlider") as Slider;
            if (_upperSlider is not null)
            {
                _upperSlider.ValueChanged += OnSliderValueChanged;
            }

            _trackBackgroundBorder = GetTemplateChild("PART_TrackBackground") as Border;
            _selectedRangeRectangle = GetTemplateChild("PART_SelectedRange") as Rectangle;

            UpdateState();
        }

        private void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_ignoreSliderValueChanging)
            {
                return;
            }

            if (_upperSlider is null || _lowerSlider is null)
            {
                return;
            }

            try
            {
                _ignoreSliderValueChanging = true;

                if (ReferenceEquals(sender, _lowerSlider))
                {
                    _upperSlider.SetCurrentValue(ValueProperty, Math.Max(_upperSlider.Value, _lowerSlider.Value));
                }
                else if (ReferenceEquals(sender, _upperSlider))
                {
                    _lowerSlider.SetCurrentValue(ValueProperty, Math.Min(_upperSlider.Value, _lowerSlider.Value));
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

            ReloadThumbs();

            StartUpdate();
        }

        private void ReloadThumbs()
        {
            _lowerSlider?.ApplyTemplate();
            _lowerTrack = _lowerSlider?.Template?.FindName("PART_Track", _lowerSlider) as Track;
            _lowerThumb = _lowerTrack?.FindVisualDescendantByType<Thumb>();

            _upperSlider?.ApplyTemplate();
            _upperTrack = _upperSlider?.Template?.FindName("PART_Track", _upperSlider) as Track;
            _upperThumb = _upperTrack?.FindVisualDescendantByType<Thumb>();
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

            if (_lowerThumb is null)
            {
                UpdateState();
            }

            UpdateRelatedValues();
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsVisible)
            {
                return;
            }

            if (_lowerThumb is not null)
            {
                if (!_lowerThumb.IsVisible)
                {
                    _lowerThumb.IsVisibleChanged += OnLowerThumbIsVisibleChanged;
                }
            }
            
            if (_upperThumb is not null)
            {
                if (!_upperThumb.IsVisible)
                {
                    _upperThumb.IsVisibleChanged += OnUpperThumbIsVisibleChanged;
                }
            }

            IsVisibleChanged -= OnIsVisibleChanged;
        }

        private void OnUpperThumbIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            StartUpdate();
            _upperThumb.IsVisibleChanged -= OnLowerThumbIsVisibleChanged;
        }

        private void OnLowerThumbIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            StartUpdate();
            _lowerThumb.IsVisibleChanged -= OnLowerThumbIsVisibleChanged;
        }

        private void UpdateRelatedValues()
        {
            if (!IsLoaded)
            {
                return;
            }

            if (_lowerSlider is null || _upperSlider is null)
            {
                return;
            }

            // As a bonus, the value will show the average
            SetCurrentValue(ValueProperty, (_lowerSlider.Value + _upperSlider.Value) / 2);

            var lowerThumb = _lowerThumb;
            var upperThumb = _upperThumb;
            var trackBackgroundBorder = _trackBackgroundBorder;
            var selectedRangeRectangle = _selectedRangeRectangle;

            if (lowerThumb is null || upperThumb is null || trackBackgroundBorder is null || selectedRangeRectangle is null)
            {
                return;
            }

            // When unloaded, item becomes IsVisible = false; If this is the case, we can't call PointToScreen,
            // it will throw an exception
            if (!lowerThumb.IsVisible || !upperThumb.IsVisible)
            {
                return;
            }
            
            var lowerThumbPosition = lowerThumb.GetCenterPointInRoot(this);
            var upperThumbPosition = upperThumb.GetCenterPointInRoot(this);

            switch (Orientation)
            {
                case Orientation.Horizontal:

                    var selectionRectLeft = lowerThumbPosition.X;
                    var selectionRectWidth = upperThumbPosition.X - lowerThumbPosition.X;

                    selectedRangeRectangle.Width = selectionRectWidth;
                    selectedRangeRectangle.Height = 3;

                    Canvas.SetTop(selectedRangeRectangle, -1d);
                    Canvas.SetLeft(selectedRangeRectangle, selectionRectLeft);

                    break;

                case Orientation.Vertical:

                    var selectionRectTop = upperThumbPosition.Y;
                    var selectionRectHeight = lowerThumbPosition.Y - upperThumbPosition.Y;

                    selectedRangeRectangle.Width = 3;
                    selectedRangeRectangle.Height = selectionRectHeight;

                    Canvas.SetTop(selectedRangeRectangle, selectionRectTop);
                    Canvas.SetLeft(selectedRangeRectangle, -1d);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            #endregion
        }
    }
}
