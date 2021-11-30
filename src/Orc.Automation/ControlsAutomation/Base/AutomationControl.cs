namespace Orc.Automation
{
    using System.Windows.Automation;
    using Catel;

    public class AutomationControl : AutomationBase
    {
        public AutomationControl(AutomationElement element, ControlType controlType)
            : this(element)
        {
            Argument.IsNotNull(() => controlType);

            if (!Equals(element.Current.ControlType, controlType))
            {
                throw new AutomationException($"Can't create instance of type {GetType().Name}, because input Automation Element is not a {controlType}");
            }
        }

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
