namespace Orc.Controls.Tests
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using Automation.Tests;
    using NUnit.Framework;
    using Orc.Automation;

    public abstract class ControlUiTestFactsBase<TControl> : UiTestFactsBase<OrcControlsUiTestModel>
        where TControl : FrameworkElement
    {

        [SetUp]
        public virtual void SetUpTest()
        {
            var window = TestModel.MainWindow;

            var testHost = window.Find(className: typeof(TestHost).FullName);
            if (testHost is null)
            {
                Assert.Fail("Can't find Test host");
            }

            var controlAssembly = typeof(TControl).Assembly;
            if (!testHost.TryExecute<bool>(nameof(TestHostAutomationPeer.LoadAssembly), controlAssembly.Location, out var loadAssemblyResult))
            {
                Assert.Fail($"Can't Execute {nameof(TestHostAutomationPeer.LoadAssembly)} function");
            }

            if (!testHost.TryExecute<bool>(nameof(TestHostAutomationPeer.LoadResources), "pack://application:,,,/Orc.Controls;component/Themes/Generic.xaml", out _))
            {
                Assert.Fail($"Can't Execute {nameof(TestHostAutomationPeer.LoadResources)} function");
            }

            if (!testHost.TryExecute<string>(nameof(TestHostAutomationPeer.PutControl), typeof(TControl).FullName, out var testedControlAutomationId))
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
           // _testHostWindow.TryCloseWindow();
        }
    }
}
