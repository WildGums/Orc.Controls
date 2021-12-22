namespace Orc.Controls.Automation
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Catel.Windows;
    using Orc.Automation;

    public class ColorBoardAutomationPeer : ControlRunMethodAutomationPeerBase<Controls.ColorBoard>
    {
        private readonly Controls.ColorBoard _owner;

        public ColorBoardAutomationPeer(Controls.ColorBoard owner) 
            : base(owner)
        {
            _owner = owner;

            owner.DoneClicked += OnDoneClicked;
            owner.CancelClicked += OnCancelClicked;
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(nameof(Controls.ColorBoard.CancelClicked), null);
        }

        private void OnDoneClicked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(nameof(Controls.ColorBoard.DoneClicked), null);
        }

        [AutomationMethod]
        public Rect GetHsvCanvasBoundingRect()
        {
            if (Owner.FindVisualDescendantByName("PART_HSVRectangle") is not Rectangle hsvCanvas)
            {
                return Rect.Empty;
            }

            var screenRect = hsvCanvas.GetScreenRect();

            return screenRect;
        }

        [AutomationMethod]
        public Point GetSV()
        {
            if (Owner.FindVisualDescendantByName("PART_HSVRectangle") is not Rectangle hsvCanvas)
            {
                return new Point(double.MaxValue, double.MaxValue);
            }

            var ellipse = Owner.FindVisualDescendantByType<Ellipse>();
            if (ellipse is null)
            {
                return new Point(double.MaxValue, double.MaxValue);
            }

            var x = (double)ellipse.GetValue(Canvas.LeftProperty) + ellipse.ActualWidth / 2;
            var y = (double)ellipse.GetValue(Canvas.TopProperty) + ellipse.ActualHeight / 2;

            var s = x / (hsvCanvas.ActualWidth - 1);
            if (s < 0d)
            {
                s = 0d;
            }
            else if (s > 1d)
            {
                s = 1d;
            }

            var v = 1 - y / (hsvCanvas.ActualHeight - 1);
            if (v < 0d)
            {
                v = 0d;
            }
            else if (v > 1d)
            {
                v = 1d;
            }

            return new Point(s, v);
        }

        [AutomationMethod]
        public Point GetColorPoint(Color color)
        {
            if (Owner.FindVisualDescendantByName("PART_HSVRectangle") is not Rectangle hsvCanvas)
            {
                return new Point(double.MaxValue, double.MaxValue);
            }

            var ellipse = Owner.FindVisualDescendantByType<Ellipse>();
            if (ellipse is null)
            {
                return new Point(double.MaxValue, double.MaxValue);
            }

            var s = ColorHelper.GetHSV_S(color);
            var v = ColorHelper.GetHSV_V(color);
            
            var x = s * hsvCanvas.ActualWidth;
            var y = (1 - v) * hsvCanvas.ActualHeight;

            return new Point(1.75 * x, 1.75 * y);
        }

        [AutomationMethod]
        public Point GetHsvColorEllipsePosition()
        {
            var ellipse = Owner.FindVisualDescendantByType<Ellipse>();
            if (ellipse is null)
            {
                return new Point(double.MaxValue, double.MaxValue);
            }

            return ellipse.GetScreenRect().GetClickablePoint();
        }
    }
}
