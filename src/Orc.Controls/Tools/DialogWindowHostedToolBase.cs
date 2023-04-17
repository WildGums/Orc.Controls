namespace Orc.Controls;

using System;
using System.Threading.Tasks;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;

public abstract class DialogWindowHostedToolBase<T> : ControlToolBase
    where T : ViewModelBase
{
    protected readonly IUIVisualizerService _uiVisualizerService;
    protected readonly ITypeFactory _typeFactory;

    protected object? _parameter;
    protected T? _windowViewModel;

    protected DialogWindowHostedToolBase(ITypeFactory typeFactory, IUIVisualizerService uiVisualizerService)
    {
        ArgumentNullException.ThrowIfNull(typeFactory);
        ArgumentNullException.ThrowIfNull(uiVisualizerService);

        _typeFactory = typeFactory;
        _uiVisualizerService = uiVisualizerService;
    }

    public virtual bool IsModal => true;

    protected override async void OnOpen(object? parameter = null)
    {
        _parameter = parameter;

        _windowViewModel = InitializeViewModel();
        _windowViewModel.ClosedAsync += OnClosedAsync;
        ApplyParameter(parameter);

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

    public override void Close()
    {
        base.Close();

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
        Close();

        return Task.CompletedTask;
    }
}
