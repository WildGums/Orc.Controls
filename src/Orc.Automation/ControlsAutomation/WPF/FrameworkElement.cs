namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    public class FrameworkElement : AutomationControl
    {
        public FrameworkElement(AutomationElement element, ControlType controlType)
            : base(element, controlType)
        {
        }

        public FrameworkElement(AutomationElement element) 
            : base(element)
        {
        }

        #region Automation Properties
        public AutomationElement.AutomationElementInformation AutomationProperties => Element.Current;
        #endregion

        public bool IsEnabled => AutomationProperties.IsEnabled;
        public bool IsVisible => Element.IsVisible();
    }
}
