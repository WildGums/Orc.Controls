namespace Orc.Controls.Tests
{
    using System.Threading;
    using System.Windows.Automation;
    using Automation;
    using Automation.Tests;
    using Automation.Tests.Services;
    using NUnit.Framework;
    using Orc.Controls.Controls;
    using Orc.Controls.Controls.Automation;

    public class OrcControlsSetupTestAutomationService : SetupTestAutomationServiceBase<OrcControlsUiTestModel>
    {
        protected override OrcControlsUiTestModel CreateUiTestModel()
        {
            return new OrcControlsUiTestModel();
        }

        protected override string GetExecutableFileLocation()
        {
            return @"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Example\net5.0-windows\Orc.Controls.Example.exe";
        }

        protected override Condition GetMainWindowCondition()
        {
            return new PropertyCondition(AutomationElement.NameProperty, "Orc.Controls example");
        }
    }
}
