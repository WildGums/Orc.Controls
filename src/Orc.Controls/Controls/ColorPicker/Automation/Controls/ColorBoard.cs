namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class ColorBoard : FrameworkElement
    {
        public ColorBoard(AutomationElement element)
            : base(element)
        {
            View = new ColorBoardView(this);
        }

        public Color Color
        {
            get => Access.GetValue<Color>();
            set => Access.SetValue(value);
        }

        public ColorBoardView View { get; }
    }
}
