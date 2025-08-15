namespace Orc.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// TabItem that supports tear-off functionality
/// </summary>
public class TearOffTabItem : TabItem
{
    private bool _isDragging;
    private Point _startPoint;
    private TearOffWindow? _tearOffWindow;

    /// <summary>
    /// Dependency property for IsTornOff
    /// </summary>
    public static readonly DependencyProperty IsTornOffProperty =
        DependencyProperty.Register(nameof(IsTornOff), typeof(bool), typeof(TearOffTabItem),
            new PropertyMetadata(false, OnIsTornOffChanged));

    /// <summary>
    /// Dependency property for TearOffThreshold
    /// </summary>
    public static readonly DependencyProperty TearOffThresholdProperty =
        DependencyProperty.Register(nameof(TearOffThreshold), typeof(double), typeof(TearOffTabItem),
            new PropertyMetadata(20.0));

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

    static TearOffTabItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TearOffTabItem),
            new FrameworkPropertyMetadata(typeof(TearOffTabItem)));
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
    /// Gets or sets the distance threshold for tear-off to activate
    /// </summary>
    public double TearOffThreshold
    {
        get => (double)GetValue(TearOffThresholdProperty);
        set => SetValue(TearOffThresholdProperty, value);
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

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (CanTearOff && !IsTornOff)
        {
            _startPoint = e.GetPosition(this);
            _isDragging = true;
            CaptureMouse();
        }

        base.OnMouseLeftButtonDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_isDragging && e.LeftButton == MouseButtonState.Pressed && CanTearOff && !IsTornOff)
        {
            var currentPoint = e.GetPosition(this);
            var distance = (currentPoint - _startPoint).Length;

            if (distance > TearOffThreshold)
            {
                InitiateTearOff();
            }
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (_isDragging)
        {
            _isDragging = false;
            ReleaseMouseCapture();
        }

        base.OnMouseLeftButtonUp(e);
    }

    private void InitiateTearOff()
    {
        _isDragging = false;
        ReleaseMouseCapture();

        var tabControl = FindParent<TabControl>(this);
        if (tabControl is null)
            return;

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

        // Create tear-off window
        _tearOffWindow = new TearOffWindow(this, content, header?.ToString() ?? "Torn Tab");
        _tearOffWindow.Show();
        _tearOffWindow.Closed += OnTearOffWindowClosed;

        // Mark as torn off
        SetCurrentValue(IsTornOffProperty, true);

        // Hide the content in the original tab
        SetCurrentValue(ContentProperty, null);
        SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
    }

    private void OnTearOffWindowClosed(object? sender, EventArgs e)
    {
        if (_tearOffWindow is not null)
        {
            _tearOffWindow.Closed -= OnTearOffWindowClosed;

            // If window was closed by docking, don't restore content here
            if (!_tearOffWindow.WasDockedBack)
            {
                DockBack(_tearOffWindow.OriginalContent);
            }

            _tearOffWindow = null;
        }
    }

    /// <summary>
    /// Docks the tab back from its torn-off state
    /// </summary>
    public void DockBack(object? content = null)
    {
        if (!IsTornOff)
            return;

        // Restore content
        SetCurrentValue(ContentProperty, content ?? _tearOffWindow?.OriginalContent);
        SetCurrentValue(VisibilityProperty, Visibility.Visible);
        SetCurrentValue(IsTornOffProperty, false);

        // Close tear-off window if it exists
        if (_tearOffWindow is not null)
        {
            _tearOffWindow.WasDockedBack = true;
            _tearOffWindow.Close();
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
