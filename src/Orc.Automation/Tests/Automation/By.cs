namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Automation;
    using Catel;
    using Catel.Linq;

    //public class By
    //{
    //    private readonly AutomationElement _element;
    //    private readonly SearchContext _searchContext = new ();

    //    public By(AutomationElement element)
    //    {
    //        Argument.IsNotNull(() => element);

    //        _element = element;
    //    }

    //    public By Id(string id)
    //    {
    //        Argument.IsNotNull(() => id);

    //        _searchContext.Id = id;

    //        return this;
    //    }

    //    public By Name(string name)
    //    {
    //        Argument.IsNotNull(() => name);

    //        _searchContext.Name = name;

    //        return this;
    //    }

    //    public By ControlType(ControlType controlType)
    //    {
    //        Argument.IsNotNull(() => controlType);

    //        _searchContext.ControlType = controlType;

    //        return this;
    //    }

    //    public By ClassName(string className)
    //    {
    //        Argument.IsNotNull(() => className);

    //        _searchContext.ClassName = className;

    //        return this;
    //    }

    //    public AutomationElement Get()
    //    {
    //        return _element.Find(_searchContext);
    //    }

    //    public IList<AutomationElement> GetAll()
    //    {
    //        return _element.FindAll(_searchContext)?.ToList();
    //    }
    //}
}
