namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Automation;
    using Catel;

    public class By
    {
        private readonly AutomationElement _element;
        private readonly SearchContext _searchContext = new();

        public By(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            _element = element;
        }

        public By Id(string id)
        {
            _searchContext.Id = id;

            return this;
        }

        public By Name(string name)
        {
            _searchContext.Name = name;

            return this;
        }

        public By ControlType(ControlType controlType)
        {
            _searchContext.ControlType = controlType;

            return this;
        }

        public By ClassName(string className)
        {
            _searchContext.ClassName = className;

            return this;
        }

        public By Condition(Condition condition)
        {
            _searchContext.Condition = condition;

            return this;
        }

        public AutomationElement One()
        {
            return _element.Find(_searchContext);
        }

        public IList<AutomationElement> Many()
        {
            return _element.FindAll(_searchContext)?.ToList();
        }
    }
}