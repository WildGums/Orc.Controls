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
            Log.Debug("Single line debug message");
            Log.Warning("Multiline debug message tha include a first line \nand a second line of the message");

            Log.Warning("Single line warning message");
            Log.Warning("Multiline warning message tha include a first line \nand a second line of the message");

            Log.Error("Single line error message");
            Log.Error("Multiline error message tha include a first line \nand a second line of the message");

            Log.Info("Single line info message");
            Log.Info("Multiline info message tha include a first line \nand a second line of the message");
        }
    }
}