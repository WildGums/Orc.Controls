// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationFilterGroupService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Catel;
    using Catel.Collections;
    using Catel.Logging;
    using Catel.Runtime.Serialization.Xml;
    using Orc.Controls.Models;

    public class ApplicationFilterGroupService : IApplicationFilterGroupService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string LogFilterGroupsConfigFile = "LogFilterGroups.xml";

        private readonly IXmlSerializer _xmlSerializer;

        public ApplicationFilterGroupService(IXmlSerializer xmlSerializer)
        {
            Argument.IsNotNull(() => xmlSerializer);

            _xmlSerializer = xmlSerializer;
        }

        public IEnumerable<LogFilterGroup> Load()
        {
            var applicationDataDirectory = Catel.IO.Path.GetApplicationDataDirectory();
            var configFile = Path.Combine(applicationDataDirectory, LogFilterGroupsConfigFile);
            if (File.Exists(configFile))
            {
                try
                {
                    using (var stream = File.OpenRead(configFile))
                    {
                        return (LogFilterGroup[]) _xmlSerializer.Deserialize(typeof(LogFilterGroup[]), stream);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }

            return ArrayShim.Empty<LogFilterGroup>();
        }

        public void Save(IEnumerable<LogFilterGroup> filterGroups)
        {
            var applicationDataDirectory = Catel.IO.Path.GetApplicationDataDirectory();
            var configFile = Path.Combine(applicationDataDirectory, LogFilterGroupsConfigFile);
            try
            {
                using (var stream = new FileStream(configFile, FileMode.Create, FileAccess.Write))
                {
                    _xmlSerializer.Serialize(filterGroups, stream);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
