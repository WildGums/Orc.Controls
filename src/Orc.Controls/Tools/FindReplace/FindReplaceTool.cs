// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindReplaceTool.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Tools.FindReplace
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;
    using Services;
    using ViewModels;

    public abstract class FindReplaceTool : ControlToolBase
    {
        #region Fields
        private readonly FindReplaceSettings _findReplaceSettings;
        private readonly ITypeFactory _typeFactory;
        private readonly IUIVisualizerService _uiVisualizerService;

        private IFindReplaceService _findReplaceService;
        private FindReplaceViewModel _findReplaceViewModel;
        #endregion

        #region Constructors
        public FindReplaceTool(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);

            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;
            _findReplaceSettings = new FindReplaceSettings();
        }
        #endregion

        #region Properties
        public override string Name => "FindReplaceTool";
        #endregion

        #region Methods
        public override void Attach(object target)
        {
            Argument.IsNotNull(() => target);

            base.Attach(target);

            var serviceLocator = target.GetServiceLocator();
            _findReplaceService = serviceLocator.TryResolveType<IFindReplaceService>(target);
            if (_findReplaceService != null)
            {
                return;
            }

            _findReplaceService = CreateFindReplaceService(target);
            //_typeFactory.CreateInstanceWithParametersAndAutoCompletion<FindReplaceService>(target);
            serviceLocator.RegisterInstance(_findReplaceService, target);
        }

        protected abstract IFindReplaceService CreateFindReplaceService(object target);

        public override void Detach()
        {
            var target = Target;

            var serviceLocator = Target.GetServiceLocator();
            if (serviceLocator.IsTypeRegistered<IFindReplaceService>(target))
            {
                serviceLocator.RemoveType<IFindReplaceService>(target);
            }
        }

        protected override void OnOpen(object parameter = null)
        {
            _findReplaceViewModel = new FindReplaceViewModel(_findReplaceSettings, _findReplaceService);

            _uiVisualizerService.ShowAsync(_findReplaceViewModel);

            _findReplaceViewModel.ClosedAsync += OnClosedAsync;
        }

        public override void Close()
        {
            base.Close();

            if (_findReplaceViewModel == null)
            {
                return;
            }

            _findReplaceViewModel.ClosedAsync -= OnClosedAsync;

#pragma warning disable 4014
            _findReplaceViewModel.CloseViewModelAsync(null);
#pragma warning restore 4014
        }

        private Task OnClosedAsync(object sender, ViewModelClosedEventArgs args)
        {
            Close();

            return TaskHelper.Completed;
        }
        #endregion
    }
}
