namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using Orc.Automation;

    public class DropDownButtonAutomationPeer : ControlRunMethodAutomationPeerBase<Orc.Controls.DropDownButton>
    {
        public DropDownButtonAutomationPeer(Controls.DropDownButton owner)
            : base(owner)
        {
           
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface is PatternInterface.Toggle)
            {
                return new ToggleButtonAutomationPeer((ToggleButton)Owner);
            }

            return base.GetPattern(patternInterface);
        }

        [AutomationMethod]
        public void AddMenuItems(List<string> items)
        {
            var contextMenu = new ContextMenu();

            foreach (var item in items ?? Enumerable.Empty<string>())
            {
                contextMenu.Items.Add(item);
            }

            Control.SetCurrentValue(Controls.DropDownButton.DropDownProperty, contextMenu);
        }
    }
}
