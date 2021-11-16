namespace Orc.Controls.Automation
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Automation;
    using Catel;

    public static class AutomationElementPatternHelper
    {
        public static bool TryRunPatternFunc<TPattern>(this AutomationElement element, Action<TPattern> action)
            where TPattern : BasePattern
        {
            Argument.IsNotNull(() => element);

            var automationPattern = TryGetPattern<TPattern>(element);
            if (automationPattern is null)
            {
                return false;
            }

            try
            {
                action?.Invoke(automationPattern);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }

        private static TPattern TryGetPattern<TPattern>(this AutomationElement element)
            where TPattern : BasePattern
        {
            var patternField = typeof(TPattern).GetField("Pattern");
            if (patternField?.GetValue(null) is not AutomationPattern pattern)
            {
                return null;
            }

            var supportedPatterns = element.GetSupportedPatterns();
            var automationPattern = supportedPatterns?.FirstOrDefault(x => x.ProgrammaticName == pattern.ProgrammaticName);
            if (automationPattern is null)
            {
                return null;
            }

            var currentPattern = element.GetCurrentPattern(automationPattern) as TPattern;

            return currentPattern;
        }
    }
}
