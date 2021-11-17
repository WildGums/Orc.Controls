namespace Orc.Automation
{
    using System.Windows;

    public static class PointExtensions
    {
        public static System.Drawing.Point ToDrawingPoint(this Point point)
        {
            return new System.Drawing.Point((int)point.X, (int)point.Y);
        }
    }
}
