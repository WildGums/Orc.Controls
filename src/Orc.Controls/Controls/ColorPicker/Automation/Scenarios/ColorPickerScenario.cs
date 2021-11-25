namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Tests;
    using Window = Orc.Automation.Tests.WindowAutomationElement;

    public class ColorPickerScenario : AutomationControlScenario
    {
        private readonly ColorPickerAutomationElement _element;
        private readonly ToggleButtonAutomationElement _toggleDropDown;

        [ControlPart(ControlType = nameof(ControlType.Button))]
        protected virtual ToggleButtonAutomationElement ToggleDropDown { get; set; }

        public ColorPickerScenario(ColorPickerAutomationElement element)
            : base(element)
        {
            _element = element;
            _toggleDropDown = _element.GetPart(controlType: ControlType.Button).As<ToggleButtonAutomationElement>();
        }

        public ColorBoardAutomationElement ShowColorEditDropDown()
        {
            var windowHost = _element.FindAncestor<Window>(x => Equals(x.Current.ControlType, ControlType.Window));
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

            return new ColorBoardAutomationElement(colorBoard);
        }
    }
}
