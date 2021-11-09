namespace Orc.Controls.Automation
{
    using System.Windows;

    public sealed class GetDependencyPropertyCommandCall : IAutomationCommandCall
    {
        public const string GetPropertyCommandNamePrefix = "GetProperty:";

        public static string ConvertPropertyToCommandName(string propertyName)
        {
            return string.IsNullOrWhiteSpace(propertyName) ? null : $"{GetPropertyCommandNamePrefix}{propertyName}";
        }
        public static string GetPropertyNameFromCommandName(string commandName)
        {
            return string.IsNullOrWhiteSpace(commandName) ? null : commandName.Replace(GetPropertyCommandNamePrefix, string.Empty);
        }

        public bool IsMatch(FrameworkElement owner, AutomationCommand command)
        {
            var commandName = command?.CommandName;
            return commandName?.StartsWith(GetPropertyCommandNamePrefix) ?? false;
        }

        public bool TryInvoke(FrameworkElement owner, AutomationCommand command, out AutomationCommandResult propertyValue)
        {
            return DependencyPropertyAutomationHelper.TryGetDependencyPropertyValue(owner, GetPropertyNameFromCommandName(command.CommandName), out propertyValue);
        }
    }
}
