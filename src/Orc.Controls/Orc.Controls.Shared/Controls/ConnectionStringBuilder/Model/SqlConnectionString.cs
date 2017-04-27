// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionString.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using Catel;
    using Catel.Data;

    public class SqlConnectionString : ModelBase
    {
        private readonly DbConnectionStringBuilder _connectionStringBuilder;

        public SqlConnectionString(DbConnectionStringBuilder connectionStringBuilder, DbProvider dbProvider)
        {
            Argument.IsNotNull(() => connectionStringBuilder);
            Argument.IsNotNull(() => dbProvider);

            _connectionStringBuilder = connectionStringBuilder;
            DbProvider = dbProvider;

            Properties = connectionStringBuilder.Keys.OfType<string>()
                .ToDictionary(x => x, x => new ConnectionStringProperty(x, connectionStringBuilder));
        }

        public Dictionary<string, ConnectionStringProperty> Properties { get; }
        public DbProvider DbProvider { get; }

        public override string ToString()
        {
            return _connectionStringBuilder.ConnectionString;
        }
    }
}