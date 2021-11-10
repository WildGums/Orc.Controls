namespace Orc.Controls.Automation.Tests
{
    using System.Windows.Automation;
    using Catel;

    public abstract class AutomationElementBase
    {
        protected readonly AutomationElement _element;

        protected AutomationElementBase(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            _element = element;
        }

        public AutomationElement Element => _element;
    }
}
