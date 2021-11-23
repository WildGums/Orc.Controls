namespace Orc.Automation
{
    using System.Windows;

    public abstract class NamedAutomationMethodRun : IAutomationMethodRun
    {
        public abstract string Name { get; }

        public virtual bool IsMatch(FrameworkElement owner, AutomationMethod method)
        {
            return Equals(method.Name, Name);
        }

        public abstract bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result);
    }
}
