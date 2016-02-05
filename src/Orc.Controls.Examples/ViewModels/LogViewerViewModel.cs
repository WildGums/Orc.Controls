// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using Catel.Logging;
    using Catel.MVVM;

    public class LogViewerViewModel : ViewModelBase
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        public LogViewerViewModel()
        {
            AddLogRecords = new Command(OnAddLogRecordsExecute);
        }
        #endregion

        public Command AddLogRecords { get; set; }

        private void OnAddLogRecordsExecute()
        {
            Log.Debug("Debug");
            Log.Warning("Warning");
            Log.Error("Error");
            Log.Info("Info");
        }
    }
}