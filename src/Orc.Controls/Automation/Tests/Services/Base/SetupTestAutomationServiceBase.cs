namespace Orc.Controls.Automation.Tests.Services
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Automation;

    public abstract class SetupTestAutomationServiceBase<TUiTestModel> : ISetupTestAutomationService<TUiTestModel>
        where TUiTestModel : UiTestModel
    {
        #region ISetupAutomationService Members
        public TUiTestModel SetUp()
        {
            var uiTestModel = CreateUiTestModel();

            InitializeTestModel(uiTestModel);

            return uiTestModel;
        }
        #endregion

        protected abstract TUiTestModel CreateUiTestModel();
        protected abstract string GetExecutableFileLocation();
        protected abstract Condition GetMainWindowCondition();

        protected virtual void InitializeTestModel(TUiTestModel uiTestModel)
        {
            var executableFileLocation = GetExecutableFileLocation();

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

            var findMainWindowCondition = GetMainWindowCondition();

            do
            {
                mainWindow = desktop.FindFirst(TreeScope.Children, findMainWindowCondition);
                ++numWaits;
                Thread.Sleep(200);
            }
            while (mainWindow is null && numWaits < 50);

            if (mainWindow is null)
            {
                throw new Exception("Failed to find Main window");
            }

            uiTestModel.MainWindow = mainWindow;
        }

        UiTestModel ISetupTestAutomationService.SetUp()
        {
            return SetUp();
        }
    }
}
