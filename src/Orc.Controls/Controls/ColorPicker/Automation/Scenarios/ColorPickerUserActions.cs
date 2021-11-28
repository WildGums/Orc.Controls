namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Tests;
    using Window = Orc.Automation.Tests.WindowAutomationElement;

    public class ColorPickerUserActions : UserActions
    {
        private readonly ColorPickerAutomationControl _control;

        [ControlPart(ControlType = nameof(ControlType.Button))]
        protected virtual ToggleButtonAutomationElement ToggleDropDown { get; set; }

        public ColorPickerUserActions(ColorPickerAutomationControl control)
            : base(control)
        {
            _control = control;
        }

        public ColorBoardAutomationControl OpenColorBoard()
        {
            var windowHost = _control.Find<Window>(controlType: ControlType.Window, scope: TreeScope.Ancestors);
            if (windowHost is null)
            {
                return null;
            }

            ToggleDropDown.Toggle();

            Wait.UntilResponsive();

            var colorPopup = windowHost.Find(className: "Popup", controlType: ControlType.Window);
            var colorBoard = colorPopup?.Find(className: typeof(ColorBoard).FullName);
            if (colorBoard is null)
            {
                return null;
            }

            return new ColorBoardAutomationControl(colorBoard);
        }
    }
}
