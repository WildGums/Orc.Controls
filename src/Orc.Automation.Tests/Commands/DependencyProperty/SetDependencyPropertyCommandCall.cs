namespace Orc.Automation
{
    using System.Windows;

    public sealed class SetDependencyPropertyCommandCall : IAutomationCommandCall
    {
        public const string SetPropertyCommandNamePrefix = "SetProperty:";

        public static string ConvertPropertyToCommandName(string propertyName)
        {
            return string.IsNullOrWhiteSpace(propertyName) ? null : $"{SetPropertyCommandNamePrefix}{propertyName}";
        }
        public static string GetPropertyNameFromCommandName(string commandName)
        {
            return string.IsNullOrWhiteSpace(commandName) ? null : commandName.Replace(SetPropertyCommandNamePrefix, string.Empty);
        }

        public bool IsMatch(FrameworkElement owner, AutomationCommand command)
        {
            var commandName = command?.CommandName;
            return commandName?.StartsWith(SetPropertyCommandNamePrefix) ?? false;
        }

        public bool TryInvoke(FrameworkElement owner, AutomationCommand command, out AutomationCommandResult result)
        {
            DependencyPropertyAutomationHelper.SetDependencyPropertyValue(owner, GetPropertyNameFromCommandName(command.CommandName), command.Data?.ExtractValue());

            result = null;

            //TODO:Vladimir;Return actual value of success
            return true;
        }
    }
}
