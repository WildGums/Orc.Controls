// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResizingAdorner.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
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
        private const string HorizontalOffsetProperty = "HorizontalOffset";
        private const string VerticalOffsetProperty = "VerticalOffset";
        private const double CornerSize = 4d;

        private readonly Thumb _left;
        private readonly Thumb _topLeft;
        private readonly Thumb _top;
        private readonly Thumb _topRight;
        private readonly Thumb _right;
        private readonly Thumb _bottomRight;
        private readonly Thumb _bottom;
        private readonly Thumb _bottomLeft;

        private readonly VisualCollection _visualChildren;

        private bool _hasCanvas;
        private bool _hasHorizontalOffset;
        private bool _hasVerticalOffset;

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

        protected override int VisualChildrenCount
        {
            get { return _visualChildren.Count; }
        }

        public static ResizingAdorner Attach(FrameworkElement element)
        {
            Argument.IsNotNull(() => element);

            var adorner = new ResizingAdorner(element);

            var fxElement = adorner.AdornedElement as FrameworkElement;
            if (fxElement != null)
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
            Argument.IsNotNull(() => adorner);

            var adornerLayer = AdornerLayer.GetAdornerLayer(adorner.AdornedElement);
            if (adornerLayer == null)
            {
                return;
            }

            adornerLayer.Remove(adorner);

            adorner._visualChildren.Clear();

            var fxElement = adorner.AdornedElement as FrameworkElement;
            if (fxElement != null)
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
            HandleResize(left: args.HorizontalChange, bottom: args.VerticalChange);
        }

        private void HandleResize(double? left = null, double? top = null, double? right = null, double? bottom = null)
        {
            var adornedElement = AdornedElement as FrameworkElement;
            if (adornedElement == null)
            {
                return;
            }

            var updateWidthViaCanvas = false;
            var updateHeightViaCanvas = false;

            EnforceSize(adornedElement);

            if (left.HasValue)
            {
                if (_hasCanvas)
                {
                    updateWidthViaCanvas = true;

                    var oldLeft = Canvas.GetLeft(adornedElement);
                    var newValue = oldLeft + left.Value;
                    Canvas.SetLeft(adornedElement, newValue);
                }
                else if (_hasHorizontalOffset)
                {
                    var oldLeft = PropertyHelper.GetPropertyValue<double>(adornedElement, HorizontalOffsetProperty, false);
                    var newValue = oldLeft + left.Value;
                    PropertyHelper.SetPropertyValue(adornedElement, HorizontalOffsetProperty, newValue, false);

                    UpdateWidth(adornedElement.Width + left.Value * -1);
                }
            }

            if (top.HasValue)
            {
                if (_hasCanvas)
                {
                    updateHeightViaCanvas = true;

                    var oldTop = Canvas.GetTop(adornedElement);
                    var newValue = oldTop + top.Value;
                    Canvas.SetTop(adornedElement, newValue);
                }
                else if (_hasVerticalOffset)
                {
                    var oldTop = PropertyHelper.GetPropertyValue<double>(adornedElement, VerticalOffsetProperty, false);
                    var newValue = oldTop + top.Value;
                    PropertyHelper.SetPropertyValue(adornedElement, VerticalOffsetProperty, newValue, false);

                    UpdateHeight(adornedElement.Height + top.Value * -1);
                }
            }

            if (right.HasValue)
            {
                if (_hasCanvas)
                {
                    updateWidthViaCanvas = true;

                    var oldRight = Canvas.GetRight(adornedElement);
                    var newValue = oldRight + right.Value;
                    Canvas.SetRight(adornedElement, newValue);
                }
                else
                {
                    UpdateWidth(adornedElement.Width + right.Value);
                }
            }

            if (bottom.HasValue)
            {
                if (_hasCanvas)
                {
                    updateHeightViaCanvas = true;

                    var oldBottom = Canvas.GetBottom(adornedElement);
                    var newValue = oldBottom + bottom.Value;
                    Canvas.SetBottom(adornedElement, newValue);
                }
                else
                {
                    UpdateHeight(adornedElement.Height + bottom.Value);
                }
            }

            if (updateWidthViaCanvas)
            {
                var difference = Math.Abs(Canvas.GetRight(adornedElement) - Canvas.GetLeft(adornedElement));

                UpdateWidth(difference);
            }

            if (updateHeightViaCanvas)
            {
                var difference = Math.Abs(Canvas.GetBottom(adornedElement) - Canvas.GetTop(adornedElement));

                UpdateHeight(difference);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var desiredWidth = AdornedElement.DesiredSize.Width;
            var desiredHeight = AdornedElement.DesiredSize.Height;

            var fullSize = CornerSize;
            var halfSize = CornerSize / 2;

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
            else if (PropertyHelper.IsPropertyAvailable(adornedElement, HorizontalOffsetProperty, false))
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
            else if (PropertyHelper.IsPropertyAvailable(adornedElement, VerticalOffsetProperty, false))
            {
                _hasVerticalOffset = true;
                enableTop = true;
            }

            _top.SetCurrentValue(IsEnabledProperty, enableTop);
            _topLeft.SetCurrentValue(IsEnabledProperty, enableLeft && enableTop);
        }

        private void UpdateWidth(double width)
        {
            var adornedElement = AdornedElement as FrameworkElement;
            if (adornedElement == null)
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
            var adornedElement = AdornedElement as FrameworkElement;
            if (adornedElement == null)
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
            if (thumb != null)
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

            var parent = adornedElement.Parent as FrameworkElement;
            if (parent != null)
            {
                adornedElement.SetCurrentValue(MaxHeightProperty, parent.ActualHeight);
                adornedElement.SetCurrentValue(MaxWidthProperty, parent.ActualWidth);
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visualChildren[index];
        }
    }
}
