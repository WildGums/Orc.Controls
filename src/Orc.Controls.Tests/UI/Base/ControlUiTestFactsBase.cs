namespace Orc.Controls.Tests
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests.Controls;

    public abstract class ControlUiTestFactsBase<TControl, TAutomationElement> : UiTestFactsBase<OrcControlsUiTestModel<TAutomationElement>>
        where TControl : FrameworkElement
        where TAutomationElement : AutomationElementBase
    {
        [SetUp]
        public virtual void SetUpTest()
        {
            var window = TestModel.MainWindow;

            var testHost = window.Find<TestHostAutomationElement>(className: typeof(TestHost).FullName);
            if (testHost is null)
            {
                Assert.Fail("Can't find Test host");
            }

            if (!testHost.TryLoadControl(typeof(TControl), out var testedControlAutomationId))
            {
                Assert.Fail($"Error Message: {testedControlAutomationId}");
            }

            Thread.Sleep(1000);

            TestModel.TargetControl = testHost.Find<TAutomationElement>(id: testedControlAutomationId);
            if (TestModel.TargetControl is null)
            {
                Assert.Fail("Can't find target control");
            }
        }

        [TearDown]
        public virtual void TearDownTest()
        {
           
        }
    }
}
