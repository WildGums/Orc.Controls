namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Automation;
    using Catel;
    using Controls;

    public class By
    {
        private readonly AutomationElement _element;
        private readonly SearchContext _searchContext = new();

        private readonly Tab _tab;

        private int? _tabIndex;

        public By(AutomationElement element, Tab tab)
            : this(element)
        {
            Argument.IsNotNull(() => tab);

            _tab = tab;
        }

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

        public By Tab(int tabIndex)
        {
            _tabIndex = tabIndex;

            return this;
        }
        
        public virtual AutomationElement One()
        {
            if (_tabIndex is not null && _tab is not null)
            {
                return _tab.InTab(_tabIndex.Value, () => _element.Find(_searchContext));
            }

            return _element.Find(_searchContext);
        }

        public virtual IList<AutomationElement> Many()
        {
            if (_tabIndex is not null && _tab is not null)
            {
                return _tab.InTab(_tabIndex.Value, () => _element.FindAll(_searchContext)?.ToList());
            }

            return _element.FindAll(_searchContext)?.ToList();
        }
    }
}
