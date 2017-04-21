// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionString.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel.Data;

    public class SqlConnectionString : ModelBase
    {
        private readonly ConnectionStringBuilder _connectionStringBuilder;

        public SqlConnectionString()
        {
            _connectionStringBuilder = new ConnectionStringBuilder();
        }
    }
}