namespace Orc.Controls.Win32
{
    using System.Runtime.InteropServices;
    using System.Windows;

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public int X
        {
            get { return Left; }
            set 
            { 
                Right -= Left - value; 
                Left = value; 
            }
        }

        public int Y
        {
            get { return Top; }
            set 
            { 
                Bottom -= Top - value; 
                Top = value; 
            }
        }

        public int Height
        {
            get 
            { 
                return Bottom - Top; 
            }
            set 
            { 
                Bottom = value + Top; 
            }
        }

        public int Width
        {
            get 
            { 
                return Right - Left; 
            }
            set 
            { 
                Right = value + Left; 
            }
        }

        public Point Location => new Point(Left, Top);

        public Size Size => new Size(Width, Height);

        public static bool operator ==(Rect r1, Rect r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(Rect r1, Rect r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(Rect r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (obj is Rect rect)
            {
                return Equals(rect);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() ^ Right.GetHashCode() ^ Top.GetHashCode() ^ Bottom.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }
}
