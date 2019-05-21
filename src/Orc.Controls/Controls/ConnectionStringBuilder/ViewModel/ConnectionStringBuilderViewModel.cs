// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilderViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    public class ConnectionStringBuilderViewModel : ViewModelBase
    {
        #region Fields
        private readonly ITypeFactory _typeFactory;
        private readonly IUIVisualizerService _uiVisualizerService;
        private DbProvider _dbProvider;
        #endregion

        #region Constructors
        public ConnectionStringBuilderViewModel(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);

            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;

            Clear = new Command(OnClear, () => ConnectionString != null);
            Edit = new TaskCommand(OnEditAsync);
        }
        #endregion

        #region Properties
        public ConnectionState ConnectionState { get; set; } = ConnectionState.Undefined;
        public string ConnectionString { get; private set; }
        public string DisplayConnectionString { get; private set; }
        public string DatabaseProvider { get; private set; }
        public bool IsInEditMode { get; set; }
        public bool IsAdvancedOptionsReadOnly { get; set; }

        public TaskCommand Edit { get; }
        public Command Clear { get; }
        #endregion

        #region Methods
        private async Task OnEditAsync()
        {
            IsInEditMode = true;

            var connectionStringEditViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<ConnectionStringEditViewModel>(ConnectionString, _dbProvider);
            connectionStringEditViewModel.IsAdvancedOptionsReadOnly = IsAdvancedOptionsReadOnly;

            if (await _uiVisualizerService.ShowDialogAsync(connectionStringEditViewModel) ?? false)
            {
                _dbProvider = connectionStringEditViewModel.DbProvider;
                var connectionString = connectionStringEditViewModel.ConnectionString;

                ConnectionString = connectionString?.ToString();
                DisplayConnectionString = connectionString?.ToDisplayString();
                ConnectionState = connectionStringEditViewModel.ConnectionState;
                DatabaseProvider = connectionString?.DbProvider.InvariantName;
            }

            IsInEditMode = false;
        }

        private void OnClear()
        {
            ConnectionString = null;
            DisplayConnectionString = null;
            ConnectionState = ConnectionState.Undefined;
            _dbProvider = null;
        }
        #endregion
    }
}
