namespace Orc.Controls.Example.ViewModels;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catel.Logging;
using Catel.MVVM;
using Microsoft.Extensions.Logging;

public class LogViewerViewModel : ViewModelBase
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(LogViewerViewModel));

    private readonly IApplicationLogFilterGroupService _applicationLogFilterGroupService;

    public LogViewerViewModel(IServiceProvider serviceProvider, IApplicationLogFilterGroupService applicationLogFilterGroupService) 
        : base(serviceProvider)
    {
        _applicationLogFilterGroupService = applicationLogFilterGroupService;

        AddLogRecords = new Command(serviceProvider, OnAddLogRecordsExecute);
        TestUnderPressure = new TaskCommand(serviceProvider, OnTestUnderPressureExecuteAsync);
    }

    public ScrollMode ScrollMode { get; set; } = ScrollMode.ManualScrollPriority;

    public Command AddLogRecords { get; }
    public TaskCommand TestUnderPressure { get; }

    private void OnAddLogRecordsExecute()
    {
        Logger.LogDebug("Single line debug message");
        Logger.LogDebug("Multiline debug message that include a first line \nand a second line of the message");

        Logger.LogInformation("Single line info message");
        Logger.LogInformation("Multiline info message that include a first line \nand a second line of the message");

        Logger.LogWarning("Single line warning message");
        Logger.LogWarning("Multiline warning message that include a first line \nand a second line of the message");

        Logger.LogError("Single line error message");
        Logger.LogError("Multiline error message that include a first line \nand a second line of the message");
    }

    private async Task OnTestUnderPressureExecuteAsync()
    {
        await Task.Run(async () =>
        {
            var levelIndex = new Random();
            var events = new List<LogLevel>
            {
                LogLevel.Debug,
                LogLevel.Information,
                LogLevel.Warning,
                LogLevel.Error
            };

            const int totalCount = 10000;
            for (var i = 0; i < totalCount; i++)
            {
                var logEventIndex = levelIndex.Next(0, events.Count);
                var logEvent = events[logEventIndex];

                Logger.Log(logEvent, $"[{i + 1} / {totalCount}] This is a stress test");

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
