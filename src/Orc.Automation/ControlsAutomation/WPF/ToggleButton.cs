namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    /// <summary>
    /// Toggle button Automation Element wrapper
    /// </summary>
    [AutomatedControl(ControlTypeName = nameof(ControlType.Button))]
    public class ToggleButton : FrameworkElement
    {
        public ToggleButton(AutomationElement element)
            : base(element, ControlType.Button)
        {

        }

        /// <summary>
        /// Get Is button is toggled
        /// </summary>
        public bool IsToggled => Element.GetToggleState() == true;

        /// <summary>
        /// Toggles button
        /// </summary>
        /// <returns>The button state after toggle</returns>
        /// <exception cref="AutomationException">if toggle pattern is not available</exception>
        /// <remarks>Note: In some WPF scenarios binded command might not be executed. In this case try use <see cref="AutomationControlExtensions.MouseClick"/></remarks>
        public bool? Toggle() => Element.Toggle();
    }
}
