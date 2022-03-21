namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class StepBarMap : AutomationBase
    {
        public StepBarMap(AutomationElement element) 
            : base(element)
        {
        }

        public List ItemList => By.One<List>();
    }
}
