namespace Orc.Controls;

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;
using Microsoft.Extensions.Logging;

public class CalloutViewModel : ViewModelBase, ICallout
{
    private readonly ILogger<CalloutViewModel> _logger;
    private readonly ICalloutManager _calloutManager;
    private readonly IDispatcherService _dispatcherService;

    private DispatcherTimer? _dispatcherTimer;

    public CalloutViewModel(ILogger<CalloutViewModel> logger, IServiceProvider serviceProvider, 
        ICalloutManager calloutManager, IDispatcherService dispatcherService)
    {
        _logger = logger;
        _calloutManager = calloutManager;
        _dispatcherService = dispatcherService;

        ValidateUsingDataAnnotations = false;

        Id = Guid.NewGuid();

        PauseTimer = new Command(serviceProvider, OnPauseTimerExecute);
        ResumeTimer = new Command(serviceProvider, OnResumeTimerExecute);
        ClosePopup = new Command(serviceProvider, OnClosePopupExecute);
    }

    public Guid Id { get; }

    public string? Name { get; set; }

    public new string Title
    {
        get { return base.Title; }
        set { base.Title = value; }
    }

    public bool IsOpen { get; set; }

    public UIElement? PlacementTarget { get; set; }

    public string? Description { get; set; }

    public object? InnerContent { get; set; }

    public object? Tag => null;

    public bool IsClosable { get; set; }

    public bool HasShown { get; private set; }

    public TimeSpan ShowTime { get; set; }

    public ICommand? Command { get; set; }

    public string? Version { get; protected set; }
        
    public event EventHandler<CalloutEventArgs>? Showing;

    public event EventHandler<CalloutEventArgs>? Hiding;

    #region Commands
    public Command PauseTimer { get; }

    private void OnPauseTimerExecute()
    {
        _dispatcherTimer?.Stop();
    }

    public Command ResumeTimer { get; }

    private void OnResumeTimerExecute()
    {
        _dispatcherTimer?.Start();
    }

    public Command ClosePopup { get; }

    private void OnClosePopupExecute()
    {
        if (IsClosable)
        {
            Hide();
        }
    }
    #endregion

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        _dispatcherTimer = new DispatcherTimer();
        _dispatcherTimer.Tick += OnDispatcherTimerTick;

        _calloutManager.Register(this);
    }

    protected override async Task CloseAsync()
    {
        if (_dispatcherTimer is null)
        {
            return;
        }

        _dispatcherTimer.Tick -= OnDispatcherTimerTick;

        Hide();

        _calloutManager.Unregister(this);

        await base.CloseAsync();
    }

    private void OnDispatcherTimerTick(object? sender, EventArgs e)
    {
        _dispatcherTimer?.Stop();

        Hide();
    }

    public void Show()
    {
        _dispatcherService.BeginInvokeIfRequired(() =>
        {
            if (IsOpen)
            {
                return;
            }

            _logger.LogDebug($"[{this}] Showing callout");

            if (ShowTime > TimeSpan.Zero && _dispatcherTimer is not null)
            {
                _logger.LogDebug($"[{this}] Starting callout timer with interval of '{ShowTime}'");

                _dispatcherTimer.Interval = ShowTime;
                _dispatcherTimer.Start();
            }

            HasShown = true;
            IsOpen = true;
        });
    }

    public void Hide()
    {
        _dispatcherService.BeginInvokeIfRequired(() =>
        {
            _dispatcherTimer?.Stop();

            if (!IsOpen)
            {
                return;
            }

            _logger.LogDebug($"[{this}] Hiding callout");

            IsOpen = false;
        });
    }

    private void OnIsOpenChanged()
    {
        if (IsOpen)
        {
            Showing?.Invoke(this, new CalloutEventArgs(this));
        }
        else
        {
            Hiding?.Invoke(this, new CalloutEventArgs(this));
        }
    }

    public override string ToString()
    {
        return $"{Name} | {Id}";
    }
}
