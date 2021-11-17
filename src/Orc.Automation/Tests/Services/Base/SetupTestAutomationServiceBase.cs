namespace Orc.Automation.Tests.Services
{
    using System;
    using System.Diagnostics;
    using System.Linq;
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
            var numWaits = 0;

            var executableFileLocation = GetExecutableFileLocation();
            var process = Process.Start(executableFileLocation);

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
            
            AutomationElement mainWindow = null;

            var findMainWindowCondition = GetMainWindowCondition();

            do
            {
                try
                {
                    mainWindow = desktop.FindFirst(TreeScope.Children, findMainWindowCondition);
                }
                catch
                {
                    //Do nothing
                }


                ++numWaits;
                Thread.Sleep(200);
            }
            while (mainWindow is null && numWaits < 500);

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
