namespace Orc.Controls
{
    using System;
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Services;
    using ViewModels;

    public class FindReplaceTool<TFindReplaceService> : ControlToolBase
        where TFindReplaceService : IFindReplaceService
    {
        private readonly FindReplaceSettings _findReplaceSettings;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly ITypeFactory _typeFactory;

        private IFindReplaceService _findReplaceService;
        private FindReplaceViewModel _findReplaceViewModel;

        public FindReplaceTool(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
        {
            ArgumentNullException.ThrowIfNull(uiVisualizerService);
            ArgumentNullException.ThrowIfNull(typeFactory);

            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;
            _findReplaceSettings = new FindReplaceSettings();
        }

        public override string Name => "FindReplaceTool";

        public override void Attach(object target)
        {
            ArgumentNullException.ThrowIfNull(target);

            base.Attach(target);

#pragma warning disable IDISP001 // Dispose created.
            var serviceLocator = target.GetServiceLocator();
#pragma warning restore IDISP001 // Dispose created.

            _findReplaceService = serviceLocator.ResolveType<IFindReplaceService>(target);
            if (_findReplaceService is not null)
            {
                return;
            }

            _findReplaceService = CreateFindReplaceService(target);
            serviceLocator.RegisterInstance(_findReplaceService, target);
        }

        protected virtual TFindReplaceService CreateFindReplaceService(object target)
        {
            return _typeFactory.CreateInstanceWithParametersAndAutoCompletion<TFindReplaceService>(target);
        }

        public override void Detach()
        {
            var target = Target;

#pragma warning disable IDISP001 // Dispose created.
            var serviceLocator = Target.GetServiceLocator();
#pragma warning restore IDISP001 // Dispose created.

            if (serviceLocator.IsTypeRegistered<IFindReplaceService>(target))
            {
                serviceLocator.RemoveType<IFindReplaceService>(target);
            }
        }

        protected override void OnOpen(object? parameter = null)
        {
            _findReplaceViewModel = new FindReplaceViewModel(_findReplaceSettings, _findReplaceService);

            _uiVisualizerService.ShowAsync(_findReplaceViewModel);

            _findReplaceViewModel.ClosedAsync += OnClosedAsync;
        }

        public override void Close()
        {
            base.Close();

            if (_findReplaceViewModel is null)
            {
                return;
            }

            _findReplaceViewModel.ClosedAsync -= OnClosedAsync;

#pragma warning disable 4014
            _findReplaceViewModel.CloseViewModelAsync(null);
#pragma warning restore 4014
        }

        private Task OnClosedAsync(object? sender, ViewModelClosedEventArgs args)
        {
            Close();

            return Task.CompletedTask;
        }
    }
}
