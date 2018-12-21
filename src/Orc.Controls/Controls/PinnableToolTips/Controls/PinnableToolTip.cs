// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PinnableToolTip.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

//#define TRACE_DETAILS

namespace Orc.Controls
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel;

    /// <summary>
    /// The pinnable toolTip control.
    /// </summary>
    [TemplatePart(Name = "PinButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "CloseButton", Type = typeof(Button))]
    [TemplatePart(Name = "DragGrip", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "GripDrawing", Type = typeof(GeometryDrawing))]
    public class PinnableToolTip : ContentControl, IControlAdornerChild
    {
        #region Constants
        private const double Epsilon = 1E-7;

        private static int _counter = 0;
        #endregion

        #region Fields
        private static readonly ConcurrentDictionary<UIElement, List<int>> OnFrontIdDictionary = new ConcurrentDictionary<UIElement, List<int>>();

        private readonly int _id;

        private Button _closeButton;
        private FrameworkElement _dragGrip;
        private GeometryDrawing _gripDrawing;
        private ControlAdorner _adorner;
        private ControlAdornerDragDrop _adornerDragDrop;
        private ResizingAdorner _adornerResizing;
        private AdornerLayer _adornerLayer;
        private bool _isPositionCalculated;
        private Point _lastPosition;
        private Size _lastSize;
        private bool _ignoreTimerStartupWhenMouseLeave;
        private UIElement _owner;
        private UIElement _userDefinedAdorner;
        private ToolTipTimer _timer;
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PinnableToolTip" /> class.
        /// </summary>
        public PinnableToolTip()
        {
            DefaultStyleKey = typeof(PinnableToolTip);
            _id = System.Threading.Interlocked.Increment(ref _counter);

            SizeChanged += OnSizeChanged;
            MouseEnter += OnPinnableToolTipMouseEnter;
            MouseLeave += OnPinnableToolTipMouseLeave;
            MouseDown += OnPinnableToolTipMouseDown;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether is timer enabled.
        /// </summary>
        internal bool IsTimerEnabled => _timer != null && _timer.IsEnabled;
        #endregion

        #region Properties
        public UIElement Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public bool IsOpen
        {
            get { return _adorner != null; }
            private set
            {
                if (value)
                {
                    CreateAdorner();
                }
                else
                {
                    RemoveAdorner();
                    StopTimer();

#pragma warning disable WPF0035 // Use SetValue in setter.
                    // Clear horizontal / vertical offset because it's used by the resizing adorner
                    SetCurrentValue(HorizontalOffsetProperty, 0d);
                    SetCurrentValue(VerticalOffsetProperty, 0d);
#pragma warning restore WPF0035 // Use SetValue in setter.
                }

#pragma warning disable WPF0036 // Avoid side effects in CLR accessors.
                OnIsOpenChanged();
#pragma warning restore WPF0036 // Avoid side effects in CLR accessors.
            }
        }

        public bool AllowCloseByUser
        {
            get { return (bool)GetValue(AllowCloseByUserProperty); }
            set { SetValue(AllowCloseByUserProperty, value); }
        }

        public static readonly DependencyProperty AllowCloseByUserProperty = DependencyProperty.Register(nameof(AllowCloseByUser), typeof(bool),
            typeof(PinnableToolTip), new PropertyMetadata(false));


        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(nameof(HorizontalOffset), typeof(double),
            typeof(PinnableToolTip), new PropertyMetadata((sender, e) => ((PinnableToolTip)sender).OnHorizontalOffsetChanged()));


        public bool IsPinned
        {
            get { return (bool)GetValue(IsPinnedProperty); }
            set { SetValue(IsPinnedProperty, value); }
        }

        public static readonly DependencyProperty IsPinnedProperty = DependencyProperty.Register(nameof(IsPinned), typeof(bool), typeof(PinnableToolTip),
            new PropertyMetadata(false, (sender, e) => ((PinnableToolTip)sender).OnIsPinnedChanged()));


        public Color GripColor
        {
            get { return (Color)GetValue(GripColorProperty); }
            set { SetValue(GripColorProperty, value); }
        }

        public static readonly DependencyProperty GripColorProperty = DependencyProperty.Register(nameof(GripColor), typeof(Color), typeof(PinnableToolTip),
            new PropertyMetadata(Color.FromRgb(204, 204, 204), (sender, e) => ((PinnableToolTip)sender).OnGripColorChanged()));


        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(nameof(VerticalOffset), typeof(double),
            typeof(PinnableToolTip), new PropertyMetadata((sender, e) => ((PinnableToolTip)sender).OnVerticalOffsetChanged()));


        public ICommand OpenLinkCommand
        {
            get { return (ICommand)GetValue(OpenLinkCommandProperty); }
            set { SetValue(OpenLinkCommandProperty, value); }
        }

        public static readonly DependencyProperty OpenLinkCommandProperty = DependencyProperty.Register(nameof(OpenLinkCommand),
            typeof(ICommand), typeof(PinnableToolTip), new PropertyMetadata(null));


        [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use AccentColorBrush markup extension instead")]
        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use AccentColorBrush markup extension instead")]
        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register(nameof(AccentColorBrush), typeof(Brush),
            typeof(PinnableToolTip), new PropertyMetadata(Brushes.LightGray, (sender, e) => ((PinnableToolTip)sender).OnAccentColorBrushChanged()));


        public ResizeMode ResizeMode
        {
            get { return (ResizeMode)GetValue(ResizeModeProperty); }
            set { SetValue(ResizeModeProperty, value); }
        }

        public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.Register(nameof(ResizeMode), typeof(ResizeMode),
            typeof(PinnableToolTip), new PropertyMetadata(ResizeMode.NoResize, (sender, e) => ((PinnableToolTip)sender).OnResizeModeChanged()));
        #endregion

        #region Events
        public event EventHandler<EventArgs> IsOpenChanged;

        public event EventHandler<EventArgs> IsPinnedChanged;
        #endregion

        #region Commands

        #endregion

        #region Public Methods and Operators
        public void IgnoreTimerStartupWhenMouseLeave(bool value)
        {
            _ignoreTimerStartupWhenMouseLeave = value;

            if (!_ignoreTimerStartupWhenMouseLeave
                && !IsMouseOver
                && !IsPinned
                && IsOpen)
            {
                StartTimer();
            }
        }

        public Point GetPosition()
        {
            var mousePosition = Mouse.GetPosition(_userDefinedAdorner);
            var rootVisual = GetRootVisual();

            var fixedOffset = 0d;
            if (ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip)
            {
                fixedOffset = 3d;
            }

            var horizontalOffset = HorizontalOffset + fixedOffset;
            var verticalOffset = VerticalOffset + fixedOffset;

            var mousePositionX = mousePosition.X;
            var mousePositionY = mousePosition.Y;

            //using this code for non UIElements
            if (_owner is null)
            {
                return GetPostionForNonUiElement(rootVisual, mousePosition, horizontalOffset, verticalOffset);
            }

            var placementMode = PinnableToolTipService.GetPlacement(_owner);
            switch (placementMode)
            {
                case PlacementMode.Mouse:
                    if (_isPositionCalculated)
                    {
                        return _lastPosition;
                    }

                    const int fontSize = 0;

                    var offsetX = Math.Max(2.0, mousePositionX + horizontalOffset);
                    var offsetY = Math.Max(2.0, mousePositionY + fontSize + verticalOffset);

                    var actualHeight = rootVisual.ActualHeight;
                    var actualWidth = rootVisual.ActualWidth;
                    var lastHeight = _lastSize.Height;
                    var lastWidth = _lastSize.Width;

                    var lastRectangle = new Rect(offsetX, offsetY, lastWidth, lastHeight);
                    var actualRectangle = new Rect(0.0, 0.0, actualWidth, actualHeight);
                    actualRectangle.Intersect(lastRectangle);

                    if (!(Math.Abs(actualRectangle.Width - lastRectangle.Width) < 2.0)
                        || !(Math.Abs(actualRectangle.Height - lastRectangle.Height) < 2.0))
                    {
                        offsetY = GetOffset(0, offsetY, actualHeight, lastRectangle.Height);
                        offsetX = GetOffset(0, offsetX, actualWidth, lastRectangle.Width);
                    }

                    _lastPosition = new Point(offsetX, offsetY);
                    _isPositionCalculated = true;

                    break;

                case PlacementMode.Bottom:
                case PlacementMode.Right:
                case PlacementMode.Left:
                case PlacementMode.Top:
                    var windowSize = Application.Current.MainWindow.GetSize();
                    var plugin = new Rect(0.0, 0.0, windowSize.Width, windowSize.Height);
                    var placementTarget = PinnableToolTipService.GetPlacementTarget(_owner) ?? _owner;
                    var targetPoints = GetTranslatedPoints((FrameworkElement)placementTarget);
                    var toolTipPoints = GetTranslatedPoints(this);
                    var popupLocation = PlacePopup(plugin, targetPoints, toolTipPoints, placementMode);

#if TRACE_DETAILS
                    Debug.WriteLine($"IsPinned: {IsPinned}");
                    Debug.WriteLine($"Offset: X = '{horizontalOffset}', Y = '{verticalOffset}'");
                    Debug.WriteLine($"Final point: '{popupLocation.X}, {popupLocation.Y}'");
#endif
                    return popupLocation;
            }

            return default(Point);
        }

        private Point GetPostionForNonUiElement(FrameworkElement rootVisual, Point mousePosition, double horizontalOffset, double verticalOffset)
        {
            var mousePositionX = mousePosition.X;
            var mousePositionY = mousePosition.Y;

            if (rootVisual is null)
            {
                return _isPositionCalculated ? _lastPosition : mousePosition;
            }

            if (_isPositionCalculated)
            {
                if (_lastPosition.Y + DesiredSize.Height > rootVisual.ActualHeight)
                {
                    _lastPosition.Y = rootVisual.ActualHeight - DesiredSize.Height;
                }

                if (_lastPosition.X + DesiredSize.Width > rootVisual.ActualWidth)
                {
                    _lastPosition.X = rootVisual.ActualWidth - DesiredSize.Width;
                }

                return _lastPosition;
            }

            var offsetX = GetOffset(mousePositionX, horizontalOffset, rootVisual.ActualWidth, DesiredSize.Width);
            var offsetY = GetOffset(mousePositionY, verticalOffset, rootVisual.ActualHeight, DesiredSize.Height);

            var position = new Point(offsetX, offsetY);

            _isPositionCalculated = DesiredSize.Height > 0;
            if (_isPositionCalculated)
            {
                _lastPosition = position;
            }

            return position;
        }

        private static double GetOffset(double mousePosition, double offset, double actualSize, double desiredSize)
        {
            var resultOffset = mousePosition + offset;
            if (resultOffset + desiredSize > actualSize)
            {
                resultOffset = actualSize - desiredSize - 2.0;
            }

            return resultOffset < 0.0 ? 0.0 : resultOffset;
        }

        private FrameworkElement GetRootVisual()
        {
            switch (_owner)
            {
                case null when _userDefinedAdorner is FrameworkElement userDefinedAdorner:
                    return userDefinedAdorner;

                case null when System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted:
                    return null;

                case null when Application.Current.MainWindow != null && (Application.Current.MainWindow.Content as FrameworkElement) != null:
                    return (FrameworkElement)Application.Current.MainWindow.Content;

                case null:
                    return Application.Current.MainWindow;
            }

            if (PinnableToolTipService.RootVisual != null)
            {
                return PinnableToolTipService.RootVisual;
            }

            return (FrameworkElement)_owner.GetVisualRoot();
        }

        /// <summary>
        ///     The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetCurrentValue(AccentColorBrushProperty, TryFindResource("AccentColorBrush") as SolidColorBrush);

            _closeButton = GetTemplateChild("CloseButton") as Button;
            if (_closeButton != null)
            {
                _closeButton.Click += OnCloseButtonClick;
            }

            _dragGrip = GetTemplateChild("DragGrip") as FrameworkElement;
            if (_dragGrip != null)
            {
                _dragGrip.PreviewMouseLeftButtonDown += OnDragGripPreviewMouseLeftButtonDown;
            }

            _gripDrawing = GetTemplateChild("GripDrawing") as GeometryDrawing;
        }
#endregion

#region Methods
        private static Point CalculatePoint(IList<Point> target, PlacementMode placement, Rect plugin, double width, double height,
            IList<Point> pointArray, int index, Rect bounds)
        {
            var x = pointArray[index].X;
            var y = pointArray[index].Y;

            if (index <= 1)
            {
                return new Point(x, y);
            }

            switch (placement)
            {
                case PlacementMode.Left:
                case PlacementMode.Right:
                    if (!(Math.Abs(y - target[0].Y) > Epsilon)
                        || !(Math.Abs(y - target[1].Y) > Epsilon)
                        || !(Math.Abs(y + height - target[0].Y) > Epsilon)
                        || !(Math.Abs(y + height - target[1].Y) > Epsilon))
                    {
                        return new Point(x, y);
                    }

                    y = CalculateLinearSize(plugin.Height, height, bounds.Top, bounds.Height);
                    break;

                case PlacementMode.Top:
                case PlacementMode.Bottom:
                    if (!(Math.Abs(x - target[0].X) > 0.0001)
                        || !(Math.Abs(x - target[1].X) > 0.0001)
                        || !(Math.Abs(x + width - target[0].X) > 0.0001)
                        || !(Math.Abs(x + width - target[1].X) > 0.0001))
                    {
                        return new Point(x, y);
                    }

                    x = CalculateLinearSize(plugin.Width, width, bounds.Left, bounds.Width);
                    break;

            }

            return new Point(x, y);
        }

        private static double CalculateLinearSize(double pluginLength, double length, double boundsStart, double boundLenght)
        {
            var middle = boundsStart + boundLenght / 2.0;
            if (middle > 0.0 && middle - 0.0 > pluginLength - middle)
            {
                return pluginLength - length;
            }

            return 0.0;
        }

        private static Rect GetBounds(params Point[] interestPoints)
        {
            double num2;
            double num4;
            var x = num2 = interestPoints[0].X;
            var y = num4 = interestPoints[0].Y;
            for (var i = 1; i < interestPoints.Length; i++)
            {
                var num6 = interestPoints[i].X;
                var num7 = interestPoints[i].Y;
                if (num6 < x)
                {
                    x = num6;
                }

                if (num6 > num2)
                {
                    num2 = num6;
                }

                if (num7 < y)
                {
                    y = num7;
                }

                if (num7 > num4)
                {
                    num4 = num7;
                }
            }

            return new Rect(x, y, (num2 - x) + 1.0, (num4 - y) + 1.0);
        }

        private static int GetIndex(Rect plugin, double width, double height, IList<Point> pointArray)
        {
            var num13 = width * height;
            var index = 0;
            var num15 = 0.0;
            for (var i = 0; i < pointArray.Count; i++)
            {
                var rect3 = new Rect(pointArray[i].X, pointArray[i].Y, width, height);
                rect3.Intersect(plugin);
                var d = rect3.Width * rect3.Height;
                if (double.IsInfinity(d))
                {
                    index = pointArray.Count - 1;
                    break;
                }

                if (d > num15)
                {
                    index = i;
                    num15 = d;
                }

                if (Math.Abs(d - num13) > Epsilon)
                {
                    continue;
                }

                index = i;
                break;
            }

            return index;
        }

        private static Point[] GetPointArray(IList<Point> target, PlacementMode placement, Rect plugin, double width, double height)
        {
            Point[] pointArray;

            switch (placement)
            {
                case PlacementMode.Bottom:
                    pointArray = new[]
                    {
                        new Point(target[2].X, Math.Max(0.0, target[2].Y + 1.0)),
                        new Point((target[3].X - width) + 1.0, Math.Max(0.0, target[2].Y + 1.0)),
                        new Point(0.0, Math.Max(0.0, target[2].Y + 1.0))
                    };
                    break;

                case PlacementMode.Right:
                    pointArray = new[]
                    {
                        new Point(Math.Max(0.0, target[1].X + 1.0), target[1].Y),
                        new Point(Math.Max(0.0, target[3].X + 1.0), (target[3].Y - height) + 1.0),
                        new Point(Math.Max(0.0, target[1].X + 1.0), 0.0)
                    };
                    break;

                case PlacementMode.Left:
                    pointArray = new[]
                    {
                        new Point(Math.Min(plugin.Width, target[0].X) - width, target[1].Y),
                        new Point(Math.Min(plugin.Width, target[2].X) - width, (target[3].Y - height) + 1.0),
                        new Point(Math.Min(plugin.Width, target[0].X) - width, 0.0)
                    };
                    break;

                case PlacementMode.Top:
                    pointArray = new[]
                    {
                        new Point(target[0].X, Math.Min(target[0].Y, plugin.Height) - height),
                        new Point((target[1].X - width) + 1.0, Math.Min(target[0].Y, plugin.Height) - height),
                        new Point(0.0, Math.Min(target[0].Y, plugin.Height) - height)
                    };
                    break;

                default:
                    pointArray = new[] { new Point(0.0, 0.0) };
                    break;
            }

            return pointArray;
        }

        private Point[] GetTranslatedPoints(FrameworkElement frameworkElement)
        {
            var pointArray = new Point[4];

            //var toolTip = this;
            var toolTip = frameworkElement as PinnableToolTip;
            if (toolTip is null || toolTip.IsOpen)
            {
                GeneralTransform generalTransform = new TranslateTransform(0, 0);

                var elementToTransform = toolTip != null ? toolTip._adornerLayer : PinnableToolTipService.RootVisual;

#if TRACE_DETAILS
                Debug.WriteLine($"Element to transform: '{elementToTransform}', placement target: '{frameworkElement}'");
#endif

                if (elementToTransform != null)
                {
                    generalTransform = frameworkElement.TransformToVisual(elementToTransform);
                }

                pointArray[0] = generalTransform.Transform(new Point(0.0, 0.0));
                pointArray[1] = generalTransform.Transform(new Point(frameworkElement.ActualWidth, 0.0));
                pointArray[1].X--;
                pointArray[2] = generalTransform.Transform(new Point(0.0, frameworkElement.ActualHeight));
                pointArray[2].Y--;
                pointArray[3] = generalTransform.Transform(new Point(frameworkElement.ActualWidth, frameworkElement.ActualHeight));
                pointArray[3].X--;
                pointArray[3].Y--;
            }

            return pointArray;
        }

        private void OnHorizontalOffsetChanged()
        {
            if (IsOpen)
            {
                PerformPlacement();
            }
        }

        private void OnIsOpenChanged()
        {
            UpdateResizingAdorner();

            IsOpenChanged.SafeInvoke(this);

            if (IsPinned)
            {
                // Stop pinning
                SetCurrentValue(IsPinnedProperty, false);
            }
        }

        private void OnIsPinnedChanged()
        {
            if (IsPinned)
            {
#if TRACE_DETAILS
                Debug.WriteLine("ToolTip just got pinned");
#endif

                if (_adornerDragDrop is null && _adorner != null)
                {
                    _adornerDragDrop = ControlAdornerDragDrop.Attach(_adorner, _dragGrip);
                }

                StopTimer();
            }
            else
            {
#if TRACE_DETAILS
                Debug.WriteLine("ToolTip just got unpinned");
#endif

                if (_adornerDragDrop != null)
                {
                    ControlAdornerDragDrop.Detach(_adornerDragDrop);
                    _adornerDragDrop = null;
                }

                Hide();
            }

            IsPinnedChanged.SafeInvoke(this);
        }

        private void OnVerticalOffsetChanged()
        {
            if (IsOpen)
            {
                PerformPlacement();
            }
        }

        private void OnGripColorChanged()
        {
            _gripDrawing?.SetCurrentValue(GeometryDrawing.BrushProperty, new SolidColorBrush(GripColor));
        }

        private Point PlacePopup(Rect plugin, Point[] target, Point[] toolTip, PlacementMode placement)
        {
            var bounds = GetBounds(target);
            var rect2 = GetBounds(toolTip);
            var width = rect2.Width;
            var height = rect2.Height;

            placement = ValidatePlacement(target, placement, plugin, width, height);

            var pointArray = GetPointArray(target, placement, plugin, width, height);
            var index = GetIndex(plugin, width, height, pointArray);
            var point = CalculatePoint(target, placement, plugin, width, height, pointArray, index, bounds);

#if TRACE_DETAILS
            Debug.WriteLine($"Placing popup");
            Debug.WriteLine($"  Target points:");

            foreach (var targetPoint in target)
            {
                Debug.WriteLine($"  '{targetPoint.X}, {targetPoint.Y}'");
            }

            Debug.WriteLine($"  ToolTip points:");

            foreach (var toolTipPoint in toolTip)
            {
                Debug.WriteLine($"  '{toolTipPoint.X}, {toolTipPoint.Y}'");
            }
#endif

            return point;
        }

        private PlacementMode ValidatePlacement(IList<Point> target, PlacementMode placement, Rect plugin, double width, double height)
        {
            // If we are in pinned mode, stop using the placement
            if (_adornerDragDrop != null)
            {
                return PlacementMode.AbsolutePoint;
            }

            switch (placement)
            {
                case PlacementMode.Right:
                    var num5 = Math.Max(0.0, target[0].X - 1.0);
                    var num6 = plugin.Width - Math.Min(plugin.Width, target[1].X + 1.0);
                    if ((num6 < width) && (num6 < num5))
                    {
                        placement = PlacementMode.Left;
                    }
                    break;

                case PlacementMode.Left:
                    var num7 = Math.Min(plugin.Width, target[1].X + width) - target[1].X;
                    var num8 = target[0].X - Math.Max(0.0, target[0].X - width);
                    if ((num8 < width) && (num8 < num7))
                    {
                        placement = PlacementMode.Right;
                    }
                    break;

                case PlacementMode.Top:
                    var num9 = target[0].Y - Math.Max(0.0, target[0].Y - height);
                    var num10 = Math.Min(plugin.Height, plugin.Height - height) - target[2].Y;
                    if ((num9 < height) && (num9 < num10))
                    {
                        placement = PlacementMode.Bottom;
                    }
                    break;

                case PlacementMode.Bottom:
                    var num11 = Math.Max(0.0, target[0].Y);
                    var num12 = plugin.Height - Math.Min(plugin.Height, target[2].Y);
                    if ((num12 < height) && (num12 < num11))
                    {
                        placement = PlacementMode.Top;
                    }
                    break;
            }

            return placement;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OnDragGripPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsPinned)
            {
                SetCurrentValue(IsPinnedProperty, true);
            }

            BringToFront();
        }

        private void OnPinnableToolTipMouseDown(object sender, MouseButtonEventArgs e)
        {
            BringToFront();
        }

        private void OnPinnableToolTipMouseLeave(object sender, MouseEventArgs e)
        {
            if (_ignoreTimerStartupWhenMouseLeave)
            {
                return;
            }

            if (IsOpen && !IsPinned)
            {
                StartTimer();
            }
        }

        private void OnPinnableToolTipMouseEnter(object sender, MouseEventArgs e)
        {
            if (IsTimerEnabled && !IsPinned)
            {
                StopTimer(false);
            }
        }

        internal void PerformPlacement()
        {
            _adornerLayer?.Update();
        }

        public void SetUserDefinedAdorner(UIElement element)
        {
            _userDefinedAdorner = element;
        }

        public void BringToFront()
        {
            if (IsInFront())
            {
                return;
            }

            if (!IsPinned || _adorner is null)
            {
                return;
            }

            if (_adornerDragDrop != null)
            {
                ControlAdornerDragDrop.Detach(_adornerDragDrop);
                _adornerDragDrop = null;
            }

            _adornerLayer.Remove(_adorner);

            var adornedElement = GetAdornerElement();
            if (adornedElement is null)
            {
                return;
            }

            _adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            if (_adornerLayer is null)
            {
                return;
            }

            _adornerLayer.Add(_adorner);
            BringFluentRibbonBackstageToFront(_adornerLayer, adornedElement);

            if (IsPinned && _adornerDragDrop is null)
            {
                _adornerDragDrop = ControlAdornerDragDrop.Attach(_adorner, _dragGrip);
            }

            RegisterBeingInFront();
        }

        public void SetupTimer(int initialShowDelay, int showDuration)
        {
            if (_timer != null)
            {
                if (_timer.IsEnabled)
                {
                    _timer.StopAndReset();
                }

                _timer.Tick -= OnTimerTick;
                _timer.Stopped -= OnTimerStopped;
            }

            _timer = new ToolTipTimer(TimeSpan.FromMilliseconds(showDuration), TimeSpan.FromMilliseconds(initialShowDelay));
            _timer.Tick += OnTimerTick;
            _timer.Stopped += OnTimerStopped;
        }

        public void StartTimer()
        {
            _timer?.StartAndReset();
        }

        public void StopTimer(bool reset = true)
        {
            if (_timer is null || !IsTimerEnabled)
            {
                return;
            }

            if (reset)
            {
                _timer.StopAndReset();
            }
            else
            {
                _timer.Stop();
            }
        }

        public void Show()
        {
            if (ContentTemplate != null)
            {
                if (Owner is FrameworkElement owner)
                {
                    SetCurrentValue(ContentProperty, owner.DataContext);
                }
            }
            else
            {
                var binding = new Binding
                {
                    Source = Owner,
                    Path = new PropertyPath("DataContext")
                };

                SetBinding(DataContextProperty, binding);
            }

            IsOpen = true;
        }

        public void Hide()
        {
            if (!IsOpen)
            {
                return;
            }

            IsOpen = false;

            BindingOperations.ClearBinding(this, DataContextProperty);
            _lastPosition = new Point(0, 0);
        }

        private void CreateAdorner()
        {
            if (_adorner != null || (Application.Current.MainWindow is null && _userDefinedAdorner is null))
            {
                return;
            }

            var adornedElement = GetAdornerElement();
            if (adornedElement is null)
            {
                return;
            }

            var layer = AdornerLayer.GetAdornerLayer(adornedElement);
            if (layer is null)
            {
                return;
            }

            _isPositionCalculated = false;

            var ad = new ControlAdorner(adornedElement)
            {
                Child = this,
                Focusable = false
            };

            KeyboardNavigation.SetTabNavigation(ad, KeyboardNavigationMode.None);
            layer.Add(ad);

            BringFluentRibbonBackstageToFront(layer, adornedElement);

            _adorner = ad;
            _adornerLayer = layer;

            if (IsPinned && _adornerDragDrop is null)
            {
                _adornerDragDrop = ControlAdornerDragDrop.Attach(_adorner, _dragGrip);
            }

            RegisterBeingInFront();
        }

        private static void BringFluentRibbonBackstageToFront(AdornerLayer layer, UIElement adornedElement)
        {
            // This is a little bit dirty way to keep the ribbon backstage the topmost.
            // I couldn't find a better way to reorder elements within AdornerLayers
            var adorners = layer.GetAdorners(adornedElement);
            if (adorners is null)
            {
                return;
            }

            const string FLUENT_RIBBON_TYPE_NAME = "Fluent.BackstageAdorner";
            foreach (var adorner in adorners)
            {
                if (!adorner.GetType().FullName.Equals(FLUENT_RIBBON_TYPE_NAME))
                {
                    continue;
                }

                layer.Remove(adorner);
                layer.Add(adorner);
                break;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _lastSize = e.NewSize;
        }

        private void OnTimerStopped(object sender, EventArgs e)
        {
            if (!IsPinned && !IsMouseOver)
            {
                Hide();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (IsOpen)
            {
                return;
            }

            if (_timer.IsEnabled && _timer.MaximumTicks.TotalMilliseconds > 0 && _timer.CurrentTick >= _timer.InitialDelay.TotalMilliseconds)
            {
                Show();
            }
        }

        private void RemoveAdorner()
        {
            if (_adorner is null || _adornerLayer is null)
            {
                return;
            }

            if (_adornerDragDrop != null)
            {
                ControlAdornerDragDrop.Detach(_adornerDragDrop);
                _adornerDragDrop = null;
            }

            if (_adornerResizing != null)
            {
                ResizingAdorner.Detach(_adornerResizing);
                _adornerResizing = null;
            }

            _adornerLayer.Remove(_adorner);
            _adorner.Child = null;
            _adorner = null;
            _adornerLayer = null;

            RegisterBeingMovedOut();
        }

        private UIElement GetAdornerElement()
        {
            if (_userDefinedAdorner != null)
            {
                return _userDefinedAdorner;
            }

            var root = Owner.GetVisualRoot() as ContentControl;

            return root?.Content as FrameworkElement
                   ?? root
                   ?? Application.Current.MainWindow.Content as FrameworkElement
                   ?? Application.Current.MainWindow;
        }

        private List<int> GetCurrentAdornersLayers()
        {
            return OnFrontIdDictionary.GetOrAdd(GetAdornerElement(), new List<int>());
        }

        /// <summary>
        /// The register being in front.
        /// </summary>
        private void RegisterBeingInFront()
        {
            var layers = GetCurrentAdornersLayers();
            lock (layers)
            {
                layers.Remove(_id);
                layers.Add(_id);
            }
        }

        private void RegisterBeingMovedOut()
        {
            var layers = GetCurrentAdornersLayers();
            lock (layers)
            {
                layers.Remove(_id);
            }
        }

        private int GetInFrontId()
        {
            var layers = GetCurrentAdornersLayers();
            lock (layers)
            {
                return layers.LastOrDefault();
            }
        }

        private bool IsInFront()
        {
            return GetInFrontId() == _id;
        }

        private void OnAccentColorBrushChanged()
        {
            if (!(AccentColorBrush is SolidColorBrush brush))
            {
                return;
            }

            var accentColor = brush.Color;
            accentColor.CreateAccentColorResourceDictionary("PinnableToolTip");
        }

        private void OnResizeModeChanged()
        {
            UpdateResizingAdorner();
        }

        private void UpdateResizingAdorner()
        {
            if (IsOpen && (ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip))
            {
                if (_adornerResizing is null && _adorner != null)
                {
                    _adornerResizing = ResizingAdorner.Attach(this);
                }
            }
            else
            {
                if (_adornerResizing is null)
                {
                    return;
                }

                ResizingAdorner.Detach(_adornerResizing);
                _adornerResizing = null;
            }
        }
#endregion
    }
}
