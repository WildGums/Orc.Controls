namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catel.IO;
using Catel.Services;
using FileSystem;
using Microsoft.Extensions.Logging;
using Orc.Serialization.Json;
using Path = System.IO.Path;

public class ApplicationLogFilterGroupService : IApplicationLogFilterGroupService
{
    private const string LogFilterGroupsConfigFile = "LogFilterGroups.json";

    private readonly ILogger<ApplicationLogFilterGroupService> _logger;
    private readonly IFileService _fileService;
    private readonly IAppDataService _appDataService;
    private readonly IJsonSerializerFactory _jsonSerializerFactory;

    public ApplicationLogFilterGroupService(ILogger<ApplicationLogFilterGroupService> logger, 
        IFileService fileService, IAppDataService appDataService, IJsonSerializerFactory jsonSerializerFactory)
    {
        _appDataService = appDataService;
        _jsonSerializerFactory = jsonSerializerFactory;
        _logger = logger;
        _fileService = fileService;
    }

    public async Task<IReadOnlyList<LogFilterGroup>> LoadAsync()
    {
        var filterGroups = new List<LogFilterGroup>();

        var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming);
        var configFile = Path.Combine(applicationDataDirectory, LogFilterGroupsConfigFile);
        if (_fileService.Exists(configFile))
        {
            try
            {
                await using var stream = _fileService.OpenRead(configFile);

                var serializer = _jsonSerializerFactory.CreateSerializer();

                if (serializer.Deserialize(stream, typeof(LogFilterGroup[])) is LogFilterGroup[] logGroups)
                {
                    filterGroups.AddRange(logGroups);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load");
            }
        }

        var runtimeFilterGroups = CreateRuntimeFilterGroups();
        if (runtimeFilterGroups.Count > 0)
        {
            _logger.LogDebug($"Adding '{runtimeFilterGroups.Count}' runtime filter groups");

            filterGroups.AddRange(runtimeFilterGroups);
        }

        return filterGroups
            .OrderBy(x => x.Name)
            .ToArray();
    }

    public async Task SaveAsync(IReadOnlyList<LogFilterGroup> filterGroups)
    {
        var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming);
        var configFile = Path.Combine(applicationDataDirectory, LogFilterGroupsConfigFile);

        var filterGroupsToSerialize = filterGroups.Where(x => !x.IsRuntime).ToList();

        try
        {
            await using var stream = _fileService.OpenWrite(configFile);

            var serializer = _jsonSerializerFactory.CreateSerializer();

            serializer.Serialize(stream, filterGroupsToSerialize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save");
        }
    }

    protected virtual List<LogFilterGroup> CreateRuntimeFilterGroups()
    {
        var filterGroups = new List<LogFilterGroup>();

        var methodTimerFilterGroup = new LogFilterGroup
        {
            Name = "Method timings",
            IsRuntime = true,
            IsEnabled = true
        };

        methodTimerFilterGroup.LogFilters.Add(new LogFilter
        {
            Name = "Exclude anything but method timer",
            Action = LogFilterAction.Exclude,
            ExpressionType = LogFilterExpressionType.NotContains,
            ExpressionValue = "METHODTIMER",
            Target = LogFilterTarget.LogMessage
        });

        filterGroups.Add(methodTimerFilterGroup);

        return filterGroups;
    }
}
