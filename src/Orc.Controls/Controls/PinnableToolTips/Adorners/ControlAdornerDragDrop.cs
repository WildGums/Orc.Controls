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
            if (_element != null)
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

            var deltaX = position.X - _mouseX;
            var deltaY = position.Y - _mouseY;

            var adornerChild = _adorner.Child;
            if (adornerChild != null)
            {
                if (PropertyHelper.TryGetPropertyValue(adornerChild, "HorizontalOffset", out var existingHorizontalOffset) &&
                    PropertyHelper.TryGetPropertyValue(adornerChild, "VerticalOffset", out var existingVerticalOffset))
                {
                    if ((double)existingHorizontalOffset == 0d && (double)existingHorizontalOffset == 0d)
                    {
                        // Note: use mouse position as default
                        deltaX = position.X;
                        deltaY = position.Y;
                        //deltaX = _adorner.ChildPosition.X - position.X;
                        //deltaY = _adorner.ChildPosition.Y - position.Y;
                    }
                }
            }

            var offset = new Point(_adorner.Offset.X + deltaX, _adorner.Offset.Y + deltaY);

            // constrain the popup to the bounds of the window
            if (_adorner.ChildPosition.X + offset.X < 0)
            {
                offset.X = -_adorner.ChildPosition.X;
            }

            if (_adorner.ChildPosition.Y + offset.Y < 0)
            {
                offset.Y = -_adorner.ChildPosition.Y;
            }

            var boundariesSize = Application.Current.MainWindow.GetSize();
            if (_adorner.AdornedElement is FrameworkElement adornedElement)
            {
                boundariesSize = new Size(adornedElement.ActualWidth, adornedElement.ActualHeight);
            }

            var maxX = boundariesSize.Width - frameworkElement.ActualWidth;
            if (_adorner.ChildPosition.X + offset.X > maxX)
            {
                offset.X = maxX - _adorner.ChildPosition.X;
            }

            var maxY = boundariesSize.Height - frameworkElement.ActualHeight;
            if (_adorner.ChildPosition.Y + offset.Y > maxY)
            {
                offset.Y = maxY - _adorner.ChildPosition.Y;
            }

            if (adornerChild != null)
            {
                PropertyHelper.TrySetPropertyValue(adornerChild, "HorizontalOffset", offset.X);
                PropertyHelper.TrySetPropertyValue(adornerChild, "VerticalOffset", offset.Y);
            }

            _mouseY = position.Y;
            _mouseX = position.X;
        }
        #endregion
    }
}
