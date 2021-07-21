// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlAdornerDragDrop.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Input;
    using Catel.Reflection;

    /// <summary>
    /// A class that makes ControlAdorner moveable.
    /// </summary>
    internal class ControlAdornerDragDrop
    {
        #region Constructors and Destructors
        /// <summary>
        /// Prevents a default instance of the <see cref="ControlAdornerDragDrop" /> class from being created.
        /// </summary>
        private ControlAdornerDragDrop()
        {
        }
        #endregion

        #region Fields
        private ControlAdorner _adorner;

        private UIElement _element;

        private bool _mouseCaptured;

        private double _mouseX;
        private double _mouseY;
        #endregion

        #region Public Methods and Operators
        public static ControlAdornerDragDrop Attach(ControlAdorner adorner, UIElement element)
        {
            if (adorner?.Child is null)
            {
                return null;
            }

            var dd = new ControlAdornerDragDrop
            {
                _adorner = adorner,
                _element = element
            };

            dd._adorner.Child.MouseLeftButtonDown += dd.MouseLeftButtonDown;
            dd._adorner.Child.MouseLeftButtonUp += dd.MouseLeftButtonUp;
            dd._adorner.Child.MouseMove += dd.MouseMove;

            // Important: the initial check should position it correctly,
            // see https://github.com/WildGums/Orc.Controls/issues/40

            var frameworkElement = (FrameworkElement)element;

            //Debug.WriteLine($"Adorner child: X = '{childPosition.X}', Y = '{childPosition.Y}'");
            //Debug.WriteLine($"Adorned element: Width = '{adornedElement.ActualWidth}', Height = '{adornedElement.ActualHeight}'");
            //Debug.WriteLine($"Initial X = '{initialX}', Y = '{initialY}'");
            dd.UpdatePosition(frameworkElement, adorner.ChildPosition, true);

            return dd;
        }

        public static void Detach(ControlAdornerDragDrop dd)
        {
            if (dd?._adorner?.Child is null)
            {
                return;
            }

            dd._element = null;
            dd._adorner.Child.MouseLeftButtonDown -= dd.MouseLeftButtonDown;
            dd._adorner.Child.MouseLeftButtonUp -= dd.MouseLeftButtonUp;
            dd._adorner.Child.MouseMove -= dd.MouseMove;
        }
        #endregion

        #region Methods
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_element is not null)
            {
                if (!_element.IsMouseOver)
                {
                    return;
                }
            }

            _adorner.Child.CaptureMouse();
            _mouseCaptured = true;

            var position = e.GetPosition(null);
            _mouseX = position.X;
            _mouseY = position.Y;
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _adorner.Child.ReleaseMouseCapture();
            _mouseCaptured = false;

            _mouseX = 0;
            _mouseY = 0;
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseCaptured)
            {
                return;
            }

            if (!(_adorner is FrameworkElement frameworkElement))
            {
                return;
            }

            var position = e.GetPosition(null);

            UpdatePosition(frameworkElement, position);
        }

        private void UpdatePosition(FrameworkElement frameworkElement, Point position, bool ignoreBoundaries = false)
        {
            var deltaX = position.X - _mouseX;
            var deltaY = position.Y - _mouseY;
            var adornerChildPosition = _adorner.ChildPosition;
            var adornerOffset = _adorner.Offset;

            //Debug.WriteLine($"Adorner offset: X = '{adornerOffset.X}', Y = '{adornerOffset.Y}'");
            //Debug.WriteLine($"Movement delta: X = '{deltaX}', Y = '{deltaY}'");

            var offset = new Point(adornerOffset.X + deltaX, adornerOffset.Y + deltaY);

            // constrain the popup to the bounds of the window
            if (adornerChildPosition.X + offset.X < 0)
            {
                offset.X = -adornerChildPosition.X;
            }

            if (adornerChildPosition.Y + offset.Y < 0)
            {
                offset.Y = -adornerChildPosition.Y;
            }

            if (!ignoreBoundaries)
            {
                var boundariesSize = Application.Current.MainWindow.GetSize();
                if (_adorner.AdornedElement is FrameworkElement adornedElement)
                {
                    boundariesSize = new Size(adornedElement.ActualWidth, adornedElement.ActualHeight);
                }

                var maxX = boundariesSize.Width - frameworkElement.ActualWidth;
                if (adornerChildPosition.X + offset.X > maxX)
                {
                    offset.X = maxX - adornerChildPosition.X;
                }

                var maxY = boundariesSize.Height - frameworkElement.ActualHeight;
                if (adornerChildPosition.Y + offset.Y > maxY)
                {
                    offset.Y = maxY - adornerChildPosition.Y;
                }
            }

            var adornerChild = _adorner.Child;
            if (adornerChild is not null)
            {
                //Debug.WriteLine($"Updating adorner child offset: X = '{offset.X}', Y = '{offset.Y}'");

                PropertyHelper.TrySetPropertyValue(adornerChild, "HorizontalOffset", offset.X);
                PropertyHelper.TrySetPropertyValue(adornerChild, "VerticalOffset", offset.Y);
            }

            _mouseX = position.X;
            _mouseY = position.Y;
        }
        #endregion
    }
}
