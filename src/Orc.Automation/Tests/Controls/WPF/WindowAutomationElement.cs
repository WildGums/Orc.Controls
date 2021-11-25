namespace Orc.Automation.Tests
{
    using System.Windows.Automation;

    public class WindowAutomationElement : ProtectedByControlTypeAutomationControl
    {
        public WindowAutomationElement(AutomationElement element)
            : base(element, ControlType.Window)
        {
        }

        /// <summary>
        /// Close window
        /// </summary>
        public void Close() => Element.CloseWindow();
    }
}
