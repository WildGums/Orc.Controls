namespace Orc.Automation
{
    using System.Windows;

    public interface IAutomationCommandCall
    {
        public abstract bool IsMatch(FrameworkElement owner, AutomationCommand command);
        public abstract bool TryInvoke(FrameworkElement owner, AutomationCommand command, out AutomationCommandResult result);
    }
}
