namespace Orc.Automation.Services
{
    using System.Diagnostics;
    using System.Windows.Automation;

    public interface ISetupAutomationService
    {
        AutomationSetup Setup(string executableFileLocation, Condition findMainWindowCondition, Process existingProcess = null);
    }
}
