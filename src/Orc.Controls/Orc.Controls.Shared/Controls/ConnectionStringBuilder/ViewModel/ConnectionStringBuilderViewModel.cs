// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilderViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
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
        private readonly ITypeFactory _typeFactory;
        private readonly IUIVisualizerService _uiVisualizerService;
        private DbProvider _dbProvider;

        public ConnectionStringBuilderViewModel(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);

            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;

            Clear = new Command(OnClear, () => ConnectionString != null);
            Edit = new TaskCommand(OnEditAsync);
        }

        public ConnectionState ConnectionState { get; set; } = ConnectionState.Undefined;
        public string ConnectionString { get; private set; }
        public string DisplayConnectionString { get; private set; }
        public bool IsInEditMode { get; set; }

        public TaskCommand Edit { get; }
        public Command Clear { get; }

        private async Task OnEditAsync()
        {
            IsInEditMode = true;

            var connectionStringEditViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<ConnectionStringEditViewModel>(ConnectionString, _dbProvider);
            if (await _uiVisualizerService.ShowDialogAsync(connectionStringEditViewModel) ?? false)
            {
                _dbProvider = connectionStringEditViewModel.DbProvider;
                var connectionString = connectionStringEditViewModel.ConnectionString;

                ConnectionString = connectionString?.ToString();
                DisplayConnectionString = connectionString?.ToDisplayString();
                ConnectionState = connectionStringEditViewModel.ConnectionState;
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
    }
}