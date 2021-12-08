namespace Orc.Automation
{
    using System;
    using System.Windows;

    public static class PointExtensions
    {
        public static System.Drawing.Point ToDrawingPoint(this Point point)
        {
            //return new System.Drawing.Point((int)point.X, (int)point.Y);
            return new System.Drawing.Point((int) Math.Round(point.X), (int)point.Y);
        }
    }
}
