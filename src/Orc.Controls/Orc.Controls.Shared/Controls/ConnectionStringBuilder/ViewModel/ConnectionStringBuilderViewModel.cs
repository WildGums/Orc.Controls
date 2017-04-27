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
        #endregion

        #region Конструкторы
        public ConnectionStringBuilderViewModel(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);

            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;

            Clear = new Command(OnClear, CanClear);
            Edit = new Command(OnEdit);
        }
        #endregion

        #region Свойства
        public override string Title => "";
        public SqlConnectionString ConnectionString { get; set; }
        public ConnectionState ConnectionState { get; set; } = ConnectionState.NotTested;

        public Command Edit { get; }
        public Command Test { get; }
        public Command Clear { get; }
        #endregion

        #region Методы
        private void OnEdit()
        {
            var connectionStringEditViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<ConnectionStringEditViewModel>(ConnectionString);
            if (_uiVisualizerService.ShowDialog(connectionStringEditViewModel) ?? false)
            {
                ConnectionString = connectionStringEditViewModel.ConnectionString;
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
            ConnectionState = ConnectionState.NotTested;
        }
        #endregion
    }
}