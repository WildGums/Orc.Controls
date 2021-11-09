namespace Orc.Controls.Automation.Tests.Services
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Automation;

    public class SetupAutomationService : ISetupAutomationService
    {
        #region ISetupAutomationService Members
        public UiTestModel SetUp(string executableFileLocation, string mainWindowTitle)
        {
            var uiTestModel = new UiTestModel();

            InitializeTestModel(uiTestModel, executableFileLocation, mainWindowTitle);

            return uiTestModel;
        }
        #endregion

        protected virtual void InitializeTestModel(UiTestModel uiTestModel, string executableFileLocation, string mainWindowTitle)
        {
            var process = Process.Start(executableFileLocation);
            var numWaits = 0;
            do
            {
                ++numWaits;
                Thread.Sleep(10);
            } 
            while (process is null && numWaits < 50);

            if (process is null)
            {
                throw new Exception($"Failed to find '{executableFileLocation}'");
            }
            uiTestModel.Process = process;

            var desktop = AutomationElement.RootElement;
            if (desktop is null)
            {
                throw new Exception("Unable to get Desktop")
                {
                    HelpLink = null,
                    HResult = 0,
                    Source = null
                };
            }
            uiTestModel.Desktop = desktop;
            numWaits = 0;
            
            AutomationElement mainWindow;
            do
            {
                mainWindow = desktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, mainWindowTitle));
                ++numWaits;
                Thread.Sleep(200);
            } while (mainWindow is null && numWaits < 50);

            if (mainWindow is null)
            {
                throw new Exception($"Failed to find Main window with title {mainWindowTitle}");
            }

            uiTestModel.MainWindow = mainWindow;
        }
    }
}
