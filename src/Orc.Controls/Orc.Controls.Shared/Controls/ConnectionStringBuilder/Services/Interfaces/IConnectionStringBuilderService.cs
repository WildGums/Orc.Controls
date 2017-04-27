// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnectionStringBuilderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Data.Common;

    public interface IConnectionStringBuilderService
    {
        ConnectionState GetConnectionState(SqlConnectionString connectionString);
        SqlConnectionString CreateConnectionString(DbProvider dbProvider, string connectionString = "");
        IList<string> GetSqlServers();
        IList<string> GetDatabases(SqlConnectionString connectionString);
    }
}