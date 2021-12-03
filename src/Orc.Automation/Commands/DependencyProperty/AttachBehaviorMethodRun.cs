namespace Orc.Automation
{
    using System;
    using System.Windows;

    public class AttachBehaviorMethodRun : IAutomationMethodRun
    {
        public const string AttachBehaviorMethodPrefix = "AttachBehavior";

        public bool IsMatch(FrameworkElement owner, AutomationMethod method)
        {
            var commandName = method?.Name;
            return commandName?.StartsWith(AttachBehaviorMethodPrefix) ?? false;
        }

        public bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            var value = method.Parameters[0].ExtractValue() as Type;

            //TODO:Vladimir: just create non generic method in orc.theming
            var methodInfo = typeof(Orc.Theming.FrameworkElementExtensions).GetMethod("AttachBehavior");

            var genericMethod = methodInfo.MakeGenericMethod(value);
            genericMethod.Invoke(null, new []{ owner }); // No target, no arguments

            result = null;

            return true;
        }
    }
}
