namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;

    public class ColorBoard : AutomationControl
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
