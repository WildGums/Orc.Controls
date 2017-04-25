// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.Sql;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Win32;

    public class ConnectionStringBuilderService : IConnectionStringBuilderService
    {
        private const string MicrosoftSqlServerRegPath = @"SOFTWARE\Microsoft\Microsoft SQL Server";

        public IList<DbProvider> GetDataProviders()
        {
            var table = DbProviderFactories.GetFactoryClasses();

            return table.Rows.OfType<DataRow>().Select(x => new DbProvider
            {
                Name = x["Name"]?.ToString(),
                Description = x["Description"]?.ToString(),
                InvariantName = x["InvariantName"]?.ToString(),
            })
            .ToList();
        }

        public IList<string> GetSqlServers()
        {
            var localServers = GetLocalSqlServerInstances();
            var remoteServers = GetRemoteSqlServerInstances();

            return localServers.Union(remoteServers)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        public IList<string> GetDatabases(string connectionString)
        {
            var databases = new List<string>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var cmd = new SqlCommand("SELECT name from sys.databases", sqlConnection))
                {
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            databases.Add(dataReader[0].ToString());
                        }
                    }
                }
            }
            return databases;
        }

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
            var servers = new List<string>(serversCount);
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
        }
    }
}