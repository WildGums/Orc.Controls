namespace Orc.Automation
{
    using System;
    using System.Diagnostics;
    using System.Windows.Automation;

    public class UiTestModel : IDisposable
    {
        public Process Process { get; set; }
        public AutomationElement Desktop { get; set; }
        public AutomationElement MainWindow { get; set; }

        #region IDisposable Members
        public void Dispose()
        {
            var process = Process;
            if (process is null)
            {
                return;
            }

            process.Kill();
            process.Dispose();
        }
        #endregion
    }
}
