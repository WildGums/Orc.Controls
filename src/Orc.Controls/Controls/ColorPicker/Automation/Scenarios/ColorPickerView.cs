namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Tests;
    using Window = Orc.Automation.Tests.WindowAutomationElement;

    public class ColorPickerView : AutomationBase
    {
        protected virtual ToggleButtonAutomationElement ToggleDropDown => By.ControlType(ControlType.Button).One<ToggleButtonAutomationElement>();

        public ColorPickerView(ColorPickerAutomationControl control)
            : base(control.Element)
        {
        }

        public ColorBoardAutomationControl OpenColorBoard()
        {
            var windowHost = Element.Find<Window>(controlType: ControlType.Window, scope: TreeScope.Ancestors);
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
