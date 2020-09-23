// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimePicker.cs" company="">
// Clock-like TimePicker control https://github.com/roy-t/TimePicker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using ControlzEx.Theming;
    using Orc.Controls.Enums;
    using Orc.Theming;
    using static Orc.Controls.ClockMath;
    public class TimePicker : ContentControl
    {
        #region Fields
        public const double HourTickRatio = 0.20;
        public const double MinuteTickRatio = 0.10;
        public const double HourIndicatorRatio = 0.70;
        public const double MinuteIndicatorRatio = 0.95;

        private readonly Canvas _containerCanvas;
        private readonly ToggleButton _amPmButton;
        private readonly TimePickerInputController _inputController;
        private readonly ControlzEx.Theming.ThemeManager _themeManager;

        private bool _showNumbers;
        #endregion
        #region Constructors
        public TimePicker()
        {
            _inputController = new TimePickerInputController(this);
            _themeManager = ControlzEx.Theming.ThemeManager.Current;
            _containerCanvas = new Canvas();
            _amPmButton = new ToggleButton
            {
                IsChecked = false
            };
            _amPmButton.Checked += OnAmPmButtonCheckedChanged;
            _amPmButton.Unchecked += OnAmPmButtonCheckedChanged;
        }
        #endregion

        #region Dependency properties
        public TimeSpan TimeValue
        {
            get { return (TimeSpan)GetValue(TimeValueProperty); }
            set { SetValue(TimeValueProperty, value); }
        }

        public static readonly DependencyProperty TimeValueProperty =
            DependencyProperty.Register(nameof(TimeValue), typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(new TimeSpan(0, 0, 0), new PropertyChangedCallback(OnTimeValueChanged)));

        public Meridiem AmPmValue
        {
            get { return (Meridiem)GetValue(AmPmValueProperty); }
            set { SetValue(AmPmValueProperty, value); }
        }

        public static readonly DependencyProperty AmPmValueProperty = DependencyProperty.Register(nameof(AmPmValue), typeof(Meridiem), typeof(TimePicker), new PropertyMetadata(Meridiem.AM, new PropertyChangedCallback(OnAmPmValueChanged)));

        public Brush HourBrush
        {
            get { return (Brush)GetValue(HourBrushProperty); }
            set { SetValue(HourBrushProperty, value); }
        }

        public static readonly DependencyProperty HourBrushProperty =
            DependencyProperty.Register(nameof(HourBrush), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.Black));

        public double HourThickness
        {
            get { return (double)GetValue(HourThicknessProperty); }
            set { SetValue(HourThicknessProperty, value); }
        }

        public static readonly DependencyProperty HourThicknessProperty =
            DependencyProperty.Register(nameof(HourThickness), typeof(double), typeof(TimePicker), new PropertyMetadata(5.0, new PropertyChangedCallback(OnThicknessChanged)));

        public Brush MinuteBrush
        {
            get { return (Brush)GetValue(MinuteBrushProperty); }
            set { SetValue(MinuteBrushProperty, value); }
        }

        public static readonly DependencyProperty MinuteBrushProperty =
            DependencyProperty.Register(nameof(MinuteBrush), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.Black));

        public double MinuteThickness
        {
            get { return (double)GetValue(MinuteThicknessProperty); }
            set { SetValue(MinuteThicknessProperty, value); }
        }

        public static readonly DependencyProperty MinuteThicknessProperty =
            DependencyProperty.Register(nameof(MinuteThickness), typeof(double), typeof(TimePicker), new PropertyMetadata(3.0, new PropertyChangedCallback(OnThicknessChanged)));

        public Brush HourTickBrush
        {
            get { return (Brush)GetValue(HourTickBrushProperty); }
            set { SetValue(HourTickBrushProperty, value); }
        }

        public static readonly DependencyProperty HourTickBrushProperty =
            DependencyProperty.Register(nameof(HourTickBrush), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.Gray));

        public double HourTickThickness
        {
            get { return (double)GetValue(HourTickThicknessProperty); }
            set { SetValue(HourTickThicknessProperty, value); }
        }

        public static readonly DependencyProperty HourTickThicknessProperty =
            DependencyProperty.Register(nameof(HourTickThickness), typeof(double), typeof(TimePicker), new PropertyMetadata(2.0, new PropertyChangedCallback(OnThicknessChanged)));

        public Brush MinuteTickBrush
        {
            get { return (Brush)GetValue(MinuteTickBrushProperty); }
            set { SetValue(MinuteTickBrushProperty, value); }
        }

        public static readonly DependencyProperty MinuteTickBrushProperty =
            DependencyProperty.Register(nameof(MinuteTickBrush), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.DarkGray));

        public double MinuteTickThickness
        {
            get { return (double)GetValue(MinuteTickThicknessProperty); }
            set { SetValue(MinuteTickThicknessProperty, value); }
        }

        public static readonly DependencyProperty MinuteTickThicknessProperty =
            DependencyProperty.Register(nameof(MinuteTickThickness), typeof(double), typeof(TimePicker), new PropertyMetadata(2.0, new PropertyChangedCallback(OnThicknessChanged)));

        public double ClockBorderThickness
        {
            get { return (double)GetValue(ClockBorderThicknessProperty); }
            set { SetValue(ClockBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty ClockBorderThicknessProperty =
            DependencyProperty.Register(nameof(ClockBorderThickness), typeof(double), typeof(TimePicker), new PropertyMetadata(2.0, new PropertyChangedCallback(OnThicknessChanged)));

        public bool ShowNumbers
        {
            get { return (bool)GetValue(ShowNumbersProperty); }
            set { SetValue(ShowNumbersProperty, value); }
        }

        public static readonly DependencyProperty ShowNumbersProperty = DependencyProperty.Register(nameof(ShowNumbers), typeof(bool),
            typeof(TimePicker), new FrameworkPropertyMetadata(false,
                (sender, e) => ((TimePicker)sender).OnShowNumbersChanged()));


        #endregion

        #region Methods

        private void OnShowNumbersChanged()
        {
            _showNumbers = ShowNumbers;
            Render();
        }
        private static void OnTimeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var timePicker = d as TimePicker;
            if (timePicker != null)
            {
                timePicker.InvalidateVisual();
            }
        }
        private static void OnAmPmValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var timePicker = d as TimePicker;
            if (timePicker != null)
            {
                timePicker.InvalidateVisual();
            }
        }
        private void OnAmPmButtonCheckedChanged(object sender, RoutedEventArgs e)
        {
            switch
                    (_amPmButton.IsChecked)
            {
                case true:
                    SetCurrentValue(AmPmValueProperty, Meridiem.PM);
                    break;
                default:
                    SetCurrentValue(AmPmValueProperty, Meridiem.AM);
                    break;
            }
            _amPmButton.SetCurrentValue(ContentProperty, AmPmValue);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            Render();
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Render();
            _themeManager.ThemeChanged += OnThemeManagerThemeChanged;
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _themeManager.ThemeChanged -= OnThemeManagerThemeChanged;
        }
        private void OnThemeManagerThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            Render();
        }
        private static void OnThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var timePicker = d as TimePicker;
            if (timePicker != null)
            {
                timePicker.InvalidateVisual();
            }
        }
        private void Render()
        {
            var width = ActualWidth;
            var height = ActualHeight;
            var radius = (Math.Min(width, height) - ClockBorderThickness) / 2.0;
            var center = new Point(width / 2.0, height / 2.0);

            SetCurrentValue(ShowNumbersProperty, _showNumbers);
            _containerCanvas.Children.Clear();

            DrawingGroup drawingGroup = new DrawingGroup();
            using (DrawingContext drawingContext = drawingGroup.Open())
            {
                RenderBackground(drawingContext, width, height);
                RenderBorder(drawingContext, radius, center);
                if (ShowNumbers)
                {
                    RenderNumbers(drawingContext, radius, center);
                }
                else
                {
                    RenderHourTicks(drawingContext, radius, center);
                }
                RenderMinuteTicks(drawingContext, radius, center);
                RenderHour(drawingContext, radius, center);
                RenderMinute(drawingContext, radius, center);
                base.OnRender(drawingContext);
            }
            Image theImage = new Image();
            DrawingImage dImageSource = new DrawingImage(drawingGroup);
            theImage.Source = dImageSource;
            _containerCanvas.Children.Add(theImage);

            _containerCanvas.Children.Add(_amPmButton);
            _amPmButton.SetCurrentValue(StyleProperty, null);
            _amPmButton.SetCurrentValue(ContentProperty, AmPmValue);
            _amPmButton.SetCurrentValue(MinHeightProperty, 18.0);
            _amPmButton.SetCurrentValue(MinWidthProperty, 22.0);
            var amPmButtonWidth = width * 0.1;
            var amPmButtonHeight = height * 0.08;
            var stringSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            var fontSize = 14d;
            while ((stringSize.Width > amPmButtonWidth || stringSize.Height > amPmButtonHeight) && fontSize > 1)
            {
                stringSize = MeasureString(_amPmButton.Content.ToString(), fontSize);
                fontSize -= 1;
            }
            _amPmButton.SetCurrentValue(FontSizeProperty, fontSize - 1);

            Canvas.SetLeft(_amPmButton, width * 0.73);
            Canvas.SetTop(_amPmButton, height * 0.47);

            _amPmButton.SetCurrentValue(StyleProperty, new Style(typeof(ToggleButton))
            {
                BasedOn = _amPmButton.Style,
                Setters =
                {
                    new Setter(WidthProperty, amPmButtonWidth),
                    new Setter(HeightProperty, amPmButtonHeight)
                }
            });

            SetCurrentValue(ContentProperty, _containerCanvas);
        }
        private Size MeasureString(string s, double fontSize)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new Size(0, 0);
            }
            var textBlock = new TextBlock { Text = s, FontSize = fontSize };
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return new Size(textBlock.DesiredSize.Width, textBlock.DesiredSize.Height);
        }
        private void RenderBackground(DrawingContext drawingContext, double width, double height)
        {
            // Always draw a transparent rectangle for hit tests
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, width, height));
        }
        private void RenderMinute(DrawingContext drawingContext, double radius, Point center)
        {
            var pen = new Pen(Theming.ThemeManager.Current.GetThemeColorBrush(ThemeColorStyle.AccentColor), MinuteThickness);

            drawingContext.DrawEllipse(MinuteBrush, pen, center, MinuteThickness * 1, MinuteThickness * 1);
            var points = LineOnCircle((Math.PI * 2.0 * TimeValue.Minutes / 60.0) - Math.PI / 2.0, center, MinuteThickness, radius * MinuteIndicatorRatio);
            drawingContext.DrawLine(pen, points[0], points[1]);
        }
        private void RenderHour(DrawingContext drawingContext, double radius, Point center)
        {
            var pen = new Pen(Theming.ThemeManager.Current.GetThemeColorBrush(ThemeColorStyle.AccentColor), HourThickness);
            drawingContext.DrawEllipse(HourBrush, pen, center, HourThickness * 1, HourThickness * 1);
            var points = LineOnCircle((Math.PI * 2.0 * TimeValue.Hours / 12.0) - Math.PI / 2.0, center, HourThickness, radius * HourIndicatorRatio);
            drawingContext.DrawLine(pen, points[0], points[1]);
        }
        private void RenderBorder(DrawingContext drawingContext, double radius, Point center)
        {
            drawingContext.DrawEllipse(Background, new Pen(Theming.ThemeManager.Current.GetThemeColorBrush(ThemeColorStyle.AccentColor), ClockBorderThickness), center, radius, radius);
        }
        private void RenderHourTicks(DrawingContext drawingContext, double radius, Point center)
        {
            var pen = new Pen(HourTickBrush, HourTickThickness);
            for (int i = 0; i < 12; i++)
            {
                var points = LineOnCircle(Math.PI * 2 * i / 12, center, radius * (1 - HourTickRatio), radius - ClockBorderThickness * 0.5);
                drawingContext.DrawLine(pen, points[0], points[1]);
            }
        }
        private void RenderNumbers(DrawingContext drawingContext, double radius, Point center) 
        {
            var pen = new Pen(HourTickBrush, HourTickThickness);
            for (int i = 1; i <= 12; i++)
            {
                var points = LineOnCircle(Math.PI * 2 * (i - 3) / 12, center, radius * (1 - MinuteTickRatio), radius - ClockBorderThickness * 0.5);
                drawingContext.DrawLine(pen, points[0], points[1]);
#pragma warning disable 0618
                var formattedText = new FormattedText(i.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Impact"), radius * 0.2, HourTickBrush, new NumberSubstitution(), TextFormattingMode.Display);
#pragma warning restore 0618
                var numberPoints = LineOnCircle(Math.PI * 2 * (i - 3) / 12, center, radius * (1 - HourTickRatio), radius - ClockBorderThickness * 0.5);
                var point = new Point(numberPoints[0].X - radius * 0.06, numberPoints[0].Y - radius * 0.13);
                drawingContext.DrawText(formattedText, point);
            }
        }
        private void RenderMinuteTicks(DrawingContext drawingContext, double radius, Point center)
        {
            var pen = new Pen(MinuteTickBrush, MinuteTickThickness);
            for (int i = 0; i < 60; i++)
            {
                if (i % 5 == 0) // Skip places where we already have an hour tick
                {
                    continue;
                } 
                var points = LineOnCircle(Math.PI * 2 * i / 60, center, radius * (1 - MinuteTickRatio), radius - ClockBorderThickness * 0.5);
                drawingContext.DrawLine(pen, points[0], points[1]);
            }
        }
        #endregion
    }
}
