namespace Orc.Controls.Settings;

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Catel.IoC;
using Catel.Logging;
using Catel.Windows.Interactivity;

public class ControlSettingsBehavior<TControl, TSettings> : BehaviorBase<TControl>, ISettingsElement
    where TControl : FrameworkElement
    where TSettings : class
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    public static readonly DependencyProperty SettingsKeyProperty = DependencyProperty.Register(
        nameof(SettingsKey), typeof(string), typeof(ControlSettingsBehavior<TControl, TSettings>),
        new(null, (sender, _) => ((ControlSettingsBehavior<TControl, TSettings>)sender).OnSettingsKeyChanged()));

    public static readonly DependencyProperty IsSettingsDirtyProperty = DependencyProperty.Register(
        nameof(IsSettingsDirty), typeof(bool), typeof(ControlSettingsBehavior<TControl, TSettings>),
        new(false));

    public static readonly DependencyProperty InitialKeySettingsProperty = DependencyProperty.Register(
        nameof(InitialKeySettings), typeof(InitialKeySettings), typeof(ControlSettingsBehavior<TControl, TSettings>),
        new(InitialKeySettings.Current));

    public static readonly DependencyProperty SettingsStorageProperty = DependencyProperty.Register(
        nameof(SettingsStorage), typeof(ISettingsStorage<TSettings>), typeof(ControlSettingsBehavior<TControl, TSettings>),
        new(null));

    public static readonly DependencyProperty ControlAdapterProperty = DependencyProperty.Register(
        nameof(ControlAdapter), typeof(IControlSettingsAdapter<TControl, TSettings>), typeof(ControlSettingsBehavior<TControl, TSettings>),
        new(null, (sender, _) => ((ControlSettingsBehavior<TControl, TSettings>)sender).OnControlAdapterChanged()));

    public static readonly DependencyProperty IsAutosaveProperty = DependencyProperty.Register(
        nameof(IsAutosave), typeof(bool), typeof(ControlSettingsBehavior<TControl, TSettings>),
        new(false));

    // New property to control synchronization behavior
    public static readonly DependencyProperty EnableSynchronizationProperty = DependencyProperty.Register(
        nameof(EnableSynchronization), typeof(bool), typeof(ControlSettingsBehavior<TControl, TSettings>),
        new(true));

    protected bool _isLoading;
    private bool _isSynchronizing; // Prevent infinite loops during sync
    private TSettings? _lastSavedSettings;
    protected ISettingsKeyManager _keyManager;
    protected ISettingsStateStorage StateStorage;
    private DispatcherTimer? _settingsTimer;
    private string? _lastLoadedSettingsKey;
    private IControlSettingsAdapter<TControl, TSettings>? _currentAdapter;
    private ISettingsKeyInteractionHub _settingsKeyInteractionHub;

    public FrameworkElement? Control => AssociatedObject;

    public string? SettingsKey
    {
        get => (string?)GetValue(SettingsKeyProperty);
        set => SetValue(SettingsKeyProperty, value);
    }

    public bool IsSettingsDirty
    {
        get => (bool)GetValue(IsSettingsDirtyProperty);
        private set => SetValue(IsSettingsDirtyProperty, value);
    }

    public InitialKeySettings InitialKeySettings
    {
        get => (InitialKeySettings)GetValue(InitialKeySettingsProperty);
        set => SetValue(InitialKeySettingsProperty, value);
    }

    public ISettingsStorage<TSettings>? SettingsStorage
    {
        get => (ISettingsStorage<TSettings>?)GetValue(SettingsStorageProperty);
        set => SetValue(SettingsStorageProperty, value);
    }

    public IControlSettingsAdapter<TControl, TSettings>? ControlAdapter
    {
        get => (IControlSettingsAdapter<TControl, TSettings>?)GetValue(ControlAdapterProperty);
        set => SetValue(ControlAdapterProperty, value);
    }

    public bool IsAutosave
    {
        get => (bool)GetValue(IsAutosaveProperty);
        set => SetValue(IsAutosaveProperty, value);
    }

    public bool EnableSynchronization
    {
        get => (bool)GetValue(EnableSynchronizationProperty);
        set => SetValue(EnableSynchronizationProperty, value);
    }

    public ControlSettingsBehavior()
    {
        var serviceLocator = this.GetServiceLocator();
        _keyManager = serviceLocator.ResolveRequiredType<ISettingsKeyManager>();
        StateStorage = serviceLocator.ResolveRequiredType<ISettingsStateStorage>();
        _settingsKeyInteractionHub = serviceLocator.ResolveRequiredType<ISettingsKeyInteractionHub>();
    }

    protected override void OnAssociatedObjectLoaded()
    {
        base.OnAssociatedObjectLoaded();

        var serviceLocator = this.GetServiceLocator();

        // If no storage is explicitly set, try to resolve from IoC
        if (SettingsStorage is null)
        {
            var settingsStorage = serviceLocator.ResolveType<ISettingsStorage<TSettings>>();
            SetCurrentValue(SettingsStorageProperty, settingsStorage);
        }

        // If no adapter is explicitly set, try to resolve from IoC
        if (ControlAdapter is null)
        {
            var controlAdapter = serviceLocator.ResolveType<IControlSettingsAdapter<TControl, TSettings>>();
            SetCurrentValue(ControlAdapterProperty, controlAdapter);
        }

        // Attach adapter to control
        AttachAdapter();

        // Subscribe to key manager events
        _keyManager.LoadRequested += OnLoadRequestedAsync;
        _keyManager.SaveRequested += OnSaveRequestedAsync;
        _keyManager.RemoveRequested += OnRemoveRequestedAsync;
        _keyManager.RenameRequested += OnRenameRequestedAsync;
        _keyManager.RefreshRequested += OnRefreshRequestedAsync;
        _keyManager.RequestControls += OnRequestControlsAsync;

        // NEW: Enable and subscribe to settings changed events for synchronization
        if (EnableSynchronization)
        {
            _keyManager.EnableSynchronization();
            _keyManager.SubscribeToSettingsChanges(OnKeyManagerSettingsChanged);
        }

        _ = LoadAndApplySettingsAsync();
    }
    
    protected override void OnAssociatedObjectUnloaded()
    {
        // Store current settings before unloading
        if (SettingsKey is not null && ControlAdapter is not null)
        {
            try
            {
                var currentSettings = ControlAdapter.GetCurrentSettings();
                if (currentSettings is not null)
                {
                    StateStorage.StoreCurrentSettings(SettingsKey, currentSettings);
                    _keyManager.SetDirty(SettingsKey, true);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error storing current settings for key '{SettingsKey}' during unload");
            }
        }

        // Detach adapter
        DetachAdapter();

        // Unsubscribe from key manager events
        _keyManager.LoadRequested -= OnLoadRequestedAsync;
        _keyManager.SaveRequested -= OnSaveRequestedAsync;
        _keyManager.RemoveRequested -= OnRemoveRequestedAsync;
        _keyManager.RenameRequested -= OnRenameRequestedAsync;
        _keyManager.RefreshRequested -= OnRefreshRequestedAsync;
        _keyManager.RequestControls -= OnRequestControlsAsync;

        // NEW: Unsubscribe from synchronization events
        if (EnableSynchronization)
        {
            _keyManager.UnsubscribeFromSettingsChanges(OnKeyManagerSettingsChanged);
        }

        CleanupTimer();
        base.OnAssociatedObjectUnloaded();
    }

    #region Key Manager Event Handlers

    // NEW: Handle settings changes from other controls with the same key
    private async void OnKeyManagerSettingsChanged(object? sender, SettingsChangedEventArgs e)
    {
        if (!EnableSynchronization || _isSynchronizing || e.SettingsKey != SettingsKey)
        {
            return;
        }

        if (e.Settings is TSettings settings)
        {
            try
            {
                _isSynchronizing = true;
                _isLoading = true;

                ControlAdapter?.ApplySettings(settings);
                await UpdateSettingsAsync();
              //  _lastSavedSettings = settings;

                Log.Debug($"Synchronized settings for key '{SettingsKey}' from external change ({typeof(TControl).Name})");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error synchronizing settings for key '{SettingsKey}' ({typeof(TControl).Name})");
            }
            finally
            {
                _isLoading = false;
                _isSynchronizing = false;
            }
        }
    }

    private async Task OnRefreshRequestedAsync(object? sender, SettingsKeyEventArgs e)
    {
        await ReloadSettingsAsync(e.SettingsKey);
    }

    private async Task OnRequestControlsAsync(object sender, ControlRequestedEventArgs e)
    {
        if (_settingsKeyInteractionHub.CanSave(this, e.SettingsKey))
        {
            e.Elements.Add(this);
        }
    }

    private async Task OnLoadRequestedAsync(object? sender, SettingsKeyEventArgs e)
    {
        if (e.SettingsKey == SettingsKey)
        {
            try
            {
                await LoadAndApplySettingsAsync();
                e.Success = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to load settings for key '{e.SettingsKey}'");
                e.Success = false;
            }
        }
    }

    private async Task OnSaveRequestedAsync(object? sender, SettingsKeyEventArgs e)
    {
      //  if (e.SettingsKey == SettingsKey)
        {
            try
            {
                await SaveSettingsAsync(e.SettingsKey);
                e.Success = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to save settings for key '{e.SettingsKey}'");
                e.Success = false;
            }
        }
    }

    private async Task OnRemoveRequestedAsync(object? sender, SettingsKeyEventArgs e)
    {
        try
        {
            await DeleteSettingsAsync(e.SettingsKey);
            e.Success = true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to remove settings for key '{e.SettingsKey}'");
            e.Success = false;
        }
    }

    private async Task OnRenameRequestedAsync(object? sender, SettingsKeyRenameEventArgs e)
    {
        if (e.OldKey == SettingsKey)
        {
            try
            {
                await RenameSettingsAsync(e.OldKey, e.NewKey);
                e.Success = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to rename settings from '{e.OldKey}' to '{e.NewKey}'");
                e.Success = false;
            }
        }
    }

    #endregion

    private void OnControlAdapterChanged()
    {
        // Detach old adapter
        DetachAdapter();

        // Attach new adapter
        AttachAdapter();

        // Reload settings with new adapter
        if (AssociatedObject is not null)
        {
            _ = LoadAndApplySettingsAsync();
        }
    }

    private void AttachAdapter()
    {
        if (ControlAdapter is not null && AssociatedObject is not null)
        {
            _currentAdapter = ControlAdapter;
            _currentAdapter.Attach(AssociatedObject);
            _currentAdapter.SettingsChanged += OnControlSettingsChanged;
        }
    }

    private void DetachAdapter()
    {
        if (_currentAdapter is not null)
        {
            _currentAdapter.SettingsChanged -= OnControlSettingsChanged;
            _currentAdapter.Detach();
            _currentAdapter = null;
        }
    }

    private void OnSettingsKeyChanged()
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var settingsKey = SettingsKey;
        if (string.IsNullOrWhiteSpace(settingsKey))
        {
            return;
        }

        _ = _keyManager.LoadAsync(settingsKey);
    }

    private void OnControlSettingsChanged(object? sender, EventArgs e)
    {
        if (_isLoading || _isSynchronizing)
        {
            return;
        }

        // Use timer to debounce frequent settings changes
        if (_settingsTimer is not null)
        {
            _settingsTimer.Stop();
            _settingsTimer.Start();
        }
        else
        {
            _settingsTimer = new()
            {
                Interval = TimeSpan.FromMilliseconds(400)
            };
            _settingsTimer.Tick += OnSettingsTimerTick;
            _settingsTimer.Start();
        }
    }

    private async void OnSettingsTimerTick(object? sender, EventArgs e)
    {
        await UpdateSettingsAsync();
    }

    private async Task UpdateSettingsAsync()
    {
        if (_isLoading || _isSynchronizing)
        {
            return;
        }

        _settingsTimer?.Stop();

        if (IsAutosave)
        {
            await SaveSettingsAsync();
        }
        else
        {
            UpdateDirtyState();

            // NEW: Notify other controls about the change
            if (EnableSynchronization && SettingsKey is not null && ControlAdapter is not null)
            {
                var currentSettings = ControlAdapter.GetCurrentSettings();
                if (currentSettings is not null)
                {
                    _keyManager.NotifySettingsChanged(SettingsKey, currentSettings);
                }
            }
        }
    }

    private async Task LoadAndApplySettingsAsync(bool bypassCache = false)
    {
        if (AssociatedObject is null
            || string.IsNullOrWhiteSpace(SettingsKey)
            || SettingsStorage is null
            || ControlAdapter is null)
        {
            return;
        }

        try
        {
            _isLoading = true;
            _lastLoadedSettingsKey = SettingsKey;

            TSettings settingsToApply;
            // First check if we have stored settings for this key
            var storedSettings = StateStorage.GetStoredSettings<TSettings>(_lastLoadedSettingsKey);
            if (storedSettings is not null && !bypassCache)
            {
                settingsToApply = storedSettings;
                _lastSavedSettings = null; // Mark as not saved since these are modified settings
                Log.Debug($"Loading stored settings for key '{_lastLoadedSettingsKey}' ({typeof(TControl).Name})");
            }
            else
            {
                // Try to load from storage
                var loadedSettings = await SettingsStorage.LoadAsync(_lastLoadedSettingsKey);
                if (loadedSettings is not null)
                {
                    settingsToApply = loadedSettings;
                    _lastSavedSettings = loadedSettings;
                    Log.Debug($"Loaded settings for key '{_lastLoadedSettingsKey}' ({typeof(TControl).Name})");
                }
                else
                {
                    _lastSavedSettings = null;
                    Log.Debug($"No settings found for key '{_lastLoadedSettingsKey}' ({typeof(TControl).Name})");

                    await _keyManager.SaveAsync(_lastLoadedSettingsKey);
                    return;
                }
            }

            ControlAdapter.ApplySettings(settingsToApply);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error loading settings for key '{_lastLoadedSettingsKey}' ({typeof(TControl).Name})");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void UpdateDirtyState()
    {
        if (AssociatedObject is null || ControlAdapter is null)
        {
            SetCurrentValue(IsSettingsDirtyProperty, false);
            _keyManager.SetDirty(SettingsKey ?? string.Empty, false);
            return;
        }

        try
        {
            var currentSettings = ControlAdapter.GetCurrentSettings();
            var isDirty = !ControlAdapter.AreSettingsEqual(currentSettings, _lastSavedSettings);

            SetCurrentValue(IsSettingsDirtyProperty, isDirty);

            if (isDirty && SettingsKey is not null && currentSettings is not null)
            {
                StateStorage.StoreCurrentSettings(SettingsKey, currentSettings);
                _keyManager.SetDirty(SettingsKey, true);
            }
            else
            {
                _keyManager.SetDirty(SettingsKey ?? string.Empty, false);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error updating dirty state for {typeof(TControl).Name}");
            SetCurrentValue(IsSettingsDirtyProperty, false);
            _keyManager.SetDirty(SettingsKey ?? string.Empty, false);
        }
    }

    private void CleanupTimer()
    {
        if (_settingsTimer is not null)
        {
            _settingsTimer.Stop();
            _settingsTimer.Tick -= OnSettingsTimerTick;
            _settingsTimer = null;
        }
    }

    private async Task RenameSettingsAsync(string oldKey, string newKey)
    {
        if (SettingsStorage is null)
        {
            return;
        }

        try
        {
            StateStorage.Rename(oldKey, newKey);

            await SettingsStorage.RenameAsync(oldKey, newKey);
            if (SettingsKey == oldKey)
            {
                SetCurrentValue(SettingsKeyProperty, newKey);
            }

            Log.Debug($"Renamed settings from '{oldKey}' to '{newKey}' ({typeof(TControl).Name})");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error renaming settings from '{oldKey}' to '{newKey}' ({typeof(TControl).Name})");
            throw;
        }
    }

    public virtual async Task SaveSettingsAsync(string? settingsKey = null)
    {
        settingsKey ??= SettingsKey;

        if (AssociatedObject is null || settingsKey is null || SettingsStorage is null || ControlAdapter is null)
        {
            return;
        }

        try
        {
            if (!_settingsKeyInteractionHub.CanSave(this, settingsKey))
            {
                return;
            }

            var settings = StateStorage.GetStoredSettings<TSettings>(settingsKey);
            if (settings is null && !Equals(settingsKey, SettingsKey))
            {
                return;
            }

            settings ??= ControlAdapter.GetCurrentSettings();
            if (settings is not null)
            {
                await SettingsStorage.SaveAsync(settingsKey, settings);

                _lastSavedSettings = settings;

                // Remove from stored settings since it's now saved
                StateStorage.RemoveStoredSettings(settingsKey);
                _keyManager.SetDirty(settingsKey, false);

                UpdateDirtyState();

                // NEW: Notify other controls about the saved settings
                if (EnableSynchronization)
                {
                    _keyManager.NotifySettingsChanged(settingsKey, settings);
                }

                Log.Debug($"Saved settings for key '{settingsKey}' ({typeof(TControl).Name})");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error saving settings for key '{settingsKey}' ({typeof(TControl).Name})");
            throw;
        }
    }

    public virtual async Task ReloadSettingsAsync(string settingsKey)
    {
        //if (!AssociatedObject.IsVisible)
        //{
        //    return;
        //}

        if (Equals(settingsKey, SettingsKey))
        {
            await LoadAndApplySettingsAsync(true);
        }

        StateStorage.RemoveStoredSettings(settingsKey);
        _keyManager.SetDirty(settingsKey, false);

        UpdateDirtyState();
    }

    public virtual async Task DeleteSettingsAsync(string settingsKey)
    {
        if (SettingsStorage is null)
        {
            return;
        }

        await SettingsStorage.DeleteAsync(settingsKey);

        // Remove from stored settings and dirty state
        StateStorage.RemoveStoredSettings(settingsKey);
        _keyManager.SetDirty(settingsKey, false);

        Log.Debug($"Deleted settings for key '{settingsKey}' ({typeof(TControl).Name})");
    }

    async Task<object?> ISettingsElement.GetSettingsAsync(string key)
    {
        var storedSettings = StateStorage.GetStoredSettings<TSettings>(key);
        if (storedSettings is not null)
        {
            return storedSettings;
        }

        var settingsStorage = SettingsStorage;
        if (settingsStorage is null)
        {
            return null;
        }

        var result = await settingsStorage.LoadAsync(key);
        if (result is null)
        {
            var currentSettings = ControlAdapter?.GetCurrentSettings();
            if (currentSettings is not null)
            {
                await settingsStorage.SaveAsync(key, currentSettings);
                result = await settingsStorage.LoadAsync(key);
            }
        }

        return result;
    }

    public async Task SaveAsync(string key)
    {
        if (!_settingsKeyInteractionHub.CanSave(this, key))
        {
            return;
        }

        var settingsStorage = SettingsStorage;
        if (settingsStorage is null)
        {
            return;
        }

        var settings = StateStorage.GetStoredSettings<TSettings>(key);
        if (settings is null)
        {
            settings ??= ControlAdapter?.GetCurrentSettings();
        }

        if (settings is null)
        {
            return;
        }
        
        await settingsStorage.SaveAsync(key, settings);
    }

    public void RefreshDirtyState()
    {
        UpdateDirtyState();
    }
}
