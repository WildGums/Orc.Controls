namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class LinkLabel : FrameworkElement<LinkLabelModel, LinkLabelMap>
    {
        public LinkLabel(AutomationElement element)
            : base(element)
        {
        }

        public string Content => Map.Text.Value;

        public void Invoke()
        {
            var hyperlink = Map.Hyperlink;

            hyperlink.Invoke();
        }

#pragma warning disable CS0067
        public event EventHandler<EventArgs> Click;
        public event EventHandler<EventArgs> RequestNavigate;
#pragma warning restore CS0067
    }
}
