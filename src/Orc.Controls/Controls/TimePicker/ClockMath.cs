namespace Orc.Controls
{
    using System;
    using System.Windows;
    internal static class ClockMath
    {
        /// <summary>
        /// Computes the two points required for an indicator line on a circle
        /// returns the point closest to the center first
        /// </summary>        
        public static Point[] LineOnCircle(double radians, Point center, double startRadius, double endRadius)
        {
            var innerX = center.X + Math.Cos(radians) * startRadius;
            var innerY = center.Y + Math.Sin(radians) * startRadius;

            var outerX = center.X + Math.Cos(radians) * endRadius;
            var outerY = center.Y + Math.Sin(radians) * endRadius;

            return new[] { new Point(innerX, innerY), new Point(outerX, outerY) };
        }

        /// <summary>
        /// Converts the position of the point to an hour. Assumes the WPF coordinate system
        /// (X+ is right, Y+ is down)
        /// </summary>        
        public static int AngleToHour(Point center, Point point)
        {
            var angle = PointToAngle(center, point);

            angle = angle % (Math.PI * 2);
            angle += Math.PI / 2;
            angle /= Math.PI;
            angle *= 6;
            angle = (angle + 12) % 12;

            return (int)Math.Round(angle);
        }

        /// <summary>
        /// Converts the position of the point to minutes. Assumes the WPF coordinate system
        /// (X+ is right, Y+ is down)
        /// </summary>        
        public static int AngleToMinutes(Point center, Point point)
        {
            var angle = PointToAngle(center, point);

            angle %= (Math.PI * 2);
            angle += Math.PI / 2;
            angle /= Math.PI;
            angle *= 30;
            angle = (angle + 60) % 60;

            return (int)Math.Floor(angle);
        }

        /// <summary>
        /// Computes the distance between two points
        /// </summary>        
        public static double Distance(Point a, Point b)
        {
            var dX = a.X - b.X;
            var dY = a.Y - b.Y;

            return Math.Sqrt(dX * dX + dY * dY);
        }

        /// <summary>
        /// Computes the angle from the point relative to the center
        /// </summary>
        public static double PointToAngle(Point center, Point point)
        {
            // translate the point to neutral space
            var vp = new Vector(point.X - center.X, point.Y - center.Y);

            // Calulate the angle between it and the {1, 0} vector 
            var vc = new Vector(1, 0);
            var degrees = Vector.AngleBetween(vc, vp);
            return ToRadians(degrees);
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        public static double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
