// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbConnectionProviderListViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.MVVM;

    public class DbConnectionProviderListViewModel : ViewModelBase
    {
        private readonly DbProvider _selectedProvider;

        public DbConnectionProviderListViewModel(DbProvider selectedProvider)
        {
            _selectedProvider = selectedProvider;

            Refresh = new Command(OnRefresh);
        }

        public override string Title => "Select provider";

        public DbProvider DbProvider { get; set; }
        public IList<DbProvider> DbProviders { get; private set; }
        public Command Refresh { get; }

        protected override Task InitializeAsync()
        {
            OnRefresh();

            return base.InitializeAsync();
        }

        private void OnRefresh()
        {
            DbProviders = DbProviderFactories.GetFactoryClasses().Rows.OfType<DataRow>()
                .Select(x => new DbProvider
                {
                    Name = x["Name"]?.ToString(),
                    Description = x["Description"]?.ToString(),
                    InvariantName = x["InvariantName"]?.ToString(),
                })
                .OrderBy(x => x.Name)
                .ToList();

            DbProvider = DbProviders.FirstOrDefault(x => x.Equals(_selectedProvider));
        }
    }
}