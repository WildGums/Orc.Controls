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
            Argument.IsNotNull(() => xmlSerializer);
            Argument.IsNotNull(() => fileService);

            _xmlSerializer = xmlSerializer;
            _fileService = fileService;
        }
        #endregion

        #region IApplicationLogFilterGroupService Members
        public async Task<IEnumerable<LogFilterGroup>> LoadAsync()
        {
            var applicationDataDirectory = Catel.IO.Path.GetApplicationDataDirectory();
            var configFile = Path.Combine(applicationDataDirectory, LogFilterGroupsConfigFile);
            if (!_fileService.Exists(configFile))
            {
                return ArrayShim.Empty<LogFilterGroup>();
            }

            try
            {
                using (var stream = _fileService.OpenRead(configFile))
                {
                    return (LogFilterGroup[])_xmlSerializer.Deserialize(typeof(LogFilterGroup[]), stream);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return ArrayShim.Empty<LogFilterGroup>();
        }

        public async Task SaveAsync(IEnumerable<LogFilterGroup> filterGroups)
        {
            var applicationDataDirectory = Catel.IO.Path.GetApplicationDataDirectory();
            var configFile = Path.Combine(applicationDataDirectory, LogFilterGroupsConfigFile);
            try
            {
                using (var stream = _fileService.OpenWrite(configFile))
                {
                    _xmlSerializer.Serialize(filterGroups, stream);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        #endregion
    }
}
