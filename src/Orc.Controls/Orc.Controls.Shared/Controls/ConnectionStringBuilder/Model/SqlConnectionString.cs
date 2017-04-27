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

        //public bool HasUserId { get; set; }
        //public bool HasPassword { get; set; }
        //public bool HasDataSource { get; set; }
        //public bool HasDatabase { get; set; }
        //public bool HasDatabaseFile { get; set; }
        //public string UserId { get; set; }
        //public string Password { get; set; }
        //public string DataSource { get; set; }
        //public string Database { get; set; }
        //public string DatabaseFile { get; set; }


        public override string ToString()
        {
            return _connectionStringBuilder.ConnectionString;
        }
    }
}