namespace Orc.Automation.Tests.Services
{
    public interface ISetupTestAutomationService
    {
        UiTestModel SetUp();
    }

    public interface ISetupTestAutomationService<TUiTestModel> : ISetupTestAutomationService
        where TUiTestModel : UiTestModel
    {
        new TUiTestModel SetUp();
    }
}
