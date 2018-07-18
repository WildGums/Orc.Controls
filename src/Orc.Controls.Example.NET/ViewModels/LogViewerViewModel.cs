// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Threading;
    using Orc.Controls.ViewModels;

    public class LogViewerViewModel : ViewModelBase
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IApplicationLogFilterGroupService _applicationLogFilterGroupService;
        #endregion

        #region Constructors
        public LogViewerViewModel(IApplicationLogFilterGroupService applicationLogFilterGroupService)
        {
            Argument.IsNotNull(() => applicationLogFilterGroupService);

            _applicationLogFilterGroupService = applicationLogFilterGroupService;

            AddLogRecords = new Command(OnAddLogRecordsExecute);
            TestUnderPressure = new TaskCommand(OnTestUnderPressureExecuteAsync);
        }
        #endregion

        #region Commands
        public Command AddLogRecords { get; set; }

        private void OnAddLogRecordsExecute()
        {
            Log.Debug("Single line debug message");
            Log.Debug("Multiline debug message that include a first line \nand a second line of the message");

            Log.Info("Single line info message");
            Log.Info("Multiline info message that include a first line \nand a second line of the message");

            Log.Warning("Single line warning message");
            Log.Warning("Multiline warning message that include a first line \nand a second line of the message");

            Log.Error("Single line error message");
            Log.Error("Multiline error message that include a first line \nand a second line of the message");
        }

        public TaskCommand TestUnderPressure { get; private set; }

        private async Task OnTestUnderPressureExecuteAsync()
        {
            await TaskHelper.Run(async () =>
            {
                var levelIndex = new Random();
                var events = new List<LogEvent>();
                events.Add(LogEvent.Debug);
                events.Add(LogEvent.Info);
                events.Add(LogEvent.Warning);
                events.Add(LogEvent.Error);

                var totalCount = 10000;
                for (var i = 0; i < totalCount; i++)
                {
                    var logEventIndex = levelIndex.Next(0, events.Count);
                    var logEvent = events[logEventIndex];

                    Log.Write(logEvent, $"[{i + 1} / {totalCount}] This is a stress test");

                    if (i % 20 == 0)
                    {
                        await Task.Delay(1);
                    }    
                }
            });
        }
        #endregion

        #region Methods
        public async Task<List<LogFilterGroup>> GetLogFilterGroupsAsync()
        {
            var items = new List<LogFilterGroup>();
            items.Add(new LogFilterGroup
            {
                Name = "None"
            });

            items.AddRange(await _applicationLogFilterGroupService.LoadAsync());

            return items;
        }
        #endregion
    }
}
