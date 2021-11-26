namespace Orc.Automation.Tests
{
    using System.Threading;
    using System.Windows;
    using System.Windows.Automation;
    using Controls;
    using NUnit.Framework;
    using Orc.Controls.Tests;

    public abstract class ControlUiTestFactsBase<TControl> : UiTestFactsBase
        where TControl : FrameworkElement
    {
        protected override string ExecutablePath => @"C:\Source\Orc.Controls\output\Debug\Orc.Automation.Host\net5.0-windows\Orc.Automation.Host.exe";
        protected override string MainWindowAutomationId => "AutomationHost";

        [SetUp]
        public virtual void SetUpTest()
        {
            var window = Setup.MainWindow;

            var testHost = window.Find<TestHostAutomationControl>(className: typeof(TestHost).FullName);
            if (testHost is null)
            {
                Assert.Fail("Can't find Test host");
            }

            Assert.That(testHost.TryLoadControl(typeof(TControl), out var testedControlAutomationId));

            Thread.Sleep(1000);

            var target = testHost.Find(id: testedControlAutomationId);
            if (target is null)
            {
                Assert.Fail("Can't find target control");
            }

            target.InitializeControlMap(this);
        }
    }
}
