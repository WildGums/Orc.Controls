namespace Orc.Automation
{
    using System.Windows.Automation;

    public class AutomationControl : AutomationModel
    {
        public AutomationControl(AutomationElement target)
            : base(target)
        {
        }
    }
}
