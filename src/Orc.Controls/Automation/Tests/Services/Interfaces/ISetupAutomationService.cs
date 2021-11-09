namespace Orc.Controls.Automation.Tests.Services
{
    public interface ISetupAutomationService
    {
        UiTestModel SetUp(string executableFileLocation, string mainWindowTitle);
    }
}
