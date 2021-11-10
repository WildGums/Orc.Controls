namespace Orc.Controls.Tests
{
    using System.Reflection;
    using Automation;
    using Automation.Tests.Services;
    using Catel.IoC;
    using NUnit.Framework;

    public abstract class UiTestFactsBase<TUiModel>
        where TUiModel : UiTestModel
    {
        private ISetupTestAutomationService _setupTestAutomationService;

        protected TUiModel TestModel { get; set; }

        protected virtual ISetupTestAutomationService SetupService 
            => _setupTestAutomationService ??= (ISetupTestAutomationService) this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(typeof(TUiModel).FindGenericTypeImplementation<ISetupTestAutomationService>(Assembly.GetExecutingAssembly()));

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

    public abstract class ControlUiTestFactsBase<TUiModel> : UiTestFactsBase<TUiModel>
        where TUiModel : UiTestModel
    {
    }
}
