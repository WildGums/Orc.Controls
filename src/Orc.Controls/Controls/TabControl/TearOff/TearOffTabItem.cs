namespace Orc.Controls;

using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Catel.IoC;
using Catel.Services;

internal sealed class PostponeDispatcherOperation : IDisposable
{
    private readonly Action _action;
    private readonly IDispatcherService _dispatcherService;
    private readonly Timer _timer;

    public PostponeDispatcherOperation(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        _action = action;

        _dispatcherService = this.GetServiceLocator().ResolveRequiredType<IDispatcherService>();

        _timer = new();
        _timer.Elapsed += OnCompareByColumnTimerElapsed;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void Execute(int delay)
    {
        if (delay <= 0)
        {
            _action();
            return;
        }

        _timer.Interval = delay;
        _timer.Start();
    }

    private void OnCompareByColumnTimerElapsed(object? _, ElapsedEventArgs e)
    {
        _timer.Stop();

        _dispatcherService.Invoke(_action);
    }
}

/// <summary>
/// Command for tearing off a tab
/// </summary>
public static class TearOffCommands
{
    public static readonly RoutedCommand TearOffTab = new RoutedCommand(
        nameof(TearOffTab),
        typeof(TearOffCommands),
        new InputGestureCollection { new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift) });
}

/// <summary>
/// TabItem that supports tear-off functionality via button click
/// </summary>
public class TearOffTabItem : TabItem
{
    /// <summary>
    /// Dependency property for IsTornOff
    /// </summary>
    public static readonly DependencyProperty IsTornOffProperty =
        DependencyProperty.Register(nameof(IsTornOff), typeof(bool), typeof(TearOffTabItem),
            new PropertyMetadata(false, OnIsTornOffChanged));

    /// <summary>
    /// Dependency property for CanTearOff
    /// </summary>
    public static readonly DependencyProperty CanTearOffProperty =
        DependencyProperty.Register(nameof(CanTearOff), typeof(bool), typeof(TearOffTabItem),
            new PropertyMetadata(true));

    /// <summary>
    /// Event raised when a tab is torn off
    /// </summary>
    public static readonly RoutedEvent TabTornOffEvent =
        EventManager.RegisterRoutedEvent(nameof(TabTornOff), RoutingStrategy.Bubble,
            typeof(TearOffEventHandler), typeof(TearOffTabItem));

    /// <summary>
    /// Event raised when a tab is docked back
    /// </summary>
    public static readonly RoutedEvent TabDockedEvent =
        EventManager.RegisterRoutedEvent(nameof(TabDocked), RoutingStrategy.Bubble,
            typeof(TearOffEventHandler), typeof(TearOffTabItem));

    private TearOffWindow? _tearOffWindow;
    private Panel? _parentPanel;

    static TearOffTabItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TearOffTabItem),
            new FrameworkPropertyMetadata(typeof(TearOffTabItem)));
    }

    public TearOffTabItem()
    {
        // Register command bindings
        CommandBindings.Add(new CommandBinding(TearOffCommands.TearOffTab, OnTearOffCommandExecuted, OnTearOffCommandCanExecute));
    }

    /// <summary>
    /// Gets or sets whether this tab is currently torn off
    /// </summary>
    public bool IsTornOff
    {
        get => (bool)GetValue(IsTornOffProperty);
        set => SetValue(IsTornOffProperty, value);
    }

    /// <summary>
    /// Gets or sets whether this tab can be torn off
    /// </summary>
    public bool CanTearOff
    {
        get => (bool)GetValue(CanTearOffProperty);
        set => SetValue(CanTearOffProperty, value);
    }

    /// <summary>
    /// Event raised when tab is torn off
    /// </summary>
    public event TearOffEventHandler TabTornOff
    {
        add => AddHandler(TabTornOffEvent, value);
        remove => RemoveHandler(TabTornOffEvent, value);
    }

    /// <summary>
    /// Event raised when tab is docked back
    /// </summary>
    public event TearOffEventHandler TabDocked
    {
        add => AddHandler(TabDockedEvent, value);
        remove => RemoveHandler(TabDockedEvent, value);
    }

    private void OnTearOffCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = CanTearOff && !IsTornOff;
    }

    private void OnTearOffCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (CanTearOff && !IsTornOff)
        {
            InitiateTearOff();
        }
    }

    private void InitiateTearOff()
    {
        var tabControl = FindParent<TabControl>(this);
        if (tabControl is null)
        {
            return;
        }

        var tearOffEventArgs = new TearOffEventArgs(TabTornOffEvent, this);
        RaiseEvent(tearOffEventArgs);

        if (!tearOffEventArgs.Handled)
        {
            PerformTearOff();
        }
    }

    private void PerformTearOff()
    {
        var content = Content;
        var header = Header;

        // Properly detach content from current parent
        var detachedContent = DetachContent(content);

        // Create tear-off window with detached content
        _tearOffWindow = new TearOffWindow(this, detachedContent, header?.ToString() ?? "Torn Tab");
        _tearOffWindow.Show();
        _tearOffWindow.Closed += OnTearOffWindowClosed;

        // Mark as torn off
        SetCurrentValue(IsTornOffProperty, true);

        // Clear the content in the original tab
        SetCurrentValue(ContentProperty, null);
        SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
    }

    private object? DetachContent(object? content)
    {
        if (content is not FrameworkElement element)
        {
            return content;
        }

        // Get the logical parent (which is what causes the "already logical child" error)
        var logicalParent = LogicalTreeHelper.GetParent(element);

        // Handle different types of logical parents
        switch (logicalParent)
        {
            case Panel parentPanel:
                parentPanel.Children.Remove(element);
                break;

            case ContentControl parentContent:
                parentContent.Content = null;
                break;

            case Decorator parentDecorator:
                parentDecorator.Child = null;
                break;

            case ContentPresenter parentPresenter:
                parentPresenter.Content = null;
                break;
        }

        // Also check if we need to remove from visual parent (in case logical and visual differ)
        var visualParent = VisualTreeHelper.GetParent(element);
        if (visualParent != logicalParent)
        {
            switch (visualParent)
            {
                case Panel parentPanel when !parentPanel.Children.Contains(element):
                    // Already removed or not there
                    break;
                case Panel parentPanel:
                    _parentPanel = parentPanel;
                    parentPanel.Children.Remove(element);
                    break;
            }
        }

        return content;
    }

    private void OnTearOffWindowClosed(object? sender, EventArgs e)
    {
        if (_tearOffWindow is not null)
        {
            _tearOffWindow.Closed -= OnTearOffWindowClosed;

            // If window was closed by docking, don't restore content here
            if (!_tearOffWindow.WasDockedBack)
            {
                DockBack(_tearOffWindow.GetDetachedContent());
            }

            _tearOffWindow = null;
        }
    }

    public void DockBack(object? content = null)
    {
        if (!IsTornOff)
        {
            return;
        }

        // Get content from tear-off window or use provided content
        var contentToRestore = content ?? _tearOffWindow?.GetDetachedContent();

        System.Diagnostics.Debug.WriteLine($"DockBack - Content to restore: {contentToRestore?.GetType().Name ?? "null"}");

        // Close tear-off window first
        if (_tearOffWindow is not null && !_tearOffWindow.IsClosing)
        {
            _tearOffWindow.WasDockedBack = true;
            _tearOffWindow.Close();
            _tearOffWindow = null;
        }

        if (contentToRestore is ContentPresenter contentPresenter)
        {
            SetCurrentValue(ContentProperty, contentPresenter.Content);
        }

        // Restore content
        System.Diagnostics.Debug.WriteLine($"Setting Content to: {contentToRestore}");

        SetCurrentValue(VisibilityProperty, Visibility.Visible);
        SetCurrentValue(IsTornOffProperty, false);

        System.Diagnostics.Debug.WriteLine($"Content after setting: {Content?.GetType().Name ?? "null"}");

        if (contentToRestore is UIElement element)
        {
            _parentPanel?.Children.Add(element);
            _parentPanel = null;
        }

        var tearOffEventArgs = new TearOffEventArgs(TabDockedEvent, this);
        RaiseEvent(tearOffEventArgs);
    }

    private static void OnIsTornOffChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TearOffTabItem tabItem && (bool)e.NewValue)
        {
            tabItem.IsSelected = false;
        }
    }

    private static T? FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        var parentObject = VisualTreeHelper.GetParent(child);

        return parentObject switch
        {
            null => null,
            T parent => parent,
            _ => FindParent<T>(parentObject)
        };
    }
}
