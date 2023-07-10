namespace Orc.Controls;

using System;
using System.Threading.Tasks;
using Catel.MVVM;
using Catel.Services;

public abstract class DialogWindowHostedToolBase<T> : ControlToolBase
    where T : ViewModelBase
{
    protected readonly IUIVisualizerService _uiVisualizerService;

    protected object? _parameter;
    protected T? _windowViewModel;

    protected DialogWindowHostedToolBase(IUIVisualizerService uiVisualizerService)
    {
        ArgumentNullException.ThrowIfNull(uiVisualizerService);

        _uiVisualizerService = uiVisualizerService;
    }

    public virtual bool IsModal => true;
    protected override bool IsStayedOpen => !IsModal;

    protected override async Task OnOpenAsync(object? parameter = null)
    {
        _parameter = parameter;

        _windowViewModel = InitializeViewModel();
        _windowViewModel.ClosedAsync += OnClosedAsync;
        ApplyParameter(_parameter);

        if (IsModal)
        {
            await _uiVisualizerService.ShowDialogAsync(_windowViewModel, OnWindowCompleted);
        }
        else
        {
            await _uiVisualizerService.ShowAsync(_windowViewModel, OnWindowCompleted);
        }
    }

    private void OnWindowCompleted(object? sender, UICompletedEventArgs args)
    {
        if (args.Result.DialogResult ?? false)
        {
            OnAccepted();
        }
    }

    protected abstract void OnAccepted();
    protected abstract T InitializeViewModel();

    protected virtual void ApplyParameter(object? parameter)
    {
    }

    public override async Task CloseAsync()
    {
        await base.CloseAsync();

        if (_windowViewModel is null)
        {
            return;
        }

        _windowViewModel.ClosedAsync -= OnClosedAsync;

#pragma warning disable 4014
        _windowViewModel.CloseViewModelAsync(null);
#pragma warning restore 4014
    }

    private Task OnClosedAsync(object? sender, ViewModelClosedEventArgs args)
    {
        return CloseAsync();
    }
}
