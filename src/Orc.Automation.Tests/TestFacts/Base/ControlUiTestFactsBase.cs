namespace Orc.Automation.Tests
{
    using System.Reflection;
    using System.Threading;
    using Controls;
    using NUnit.Framework;
    using Orc.Controls.Tests;

    public abstract class ControlUiTestFactsBase<TControl> : UiTestFactsBase
        where TControl : System.Windows.FrameworkElement
    {
        protected override string ExecutablePath => @"C:\Source\Orc.Controls\output\Debug\Orc.Automation.Host\net6.0-windows\Orc.Automation.Host.exe";
        protected override string MainWindowAutomationId => "AutomationHost";

        [SetUp]
        public virtual void SetUpTest()
        {
            var window = Setup.MainWindow;

            var testHost = window.Find<TestHostAutomationControl>(className: typeof(Orc.Automation.TestHost).FullName);
            if (testHost is null)
            {
                Assert.Fail("Can't find Test host");
            }

            BeforeLoadingControl(testHost);

            Assert.That(TryLoadControl(testHost, out var testedControlAutomationId));

            AfterLoadingControl(testHost);

            Thread.Sleep(200);

            var target = testHost.Find(id: testedControlAutomationId);
            if (target is null)
            {
                Assert.Fail("Can't find target control");
            }

            target.InitializeControlMap(this);
        }

        protected virtual bool TryLoadControl(TestHostAutomationControl testHost, out string testedControlAutomationId)
        {
            return testHost.TryLoadControl(typeof(TControl), out testedControlAutomationId);
        }

        protected virtual void BeforeLoadingControl(TestHostAutomationControl testHost)
        {
        }

        protected virtual void AfterLoadingControl(TestHostAutomationControl testHost)
        {
        }
    }
}
