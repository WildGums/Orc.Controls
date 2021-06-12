namespace Orc.Controls
{
    using System;
    using System.Runtime.InteropServices;
    using Orc.Controls.Win32;

    // Credits:
    // https://www.pinvoke.net/default.aspx/Enums/SetWindowPosFlags.html
    internal static class User32
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosition uFlags);
    }

}
