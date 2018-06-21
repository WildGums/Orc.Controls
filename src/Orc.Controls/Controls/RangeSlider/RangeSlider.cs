namespace Orc.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Shapes;
    using Catel.Windows;
    using Catel.Windows.Threading;

    [TemplatePart(Name = "PART_TrackBackground", Type = typeof(Border))]
    [TemplatePart(Name = "PART_SelectedRange", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_LowerSlider", Type = typeof(Slider))]
    [TemplatePart(Name = "PART_UpperSlider", Type = typeof(Slider))]
    public class RangeSlider : RangeBase
    {
        private static readonly Point LeftTop = new Point(0, 0);

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
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(typeof(RangeSlider)));
        }

        public RangeSlider()
        {
        }

        [Category("Behavior"), Bindable(true)]
        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        public static readonly DependencyProperty LowerValueProperty = DependencyProperty.Register(nameof(LowerValue), typeof(double),
            typeof(RangeSlider), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        [Category("Behavior"), Bindable(true)]
        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }

        public static readonly DependencyProperty UpperValueProperty = DependencyProperty.Register(nameof(UpperValue), typeof(double),
            typeof(RangeSlider), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


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

        private void OnOrientationChanged()
        {
            UpdateState();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _lowerSlider = GetTemplateChild("PART_LowerSlider") as Slider;
            if (_lowerSlider != null)
            {
                _lowerSlider.ValueChanged += OnLowerSliderValueChanged;
            }

            _upperSlider = GetTemplateChild("PART_UpperSlider") as Slider;
            if (_upperSlider != null)
            {
                _upperSlider.ValueChanged += OnLowerSliderValueChanged;
            }

            _trackBackgroundBorder = GetTemplateChild("PART_TrackBackground") as Border;
            _selectedRangeRectangle = GetTemplateChild("PART_SelectedRange") as Rectangle;

            Dispatcher.BeginInvoke(() => UpdateState());
        }

        private void OnLowerSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _upperSlider.Value = Math.Max(_upperSlider.Value, _lowerSlider.Value);

            UpdateRelatedValues();
        }

        private void OnUpperSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _lowerSlider.Value = Math.Min(_upperSlider.Value, _lowerSlider.Value);

            UpdateRelatedValues();
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

            UpdateRelatedValues();
        }

        private void UpdateRelatedValues()
        {
            if (!IsLoaded)
            {
                return;
            }

            // As a bonus, the value will show the average
            Value = (_lowerSlider.Value + _upperSlider.Value) / 2;

            var lowerThumb = _lowerThumb;
            var upperThumb = _upperThumb;
            var trackBackgroundBorder = _trackBackgroundBorder;
            var selectedRangeRectangle = _selectedRangeRectangle;

            if (lowerThumb != null && upperThumb != null && trackBackgroundBorder != null && selectedRangeRectangle != null)
            {
                // When unloaded, item becomes IsVisible = false; If this is the case, we can't call PointToScreen,
                // it will throw an exception
                if (lowerThumb.IsVisible && upperThumb.IsVisible)
                {
                    var lowerThumbCenterX = lowerThumb.Width / 2;
                    var lowerThumbCenterY = lowerThumb.Height / 2;
                    var lowerThumbPosition = lowerThumb.PointToScreen(LeftTop);

                    var upperThumbCenterX = upperThumb.Width / 2;
                    var upperThumbCenterY = upperThumb.Height / 2;
                    var upperThumbPosition = upperThumb.PointToScreen(LeftTop);

                    var containerWidth = trackBackgroundBorder.ActualWidth;
                    var containerHeight = trackBackgroundBorder.ActualHeight;
                    var width = ActualWidth;
                    var height = ActualHeight;
                    var widthRatio = (containerWidth * 100) / width;
                    var heighRatio = (containerHeight * 100) / height;

                    var leftTop = PointToScreen(LeftTop);

                    if (lowerThumb != null && upperThumb != null)
                    {
                        switch (Orientation)
                        {
                            case Orientation.Horizontal:
                                // Draw left => right
                                var left = (lowerThumbPosition.X - leftTop.X) + lowerThumbCenterX;
                                var finalLeft = (left / 100) * widthRatio;

                                var right = (upperThumbPosition.X - leftTop.X) + upperThumbCenterX;
                                var finalRight = (right / 100) * widthRatio;

                                selectedRangeRectangle.Width = right - left;

                                Canvas.SetLeft(selectedRangeRectangle, finalLeft);
                                //Canvas.SetRight(selectedRangeRectangle, finalRight);
                                break;

                            case Orientation.Vertical:
                                // Draw bottom => top
                                break;
                        }
                    }
                }
            }
        }
    }
}
