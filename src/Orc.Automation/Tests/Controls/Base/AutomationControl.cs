namespace Orc.Automation
{
    using System.Windows.Automation;

    public class AutomationControl : AutomationBase
    {
        public AutomationControl(AutomationElement element)
            : base(element)
        {
            Access = new AutomationElementAccessor(element);
            Access.AutomationEvent += OnEvent;
        }

        protected AutomationElementAccessor Access { get; }

        protected virtual void OnEvent(object sender, AutomationEventArgs args)
        {
        }
    }
}
