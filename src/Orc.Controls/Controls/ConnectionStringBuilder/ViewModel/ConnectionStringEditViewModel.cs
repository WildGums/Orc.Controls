// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringEditViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
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
        #region Fields
        private static bool _isServersInitialized = false;
        private static readonly FastObservableCollection<string> CachedServers = new FastObservableCollection<string>();

        private readonly IConnectionStringBuilderService _connectionStringBuilderService;
        private readonly IMessageService _messageService;
        private readonly ITypeFactory _typeFactory;
        private readonly IUIVisualizerService _uiVisualizerService;

        private bool _isDatabasesInitialized = false;
        #endregion

        #region Constructors
        public ConnectionStringEditViewModel(string connectionString, DbProvider provider, IMessageService messageService,
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

            using (SuspendChangeNotifications())
            {
                DbProvider = provider;
            }

            ConnectionString = provider != null ? _connectionStringBuilderService.CreateConnectionString(provider, connectionString) : null;

            InitServers = new Command(() => InitServersAsync(), () => !IsServersRefreshing);
            RefreshServers = new Command(() => RefreshServersAsync(), () => !IsServersRefreshing);

            InitDatabases = new Command(() => InitDatabasesAsync(), () => !IsDatabasesRefreshing);
            RefreshDatabases = new Command(() => RefreshDatabasesAsync(), CanInitDatabases);

            TestConnection = new Command(OnTestConnection);
            ShowAdvancedOptions = new TaskCommand(OnShowAdvancedOptionsAsync, () => ConnectionString != null);
        }
        #endregion

        #region Properties
        public ConnectionStringProperty DataSource => ConnectionString.TryGetProperty("Data Source");
        public ConnectionStringProperty UserId => ConnectionString.TryGetProperty("User ID");
        public ConnectionStringProperty Password => ConnectionString.TryGetProperty("Password");
        public ConnectionStringProperty IntegratedSecurity => ConnectionString.TryGetProperty("Integrated Security");

        public bool IsAdvancedOptionsReadOnly { get; set; }

        public bool? IntegratedSecurityValue
        {
            get { return (bool?)IntegratedSecurity?.Value; }
            set
            {
                if (IntegratedSecurity == null)
                {
                    return;
                }

                if ((bool)IntegratedSecurity.Value == value)
                {
                    return;
                }

                IntegratedSecurity.Value = value;
                RaisePropertyChanged(nameof(IntegratedSecurityValue));
            }
        }

        public ConnectionStringProperty InitialCatalog => ConnectionString.TryGetProperty("Initial Catalog");
        public bool CanLogOnToServer => Password != null || UserId != null;
        public bool IsLogOnEnabled => CanLogOnToServer && !(IntegratedSecurityValue ?? false);

        public bool IsServerListVisible { get; set; } = false;
        public bool IsDatabaseListVisible { get; set; } = false;
        public bool IsServersRefreshing { get; private set; } = false;
        public bool IsDatabasesRefreshing { get; private set; } = false;
        public ConnectionState ConnectionState { get; private set; } = ConnectionState.Undefined;
        public override string Title => "Connection properties";
        public SqlConnectionString ConnectionString { get; private set; }
        public DbProvider DbProvider { get; set; }
        public Command RefreshServers { get; }
        public Command InitServers { get; }
        public Command TestConnection { get; }
        public TaskCommand ShowAdvancedOptions { get; }
        public Command RefreshDatabases { get; }
        public Command InitDatabases { get; }

        public FastObservableCollection<string> Servers => CachedServers;
        public FastObservableCollection<string> Databases { get; } = new FastObservableCollection<string>();
        #endregion

        #region Methods
        private void OnDbProviderChanged()
        {
            var dbProvider = DbProvider;
            if (dbProvider == null)
            {
                return;
            }

            ConnectionString = _connectionStringBuilderService.CreateConnectionString(dbProvider);
            SetIntegratedSecurityToDefault();
        }

        private void SetIntegratedSecurityToDefault()
        {
            var integratedSecurityProperty = IntegratedSecurity;
            if (integratedSecurityProperty == null)
            {
                return;
            }

            integratedSecurityProperty.Value = true;
            RaisePropertyChanged(nameof(IntegratedSecurity));
        }

        private async Task OnShowAdvancedOptionsAsync()
        {
            var connectionString = ConnectionString;
            if (connectionString == null)
            {
                return;
            }

            var advancedOptionsViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<ConnectionStringAdvancedOptionsViewModel>(connectionString);
            advancedOptionsViewModel.IsAdvancedOptionsReadOnly = IsAdvancedOptionsReadOnly;

            await _uiVisualizerService.ShowDialogAsync(advancedOptionsViewModel);
        }

        private void OnTestConnection()
        {
            ConnectionState = _connectionStringBuilderService.GetConnectionState(ConnectionString);

            _messageService.ShowAsync($"{ConnectionState} connection!", "Connection test result");
        }

        private void OnDataSourceChanged()
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
                var servers = _connectionStringBuilderService.GetDataSources(ConnectionString);
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
                var connectionState = _connectionStringBuilderService.GetConnectionState(ConnectionString);
                if (connectionState != ConnectionState.Invalid)
                {
                    var databases = _connectionStringBuilderService.GetDatabases(connectionString);
                    Databases.AddItems(databases);
                }
                else
                {
                    ConnectionState = connectionState;
                }

                IsDatabasesRefreshing = false;
                _isDatabasesInitialized = true;
                IsDatabaseListVisible = true;
            });
        }
        #endregion
    }
}
