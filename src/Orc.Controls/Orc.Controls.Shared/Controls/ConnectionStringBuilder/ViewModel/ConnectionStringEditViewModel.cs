// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringEditViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
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
        private readonly IMessageService _messageService;
        private readonly ITypeFactory _typeFactory;
        private readonly IUIVisualizerService _uiVisualizerService;

        private bool _isDatabasesInitialized = false;
        private bool _isServersInitialized = false;

        public ConnectionStringEditViewModel(SqlConnectionString connectionString, IMessageService messageService,
            IConnectionStringBuilderService connectionStringBuilderService, IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => connectionStringBuilderService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => messageService);

            _messageService = messageService;
            _connectionStringBuilderService = connectionStringBuilderService;
            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;

            ConnectionString = connectionString;
            DbProvider = connectionString?.DbProvider;

            InitServers = new Command(() => InitServersAsync(), () => !IsServersRefreshing);
            RefreshServers = new Command(() => RefreshServersAsync(), () => !IsServersRefreshing);

            InitDatabases = new Command(() => InitDatabasesAsync(), () => !IsDatabasesRefreshing);
            RefreshDatabases = new Command(() => RefreshDatabasesAsync(), CanInitDatabases);

            TestConnection = new Command(OnTestConnection);
            ShowAdvancedOptions = new Command(OnShowAdvancedOptions);
        }

        public ConnectionStringProperty DataSource => ConnectionString?.Properties["Data Source"];
        public ConnectionStringProperty UserId => ConnectionString?.Properties["User ID"];
        public ConnectionStringProperty Password => ConnectionString?.Properties["Password"];
        public ConnectionStringProperty IntegratedSecurity => ConnectionString?.Properties["Integrated Security"];
        public ConnectionStringProperty InitialCatalog => ConnectionString?.Properties["Initial Catalog"];
        public bool CanLogOnToServer => Password != null || UserId != null;

        public bool IsServerListVisible { get; set; } = false;
        public bool IsDatabaseListVisible { get; set; } = false;
        public bool IsServersRefreshing { get; private set; } = false;
        public bool IsDatabasesRefreshing { get; private set; } = false;
        public ConnectionState ConnectionState { get; private set; } = ConnectionState.NotTested;
        public override string Title => "Connection properties";
        public SqlConnectionString ConnectionString { get; private set; }
        public DbProvider DbProvider { get; set; }
        public Command RefreshServers { get; }
        public Command InitServers { get; }
        public Command TestConnection { get; }
        public Command ShowAdvancedOptions { get; }
        public Command RefreshDatabases { get; }
        public Command InitDatabases { get; }

        public FastObservableCollection<string> Servers { get; } = new FastObservableCollection<string>();
        public FastObservableCollection<string> Databases { get; } = new FastObservableCollection<string>();

        private void OnDbProviderChanged()
        {
            var dbProvider = DbProvider;
            if (dbProvider == null)
            {
                return;
            }

            ConnectionString = _connectionStringBuilderService.GetConnectionString(dbProvider);
        }

        private void OnShowAdvancedOptions()
        {
            var advancedOptionsViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<ConnectionStringAdvancedOptionsViewModel>(ConnectionString);

            _uiVisualizerService.ShowDialog(advancedOptionsViewModel);
        }

        private void OnTestConnection()
        {
            ConnectionState = _connectionStringBuilderService.GetConnectionState(ConnectionString);

            _messageService.ShowAsync(ConnectionState.ToString());
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
            return _isDatabasesInitialized ? TaskHelper.Completed : RefreshDatabasesAsync();
        }

        private Task RefreshDatabasesAsync()
        {
            var connectionString = ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString?.ToString()))
            {

                return TaskHelper.Completed;
            }

            IsDatabasesRefreshing = true;

            Databases.Clear();

            return TaskHelper.RunAndWaitAsync(() =>
            {
                var databases = _connectionStringBuilderService.GetDatabases(connectionString);
                Databases.AddItems(databases);

                IsDatabasesRefreshing = false;
                _isDatabasesInitialized = true;
                IsDatabaseListVisible = true;
            });
        }
    }
}