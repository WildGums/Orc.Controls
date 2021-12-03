namespace Orc.Automation
{
    using System.Windows.Automation;
    using Catel;
    using Catel.IoC;

    public static partial class AutomationElementExtensions
    {
        public static TAutomationControl As<TAutomationControl>(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return element.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<TAutomationControl>(element);
        }

        public static bool IsVisible(this AutomationElement element)
        {
            if (element is null)
            {
                return false;
            }

            return !IsOffscreen(element);
        }

        public static bool IsOffscreen(this AutomationElement element)
        {
            if (element is null)
            {
                return true;
            }

            return (bool)element.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty);
        }
    }
}
