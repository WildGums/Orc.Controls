// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringAdvancedOptionsViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;

    public class ConnectionStringAdvancedOptionsViewModel : ViewModelBase
    {
        public ConnectionStringAdvancedOptionsViewModel(SqlConnectionString connectionString)
        {
            Argument.IsNotNull(() => connectionString);

            ConnectionString = connectionString;
        }

        public override string Title => "Advanced options";

        public IList<ConnectionStringProperty> ConnectionStringProperties { get; private set; }

        public bool IsAdvancedOptionsReadOnly { get; set; }
        public SqlConnectionString ConnectionString { get; }

        protected override Task InitializeAsync()
        {
            ConnectionStringProperties = ConnectionString.Properties.Values.Where(x => !x.IsSensitive)
                .OrderBy(x => x.Name)
                .ToList();

            return base.InitializeAsync();
        }
    }
}
