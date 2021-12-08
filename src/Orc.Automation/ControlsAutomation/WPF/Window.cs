namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Window))]
    public class Window : FrameworkElement
    {
        public Window(AutomationElement element)
            : base(element, ControlType.Window)
        {
        }

        /// <summary>
        /// Close window
        /// </summary>
        public void Close() => Element.CloseWindow();
    }
}
