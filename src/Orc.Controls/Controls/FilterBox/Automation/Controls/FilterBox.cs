namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;


    [AutomatedControl(Class = typeof(Controls.FilterBox), ControlTypeName = nameof(ControlType.Edit))]
    public class FilterBox : Edit
    {
        public FilterBox(AutomationElement element) 
            : base(element)
        {
        }
    }
}
