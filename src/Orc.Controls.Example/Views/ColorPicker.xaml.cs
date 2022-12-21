namespace Orc.Controls.Example.Views
{
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Automation;
    using Catel.Windows;
    using Orc.Automation;

    public partial class ColorPicker
    {
        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }


        public ColorPicker()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var color = //Color.FromArgb(0x43, 0x88, 0xFA, 0xEE);
            //Colors.Red; 
            Color.FromArgb(0x12, 0x34, 0x56, 0x78);

            var peer = new ColorBoardAutomationPeer(ColorBoard);

            var hSlider = ColorBoard.FindVisualDescendantByName("PART_HSVSlider") as Slider;
            var aSlider = ColorBoard.FindVisualDescendantByName("PART_ASlider") as Slider;
            var rect = ColorBoard.FindVisualDescendantByName("PART_HSVRectangle") is not Rectangle hsvRect;

            var colorPosition = peer.GetColorPoint(color);

            //   hsvRect.RaiseEvent(new RoutedEventArgs(MouseDoubleClickEvent, rect));

            var boundingRect = peer.GetHsvCanvasBoundingRect();
            var absoluteColorPosition = new Point(boundingRect.X + colorPosition.X, boundingRect.Y + colorPosition.Y);

            //LeftMouseClick((int)absoluteColorPosition.X, (int)absoluteColorPosition.Y);

            MouseInput.MoveTo(absoluteColorPosition);
            MouseInput.Click();

            hSlider.Value = ColorHelper.GetHSV_H(color);

            //Thread.Sleep(2000);

            aSlider.Value = color.A;
            //ColorBoard.SetHsvColor(Color.FromArgb(0x12, 0x34, 0x56, 0x78));
        }
    }
}
