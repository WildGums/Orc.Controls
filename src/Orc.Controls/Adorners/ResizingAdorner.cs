// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResizingAdorner.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel;
    using Catel.Reflection;

    public class ResizingAdorner : Adorner
    {
        #region Constants
        private const string HorizontalOffsetProperty = "HorizontalOffset";
        private const string VerticalOffsetProperty = "VerticalOffset";
        private const double CornerSize = 4d;
        #endregion

        #region Fields
        private readonly Thumb _bottom;
        private readonly Thumb _bottomLeft;
        private readonly Thumb _bottomRight;

        private readonly Thumb _left;
        private readonly Thumb _right;
        private readonly Thumb _top;
        private readonly Thumb _topLeft;
        private readonly Thumb _topRight;
        private readonly VisualCollection _visualChildren;

        private bool _hasCanvas;
        private bool _hasHorizontalOffset;
        private bool _hasVerticalOffset;
        #endregion

        #region Constructors
        private ResizingAdorner(FrameworkElement adornedElement)
            : base(adornedElement)
        {
            _visualChildren = new VisualCollection(this);

            BuildAdornerElement(ref _left, Cursors.SizeWE);
            BuildAdornerElement(ref _topLeft, Cursors.SizeNWSE);
            BuildAdornerElement(ref _top, Cursors.SizeNS);
            BuildAdornerElement(ref _topRight, Cursors.SizeNESW);
            BuildAdornerElement(ref _right, Cursors.SizeWE);
            BuildAdornerElement(ref _bottomRight, Cursors.SizeNWSE);
            BuildAdornerElement(ref _bottom, Cursors.SizeNS);
            BuildAdornerElement(ref _bottomLeft, Cursors.SizeNESW);
        }
        #endregion

        #region Properties
        protected override int VisualChildrenCount => _visualChildren.Count;
        #endregion

        #region Methods
        public static ResizingAdorner Attach(FrameworkElement element)
        {
            ArgumentNullException.ThrowIfNull(element);

            var adorner = new ResizingAdorner(element);

            if (adorner.AdornedElement is FrameworkElement fxElement)
            {
                fxElement.Loaded += adorner.OnAdornedElementLoaded;
            }

            adorner._left.DragDelta += adorner.HandleLeft;
            adorner._topLeft.DragDelta += adorner.HandleTopLeft;
            adorner._top.DragDelta += adorner.HandleTop;
            adorner._topRight.DragDelta += adorner.HandleTopRight;
            adorner._right.DragDelta += adorner.HandleRight;
            adorner._bottomRight.DragDelta += adorner.HandleBottomRight;
            adorner._bottom.DragDelta += adorner.HandleBottom;
            adorner._bottomLeft.DragDelta += adorner.HandleBottomLeft;

            var adornerLayer = AdornerLayer.GetAdornerLayer(element);
            adornerLayer.Add(adorner);

            adorner.UpdateState();

            return adorner;
        }

        public static void Detach(ResizingAdorner adorner)
        {
            ArgumentNullException.ThrowIfNull(adorner);

            var adornerLayer = AdornerLayer.GetAdornerLayer(adorner.AdornedElement);
            if (adornerLayer is null)
            {
                return;
            }

            adornerLayer.Remove(adorner);

            adorner._visualChildren.Clear();

            if (adorner.AdornedElement is FrameworkElement fxElement)
            {
                fxElement.Loaded -= adorner.OnAdornedElementLoaded;
            }

            adorner._left.DragDelta -= adorner.HandleLeft;
            adorner._topLeft.DragDelta -= adorner.HandleTopLeft;
            adorner._top.DragDelta -= adorner.HandleTop;
            adorner._topRight.DragDelta -= adorner.HandleTopRight;
            adorner._right.DragDelta -= adorner.HandleRight;
            adorner._bottomRight.DragDelta -= adorner.HandleBottomRight;
            adorner._bottom.DragDelta -= adorner.HandleBottom;
            adorner._bottomLeft.DragDelta -= adorner.HandleBottomLeft;
        }

        private void OnAdornedElementLoaded(object sender, RoutedEventArgs e)
        {
            EnforceSize((FrameworkElement)sender);
        }

        private void HandleLeft(object sender, DragDeltaEventArgs args)
        {
            HandleResize(args.HorizontalChange);
        }

        private void HandleTopLeft(object sender, DragDeltaEventArgs args)
        {
            HandleResize(args.HorizontalChange, args.VerticalChange);
        }

        private void HandleTop(object sender, DragDeltaEventArgs args)
        {
            HandleResize(top: args.VerticalChange);
        }

        private void HandleTopRight(object sender, DragDeltaEventArgs args)
        {
            HandleResize(top: args.VerticalChange, right: args.HorizontalChange);
        }

        private void HandleRight(object sender, DragDeltaEventArgs args)
        {
            HandleResize(right: args.HorizontalChange);
        }

        private void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            HandleResize(right: args.HorizontalChange, bottom: args.VerticalChange);
        }

        private void HandleBottom(object sender, DragDeltaEventArgs args)
        {
            HandleResize(bottom: args.VerticalChange);
        }

        private void HandleBottomLeft(object sender, DragDeltaEventArgs args)
        {
            HandleResize(args.HorizontalChange, bottom: args.VerticalChange);
        }

        private void HandleResize(double? left = null, double? top = null, double? right = null, double? bottom = null)
        {
            if (AdornedElement is not FrameworkElement adornedElement)
            {
                return;
            }

            EnforceSize(adornedElement);

            if (UpdateWidthByLeftCornerViaCanvas(left, adornedElement) || UpdateWidthByRightCornerViaCanvas(right, adornedElement))
            {
                var difference = Math.Abs(Canvas.GetRight(adornedElement) - Canvas.GetLeft(adornedElement));

                UpdateWidth(difference);
            }

            if (UpdateHeightByTopCornerViaCanvas(top, adornedElement) || UpdateHeightByBottomCornerViaCanvas(bottom, adornedElement))
            {
                var difference = Math.Abs(Canvas.GetBottom(adornedElement) - Canvas.GetTop(adornedElement));

                UpdateHeight(difference);
            }
        }

        private bool UpdateHeightByBottomCornerViaCanvas(double? bottom, FrameworkElement frameworkElement)
        {
            if (!bottom.HasValue)
            {
                return false;
            }

            if (_hasCanvas)
            {
                var oldBottom = Canvas.GetBottom(frameworkElement);
                var newValue = oldBottom + bottom.Value;
                Canvas.SetBottom(frameworkElement, newValue);

                return true;
            }

            UpdateHeight(frameworkElement.Height + bottom.Value);

            return false;
        }

        private bool UpdateHeightByTopCornerViaCanvas(double? top, FrameworkElement frameworkElement)
        {
            if (!top.HasValue)
            {
                return false;
            }

            if (_hasCanvas)
            {
                Canvas.SetTop(frameworkElement, Canvas.GetTop(frameworkElement) + top.Value);

                return true;
            }

            if (!_hasVerticalOffset)
            {
                return false;
            }

            var oldTop = PropertyHelper.GetPropertyValue<double>(frameworkElement, VerticalOffsetProperty);
            var newValue = oldTop + top.Value;
            PropertyHelper.SetPropertyValue(frameworkElement, VerticalOffsetProperty, newValue);

            UpdateHeight(frameworkElement.Height - top.Value);

            return false;
        }

        private bool UpdateWidthByLeftCornerViaCanvas(double? left, FrameworkElement frameworkElement)
        {
            if (!left.HasValue)
            {
                return false;
            }

            if (_hasCanvas)
            {
                Canvas.SetLeft(frameworkElement, Canvas.GetLeft(frameworkElement) + left.Value);

                return true;
            }

            if (!_hasHorizontalOffset)
            {
                return false;
            }

            var oldLeft = PropertyHelper.GetPropertyValue<double>(frameworkElement, HorizontalOffsetProperty);
            var newValue = oldLeft + left.Value;
            PropertyHelper.SetPropertyValue(frameworkElement, HorizontalOffsetProperty, newValue);

            UpdateWidth(frameworkElement.Width - left.Value);

            return false;
        }

        private bool UpdateWidthByRightCornerViaCanvas(double? right, FrameworkElement frameworkElement)
        {
            if (!right.HasValue)
            {
                return false;
            }

            if (_hasCanvas)
            {
                Canvas.SetRight(frameworkElement, Canvas.GetRight(frameworkElement) + right.Value);

                return true;
            }

            UpdateWidth(frameworkElement.Width + right.Value);

            return false;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var desiredWidth = AdornedElement.DesiredSize.Width;
            var desiredHeight = AdornedElement.DesiredSize.Height;

            const double fullSize = CornerSize;
            const double halfSize = CornerSize / 2;

            UpdateSizeAndPosition(_left, -halfSize, halfSize, fullSize, desiredHeight - fullSize);
            UpdateSizeAndPosition(_topLeft, -halfSize, -halfSize, fullSize, fullSize);
            UpdateSizeAndPosition(_top, halfSize, -halfSize, desiredWidth - fullSize, fullSize);
            UpdateSizeAndPosition(_topRight, desiredWidth - halfSize, -halfSize, fullSize, fullSize);
            UpdateSizeAndPosition(_right, desiredWidth - halfSize, halfSize, fullSize, desiredHeight - fullSize);
            UpdateSizeAndPosition(_bottomRight, desiredWidth - halfSize, desiredHeight - halfSize, fullSize, fullSize);
            UpdateSizeAndPosition(_bottom, halfSize, desiredHeight - halfSize, desiredWidth - fullSize, fullSize);
            UpdateSizeAndPosition(_bottomLeft, -halfSize, desiredHeight - halfSize, fullSize, fullSize);

            return finalSize;
        }

        private void UpdateState()
        {
            var adornedElement = AdornedElement;

            // Note: we only enable the left / top resizing if the adorned object either is used on a canvas
            // or when it has a HorizontalOffset and VerticalOffset property

            var enableLeft = false;

            var leftCanvas = Canvas.GetLeft(adornedElement);
            if (!double.IsNaN(leftCanvas))
            {
                _hasCanvas = true;
                enableLeft = true;
            }
            else if (PropertyHelper.IsPropertyAvailable(adornedElement, HorizontalOffsetProperty))
            {
                _hasHorizontalOffset = true;
                enableLeft = true;
            }

            _bottomLeft.SetCurrentValue(IsEnabledProperty, enableLeft);
            _left.SetCurrentValue(IsEnabledProperty, enableLeft);

            var enableTop = false;

            var topCanvas = Canvas.GetTop(adornedElement);
            if (!double.IsNaN(topCanvas))
            {
                _hasCanvas = true;
                enableTop = true;
            }
            else if (PropertyHelper.IsPropertyAvailable(adornedElement, VerticalOffsetProperty))
            {
                _hasVerticalOffset = true;
                enableTop = true;
            }

            _top.SetCurrentValue(IsEnabledProperty, enableTop);
            _topLeft.SetCurrentValue(IsEnabledProperty, enableLeft && enableTop);
        }

        private void UpdateWidth(double width)
        {
            if (AdornedElement is not FrameworkElement adornedElement)
            {
                return;
            }

            if (!double.IsNaN(adornedElement.MinWidth) && adornedElement.MinWidth > width)
            {
                width = adornedElement.MinWidth;
            }

            adornedElement.SetCurrentValue(WidthProperty, width);
        }

        private void UpdateHeight(double height)
        {
            if (AdornedElement is not FrameworkElement adornedElement)
            {
                return;
            }

            if (!double.IsNaN(adornedElement.MinHeight) && adornedElement.MinHeight > height)
            {
                height = adornedElement.MinHeight;
            }

            adornedElement.SetCurrentValue(HeightProperty, height);
        }

        private void UpdateSizeAndPosition(Thumb thumb, double left, double right, double width, double height)
        {
            if (width < 0d)
            {
                width = 0d;
            }

            if (height < 0d)
            {
                height = 0d;
            }

            thumb.SetCurrentValue(WidthProperty, width);
            thumb.SetCurrentValue(HeightProperty, height);

            thumb.Arrange(new Rect(left, right, thumb.Width, thumb.Height));
        }

        private void BuildAdornerElement(ref Thumb thumb, Cursor customizedCursor)
        {
            if (thumb is not null)
            {
                return;
            }

            thumb = new Thumb
            {
                //Background = new SolidColorBrush(Colors.MediumBlue),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0d),
                Cursor = customizedCursor,
                //Opacity = 0.40d,
                OpacityMask = Brushes.Transparent, // ORCOMP-338 - Make thumb transparent
                Width = CornerSize,
                Height = CornerSize
            };

            _visualChildren.Add(thumb);
        }

        private void EnforceSize(FrameworkElement adornedElement)
        {
            if (adornedElement.Width.Equals(double.NaN))
            {
                adornedElement.SetCurrentValue(WidthProperty, adornedElement.DesiredSize.Width);
            }

            if (adornedElement.Height.Equals(double.NaN))
            {
                adornedElement.SetCurrentValue(HeightProperty, adornedElement.DesiredSize.Height);
            }

            if (double.IsNaN(Canvas.GetLeft(adornedElement)))
            {
                Canvas.SetLeft(adornedElement, 0d);
            }

            if (double.IsNaN(Canvas.GetTop(adornedElement)))
            {
                Canvas.SetTop(adornedElement, 0d);
            }

            if (double.IsNaN(Canvas.GetRight(adornedElement)))
            {
                Canvas.SetRight(adornedElement, adornedElement.Width);
            }

            if (double.IsNaN(Canvas.GetBottom(adornedElement)))
            {
                Canvas.SetBottom(adornedElement, adornedElement.Height);
            }

            if (adornedElement.Parent is FrameworkElement parent)
            {
                adornedElement.SetCurrentValue(MaxHeightProperty, parent.ActualHeight);
                adornedElement.SetCurrentValue(MaxWidthProperty, parent.ActualWidth);
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visualChildren[index];
        }
        #endregion
    }
}
