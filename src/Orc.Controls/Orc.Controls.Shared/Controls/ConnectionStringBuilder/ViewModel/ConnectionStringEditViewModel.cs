// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringEditViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.Threading;

    public class ConnectionStringEditViewModel : ViewModelBase
    {
        private readonly IConnectionStringBuilderService _connectionStringBuilderService;

        private bool _isServersInitialized = false;
        private bool _isDatabasesInitialized = false;

        public ConnectionStringEditViewModel(string connectionString,
            IConnectionStringBuilderService connectionStringBuilderService)
        {
            Argument.IsNotNull(() => connectionStringBuilderService);

            _connectionStringBuilderService = connectionStringBuilderService;

            ConnectionStringBuiler = new SqlConnectionStringBuilder(connectionString);

            InitServers = new Command(() => InitServersAsync(), () => !IsServersRefreshing);
            RefreshServers = new Command(() => RefreshServersAsync(), () => !IsServersRefreshing);

            InitDatabases = new Command(() => InitDatabasesAsync(), () => !IsDatabasesRefreshing);
            RefreshDatabases = new Command(() => RefreshDatabasesAsync(), CanInitDatabases);
        }
        public Command RefreshServers { get; }
        public Command InitServers { get; }
        public bool IsServersRefreshing { get; private set; } = false;

        public string SelectedServer { get; set; }

        private void OnSelectedServerChaged()
        {
            _isDatabasesInitialized = false;
            InitDatabases.RaiseCanExecuteChanged();
        }

        public bool CanInitDatabases()
        {
            return SelectedServer != null && !IsDatabasesRefreshing;
        }

        public Command RefreshDatabases { get; }
        public Command InitDatabases { get; }
        public bool IsDatabasesRefreshing { get; private set; } = false;

        public override string Title => "Connection properties";
        public SqlConnectionStringBuilder ConnectionStringBuiler { get; }
        public string ConnectionString { get; private set; }
        
        public string DataSource { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseSqlServerAuthentication { get; set; }

        public FastObservableCollection<string> Servers { get; } = new FastObservableCollection<string>();
        public FastObservableCollection<string> Databases { get; } = new FastObservableCollection<string>();

        private Task InitServersAsync()
        {
            if (_isServersInitialized)
            {
                return TaskHelper.Completed;
            }

            return RefreshServersAsync();
        }

        private Task RefreshServersAsync()
        {
            IsServersRefreshing = true;

            Servers.Clear();

            return TaskHelper.RunAndWaitAsync(() =>
            {
                var servers = _connectionStringBuilderService.GetSqlServers();
                Servers.AddItems(servers);

                IsServersRefreshing = false;
                _isServersInitialized = true;
            });
        }

        private Task InitDatabasesAsync()
        {
            if (_isDatabasesInitialized)
            {
                return TaskHelper.Completed;
            }

            return RefreshDatabasesAsync();
        }

        private Task RefreshDatabasesAsync()
        {
            IsDatabasesRefreshing = true;

            Databases.Clear();

            if (UseSqlServerAuthentication)
            {
                ConnectionStringBuiler.Password = Password;
                ConnectionStringBuiler.UserID = UserName;
            }

            ConnectionStringBuiler.IntegratedSecurity = !UseSqlServerAuthentication;
            ConnectionStringBuiler.DataSource = SelectedServer;

            var connectionString = ConnectionStringBuiler.ToString();

            return TaskHelper.RunAndWaitAsync(() =>
            {
                var databases = _connectionStringBuilderService.GetDatabases(connectionString);
                Databases.AddItems(databases);

                IsDatabasesRefreshing = false;
                _isDatabasesInitialized = true;
            });
        }

        protected override Task<bool> SaveAsync()
        {
            ConnectionString = ConnectionStringBuiler.ToString();

            return base.SaveAsync();
        }
    }
}