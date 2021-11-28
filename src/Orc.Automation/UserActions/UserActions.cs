namespace Orc.Automation
{
    using System.Windows.Automation;

    public class UserActions : AutomationModel
    {
        public UserActions(AutomationElement target)
            : base(target)
        {
        }

        public UserActions(AutomationControl targetControl)
            : base(targetControl)
        {
        }
    }
}
