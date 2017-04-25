// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbProviderControlViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    public class DbProviderControlViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly ITypeFactory _typeFactory;

        public DbProviderControlViewModel(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);

            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;

            ChangeDbProvider = new Command(OnChangeDbProvider);
        }

        public DbProvider DbProvider { get; set; }
        public Command ChangeDbProvider { get; }

        private void OnChangeDbProvider()
        {
            var dbProviderListViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<DbConnectionProviderListViewModel>(DbProvider);
            if (_uiVisualizerService.ShowDialog(dbProviderListViewModel) ?? false)
            {
                DbProvider = dbProviderListViewModel.DbProvider;
            }
        }
    }
}