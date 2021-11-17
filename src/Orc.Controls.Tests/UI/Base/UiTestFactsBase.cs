namespace Orc.Controls.Tests
{
    using System.Reflection;
    using Catel.IoC;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests.Services;
    using TypeExtensions = Orc.Controls.TypeExtensions;

    public abstract class UiTestFactsBase<TUiModel>
        where TUiModel : AutomationSetup
    {
        private ISetupTestAutomationService _setupTestAutomationService;

        protected TUiModel TestModel { get; set; }

        protected virtual ISetupTestAutomationService SetupService 
            => _setupTestAutomationService ??= (ISetupTestAutomationService) this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(TypeExtensions.FindGenericTypeImplementation<ISetupTestAutomationService>(typeof(TUiModel), Assembly.GetExecutingAssembly()));

        [OneTimeSetUp]
        public virtual void SetUp()
        {
            TestModel ??= (TUiModel) SetupService?.SetUp();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            TestModel?.Dispose();
        }
    }
}
