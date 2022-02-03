namespace Orc.Controls.Tests
{
    using Orc.Automation.Controls;
    using Orc.Automation.Tests;

    public abstract class StyledControlTestFacts<TControl> : ControlUiTestFactsBase<TControl>
        where TControl : System.Windows.FrameworkElement
    {
        protected override bool TryLoadControl(TestHostAutomationControl testHost, out string testedControlAutomationId)
        {
            var controlType = typeof(TControl);
            
            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\DiffEngine.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\ApprovalUtilities.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\ApprovalTests.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Controls.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Automation.Tests.dll");

            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Controls.Tests.dll");

            var result = testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Controls.Tests.dll");

            testHost.TryLoadResources("pack://application:,,,/Orc.Controls;component/Themes/Generic.xaml");


            return testHost.TryLoadControlWithForwarders(controlType, out testedControlAutomationId, $"pack://application:,,,/{controlType.Assembly.GetName().Name};component/Themes/Generic.xaml");
        }
    }
}
