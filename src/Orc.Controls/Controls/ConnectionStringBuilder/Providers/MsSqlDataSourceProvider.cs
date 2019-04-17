// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MsSqlDataSourceProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Sql;
    using System.Globalization;
    using System.Linq;
    using Catel.Logging;
    using Microsoft.Win32;

    public class MsSqlDataSourceProvider : IDataSourceProvider
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string MicrosoftSqlServerRegPath = @"SOFTWARE\Microsoft\Microsoft SQL Server";
        #endregion

        #region Properties
        public string DataBasesQuery => "SELECT name from sys.databases";
        #endregion

        #region IDataSourceProvider Members
        public IList<string> GetDataSources()
        {
            var localServers = GetLocalSqlServerInstances();
            var remoteServers = GetRemoteSqlServerInstances();

            return localServers.Union(remoteServers)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }
        #endregion

        #region Methods
        private IEnumerable<string> GetLocalSqlServerInstances()
        {
            var machineName = Environment.MachineName;

            var localSqlInstances32 = GetInstalledInstancesInRegistryView(RegistryView.Registry32);
            var localSqlInstances64 = GetInstalledInstancesInRegistryView(RegistryView.Registry64);

            return localSqlInstances32.Union(localSqlInstances64)
                .Select(x => $"{machineName}\\{x}");
        }

        private IList<string> GetInstalledInstancesInRegistryView(RegistryView registryView)
        {
            var regView = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
            using (var sqlServNode = regView.OpenSubKey(MicrosoftSqlServerRegPath, false))
            {
                return sqlServNode?.GetValue("InstalledInstances") as IList<string> ?? new List<string>();
            }
        }

        private IList<string> GetRemoteSqlServerInstances()
        {
#if NETCORE
            throw Log.ErrorAndCreateException<NotSupportedException>($"Not supported on .NET Core, SqlDataSourceEnumerator is not (yet) available. See https://github.com/dotnet/corefx/issues/32874");
#else
            DataTable dataTable;

            try
            {
                dataTable = SqlDataSourceEnumerator.Instance.GetDataSources();
            }
            catch
            {
                dataTable = new DataTable {Locale = CultureInfo.InvariantCulture};
            }

            var serversCount = dataTable.Rows.Count;
            var servers = new string[serversCount];

            for (var i = 0; i < serversCount; i++)
            {
                var name = dataTable.Rows[i]["ServerName"].ToString();
                var instance = dataTable.Rows[i]["InstanceName"].ToString();

                servers[i] = name;
                if (instance.Any())
                {
                    servers[i] += "\\" + instance;
                }
            }

            return servers;
#endif
        }
        #endregion
    }
}
