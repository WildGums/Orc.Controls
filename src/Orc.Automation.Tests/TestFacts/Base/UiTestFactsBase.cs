namespace Orc.Controls.Tests
{
    using System.Windows.Automation;
    using Automation;
    using Automation.Services;
    using Catel.IoC;
    using NUnit.Framework;

    public abstract class UiTestFactsBase
    {
        private ISetupAutomationService _setupAutomationService;
#pragma warning disable IDISP006 // Don't ignore created IDisposable.
        protected AutomationSetup Setup { get; private set; }
#pragma warning disable IDISP006 // Don't ignore created IDisposable.
        protected virtual string ExecutablePath => string.Empty;
        protected virtual string MainWindowAutomationId => string.Empty;
        protected virtual Condition FindMainWindowCondition => new PropertyCondition(AutomationElement.AutomationIdProperty, MainWindowAutomationId);

        protected ISetupAutomationService SetupAutomationService => _setupAutomationService ??= CreateSetupAutomationService();

        [OneTimeSetUp]
        public virtual void SetUp()
        {
#pragma warning disable IDISP003 // Don't ignore created IDisposable.
            Setup = SetupAutomationService?.Setup(ExecutablePath, FindMainWindowCondition);
#pragma warning disable IDISP003 // Don't ignore created IDisposable.

            Assert.IsNotNull(Setup);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Setup?.Dispose();
            Setup = null;
        }

        protected virtual ISetupAutomationService CreateSetupAutomationService()
        {
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
            return this.GetServiceLocator().ResolveType<ISetupAutomationService>();
#pragma warning restore IDISP004 // Don't ignore created IDisposable.
        }
    }
}
