namespace Orc.Automation.Tests.Services
{
    public interface ISetupTestAutomationService
    {
        AutomationSetup SetUp();
    }

    public interface ISetupTestAutomationService<TUiTestModel> : ISetupTestAutomationService
        where TUiTestModel : AutomationSetup
    {
        new TUiTestModel SetUp();
    }
}
