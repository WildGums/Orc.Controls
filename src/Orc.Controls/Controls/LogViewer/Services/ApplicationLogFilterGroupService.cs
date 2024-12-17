﻿namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catel.IO;
using Catel.Logging;
using Catel.Runtime.Serialization.Xml;
using Catel.Services;
using FileSystem;
using Path = System.IO.Path;

public class ApplicationLogFilterGroupService : IApplicationLogFilterGroupService
{
    private const string LogFilterGroupsConfigFile = "LogFilterGroups.xml";

    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly IFileService _fileService;
    private readonly IXmlSerializer _xmlSerializer;
    private readonly IAppDataService _appDataService;

    public ApplicationLogFilterGroupService(IFileService fileService, IXmlSerializer xmlSerializer, IAppDataService appDataService)
    {
        ArgumentNullException.ThrowIfNull(xmlSerializer);
        ArgumentNullException.ThrowIfNull(fileService);
        ArgumentNullException.ThrowIfNull(appDataService);

        _xmlSerializer = xmlSerializer;
        _appDataService = appDataService;
        _fileService = fileService;
    }

    public async Task<IEnumerable<LogFilterGroup>> LoadAsync()
    {
        var filterGroups = new List<LogFilterGroup>();

        var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming);
        var configFile = Path.Combine(applicationDataDirectory, LogFilterGroupsConfigFile);
        if (_fileService.Exists(configFile))
        {
            try
            {
                await using var stream = _fileService.OpenRead(configFile);
                if (_xmlSerializer.Deserialize(typeof(LogFilterGroup[]), stream) is LogFilterGroup[] logGroups)
                {
                    filterGroups.AddRange(logGroups);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        var runtimeFilterGroups = CreateRuntimeFilterGroups();
        if (runtimeFilterGroups.Count > 0)
        {
            Log.Debug($"Adding '{runtimeFilterGroups.Count}' runtime filter groups");

            filterGroups.AddRange(runtimeFilterGroups);
        }

        return filterGroups.OrderBy(x => x.Name);
    }

    public async Task SaveAsync(IEnumerable<LogFilterGroup> filterGroups)
    {
        var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming);
        var configFile = Path.Combine(applicationDataDirectory, LogFilterGroupsConfigFile);

        var filterGroupsToSerialize = filterGroups.Where(x => !x.IsRuntime).ToList();

        try
        {
            await using var stream = _fileService.OpenWrite(configFile);
            _xmlSerializer.Serialize(filterGroupsToSerialize, stream);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
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
