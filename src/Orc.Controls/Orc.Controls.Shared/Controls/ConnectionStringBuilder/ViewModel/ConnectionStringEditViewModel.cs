// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringEditViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;

    public class ConnectionStringEditViewModel : ViewModelBase
    {
        private readonly IConnectionStringBuilderService _connectionStringBuilderService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly ITypeFactory _typeFactory;

        private bool _isServersInitialized = false;
        private bool _isDatabasesInitialized = false;

        public ConnectionStringEditViewModel(string connectionString,
            IConnectionStringBuilderService connectionStringBuilderService, IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => connectionStringBuilderService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);

            _connectionStringBuilderService = connectionStringBuilderService;
            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;

            var providers = connectionStringBuilderService.GetDataProviders();

            ConnectionStringBuiler = new SqlConnectionStringBuilder(connectionString);

            InitServers = new Command(() => InitServersAsync(), () => !IsServersRefreshing);
            RefreshServers = new Command(() => RefreshServersAsync(), () => !IsServersRefreshing);
            
            InitDatabases = new Command(() => InitDatabasesAsync(), () => !IsDatabasesRefreshing);
            RefreshDatabases = new Command(() => RefreshDatabasesAsync(), CanInitDatabases);

            TestConnection = new Command(OnTestConnection);
            ShowAdvancedOptions = new Command(OnShowAdvancedOptions);
        }

        public bool IsServerListVisible { get; set; } = false;
        public bool IsDatabaseListVisible { get; set; } = false;

        public override string Title => "Connection properties";
        public SqlConnectionStringBuilder ConnectionStringBuiler { get; }

        public DbProvider DbProvider { get; set; }

        private void OnDbProviderChanged()
        {
            
        }

        public Command RefreshServers { get; }
        public Command InitServers { get; }
        public bool IsServersRefreshing { get; private set; } = false;

        public Command TestConnection { get; }
        public Command ShowAdvancedOptions { get; }
        
        private void OnShowAdvancedOptions()
        {
            var advancedOptionsViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<ConnectionStringAdvancedOptionsViewModel>();

            _uiVisualizerService.ShowDialog(advancedOptionsViewModel);
        }

        private void OnTestConnection()
        {
            
        }

        private void OnSelectedServerChaged()
        {
            _isDatabasesInitialized = false;
            InitDatabases.RaiseCanExecuteChanged();
        }

        public bool CanInitDatabases()
        {
            return !IsDatabasesRefreshing;
        }

        public Command RefreshDatabases { get; }
        public Command InitDatabases { get; }
        public bool IsDatabasesRefreshing { get; private set; } = false;

        public FastObservableCollection<string> Servers { get; } = new FastObservableCollection<string>();
        public FastObservableCollection<string> Databases { get; } = new FastObservableCollection<string>();

        public string ConnectionString { get; private set; }
        
        public string DataSource { get; set; }
        public bool UseSqlServerAuthentication { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

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
                IsServerListVisible = true;
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

            var connectionString = ConnectionStringBuiler.ToString();

            return TaskHelper.RunAndWaitAsync(() =>
            {
                var databases = _connectionStringBuilderService.GetDatabases(connectionString);
                Databases.AddItems(databases);

                IsDatabasesRefreshing = false;
                _isDatabasesInitialized = true;
                IsDatabaseListVisible = true;
            });
        }

        protected override Task<bool> SaveAsync()
        {
            ConnectionString = ConnectionStringBuiler.ToString();

            return base.SaveAsync();
        }
    }
}