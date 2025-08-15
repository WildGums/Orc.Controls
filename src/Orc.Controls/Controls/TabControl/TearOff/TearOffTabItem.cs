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
    /// <summary>
    /// Dependency property for IsTornOff
    /// </summary>
    public static readonly DependencyProperty IsTornOffProperty =
        DependencyProperty.Register(nameof(IsTornOff), typeof(bool), typeof(TearOffTabItem),
            new(false, OnIsTornOffChanged));

    /// <summary>
    /// Dependency property for TearOffThreshold
    /// </summary>
    public static readonly DependencyProperty TearOffThresholdProperty =
        DependencyProperty.Register(nameof(TearOffThreshold), typeof(double), typeof(TearOffTabItem),
            new(20.0));

    /// <summary>
    /// Dependency property for CanTearOff
    /// </summary>
    public static readonly DependencyProperty CanTearOffProperty =
        DependencyProperty.Register(nameof(CanTearOff), typeof(bool), typeof(TearOffTabItem),
            new(true));

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

    private bool _isDragging;
    private Point _startPoint;
    private TearOffWindow? _tearOffWindow;

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
        _tearOffWindow = new(this, detachedContent, header?.ToString() ?? "Torn Tab");
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

            //case TabItem parentTabItem:
            //    // This is the case you encountered - the logical parent is the TabItem itself
            //    parentTabItem.Content = null;
            //    break;
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

    /// <summary>
    /// Docks the tab back from its torn-off state
    /// </summary>
    public void DockBack(object? content = null)
    {
        if (!IsTornOff)
        {
            return;
        }

        // Get content from tear-off window or use provided content
        var contentToRestore = content ?? _tearOffWindow?.GetDetachedContent();

        // Restore content
        SetCurrentValue(ContentProperty, contentToRestore);
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
