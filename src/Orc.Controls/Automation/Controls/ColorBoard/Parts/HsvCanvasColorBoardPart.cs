namespace Orc.Controls.Automation
{
    using System.Windows;
    using System.Windows.Media;
    using Orc.Automation;

    public class HsvCanvasColorBoardPart : AutomationControl
    {
        public HsvCanvasColorBoardPart(ColorBoard colorBoard) 
            : base(colorBoard.Element)
        {
        }

        public override Rect BoundingRectangle => (Rect)Access.Execute(nameof(ColorBoardAutomationPeer.GetHsvCanvasBoundingRect));
        private Point ColorPosition => (Point)Access.Execute(nameof(ColorBoardAutomationPeer.GetHsvColorEllipsePosition));

        public Point GetSV()
        {
            return (Point)Access.Execute(nameof(ColorBoardAutomationPeer.GetSV));
        }

        public void SetColor(Color color)
        {
            var colorPosition = (Point)Access.Execute(nameof(ColorBoardAutomationPeer.GetColorPoint), color);

            var boundingRect = BoundingRectangle;
            var absoluteColorPosition = new Point(boundingRect.X + colorPosition.X, boundingRect.Y + colorPosition.Y);

            MouseInput.MoveTo(absoluteColorPosition);
            MouseInput.Click();
        }
    }
}
