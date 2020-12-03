// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DialogWindowHostedToolBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;

    public abstract class DialogWindowHostedToolBase<T> : ControlToolBase
        where T : ViewModelBase
    {
        #region Fields
        private readonly IUIVisualizerService _uiVisualizerService;
        protected readonly ITypeFactory _typeFactory;
        protected object _parameter;

        protected T _windowViewModel;
        #endregion

        #region Constructors
        protected DialogWindowHostedToolBase(ITypeFactory typeFactory, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => uiVisualizerService);

            _typeFactory = typeFactory;
            _uiVisualizerService = uiVisualizerService;
        }
        #endregion

        #region Properties
        public virtual bool IsModal => true;
        #endregion

        #region Methods
        protected override void OnOpen(object parameter = null)
        {
            _parameter = parameter;

            _windowViewModel = InitializeViewModel();
            _windowViewModel.ClosedAsync += OnClosedAsync;
            ApplyParameter(parameter);

            if (IsModal)
            {
                _uiVisualizerService.ShowDialogAsync(_windowViewModel, OnWindowCompleted);
            }
            else
            {
                _uiVisualizerService.ShowAsync(_windowViewModel, OnWindowCompleted);
            }
        }

        private void OnWindowCompleted(object sender, UICompletedEventArgs args)
        {
            if (!(args.Result ?? false))
            {
                return;
            }

            OnAccepted();
        }

        protected abstract void OnAccepted();
        protected abstract T InitializeViewModel();

        protected virtual void ApplyParameter(object parameter)
        {
        }

        public override void Close()
        {
            base.Close();

            if (_windowViewModel == null)
            {
                return;
            }

            _windowViewModel.ClosedAsync -= OnClosedAsync;

#pragma warning disable 4014
            _windowViewModel.CloseViewModelAsync(null);
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
