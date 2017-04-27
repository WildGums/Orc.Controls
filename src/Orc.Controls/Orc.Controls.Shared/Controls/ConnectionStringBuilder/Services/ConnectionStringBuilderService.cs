// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Data.Sql;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using Catel;
    using Microsoft.Win32;

    public class ConnectionStringBuilderService : IConnectionStringBuilderService
    {
        private const string MicrosoftSqlServerRegPath = @"SOFTWARE\Microsoft\Microsoft SQL Server";

        public ConnectionState GetConnectionState(SqlConnectionString connectionString)
        {
            var connectionStringStr = connectionString?.ToString();

            if (string.IsNullOrWhiteSpace(connectionStringStr))
            {
                return ConnectionState.BadConnection;
            }

            var factory = DbProviderFactories.GetFactory(connectionString.DbProvider.InvariantName);
            var connection = factory.CreateConnection();
            if (connection == null)
            {
                return ConnectionState.BadConnection;
            }

            // Try to open it
            try
            {
                connection.ConnectionString = connectionStringStr;
                connection.Open();
            }
            catch
            {
                return ConnectionState.BadConnection;
            }
            finally
            {
                connection.Dispose();
            }

            return ConnectionState.Tested;
        }

        public SqlConnectionString GetConnectionString(DbProvider dbProvider)
        {
            Argument.IsNotNull(() => dbProvider);

            var factory = DbProviderFactories.GetFactory(dbProvider.InvariantName);
            var connectionStringBuilder = factory.CreateConnectionStringBuilder();
            return new SqlConnectionString(connectionStringBuilder, dbProvider);
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

        public IList<string> GetDatabases(SqlConnectionString connectionString)
        {
            var databases = new List<string>();
            using (var sqlConnection = new SqlConnection(connectionString.ToString()))
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

        //public IList<string> GetDatabases(SqlConnectionString connectionString)
        //{
        //    var factory = DbProviderFactories.GetFactory(connectionString.DbProvider.InvariantName);
        //    var databases = new List<string>();
            
        //    using (var connection = factory.CreateConnection())
        //    {
        //        connection.Open();
        //        var db = connection.Database;
        //        //using (var cmd = new SqlCommand("SELECT name from sys.databases", connection))
        //        //{
        //        //    using (IDataReader dataReader = cmd.ExecuteReader())
        //        //    {
        //        //        while (dataReader.Read())
        //        //        {
        //        //            databases.Add(dataReader[0].ToString());
        //        //        }
        //        //    }
        //        //}
        //    }
        //    return databases;
        //}

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