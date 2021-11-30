namespace Orc.Controls.Automation
{
    using System.Windows.Automation.Peers;

    public class DropDownButtonAutomationPeer : ToggleButtonAutomationPeer
    {
        public DropDownButtonAutomationPeer(Controls.DropDownButton owner)
            : base(owner)
        {
           
        }

        protected override string GetClassNameCore()
        {
            return typeof(Controls.DropDownButton).FullName;
        }
    }
}
