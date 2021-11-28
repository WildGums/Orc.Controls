namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;

    public class ColorBoardAutomationControl : RunMethodAutomationControl
    {
        private ColorBoardUserActions _userActions;

        public ColorBoardAutomationControl(AutomationElement element)
            : base(element)
        {
        }

        public Color Color
        {
            get => (Color) GetValue(nameof(ColorBoard.Color));
            set => SetValue(nameof(ColorBoard.Color), value);
        }

        public ColorBoardUserActions Simulate => _userActions ??= this.CreateUserActions<ColorBoardUserActions>();
    }
}
