namespace Orc.Controls.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class TabControlAutomationPeer : System.Windows.Automation.Peers.TabControlAutomationPeer
    {
        public TabControlAutomationPeer(TabControl owner) 
            : base(owner)
        {
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            var list = base.GetChildrenCore();
            list?.AddRange(GetChildPeers(Owner));
            return list;
        }

        private static IEnumerable<AutomationPeer> GetChildPeers(DependencyObject element)
        {
            var list = new List<AutomationPeer>();
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++) 
            {
                var child = VisualTreeHelper.GetChild(element, i) as UIElement;
                if (child is null)
                {
                    continue;
                }

                var childPeer = CreatePeerForElement(child);
                if (childPeer is not null)
                {
                    list.Add(childPeer);
                }
                else
                {
                    list.AddRange(GetChildPeers(child));
                }
            }
            return list;
        }
    }
}
