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
            if (adorner == null || adorner.Child == null)
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
            if (dd == null || dd._adorner == null || dd._adorner.Child == null)
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
            _mouseY = e.GetPosition(null).Y;
            _mouseX = e.GetPosition(null).X;
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _adorner.Child.ReleaseMouseCapture();
            _mouseCaptured = false;
            _mouseY = 0;
            _mouseX = 0;
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseCaptured)
            {
                return;
            }

            var frameworkElement = _adorner as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }

            var deltaY = e.GetPosition(null).Y - _mouseY;
            var deltaX = e.GetPosition(null).X - _mouseX;

            var offset = new Point(_adorner.Offset.X + deltaX, _adorner.Offset.Y + deltaY);

            // constrain the popup to the bounds of the window
            if (_adorner.ChildPosition.Y + offset.Y < 0)
            {
                offset.Y = -_adorner.ChildPosition.Y;
            }

            if (_adorner.ChildPosition.X + offset.X < 0)
            {
                offset.X = -_adorner.ChildPosition.X;
            }

            var boundariesSize = ScreenHelper.GetWindowSize(null);
            var adornedElement = _adorner.AdornedElement as FrameworkElement;
            if (adornedElement != null)
            {
                boundariesSize = new Size(adornedElement.ActualWidth, adornedElement.ActualHeight);
            }

            var maxY = boundariesSize.Height - frameworkElement.ActualHeight;
            if (_adorner.ChildPosition.Y + offset.Y > maxY)
            {
                offset.Y = maxY - _adorner.ChildPosition.Y;
            }

            var maxX = boundariesSize.Width - frameworkElement.ActualWidth;
            if (_adorner.ChildPosition.X + offset.X > maxX)
            {
                offset.X = maxX - _adorner.ChildPosition.X;
            }

            var adornerChild = _adorner.Child;
            if (adornerChild != null)
            {
                PropertyHelper.TrySetPropertyValue(adornerChild, "HorizontalOffset", offset.X, false);
                PropertyHelper.TrySetPropertyValue(adornerChild, "VerticalOffset", offset.Y, false);
            }

            _mouseY = e.GetPosition(null).Y;
            _mouseX = e.GetPosition(null).X;
        }
        #endregion
    }
}