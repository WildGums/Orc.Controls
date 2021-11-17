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

        protected AutomationSetup Setup { get; private set; }
        protected virtual string ExecutablePath => string.Empty;
        protected virtual string MainWindowAutomationId => string.Empty;
        protected virtual Condition FindMainWindowCondition => new PropertyCondition(AutomationElement.AutomationIdProperty, MainWindowAutomationId);

        protected ISetupAutomationService SetupAutomationService => _setupAutomationService ??= CreateSetupAutomationService();

        [OneTimeSetUp]
        public virtual void SetUp()
        {
            Setup = SetupAutomationService?.Setup(ExecutablePath, FindMainWindowCondition);

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
            return this.GetServiceLocator().ResolveType<ISetupAutomationService>();
        }
    }
}
