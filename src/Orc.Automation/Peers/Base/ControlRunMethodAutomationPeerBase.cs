namespace Orc.Automation
{
    using System.Windows;

    public abstract class ControlRunMethodAutomationPeerBase<TControl> : RunMethodAutomationPeerBase
        where TControl : FrameworkElement
    {
        protected ControlRunMethodAutomationPeerBase(TControl owner)
            : base(owner)
        {
            Control = owner;
        }

        protected TControl Control { get; }

        protected override string GetClassNameCore()
        {
            return typeof(TControl).FullName;
        }

        [AutomationMethod]
        public object TryFindResource(string key)
        {
            return Control.TryFindResource(key);
        }
    }
}
