namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class LinkLabel : FrameworkElement<LinkLabelModel>
    {
        public LinkLabel(AutomationElement element)
            : base(element)
        {
        }

        public void Invoke()
        {
            var rawTreeWalker = TreeWalker.RawViewWalker;
            var rawElement = rawTreeWalker.GetFirstChild(Element);

            rawElement?.MouseClick();
        }

#pragma warning disable CS0067
        public event EventHandler<EventArgs> Click;
        public event EventHandler<EventArgs> RequestNavigate;
#pragma warning restore CS0067
    }
}
