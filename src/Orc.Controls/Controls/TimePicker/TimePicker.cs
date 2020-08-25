// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimePicker.cs" company="">
// Clock-like TimePicker control https://github.com/roy-t/TimePicker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using ControlzEx.Theming;
    using Orc.Theming;
    using static Orc.Controls.ClockMath;
    public class TimePicker : Control
    {
        public const double HourTickRatio = 0.20;
        public const double MinuteTickRatio = 0.10;
        public const double HourIndicatorRatio = 0.70;
        public const double MinuteIndicatorRatio = 0.95;

        private readonly TimePickerInputController _inputController;
        private readonly ControlzEx.Theming.ThemeManager _themeManager;

        private DrawingContext _drawingContext;
        private readonly ToggleButton _amPmButton;

        public TimePicker()
        {
            _inputController = new TimePickerInputController(this);
            _amPmButton = new ToggleButton();
            _themeManager = ControlzEx.Theming.ThemeManager.Current;
        }

        public TimeSpan TimeValue
        {
            get { return (TimeSpan)GetValue(TimeValueProperty); }
            set { SetValue(TimeValueProperty, value); }
        }

        public static readonly DependencyProperty TimeValueProperty =
            DependencyProperty.Register(nameof(TimeValue), typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(new TimeSpan(0, 0, 0), new PropertyChangedCallback(OnTimeValueChanged)));

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
            DependencyProperty.Register(nameof(HourThickness), typeof(double), typeof(TimePicker), new PropertyMetadata(5.0));

        public Brush MinuteBrush
        {
            get { return (Brush)GetValue(MinuteBrushProperty); }
            set { SetValue(MinuteBrushProperty, value); }
        }

        public static readonly DependencyProperty MinuteBrushProperty =
            DependencyProperty.Register(nameof(MinuteBrush), typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.DarkBlue));

        public double MinuteThickness
        {
            get { return (double)GetValue(MinuteThicknessProperty); }
            set { SetValue(MinuteThicknessProperty, value); }
        }

        public static readonly DependencyProperty MinuteThicknessProperty =
            DependencyProperty.Register(nameof(MinuteThickness), typeof(double), typeof(TimePicker), new PropertyMetadata(3.0));

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
            DependencyProperty.Register(nameof(HourTickThickness), typeof(double), typeof(TimePicker), new PropertyMetadata(2.0));

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
            DependencyProperty.Register(nameof(MinuteTickThickness), typeof(double), typeof(TimePicker), new PropertyMetadata(2.0));

        private static void OnTimeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var timePicker = d as TimePicker;
            if (timePicker != null)
            {
                timePicker.InvalidateVisual();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            _drawingContext = drawingContext;
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

        private void Render()
        {
            var width = ActualWidth;
            var height = ActualHeight;
            var radius = (Math.Min(width, height) - BorderThickness.Left) / 2.0;
            var center = new Point(width / 2.0, height / 2.0);

            RenderBackground(_drawingContext, width, height);

            RenderBorder(_drawingContext, radius, center);
            RenderHourTicks(_drawingContext, radius, center);
            RenderMinuteTicks(_drawingContext, radius, center);
            RenderAmPmButton(_drawingContext, radius, center);
            RenderHour(_drawingContext, radius, center);
            RenderMinute(_drawingContext, radius, center);

            base.OnRender(_drawingContext);
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
            var points = LineOnCircle((Math.PI * 2.0 * TimeValue.Minutes / 60.0) - Math.PI / 2.0, center, 0, radius * MinuteIndicatorRatio);
            drawingContext.DrawLine(pen, points[0], points[1]);
        }

        private void RenderHour(DrawingContext drawingContext, double radius, Point center)
        {
            var pen = new Pen(Theming.ThemeManager.Current.GetThemeColorBrush(ThemeColorStyle.AccentColor), HourThickness);

            drawingContext.DrawEllipse(HourBrush, pen, center, HourThickness * 1, HourThickness * 1);
            var points = LineOnCircle((Math.PI * 2.0 * TimeValue.Hours / 12.0) - Math.PI / 2.0, center, 0, radius * HourIndicatorRatio);
            drawingContext.DrawLine(pen, points[0], points[1]);
        }


        private void RenderBorder(DrawingContext drawingContext, double radius, Point center)
        {
            drawingContext.DrawEllipse(Background, new Pen(BorderBrush, BorderThickness.Left), center, radius, radius);
        }

        private void RenderHourTicks(DrawingContext drawingContext, double radius, Point center)
        {
            var pen = new Pen(HourTickBrush, HourTickThickness);
            for (int i = 0; i < 12; i++)
            {
                var points = LineOnCircle(Math.PI * 2 * i / 12, center, radius * (1 - HourTickRatio), radius - BorderThickness.Left * 0.5);
                drawingContext.DrawLine(pen, points[0], points[1]);
            }
        }

        private void RenderMinuteTicks(DrawingContext drawingContext, double radius, Point center)
        {
            var pen = new Pen(MinuteTickBrush, MinuteTickThickness);
            for (int i = 0; i < 60; i++)
            {
                if (i % 5 == 0) continue; // Skip places where we already have an hour tick

                var points = LineOnCircle(Math.PI * 2 * i / 60, center, radius * (1 - MinuteTickRatio), radius - BorderThickness.Left * 0.5);
                drawingContext.DrawLine(pen, points[0], points[1]);
            }
        }

        private void RenderAmPmButton(DrawingContext drawingContext, double radius, Point center) 
        {
            Point startPoint = new Point(center.X + radius / 6, center.Y + radius / 6);
            Size size = new Size(radius / 3, radius / 4);
            drawingContext.DrawRectangle(Theming.ThemeManager.Current.GetThemeColorBrush(ThemeColorStyle.AccentColor), null, new Rect(startPoint, size));
            PresentationSource source = PresentationSource.FromVisual(this);
            double dpiX, dpiY;
            if (source != null)
            {
                dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            }
                FormattedText text = new FormattedText("AM", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Italic"), radius/6, Brushes.Black);
            Point textPoint = new Point(startPoint.X + radius / 15, startPoint.Y + radius / 20);
            drawingContext.DrawText(text, textPoint);
        }

    }
}
