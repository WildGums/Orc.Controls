// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringEditViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Threading.Tasks;
    using System.Timers;
    using Catel;
    using Catel.Collections;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;
    using Timer = System.Timers.Timer;

    public class ConnectionStringEditViewModel : ViewModelBase
    {
        #region Constants
        private static bool _isServersInitialized = false;
        private static readonly FastObservableCollection<string> CachedServers = new FastObservableCollection<string>();
        #endregion

        #region Fields
        private readonly IConnectionStringBuilderService _connectionStringBuilderService;
        private readonly IDispatcherService _dispatcherService;

        private readonly DbProvider _initalDbProvider;
        private readonly string _initialConnectionString;
        private readonly IMessageService _messageService;
        private readonly ITypeFactory _typeFactory;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly Timer _initializeTimer = new Timer(200);

        private bool _isDatabasesInitialized = false;
        #endregion

        #region Constructors
        public ConnectionStringEditViewModel(string connectionString, DbProvider provider, IMessageService messageService,
            IConnectionStringBuilderService connectionStringBuilderService, IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory, IDispatcherService dispatcherService)
        {
            Argument.IsNotNull(() => connectionStringBuilderService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => dispatcherService);

            _messageService = messageService;
            _connectionStringBuilderService = connectionStringBuilderService;
            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;
            _dispatcherService = dispatcherService;

            _initalDbProvider = provider;
            _initialConnectionString = connectionString;

            InitServers = new Command(() => InitServersAsync(), () => !IsServersRefreshing);
            RefreshServers = new Command(() => RefreshServersAsync(), () => !IsServersRefreshing);

            InitDatabases = new Command(() => InitDatabasesAsync(), () => !IsDatabasesRefreshing);
            RefreshDatabases = new Command(() => RefreshDatabasesAsync(), CanInitDatabases);

            TestConnection = new Command(OnTestConnection);
            ShowAdvancedOptions = new TaskCommand(OnShowAdvancedOptionsAsync, () => ConnectionString != null);

            _initializeTimer.Elapsed += OnInitializeTimerElapsed;
        }
        #endregion

        #region Properties
        public ConnectionStringProperty DataSource => ConnectionString.TryGetProperty("Data Source")
                                                      ?? ConnectionString.TryGetProperty("Server")
                                                      ?? ConnectionString.TryGetProperty("Host");
        public ConnectionStringProperty UserId => ConnectionString.TryGetProperty("User ID")
                                                  ?? ConnectionString.TryGetProperty("User name");
        public ConnectionStringProperty Password => ConnectionString.TryGetProperty("Password");

        public ConnectionStringProperty Port => ConnectionString.TryGetProperty("Port");
        public ConnectionStringProperty IntegratedSecurity => ConnectionString.TryGetProperty("Integrated Security");

        public ConnectionStringProperty InitialCatalog => ConnectionString.TryGetProperty("Initial Catalog") 
                                                          ?? ConnectionString.TryGetProperty("Database") ;

        public bool IsAdvancedOptionsReadOnly { get; set; }

        public bool? IntegratedSecurityValue
        {
            get => IntegratedSecurity?.Value as bool?;
            set
            {
                if (IntegratedSecurity == null)
                {
                    return;
                }

                if (Equals(IntegratedSecurity.Value, value))
                {
                    return;
                }

                IntegratedSecurity.Value = value;
                RaisePropertyChanged(nameof(IntegratedSecurityValue));
            }
        }
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
        private void OnInitializeTimerElapsed(object sender, ElapsedEventArgs args)
        {
            _dispatcherService.Invoke(SetInitialState);
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _initializeTimer.Start();
        }

        private void SetInitialState()
        {
            _initializeTimer.Stop();

            using (SuspendChangeNotifications())
            {
                DbProvider = _initalDbProvider;
            }
            
            ConnectionString = _initalDbProvider != null ? _connectionStringBuilderService.CreateConnectionString(_initalDbProvider, _initialConnectionString) : null;
        }

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
