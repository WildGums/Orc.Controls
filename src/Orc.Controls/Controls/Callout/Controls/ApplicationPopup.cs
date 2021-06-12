namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interop;

    internal class ApplicationPopup : Popup
    {
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        private Window _owner;

        public ApplicationPopup()
        {
            Loaded += OnApplicationPopupLoaded;
            Unloaded += OnApplicationPopupUnloaded;
        }

        private void OnApplicationPopupLoaded(object sender, RoutedEventArgs e)
        {
            _owner = Window.GetWindow(this);

            if (_owner is null)
            {
                return;
            }

            _owner.Activated += OnOwnerWindowActivated;
            _owner.Deactivated += OnOwnerWindowDeactivated;
        }

        private void OnApplicationPopupUnloaded(object sender, RoutedEventArgs e)
        {
            if (_owner is not null)
            {
                _owner.Activated -= OnOwnerWindowActivated;
                _owner.Deactivated -= OnOwnerWindowDeactivated;
            }
        }

        private void OnOwnerWindowDeactivated(object sender, EventArgs e)
        {
        }

        private void OnOwnerWindowActivated(object sender, EventArgs e)
        {
            if (IsOpen)
            {
                // Force pop-up to show when window re-activated
                var hwnd = ((HwndSource)PresentationSource.FromVisual(Child)).Handle;
                if (User32.GetWindowRect(hwnd, out var rect))
                {
                    User32.SetWindowPos(hwnd, HWND_TOP, rect.Left, rect.Top, (int)Width, (int)Height, TOPMOST_FLAGS);
                }
            }
        }

        protected override void OnOpened(EventArgs e)
        {
            var hwnd = ((HwndSource)PresentationSource.FromVisual(Child)).Handle;

            if (User32.GetWindowRect(hwnd, out var rect))
            {
                // Note: setting non-topmost alone doesn't have effect
                // We need to set HWND_BOTTOM to lose topmost status, then HWND_TOP to re-shuffle it on top of all non-topmost windows.
                User32.SetWindowPos(hwnd, HWND_BOTTOM, rect.Left, rect.Top, (int)Width, (int)Height, TOPMOST_FLAGS);
                User32.SetWindowPos(hwnd, HWND_TOP, rect.Left, rect.Top, (int)Width, (int)Height, TOPMOST_FLAGS);
                User32.SetWindowPos(hwnd, HWND_NOTOPMOST, rect.Left, rect.Top, (int)Width, (int)Height, TOPMOST_FLAGS);
            }
        }

        private static readonly SetWindowPosition TOPMOST_FLAGS =
       SetWindowPosition.DoNotActivate | SetWindowPosition.DoNotChangeOwnerZOrder | SetWindowPosition.IgnoreResize | SetWindowPosition.IgnoreMove |
            SetWindowPosition.DoNotRedraw | SetWindowPosition.DoNotSendChangingEvent;

    }
}
