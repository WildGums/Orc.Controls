namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using Orc.Automation;

    //public class AutomationElementPartAttribute : Attribute
    //{
    //    public string AutomationId { get; set; }
    //    public string Name { get; set; }
    //    public string ClassName { get; set; }
    //    public string ControlType { get; set; }
    //}

    public class ColorLegendAutomationElement : CommandAutomationElement
    {
        public ColorLegendAutomationElement(AutomationElement element) 
            : base(element)
        {
        }

        public virtual AutomationElement SearchBox => GetPart(controlType: ControlType.Text); 
        public virtual AutomationElement SettingsButton => GetPart();
        public virtual AutomationElement AllVisibleCheckBox => GetPart();
        public virtual AutomationElement UnselectAllButton => GetPart();
    }
}
