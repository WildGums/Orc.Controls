// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilderServiceInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel;
    using Catel.IoC;

    public class ConnectionStringBuilderServiceInitializer : IConnectionStringBuilderServiceInitializer
    {
        #region Fields
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public ConnectionStringBuilderServiceInitializer(ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => typeFactory);

            _typeFactory = typeFactory;
        }
        #endregion

        #region IConnectionStringBuilderServiceInitializer Members
        public void Initialize(IConnectionStringBuilderService connectionStringBuilderService)
        {
            Argument.IsNotNull(() => connectionStringBuilderService);

            var msSqlDataProvider = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<MsSqlDataSourceProvider>();
            connectionStringBuilderService.AddDataSourceProvider("System.Data.SqlClient", msSqlDataProvider);
        }
        #endregion
    }
}
