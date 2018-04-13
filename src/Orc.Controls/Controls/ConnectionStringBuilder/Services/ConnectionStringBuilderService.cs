// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Catel;

    public class ConnectionStringBuilderService : IConnectionStringBuilderService
    {
        private readonly Dictionary<string, IDataSourceProvider> _providers = new Dictionary<string, IDataSourceProvider>();

        public ConnectionStringBuilderService(IConnectionStringBuilderServiceInitializer connectionStringBuilderServiceInitializer)
        {
            Argument.IsNotNull(() => connectionStringBuilderServiceInitializer);

            connectionStringBuilderServiceInitializer.Initialize(this);
        }

        public ConnectionState GetConnectionState(SqlConnectionString connectionString)
        {
            var connectionStringStr = connectionString?.ToString();

            if (string.IsNullOrWhiteSpace(connectionStringStr))
            {
                return ConnectionState.Invalid;
            }

            var factory = DbProviderFactories.GetFactory(connectionString.DbProvider.InvariantName);
            var connection = factory.CreateConnection();
            if (connection == null)
            {
                return ConnectionState.Invalid;
            }

            // Try to open
            try
            {
                connection.ConnectionString = connectionStringStr;
                connection.Open();
            }
            catch
            {
                return ConnectionState.Invalid;
            }
            finally
            {
                connection.Dispose();
            }

            return ConnectionState.Valid;
        }

        public SqlConnectionString CreateConnectionString(DbProvider dbProvider, string connectionString = "")
        {
            Argument.IsNotNull(() => dbProvider);

            var factory = DbProviderFactories.GetFactory(dbProvider.InvariantName);
            var connectionStringBuilder = factory.CreateConnectionStringBuilder();
            if (connectionStringBuilder == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                connectionStringBuilder.ConnectionString = connectionString;
            }

            return new SqlConnectionString(connectionStringBuilder, dbProvider);
        }

        public IList<string> GetDatabases(SqlConnectionString connectionString)
        {
            var dbProvider = connectionString.DbProvider?.InvariantName;
            if (string.IsNullOrWhiteSpace(dbProvider))
            {
                return new List<string>();
            }

            IDataSourceProvider provider = null;
            _providers.TryGetValue(dbProvider, out provider);
            if (provider == null)
            {
                return new List<string>();
            }

            var factory = DbProviderFactories.GetFactory(dbProvider);

            var databases = new List<string>();
            using (var sqlConnection = factory.CreateConnection())
            {
                if (sqlConnection == null)
                {
                    return new List<string>();
                }

                sqlConnection.ConnectionString = connectionString.ToString();
                sqlConnection.Open();

                using (var command = factory.CreateCommand())
                {
                    if (command == null)
                    {
                        return new List<string>();
                    }

                    command.Connection = sqlConnection;
                    command.CommandText = provider.DataBasesQuery;
                    command.CommandType = CommandType.Text;
                    using (IDataReader dataReader = command.ExecuteReader())
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

        public void AddDataSourceProvider(string invariantName, IDataSourceProvider provider)
        {
            if (_providers.ContainsKey(invariantName))
            {
                return;
            }

            _providers.Add(invariantName, provider);
        }

        public IList<string> GetDataSources(SqlConnectionString connectionString)
        {
            var dbProvider = connectionString.DbProvider?.InvariantName;
            if (string.IsNullOrWhiteSpace(dbProvider))
            {
                return new List<string>();
            }

            IDataSourceProvider provider = null;
            _providers.TryGetValue(dbProvider, out provider);
            return provider != null ? provider.GetDataSources() : new List<string>();
        }
    }
}