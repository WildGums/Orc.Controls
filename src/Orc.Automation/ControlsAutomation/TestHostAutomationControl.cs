namespace Orc.Automation.Controls
{
    using System;
    using System.Linq;
    using System.Windows.Automation;

    public class TestHostAutomationControl : AutomationControl
    {
        public TestHostAutomationControl(AutomationElement element) 
            : base(element)
        {
        }

        public string TryLoadControl(string fullName, string assemblyLocation, params string[] resources)
        {
            if (!TryLoadAssembly(assemblyLocation))
            {
                return $"Error! Can't load control assembly from: {assemblyLocation}";
            }

            foreach (var resource in resources ?? Enumerable.Empty<string>())
            {
                if (!Access.Execute<bool>(nameof(TestHostAutomationPeer.LoadResources), resource))
                {
                    return $"Error! Can't load control resource: {resource}";
                }
            }

            var testedControlAutomationId = Access.Execute<string>(nameof(TestHostAutomationPeer.PutControl), fullName);
            if (string.IsNullOrWhiteSpace(testedControlAutomationId) || testedControlAutomationId.StartsWith("Error"))
            {
                return $"Error! Can't put control inside test host control: {fullName}";
            }

            return testedControlAutomationId;
        }

        public string PutControl(string controlFullName)
        {
            return Access.Execute<string>(nameof(TestHostAutomationPeer.PutControl), controlFullName);
        }

        public bool TryLoadResources(string assemblyLocation)
        {
            return Access.Execute<bool>(nameof(TestHostAutomationPeer.LoadResources), assemblyLocation);
        }

        public bool TryLoadAssembly(string assemblyLocation)
        {
            return Access.Execute<bool>(nameof(TestHostAutomationPeer.LoadAssembly), assemblyLocation);
        }

        public bool TryLoadUnmanaged(string assemblyLocation)
        {
            try
            {
                Access.Execute(nameof(TestHostAutomationPeer.LoadUnmanaged), assemblyLocation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }
    }
}
