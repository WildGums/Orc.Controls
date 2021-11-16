namespace Orc.Controls.Automation.Tests
{
    using System.Linq;
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
        
        private AutomationPattern GetSpecifiedPattern(AutomationPattern automationPattern)
        {
            var supportedPatterns = _element.GetSupportedPatterns();

            return supportedPatterns.FirstOrDefault(pattern => pattern.ProgrammaticName == automationPattern.ProgrammaticName);
        }


        public AutomationElement Element => _element;
    }
}
