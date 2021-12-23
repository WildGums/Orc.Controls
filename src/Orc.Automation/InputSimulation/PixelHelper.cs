namespace Orc.Automation
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using ControlzEx.Standard;
    using Color = System.Windows.Media.Color;

    public class PixelHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr window);
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);

        public static Color GetColorAt(System.Windows.Point point)
        {
            var desk = GetDesktopWindow();
            var dc = GetWindowDC(desk);
            var a = (int)GetPixel(dc, (int)point.X, (int)point.Y);
            ReleaseDC(desk, dc);
            return Color.FromArgb(255, (byte) ((a >> 0) & 0xff), (byte)((a >> 8) & 0xff), (byte)((a >> 16) & 0xff));
        }
    }
}
