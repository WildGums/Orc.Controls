namespace Orc.Automation
{
    using System.Windows.Automation;

    public class ControlMap : AutomationModel
    {
        public ControlMap(AutomationElement target) 
            : base(target)
        {
        }

        public ControlMap(AutomationControl targetControl)
            : base(targetControl)
        {
        }
    }
}