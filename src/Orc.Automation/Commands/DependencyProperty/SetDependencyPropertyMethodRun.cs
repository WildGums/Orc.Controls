namespace Orc.Automation
{
    using System.Linq;
    using System.Windows;

    public sealed class SetDependencyPropertyMethodRun : IAutomationMethodRun
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

        public bool IsMatch(FrameworkElement owner, AutomationMethod method)
        {
            var commandName = method?.Name;
            return commandName?.StartsWith(SetPropertyCommandNamePrefix) ?? false;
        }

        public bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            DependencyPropertyAutomationHelper.SetDependencyPropertyValue(owner, GetPropertyNameFromCommandName(method.Name), method.Parameters?.FirstOrDefault()?.ExtractValue());

            result = null;

            //TODO:Vladimir;Return actual value of success
            return true;
        }
    }
}
