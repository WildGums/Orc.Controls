namespace Orc.Controls;

using System;
using System.Threading.Tasks;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;

public abstract class DialogWindowHostedToolBase<T> : ControlToolBase
    where T : ViewModelBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    protected readonly IUIVisualizerService _uiVisualizerService;

    protected object? _parameter;
    protected T? _windowViewModel;

    protected DialogWindowHostedToolBase(IUIVisualizerService uiVisualizerService)
    {
        ArgumentNullException.ThrowIfNull(uiVisualizerService);

        _uiVisualizerService = uiVisualizerService;
    }

    public virtual bool IsModal => true;
    protected override bool StaysOpen => _windowViewModel is not null;

    protected override Task OnOpenAsync(object? parameter = null)
    {
        try
        {
            _windowViewModel = InitializeViewModel();
        }
        catch (Exception ex)
        {
            Log.Error(ex);

            return Task.CompletedTask;
        }

        _parameter = parameter;
        ApplyParameter(_parameter);

        if (IsModal)
        {
            Task.Run(() => _uiVisualizerService.ShowDialogAsync(_windowViewModel, OnWindowCompleted));
        }
        else
        {
            Task.Run(() => _uiVisualizerService.ShowAsync(_windowViewModel, OnWindowCompleted));
        }

        return Task.CompletedTask;
    }

    private async void OnWindowCompleted(object? sender, UICompletedEventArgs args)
    {
        await base.CloseAsync();

        if (args.Result.DialogResult ?? false)
        {
            OnAccepted();
        }
        else
        {
            OnRejected();
        }
    }

    protected virtual void OnRejected()
    {
        
    }

    protected virtual void OnAccepted()
    {

    }

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
        
        await _windowViewModel.CloseViewModelAsync(null);
    }
}
