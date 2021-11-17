namespace Orc.Automation
{
    using System.Windows;

    public static class RectExtensions
    {
        #region Methods
        public static Point GetClickablePoint(this Rect rect)
        {
            return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
        #endregion
    }
}
