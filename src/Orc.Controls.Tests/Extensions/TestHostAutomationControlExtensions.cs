namespace Orc.Controls.Tests
{
    using System;
    using System.Linq;
    using Orc.Automation;
    using Orc.Automation.Controls;
    using Theming;
    using FrameworkElement = System.Windows.FrameworkElement;

    public static class TestHostAutomationControlExtensions
    {
        public static bool TryLoadControlWithForwarders(this TestHostAutomationControl testHost, Type controlType, out string testHostAutomationId, params string[] resources)
        {
            var controlAssembly = controlType.Assembly;

            var controlTypeFullName = controlType.FullName;
            var controlAssemblyLocation = controlAssembly.Location;

            testHostAutomationId = string.Empty;

            if (!testHost.TryLoadAssembly(controlAssemblyLocation))
            {
                testHostAutomationId = $"Error! Can't load control assembly from: {controlAssemblyLocation}";

                return false;
            }

            foreach (var resource in resources ?? Enumerable.Empty<string>())
            {
                if (!testHost.TryLoadResources(resource))
                {
                    testHostAutomationId = $"Error! Can't load control resource: {resource}";
                }
            }

            testHost.Execute<SetupThemeAutomationMethodRun>();
           
            testHostAutomationId = testHost.PutControl(controlTypeFullName);
            if (string.IsNullOrWhiteSpace(testHostAutomationId) || testHostAutomationId.StartsWith("Error"))
            {
                testHostAutomationId = $"Error! Can't put control inside test host control: {controlTypeFullName}";

                return false;
            }
            
            return true;
        }
    }

    public class SetupThemeAutomationMethodRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.FromValue(true);

            StyleHelper.CreateStyleForwardersForDefaultStyles();
            ThemeManager.Current.SynchronizeTheme();

            return true;
        }
    }
}
