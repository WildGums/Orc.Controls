namespace Orc.Automation
{
    using System.Windows;

    public sealed class GetDependencyPropertyMethodRun : IAutomationMethodRun
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

        public bool IsMatch(FrameworkElement owner, AutomationMethod method)
        {
            var commandName = method?.Name;
            return commandName?.StartsWith(GetPropertyCommandNamePrefix) ?? false;
        }

        public bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue automationPropertyValue)
        {
            automationPropertyValue = null;

            if (DependencyPropertyAutomationHelper.TryGetDependencyPropertyValue(owner, GetPropertyNameFromCommandName(method.Name), out var propertyValue))
            {
                automationPropertyValue = AutomationValue.FromValue(propertyValue);

                return true;
            }

            return false;
        }
    }
}
