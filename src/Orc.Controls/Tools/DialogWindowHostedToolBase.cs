namespace Orc.Controls
{
    using System;
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    public abstract class DialogWindowHostedToolBase<T> : ControlToolBase
        where T : ViewModelBase
    {
        protected readonly IUIVisualizerService UIVisualizerService;
        protected readonly ITypeFactory TypeFactory;
        protected object Parameter;

        protected T WindowViewModel;

        protected DialogWindowHostedToolBase(ITypeFactory typeFactory, IUIVisualizerService uiVisualizerService)
        {
            ArgumentNullException.ThrowIfNull(typeFactory);
            ArgumentNullException.ThrowIfNull(uiVisualizerService);

            TypeFactory = typeFactory;
            UIVisualizerService = uiVisualizerService;
        }

        public virtual bool IsModal => true;

        protected override void OnOpen(object? parameter = null)
        {
            Parameter = parameter;

            WindowViewModel = InitializeViewModel();
            WindowViewModel.ClosedAsync += OnClosedAsync;
            ApplyParameter(parameter);

            if (IsModal)
            {
                UIVisualizerService.ShowDialogAsync(WindowViewModel, OnWindowCompleted);
            }
            else
            {
                UIVisualizerService.ShowAsync(WindowViewModel, OnWindowCompleted);
            }
        }

        private void OnWindowCompleted(object? sender, UICompletedEventArgs args)
        {
            if (args.Result.DialogResult ?? false)
            {
                return;
            }

            OnAccepted();
        }

        protected abstract void OnAccepted();
        protected abstract T InitializeViewModel();

        protected virtual void ApplyParameter(object? parameter)
        {
        }

        public override void Close()
        {
            base.Close();

            if (WindowViewModel is null)
            {
                return;
            }

            WindowViewModel.ClosedAsync -= OnClosedAsync;

#pragma warning disable 4014
            WindowViewModel.CloseViewModelAsync(null);
#pragma warning restore 4014
        }

        private Task OnClosedAsync(object? sender, ViewModelClosedEventArgs args)
        {
            Close();

            return Task.CompletedTask;
        }
    }
}
