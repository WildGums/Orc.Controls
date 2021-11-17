namespace Orc.Automation
{
    using System.Windows;

    public abstract class ControlCommandAutomationPeerBase<TControl> : CommandAutomationPeerBase
        where TControl : FrameworkElement
    {
        protected ControlCommandAutomationPeerBase(TControl owner)
            : base(owner)
        {
            Control = owner;
        }

        protected TControl Control { get; }

        protected override string GetClassNameCore()
        {
            return typeof(TControl).FullName;
        }
    }
}
