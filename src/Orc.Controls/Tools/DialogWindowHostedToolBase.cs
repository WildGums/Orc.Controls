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
        protected readonly ITypeFactory TypeFactory;
        protected object Parameter;

        protected T WindowViewModel;
        #endregion

        #region Constructors
        protected DialogWindowHostedToolBase(ITypeFactory typeFactory, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => uiVisualizerService);

            TypeFactory = typeFactory;
            _uiVisualizerService = uiVisualizerService;
        }
        #endregion

        #region Methods
        protected override void OnOpen(object parameter = null)
        {
            Parameter = parameter;

            WindowViewModel = InitializeViewModel();
            WindowViewModel.ClosedAsync += OnClosedAsync;

            _uiVisualizerService.ShowDialogAsync(WindowViewModel, (sender, args) =>
            {
                if (!(args.Result ?? false))
                {
                    return;
                }

                OnAccepted();
            });
        }

        protected abstract void OnAccepted();
        protected abstract T InitializeViewModel();

        public override void Close()
        {
            base.Close();

            if (WindowViewModel == null)
            {
                return;
            }

            WindowViewModel.ClosedAsync -= OnClosedAsync;

#pragma warning disable 4014
            WindowViewModel.CloseViewModelAsync(null);
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
