namespace Orc.Controls.Tests
{
    using System.Windows.Automation;
    using Automation;
    using Orc.Automation;
    using Orc.Automation.Tests.Services;

    public class ColorLegendAutomationElementSetupTestAutomationService : SetupTestAutomationServiceBase<OrcControlsUiTestModel<ColorLegendAutomationElement>>
    {
        protected override OrcControlsUiTestModel<ColorLegendAutomationElement> CreateUiTestModel()
        {
            return new OrcControlsUiTestModel<ColorLegendAutomationElement>();
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

    public class OrcControlsSetupTestAutomationService : SetupTestAutomationServiceBase<OrcControlsUiTestModel<CommandAutomationElement>>
    {
        protected override OrcControlsUiTestModel<CommandAutomationElement> CreateUiTestModel()
        {
            return new OrcControlsUiTestModel<CommandAutomationElement>();
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
