// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilderViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    public class ConnectionStringBuilderViewModel : ViewModelBase
    {
        #region Поля
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly ITypeFactory _typeFactory;
        private readonly IConnectionStringBuilderService _connectionStringBuilderService;
        private DbProvider _dbProvider;
        #endregion

        #region Конструкторы
        public ConnectionStringBuilderViewModel(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory, 
            IConnectionStringBuilderService connectionStringBuilderService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => connectionStringBuilderService);

            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;
            _connectionStringBuilderService = connectionStringBuilderService;

            Clear = new Command(OnClear, CanClear);
            Edit = new Command(OnEdit);
        }
        #endregion

        #region Свойства
        public override string Title => "";
        public ConnectionState ConnectionState { get; set; } = ConnectionState.NotTested;
        public string ConnectionString { get; private set; }
        public string DisplayConnectionString { get; private set; }

        public Command Edit { get; }
        public Command Test { get; }
        public Command Clear { get; }
        #endregion

        #region Методы
        private void OnEdit()
        {
            var connectionStringEditViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<ConnectionStringEditViewModel>(ConnectionString, _dbProvider);
            if (_uiVisualizerService.ShowDialog(connectionStringEditViewModel) ?? false)
            {
                _dbProvider = connectionStringEditViewModel.DbProvider;
                var connectionString = connectionStringEditViewModel.ConnectionString;
                
                ConnectionString = connectionString?.ToString();
                DisplayConnectionString = connectionString?.ToDisplayString();
                ConnectionState = connectionStringEditViewModel.ConnectionState;
            }
        }

        private bool CanClear()
        {
            return ConnectionString != null;
        }

        private void OnClear()
        {
            ConnectionString = null;
            DisplayConnectionString = null;
            ConnectionState = ConnectionState.NotTested;
        }
        #endregion
    }
}