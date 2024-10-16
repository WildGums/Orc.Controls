namespace Orc.Controls.Tests
{
    using System.IO;
    using NUnit.Framework;
    using Orc.Automation.Controls;
    using Orc.Automation.Tests;

    public abstract class StyledControlTestFacts<TControl> : ControlUiTestsBase<TControl>
        where TControl : System.Windows.FrameworkElement
    {
        protected override bool TryLoadControl(TestHostAutomationControl testHost, out string testedControlAutomationId)
        {
            var controlType = typeof(TControl);

            var testDirectory = TestContext.CurrentContext.TestDirectory;

            testHost.TryLoadAssembly(Path.Combine(testDirectory, "DiffEngine.dll"));
            testHost.TryLoadAssembly(Path.Combine(testDirectory, "ApprovalUtilities.dll"));
            testHost.TryLoadAssembly(Path.Combine(testDirectory, "ApprovalTests.dll"));
            testHost.TryLoadAssembly(Path.Combine(testDirectory, "ControlzEx.dll"));
            testHost.TryLoadAssembly(Path.Combine(testDirectory, "Orc.Theming.dll"));
            testHost.TryLoadAssembly(Path.Combine(testDirectory, "Orc.Controls.dll"));
            testHost.TryLoadAssembly(Path.Combine(testDirectory, "Orc.Automation.Tests.dll"));
            testHost.TryLoadAssembly(Path.Combine(testDirectory, "Orc.Controls.Tests.dll"));
            
            testHost.TryLoadResources("pack://application:,,,/Orc.Theming;component/Themes/Generic.xaml");
            testHost.TryLoadResources("pack://application:,,,/Orc.Controls;component/Themes/Generic.xaml");
            
            return testHost.TryLoadControlWithForwarders(controlType, out testedControlAutomationId, $"pack://application:,,,/{controlType.Assembly.GetName().Name};component/Themes/Generic.xaml");
        }
    }
}
