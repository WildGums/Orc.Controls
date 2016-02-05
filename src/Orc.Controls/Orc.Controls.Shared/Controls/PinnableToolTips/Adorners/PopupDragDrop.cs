// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PopupDragDrop.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    /// <summary>
    /// A class that makes popups moveable.
    /// </summary>
    internal class PopupDragDrop
    {
        #region Constructors and Destructors
        /// <summary>
        /// Prevents a default instance of the <see cref="PopupDragDrop" /> class from being created.
        /// </summary>
        private PopupDragDrop()
        {
        }
        #endregion

        #region Fields
        private bool _mouseCaptured;

        private double _mouseX;

        private double _mouseY;

        private Popup _popup;
        #endregion

        #region Methods
        public static PopupDragDrop Attach(Popup popup)
        {
            if (popup == null || popup.Child == null || !(popup.Child is FrameworkElement))
            {
                return null;
            }

            var pdd = new PopupDragDrop
            {
                _popup = popup
            };

            pdd._popup.Child.MouseLeftButtonDown += pdd.MouseLeftButtonDown;
            pdd._popup.Child.MouseLeftButtonUp += pdd.MouseLeftButtonUp;
            pdd._popup.Child.MouseMove += pdd.MouseMove;

            return pdd;
        }

        public static void Detach(PopupDragDrop pdd)
        {
            if (pdd == null || pdd._popup == null || pdd._popup.Child == null)
            {
                return;
            }

            pdd._popup.Child.MouseLeftButtonDown -= pdd.MouseLeftButtonDown;
            pdd._popup.Child.MouseLeftButtonUp -= pdd.MouseLeftButtonUp;
            pdd._popup.Child.MouseMove -= pdd.MouseMove;
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _popup.Child.CaptureMouse();
            _mouseCaptured = true;
            _mouseY = e.GetPosition(null).Y;
            _mouseX = e.GetPosition(null).X;
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _popup.Child.ReleaseMouseCapture();
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

            var frameworkElement = _popup.Child as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }

            var deltaY = e.GetPosition(null).Y - _mouseY;
            var deltaX = e.GetPosition(null).X - _mouseX;
            _popup.VerticalOffset += deltaY;
            _popup.HorizontalOffset += deltaX;

            // constrain the popup to the bounds of the silverlight window
            if (_popup.VerticalOffset < 0)
            {
                _popup.VerticalOffset = 0;
            }

            if (_popup.HorizontalOffset < 0)
            {
                _popup.HorizontalOffset = 0;
            }

            var windowSize = ScreenHelper.GetWindowSize(null);

            var maxY = windowSize.Height - frameworkElement.ActualHeight;
            if (_popup.VerticalOffset > maxY)
            {
                _popup.VerticalOffset = maxY;
            }

            var maxX = windowSize.Width - frameworkElement.ActualWidth;
            if (_popup.HorizontalOffset > maxX)
            {
                _popup.HorizontalOffset = maxX;
            }

            _mouseY = e.GetPosition(null).Y;
            _mouseX = e.GetPosition(null).X;
        }
        #endregion
    }
}