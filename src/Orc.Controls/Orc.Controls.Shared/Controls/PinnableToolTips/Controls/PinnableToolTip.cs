// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PinnableToolTip.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
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
        internal bool IsTimerEnabled
        {
            get { return _timer != null && _timer.IsEnabled; }
        }
        #endregion

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

        private AdornerLayer _adornerLayer;

        private bool _isPositionCalculated;

        private Point _lastPosition;

        private Size _lastSize;

        private UIElement _owner;

        private UIElement _userDefinedAdorner;

        private ToolTipTimer _timer;
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
                }

                OnIsOpenChanged();
            }
        }

        public bool AllowCloseByUser
        {
            get { return (bool)GetValue(AllowCloseByUserProperty); }
            set { SetValue(AllowCloseByUserProperty, value); }
        }

        public static readonly DependencyProperty AllowCloseByUserProperty = DependencyProperty.Register("AllowCloseByUser", typeof(bool),
            typeof(PinnableToolTip), new PropertyMetadata(false));


        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double),
            typeof(PinnableToolTip), new PropertyMetadata((sender, e) => ((PinnableToolTip)sender).OnHorizontalOffsetChanged()));


        public bool IsPinned
        {
            get { return (bool)GetValue(IsPinnedProperty); }
            set { SetValue(IsPinnedProperty, value); }
        }

        public static readonly DependencyProperty IsPinnedProperty = DependencyProperty.Register("IsPinned", typeof(bool), typeof(PinnableToolTip),
            new PropertyMetadata(false, (sender, e) => ((PinnableToolTip)sender).OnIsPinnedChanged()));


        public Color GripColor
        {
            get { return (Color)GetValue(GripColorProperty); }
            set { SetValue(GripColorProperty, value); }
        }

        public static readonly DependencyProperty GripColorProperty = DependencyProperty.Register("GripColor", typeof(Color), typeof(PinnableToolTip),
            new PropertyMetadata(Color.FromRgb(204, 204, 204), (sender, e) => ((PinnableToolTip)sender).OnGripColorChanged()));


        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double),
            typeof(PinnableToolTip), new PropertyMetadata((sender, e) => ((PinnableToolTip)sender).OnVerticalOffsetChanged()));


        public ICommand OpenLinkCommand
        {
            get { return (ICommand)GetValue(OpenLinkCommandProperty); }
            set { SetValue(OpenLinkCommandProperty, value); }
        }

        public static readonly DependencyProperty OpenLinkCommandProperty = DependencyProperty.Register("OpenLinkCommand",
            typeof(ICommand), typeof(PinnableToolTip), new PropertyMetadata(null));

        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register("AccentColorBrush", typeof(Brush),
            typeof(PinnableToolTip), new PropertyMetadata(Brushes.LightGray, (sender, e) => ((PinnableToolTip)sender).OnAccentColorBrushChanged()));
        #endregion

        #region Events
        public event EventHandler<EventArgs> IsOpenChanged;

        public event EventHandler<EventArgs> IsPinnedChanged;
        #endregion

        #region Commands

        #endregion

        #region Public Methods and Operators
        public Point GetPosition()
        {
            var position = new Point();

            var mousePosition = PinnableToolTipService.MousePosition;
            var rootVisual = PinnableToolTipService.RootVisual;

            var horizontalOffset = HorizontalOffset;
            var verticalOffset = VerticalOffset;

            //using this code for non UIElements
            if (_owner == null)
            {
                mousePosition = Mouse.GetPosition(_userDefinedAdorner);

                if ((_userDefinedAdorner as FrameworkElement) == null)
                {
                    rootVisual = System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted
                        ? null
                        : (Application.Current.MainWindow.Content as FrameworkElement) != null
                            ? Application.Current.MainWindow.Content as FrameworkElement
                            : Application.Current.MainWindow;
                }
                else
                {
                    rootVisual = _userDefinedAdorner as FrameworkElement;
                }

                if (rootVisual == null)
                {
                    position = _isPositionCalculated ? _lastPosition : mousePosition;
                    return position;
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

                    position = _lastPosition;
                    return position;
                }

                var offsetX = mousePosition.X + horizontalOffset;
                var offsetY = mousePosition.Y + verticalOffset;

                //offsetX = Math.Max(2.0, offsetX);
                //offsetY = Math.Max(2.0, offsetY);

                var actualHeight = rootVisual.ActualHeight;
                var actualWidth = rootVisual.ActualWidth;
                var lastHeight = _lastSize.Height;
                var lastWidth = _lastSize.Width;

                var lastRectangle = new Rect(offsetX, offsetY, lastWidth, lastHeight);
                var actualRectangle = new Rect(0.0, 0.0, actualWidth, actualHeight);
                actualRectangle.Intersect(lastRectangle);

                if ((offsetY + DesiredSize.Height) > actualHeight)
                {
                    offsetY = (actualHeight - DesiredSize.Height) - 2.0;
                }

                if (offsetY < 0.0)
                {
                    offsetY = 0.0;
                }

                if ((offsetX + DesiredSize.Width) > actualWidth)
                {
                    offsetX = (actualWidth - DesiredSize.Width) - 2.0;
                }

                if (offsetX < 0.0)
                {
                    offsetX = 0.0;
                }

                position.Y = offsetY;
                position.X = offsetX;

                _isPositionCalculated = DesiredSize.Height > 0;
                if (_isPositionCalculated)
                {
                    _lastPosition = position;
                }

                return position;
            }

            var placementMode = PinnableToolTipService.GetPlacement(_owner);
            var placementTarget = PinnableToolTipService.GetPlacementTarget(_owner) ?? _owner;

            switch (placementMode)
            {
                case PlacementMode.Mouse:
                    if (_isPositionCalculated)
                    {
                        position = _lastPosition;
                        return position;
                    }

                    var offsetX = mousePosition.X + horizontalOffset;
                    var offsetY = mousePosition.Y + new TextBlock().FontSize + verticalOffset;

                    offsetX = Math.Max(2.0, offsetX);
                    offsetY = Math.Max(2.0, offsetY);

                    var actualHeight = rootVisual.ActualHeight;
                    var actualWidth = rootVisual.ActualWidth;
                    var lastHeight = _lastSize.Height;
                    var lastWidth = _lastSize.Width;

                    var lastRectangle = new Rect(offsetX, offsetY, lastWidth, lastHeight);
                    var actualRectangle = new Rect(0.0, 0.0, actualWidth, actualHeight);
                    actualRectangle.Intersect(lastRectangle);

                    if ((Math.Abs(actualRectangle.Width - lastRectangle.Width) < 2.0)
                        && (Math.Abs(actualRectangle.Height - lastRectangle.Height) < 2.0))
                    {
                        position.Y = offsetY;
                        position.X = offsetX;
                    }
                    else
                    {
                        if ((offsetY + lastRectangle.Height) > actualHeight)
                        {
                            offsetY = (actualHeight - lastRectangle.Height) - 2.0;
                        }

                        if (offsetY < 0.0)
                        {
                            offsetY = 0.0;
                        }

                        if ((offsetX + lastRectangle.Width) > actualWidth)
                        {
                            offsetX = (actualWidth - lastRectangle.Width) - 2.0;
                        }

                        if (offsetX < 0.0)
                        {
                            offsetX = 0.0;
                        }

                        position.Y = offsetY;
                        position.X = offsetX;
                    }

                    _lastPosition = position;
                    _isPositionCalculated = true;
                    break;

                case PlacementMode.Bottom:
                case PlacementMode.Right:
                case PlacementMode.Left:
                case PlacementMode.Top:
                    var windowSize = ScreenHelper.GetWindowSize(null);
                    var plugin = new Rect(0.0, 0.0, windowSize.Width, windowSize.Height);
                    var translatedPoints = GetTranslatedPoints((FrameworkElement)placementTarget);
                    var toolTipPoints = GetTranslatedPoints(this);
                    var popupLocation = PlacePopup(plugin, translatedPoints, toolTipPoints, placementMode);

                    position.Y = popupLocation.Y + verticalOffset;
                    position.X = popupLocation.X + horizontalOffset;
                    break;
            }

            return position;
        }

        /// <summary>
        ///     The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AccentColorBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;

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
            if (index > 1)
            {
                if ((placement == PlacementMode.Left) || (placement == PlacementMode.Right))
                {
                    if (((Math.Abs(y - target[0].Y) > Epsilon) && (Math.Abs(y - target[1].Y) > Epsilon))
                        && ((Math.Abs((y + height) - target[0].Y) > Epsilon)
                            && (Math.Abs((y + height) - target[1].Y) > Epsilon)))
                    {
                        var num18 = bounds.Top + (bounds.Height / 2.0);
                        if ((num18 > 0.0) && ((num18 - 0.0) > (plugin.Height - num18)))
                        {
                            y = plugin.Height - height;
                        }
                        else
                        {
                            y = 0.0;
                        }
                    }
                }
                else if (((placement == PlacementMode.Top) || (placement == PlacementMode.Bottom))
                         && (((Math.Abs(x - target[0].X) > 0.0001) && (Math.Abs(x - target[1].X) > 0.0001))
                             && ((Math.Abs((x + width) - target[0].X) > 0.0001) && (Math.Abs((x + width) - target[1].X) > 0.0001))))
                {
                    var num19 = bounds.Left + (bounds.Width / 2.0);
                    if ((num19 > 0.0) && ((num19 - 0.0) > (plugin.Width - num19)))
                    {
                        x = plugin.Width - width;
                    }
                    else
                    {
                        x = 0.0;
                    }
                }
            }

            return new Point(x, y);
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

        private static Point[] GetPointArray(
            IList<Point> target, PlacementMode placement, Rect plugin, double width, double height)
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
                        new Point(
                            Math.Min(plugin.Width, target[2].X) - width, (target[3].Y - height) + 1.0),
                        new Point(Math.Min(plugin.Width, target[0].X) - width, 0.0)
                    };
                    break;

                case PlacementMode.Top:
                    pointArray = new[]
                    {
                        new Point(target[0].X, Math.Min(target[0].Y, plugin.Height) - height),
                        new Point(
                            (target[1].X - width) + 1.0, Math.Min(target[0].Y, plugin.Height) - height),
                        new Point(0.0, Math.Min(target[0].Y, plugin.Height) - height)
                    };
                    break;

                default:
                    pointArray = new[] { new Point(0.0, 0.0) };
                    break;
            }

            return pointArray;
        }

        private static Point[] GetTranslatedPoints(FrameworkElement frameworkElement)
        {
            var pointArray = new Point[4];

            var toolTip = frameworkElement as PinnableToolTip;
            if (toolTip == null || toolTip.IsOpen)
            {
                var generalTransform = frameworkElement.TransformToVisual(toolTip != null ? toolTip._adornerLayer : PinnableToolTipService.RootVisual);
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
            IsOpenChanged.SafeInvoke(this);
        }

        private void OnIsPinnedChanged()
        {
            if (IsPinned)
            {
                if (_adornerDragDrop == null && _adorner != null)
                {
                    _adornerDragDrop = ControlAdornerDragDrop.Attach(_adorner, _dragGrip);
                }

                StopTimer();
            }
            else
            {
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
            if (_gripDrawing != null)
            {
                _gripDrawing.Brush = new SolidColorBrush(GripColor);
            }
        }

        private static Point PlacePopup(Rect plugin, Point[] target, Point[] toolTip, PlacementMode placement)
        {
            var bounds = GetBounds(target);
            var rect2 = GetBounds(toolTip);
            var width = rect2.Width;
            var height = rect2.Height;

            placement = ValidatePlacement(target, placement, plugin, width, height);

            var pointArray = GetPointArray(target, placement, plugin, width, height);
            var index = GetIndex(plugin, width, height, pointArray);
            var point = CalculatePoint(target, placement, plugin, width, height, pointArray, index, bounds);

            return point;
        }

        private static PlacementMode ValidatePlacement(IList<Point> target, PlacementMode placement, Rect plugin, double width, double height)
        {
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
                IsPinned = true;
            }

            BringToFront();
        }

        private void OnPinnableToolTipMouseDown(object sender, MouseButtonEventArgs e)
        {
            BringToFront();
        }

        private void OnPinnableToolTipMouseLeave(object sender, MouseEventArgs e)
        {
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
            if (_adornerLayer == null)
            {
                return;
            }

            _adornerLayer.Update();
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

            if (IsPinned && _adorner != null)
            {
                if (_adornerDragDrop != null)
                {
                    ControlAdornerDragDrop.Detach(_adornerDragDrop);
                    _adornerDragDrop = null;
                }

                _adornerLayer.Remove(_adorner);

                var adornedElement = GetAdornerElement();
                if (adornedElement == null)
                {
                    return;
                }

                _adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
                if (_adornerLayer == null)
                {
                    return;
                }

                _adornerLayer.Add(_adorner);

                if (IsPinned && _adornerDragDrop == null)
                {
                    _adornerDragDrop = ControlAdornerDragDrop.Attach(_adorner, _dragGrip);
                }

                RegisterBeingInFront();
            }
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
            if (_timer != null)
            {
                _timer.StartAndReset();
            }
        }

        public void StopTimer(bool reset = true)
        {
            if (_timer != null && IsTimerEnabled)
            {
                if (reset)
                {
                    _timer.StopAndReset();
                }
                else
                {
                    _timer.Stop();
                }
            }
        }

        public void Show()
        {
            var binding = new Binding
            {
                Source = Owner,
                Path = new PropertyPath("DataContext")
            };

            SetBinding(PinnableToolTip.DataContextProperty, binding);

            IsOpen = true;
        }

        public void Hide()
        {
            IsOpen = false;

            BindingOperations.ClearBinding(this, PinnableToolTip.DataContextProperty);
            //SetOwner(null);
            _lastPosition = new Point(0, 0);
        }

        private void CreateAdorner()
        {
            if (_adorner != null || (Application.Current.MainWindow == null && _userDefinedAdorner == null))
            {
                return;
            }

            var adornedElement = GetAdornerElement();
            if (adornedElement == null)
            {
                return;
            }

            var layer = AdornerLayer.GetAdornerLayer(adornedElement);
            if (layer == null)
            {
                return;
            }

            _isPositionCalculated = false;
            var ad = new ControlAdorner(adornedElement) { Child = this, Focusable = false };
            KeyboardNavigation.SetTabNavigation(ad, KeyboardNavigationMode.None);
            layer.Add(ad);
            _adorner = ad;
            _adornerLayer = layer;

            if (IsPinned && _adornerDragDrop == null)
            {
                _adornerDragDrop = ControlAdornerDragDrop.Attach(_adorner, _dragGrip);
            }

            RegisterBeingInFront();
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
            if (_adorner == null || _adornerLayer == null)
            {
                return;
            }

            if (_adornerDragDrop != null)
            {
                ControlAdornerDragDrop.Detach(_adornerDragDrop);
                _adornerDragDrop = null;
            }

            _adornerLayer.Remove(_adorner);
            _adorner.Child = null;
            _adorner = null;
            _adornerLayer = null;

            RegisterBeingMovedOut();
        }

        private UIElement GetAdornerElement()
        {
            return _userDefinedAdorner ?? Application.Current.MainWindow.Content as UIElement;
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
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush)AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("PinnableToolTip");
            }
        }
        #endregion
    }
}