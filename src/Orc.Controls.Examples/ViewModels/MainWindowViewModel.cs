// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Catel.Logging;
    using Catel.MVVM;
    using Models;

    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            TimeSpanValue = new TimeSpan(10, 11, 12, 13);
            DateTimeValue = DateTime.Now;
            AccentColorBrush = Brushes.Orange;

            FilterSource = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("1", "abcd"),
                new KeyValuePair<string, string>("2", "basdf"),
                new KeyValuePair<string, string>("3", "bwerr"),
                new KeyValuePair<string, string>("4", "oiydd"),
                new KeyValuePair<string, string>("5", "klhhs"),
                new KeyValuePair<string, string>("6", "sdfhi"),
            };
			
			AddLogRecords = new Command(OnAddLogRecordsExecute);

            FlowDoc = new FlowDocument();
            FlowDoc.Foreground = AccentColorBrush.Clone();
        }

        #endregion

        #region Properties
        public TimeSpan TimeSpanValue { get; set; }

        public DateTime DateTimeValue { get; set; }

        public Brush AccentColorBrush { get; set; }

        public List<KeyValuePair<string, string>> FilterSource { get; private set; }

        public string FilterText { get; set; }
        #endregion

        #region Events
        public Command AddLogRecords { get; set; }

        private void OnAddLogRecordsExecute()
        {
            Log.Debug("Debug");
            Log.Warning("Warning");
            Log.Error("Error");
            Log.Info("Info");
        }
        #endregion
        public FlowDocument FlowDoc { get; set; }
    }
}