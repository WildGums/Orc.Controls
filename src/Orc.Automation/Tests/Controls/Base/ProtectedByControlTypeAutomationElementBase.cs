namespace Orc.Automation.Tests
{
    using System.Windows.Automation;
    using Catel;

    public class ProtectedByControlTypeAutomationElementBase : AutomationElementBase
    {
        public ProtectedByControlTypeAutomationElementBase(AutomationElement element, ControlType controlType) 
            : base(element)
        {
            Argument.IsNotNull(() => controlType);

            if (!Equals(element.Current.ControlType, controlType))
            {
                throw new AutomationException($"Can't create instance of type {GetType().Name}, because input Automation Element is not a {controlType}");
            }
        }
    }
}
