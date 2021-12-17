namespace Orc.Automation
{
    using System;
    using System.Diagnostics;
    using System.Windows.Automation;

#pragma warning disable IDISP025 // Class with no virtual dispose method should be sealed.
    public class AutomationSetup : IDisposable
#pragma warning restore IDISP025 // Class with no virtual dispose method should be sealed.
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
