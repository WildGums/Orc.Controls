namespace Orc.Automation
{
    using System.Windows.Automation;
    using Catel;

    public static partial class AutomationElementExtensions
    {
        public static bool IsVisible(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return !IsOffscreen(element);
        }

        public static bool IsOffscreen(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return (bool)element.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty);
        }
    }
}
