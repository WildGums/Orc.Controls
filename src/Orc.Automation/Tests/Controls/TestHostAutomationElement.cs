namespace Orc.Automation.Tests.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;

    public class TestHostAutomationElement : RunMethodAutomationElement
    {
        public TestHostAutomationElement(AutomationElement element) 
            : base(element)
        {
        }

        public string TryLoadControl(string fullName, string assemblyLocation, params string[] resources)
        {
            if (!Element.TryExecute<bool>(nameof(TestHostAutomationPeer.LoadAssembly), assemblyLocation, out var loadAssemblyResult))
            {
                return $"Error! Can't load control assembly from: {assemblyLocation}";
            }

            if (!loadAssemblyResult)
            {
                return $"Error! Can't load control assembly from: {assemblyLocation}";
            }

            foreach (var resource in resources ?? Enumerable.Empty<string>())
            {
                if (!Element.TryExecute<bool>(nameof(TestHostAutomationPeer.LoadResources), resource, out _))
                {
                    return $"Error! Can't load control resource: {resource}";
                }
            }

            if (!Element.TryExecute<string>(nameof(TestHostAutomationPeer.PutControl), fullName, out var testedControlAutomationId))
            {
                return $"Error! Can't put control inside test host control: {fullName}";
            }

            return testedControlAutomationId;
        }
    }
}
