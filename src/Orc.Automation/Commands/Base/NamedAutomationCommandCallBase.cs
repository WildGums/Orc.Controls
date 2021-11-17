namespace Orc.Automation
{
    using System.Windows;

    public abstract class NamedAutomationCommandCallBase : IAutomationCommandCall
    {
        public abstract string Name { get; }

        public virtual bool IsMatch(FrameworkElement owner, AutomationCommand command)
        {
            return Equals(command.CommandName, Name);
        }

        public abstract bool TryInvoke(FrameworkElement owner, AutomationCommand command, out AutomationCommandResult result);
    }
}
