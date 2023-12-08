namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.MVVM;

    public class LogViewerViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IApplicationLogFilterGroupService _applicationLogFilterGroupService;

        public LogViewerViewModel(IApplicationLogFilterGroupService applicationLogFilterGroupService)
        {
            ArgumentNullException.ThrowIfNull(applicationLogFilterGroupService);

            _applicationLogFilterGroupService = applicationLogFilterGroupService;

            AddLogRecords = new Command(OnAddLogRecordsExecute);
            TestUnderPressure = new TaskCommand(OnTestUnderPressureExecuteAsync);
        }

        public ScrollMode ScrollMode { get; set; } = ScrollMode.ManualScrollPriority;

        public Command AddLogRecords { get; }
        public TaskCommand TestUnderPressure { get; }

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

        private async Task OnTestUnderPressureExecuteAsync()
        {
            await Task.Run(async () =>
            {
                var levelIndex = new Random();
                var events = new List<LogEvent>
                {
                    LogEvent.Debug,
                    LogEvent.Info,
                    LogEvent.Warning,
                    LogEvent.Error
                };

                const int totalCount = 10000;
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

        public async Task<List<LogFilterGroup>> GetLogFilterGroupsAsync()
        {
            var items = new List<LogFilterGroup>
            {
                new LogFilterGroup
                {
                    Name = "None"
                }
            };

            items.AddRange(await _applicationLogFilterGroupService.LoadAsync());

            return items;
        }
    }
}
