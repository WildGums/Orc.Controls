// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnectionStringBuilderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;

    public interface IConnectionStringBuilderService
    {
        #region Methods
        void AddDataSourceProvider(string invariantName, IDataSourceProvider provider);
        SqlConnectionString CreateConnectionString(DbProvider dbProvider, string connectionString = "");
        ConnectionState GetConnectionState(SqlConnectionString connectionString);
        IList<string> GetDataSources(SqlConnectionString connectionString);
        IList<string> GetDatabases(SqlConnectionString connectionString);
        #endregion
    }
}
