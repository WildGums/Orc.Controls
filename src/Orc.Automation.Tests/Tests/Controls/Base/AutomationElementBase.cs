namespace Orc.Automation.Tests
{
    using System.Windows.Automation;
    using Catel;

    public abstract class AutomationElementBase
    {
        protected AutomationElementBase(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            Element = element;
        }

        public AutomationElement Element { get; }
    }
}
