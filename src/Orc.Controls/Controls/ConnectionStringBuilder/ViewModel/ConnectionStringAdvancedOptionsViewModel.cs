// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringAdvancedOptionsViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;

    [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use ConnectionStringAdvancedOptionsViewModel from Orc.DataAccess.Xaml library instead")]
    public class ConnectionStringAdvancedOptionsViewModel : ViewModelBase
    {
        #region Constructors
        public ConnectionStringAdvancedOptionsViewModel(SqlConnectionString connectionString)
        {
            Argument.IsNotNull(() => connectionString);

            ConnectionString = connectionString;
        }
        #endregion

        #region Properties
        public override string Title => "Advanced options";
        public IList<ConnectionStringProperty> ConnectionStringProperties { get; private set; }
        public bool IsAdvancedOptionsReadOnly { get; set; }
        public SqlConnectionString ConnectionString { get; }
        #endregion

        #region Methods
        protected override Task InitializeAsync()
        {
            ConnectionStringProperties = ConnectionString.Properties.Values.Where(x => !x.IsSensitive)
                .OrderBy(x => x.Name)
                .ToList();

            return base.InitializeAsync();
        }
        #endregion
    }
}
