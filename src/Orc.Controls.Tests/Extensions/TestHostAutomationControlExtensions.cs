namespace Orc.Controls.Tests
{
    using System;
    using System.Linq;
    using Orc.Automation.Controls;
    using Theming;

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

            //Apply style forwarders
            testHost.RunMethod(typeof(StyleHelper), nameof(StyleHelper.CreateStyleForwardersForDefaultStyles));
            //testHost.ExecuteAutomationMethod<CreateStyleForwardersMethodRun>();
           
            testHostAutomationId = testHost.PutControl(controlTypeFullName);
            if (string.IsNullOrWhiteSpace(testHostAutomationId) || testHostAutomationId.StartsWith("Error"))
            {
                testHostAutomationId = $"Error! Can't put control inside test host control: {controlTypeFullName}";

                return false;
            }
            
            return true;
        }
    }
}
