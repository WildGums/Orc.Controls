namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;

    public class ColorBoardAutomationControl : CustomAutomationControl
    {
        public ColorBoardAutomationControl(AutomationElement element)
            : base(element)
        {
        }

        public Color Color
        {
            get => Access.GetValue<Color>();
            set => Access.SetValue(value);
        }

        public ColorBoardUserActions Simulate => Simulate<ColorBoardUserActions>();
    }
}
