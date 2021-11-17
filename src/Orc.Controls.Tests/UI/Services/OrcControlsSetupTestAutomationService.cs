namespace Orc.Controls.Tests
{
    using System.Windows.Automation;
    using Automation.Tests.Services;

    public class OrcControlsSetupTestAutomationService : SetupTestAutomationServiceBase<OrcControlsUiTestModel>
    {
        protected override OrcControlsUiTestModel CreateUiTestModel()
        {
            return new OrcControlsUiTestModel();
        }

        protected override string GetExecutableFileLocation()
        {
            return @"C:\Source\Orc.Controls\output\Debug\Orc.Automation.Host\net5.0-windows\Orc.Automation.Host.exe";
        }

        protected override Condition GetMainWindowCondition()
        {
            return new PropertyCondition(AutomationElement.AutomationIdProperty, "AutomationHost");
        }
    }
}
