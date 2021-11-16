namespace Orc.Controls.Tests
{
    using System.Threading;
    using System.Windows;
    using System.Windows.Automation;
    using Automation;
    using Automation.Tests;
    using NUnit.Framework;
    using Orc.Controls.Controls;
    using Orc.Controls.Controls.Automation;

    public abstract class ControlUiTestFactsBase<TControl> : UiTestFactsBase<OrcControlsUiTestModel>
        where TControl : FrameworkElement
    {
        private AutomationElement _testHostWindow;

        [SetUp]
        public virtual void SetUpTest()
        {
            var window = TestModel.MainWindow;

            var button = window.Find(id: "TestButtonId");
            if (button is null)
            {
                Assert.Fail("Can't find Start test button");
            }

            var numWaits = 10;
            while (!button.TryInvoke() && numWaits > 0)
            {
                Thread.Sleep(200);
                numWaits--;
            }

            if (numWaits < 0)
            {
                Assert.Fail("Can't Invoke start test button");
            }

            Thread.Sleep(500);

            _testHostWindow = window.Find(className: "Orc.Controls.Example.Views.TestHostWindow");
            if (_testHostWindow is null)
            {
                Assert.Fail("Can't find Test host window");
            }

            var testHost = window.Find(className: typeof(TestHost).FullName);
            if (testHost is null)
            {
                Assert.Fail("Can't find Test host");
            }

            if(!testHost.TryExecute<string>(nameof(TestHostAutomationPeer.PutControl), typeof(TControl).FullName, out var testedControlAutomationId))
            {
                Assert.Fail($"Can't Execute {nameof(TestHostAutomationPeer.PutControl)} function");
            }

            if (testedControlAutomationId is null)
            {
                Assert.Fail("Can't initialize target control");
            }

            if (testedControlAutomationId.StartsWith("Error"))
            {
                Assert.Fail(testedControlAutomationId);
            }

            Thread.Sleep(1000);

            TestModel.TargetControl = testHost.Find(id: testedControlAutomationId);
            if (TestModel.TargetControl is null)
            {
                Assert.Fail("Can't find target control");
            }
        }

        [TearDown]
        public virtual void TearDownTest()
        {
            _testHostWindow.TryCloseWindow();
        }
    }
}
