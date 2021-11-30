namespace Orc.Automation
{
    using System;
    using Controls;

    public static class TestHostAutomationElementExtensions
    {
        public static bool TryLoadControl(this TestHostAutomationControl testHost, Type controlType, out string testHostAutomationId)
        {
            var controlAssembly = controlType.Assembly;
            testHostAutomationId = testHost.TryLoadControl(controlType.FullName, controlAssembly.Location, $"pack://application:,,,/{controlAssembly.GetName().Name};component/Themes/Generic.xaml");

            if (testHostAutomationId is null)
            {
                return false;
            }

            return !testHostAutomationId.StartsWith("Error");
        }
    }
}
