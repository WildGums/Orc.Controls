// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationLogFilterGroupService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.Logging;
    using Catel.Runtime.Serialization.Xml;
    using FileSystem;

    public class ApplicationLogFilterGroupService : IApplicationLogFilterGroupService
    {
        #region Constants
        private const string LogFilterGroupsConfigFile = "LogFilterGroups.xml";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly IFileService _fileService;
        private readonly IXmlSerializer _xmlSerializer;
        #endregion

        #region Constructors
        public ApplicationLogFilterGroupService(IFileService fileService, IXmlSerializer xmlSerializer)
        {
            ArgumentNullException.ThrowIfNull(xmlSerializer);
            ArgumentNullException.ThrowIfNull(fileService);

            _xmlSerializer = xmlSerializer;
            _fileService = fileService;
        }
        #endregion

        #region IApplicationLogFilterGroupService Members
        public async Task<IEnumerable<LogFilterGroup>> LoadAsync()
        {
            var filterGroups = new List<LogFilterGroup>();

            var applicationDataDirectory = Catel.IO.Path.GetApplicationDataDirectory();
            var configFile = Path.Combine(applicationDataDirectory, LogFilterGroupsConfigFile);
            if (_fileService.Exists(configFile))
            {
                try
                {
                    using (var stream = _fileService.OpenRead(configFile))
                    {
                        filterGroups.AddRange((LogFilterGroup[])_xmlSerializer.Deserialize(typeof(LogFilterGroup[]), stream));
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
            var applicationDataDirectory = Catel.IO.Path.GetApplicationDataDirectory();
            var configFile = Path.Combine(applicationDataDirectory, LogFilterGroupsConfigFile);

            var filterGroupsToSerialize = filterGroups.Where(x => !x.IsRuntime).ToList();

            try
            {
                using (var stream = _fileService.OpenWrite(configFile))
                {
                    _xmlSerializer.Serialize(filterGroupsToSerialize, stream);
                }
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
        #endregion
    }
}
