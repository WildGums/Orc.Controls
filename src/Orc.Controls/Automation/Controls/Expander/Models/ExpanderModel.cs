namespace Orc.Controls.Automation;

using Orc.Automation;

[AutomationAccessType]
public class ExpanderModel : ControlModel
{
    public ExpanderModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public bool IsExpanded { get; set; }
    public ExpandDirection ExpandDirection { get; set; }
    public double ExpandDuration { get; set; }
    public bool AutoResizeGrid { get; set; }
}