namespace Orc.Controls.Settings;

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using Catel.Windows;
using Catel.Windows.Markup;
using Microsoft.Xaml.Behaviors;
using Orc.Controls.Helpers;

[MarkupExtensionReturnType(typeof(string))]
public class SettingsKeyBindingExtension : UpdatableMarkupExtension
{
    private bool _isListening;
    private bool _isVisibilityListening;
    private WeakReference<FrameworkElement>? _targetFrameworkElement;
    private DispatcherTimer? _updateTimer;

    /// <summary>
    /// The separator to use between combined keys
    /// </summary>
    public string Separator { get; set; } = "/";

    public int Delay { get; set; } = 1;

    /// <summary>
    /// Whether to include the current element in the search
    /// </summary>
    public bool IncludeSelf { get; set; } = true;

    /// <summary>
    /// Debounced UpdateValue method that ensures UpdateValue is called only once after a 500ms delay
    /// </summary>
    private void DebouncedUpdateValue()
    {
        // Stop and restart the timer
        if (_updateTimer is not null)
        {
            _updateTimer.Stop();
        }
        else
        {
            _updateTimer = new()
            {
                Interval = TimeSpan.FromMilliseconds(Delay)
            };
            _updateTimer.Tick += (_, _) =>
            {
                _updateTimer.Stop();
                UpdateValue();
            };
        }

        _updateTimer.Start();
    }

    /// <summary>
    /// Provides the combined SettingsKey value
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <returns>The combined SettingsKey value</returns>
    protected override object? ProvideDynamicValue(IServiceProvider? serviceProvider)
    {
        var targetObject = TargetObject;
        var attachedObject = targetObject as IAttachedObject;
        if (attachedObject is not null)
        {
            targetObject = attachedObject.AssociatedObject;

            // If AssociatedObject is null, the UpdatableMarkupExtension will handle re-evaluation
            if (targetObject is null)
            {
                // TODO: Temporary postpone
                PostponeDispatcherTimerAction.Execute(UpdateValue, 500);

                return string.Empty;
            }
        }

        if (targetObject is not DependencyObject dependencyObject)
        {
            return string.Empty;
        }

        // If we're in design mode, return a placeholder
        if (DesignerProperties.GetIsInDesignMode(dependencyObject))
        {
            return "Default";
        }

        // Check if the control is visible in the visual tree
        if (!IsControlVisible(dependencyObject))
        {
            // Subscribe to visibility changes if not already listening
            SubscribeToVisibilityChanges(dependencyObject);

            // Return empty string for now, will update when becomes visible
            return string.Empty;
        }

        // Subscribe to SettingsKey changes if not already listening
        if (!_isListening)
        {
            SubscribeToSettingsKeyChanges(dependencyObject);
            _isListening = true;
        }

        var startElement = IncludeSelf ? dependencyObject : dependencyObject.GetVisualParent() ?? dependencyObject.GetLogicalParent();
        if (startElement is null)
        {
            return string.Empty;
        }

        var resultKey = SettingsHelper.GetCombinedSettingsKey(startElement, Separator);
        if (attachedObject is not DependencyObject attachedDependencyObject)
        {
            return resultKey;
        }

        var isSet = DependencyPropertyHelper.IsPropertySet(attachedDependencyObject, SettingsManagement.SettingsKeyProperty);
        var attachedObjectSettingsKey = SettingsManagement.GetSettingsKey(attachedDependencyObject)?.ToString();

        if (!string.IsNullOrWhiteSpace(attachedObjectSettingsKey))
        {
            resultKey += Separator + attachedObjectSettingsKey;
        }
        else
        {
            if (isSet)
            {
                return null;
            }
        }

        return resultKey;
    }

    /// <summary>
    /// Check if the control is visible in the visual tree
    /// </summary>
    /// <param name="dependencyObject">The dependency object to check</param>
    /// <returns>True if the control is visible</returns>
    private bool IsControlVisible(DependencyObject dependencyObject)
    {
        // Walk up the visual tree to check if all ancestors are visible
        var current = dependencyObject;
        while (current is not null)
        {
            if (current is FrameworkElement frameworkElement)
            {
                // Check if the element is visible
                if (!frameworkElement.IsVisible)
                {
                    return false;
                }

                // Special handling for TabItems - check if they're selected
                if (IsInUnselectedTabItem(frameworkElement))
                {
                    return false;
                }
            }

            current = current.GetVisualParent() ?? current.GetLogicalParent();
        }

        return true;
    }

    /// <summary>
    /// Check if the element is inside an unselected TabItem
    /// </summary>
    /// <param name="element">The element to check</param>
    /// <returns>True if inside an unselected TabItem</returns>
    private bool IsInUnselectedTabItem(FrameworkElement element)
    {
        var current = element;
        while (current is not null)
        {
            // Check if we're in a TabItem that's not selected
            if (current is System.Windows.Controls.TabItem tabItem)
            {
                return !tabItem.IsSelected;
            }

            current = current.GetVisualParent() as FrameworkElement ?? current.GetLogicalParent() as FrameworkElement;
        }

        return false;
    }

    /// <summary>
    /// Subscribe to visibility changes
    /// </summary>
    /// <param name="dependencyObject">The dependency object to monitor</param>
    private void SubscribeToVisibilityChanges(DependencyObject dependencyObject)
    {
        if (_isVisibilityListening)
        {
            return;
        }

        if (dependencyObject is FrameworkElement frameworkElement)
        {
            _targetFrameworkElement = new(frameworkElement);

            // Subscribe to IsVisibleChanged event
            frameworkElement.IsVisibleChanged += OnIsVisibleChanged;

            // Subscribe to Loaded event in case the element becomes part of visual tree later
            frameworkElement.Loaded += OnFrameworkElementLoaded;

            // Subscribe to parent TabControl selection changes if applicable
            SubscribeToTabControlSelectionChanges(frameworkElement);

            _isVisibilityListening = true;
        }
    }

    /// <summary>
    /// Subscribe to TabControl selection changes
    /// </summary>
    /// <param name="element">The element to check for TabControl ancestry</param>
    private void SubscribeToTabControlSelectionChanges(FrameworkElement element)
    {
        var current = element;
        while (current is not null)
        {
            if (current is System.Windows.Controls.TabControl tabControl)
            {
                tabControl.SelectionChanged += OnTabControlSelectionChanged;
                break;
            }

            current = current.GetVisualParent() as FrameworkElement ?? current.GetLogicalParent() as FrameworkElement;
        }
    }

    /// <summary>
    /// Handle TabControl selection changes
    /// </summary>
    private void OnTabControlSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        // Check if our target element is now visible due to tab selection change
        if (_targetFrameworkElement?.TryGetTarget(out var target) == true)
        {
            if (IsControlVisible(target))
            {
                DebouncedUpdateValue();
            }
        }
    }

    /// <summary>
    /// Handle visibility changes
    /// </summary>
    private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if ((bool)e.NewValue) // Became visible
        {
            DebouncedUpdateValue();
        }
    }

    /// <summary>
    /// Handle loaded event
    /// </summary>
    private void OnFrameworkElementLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement frameworkElement && IsControlVisible(frameworkElement))
        {
            DebouncedUpdateValue();
        }
    }

    /// <summary>
    /// Subscribe to SettingsKey property changes in the visual tree
    /// </summary>
    /// <param name="startElement">The element to start monitoring from</param>
    private void SubscribeToSettingsKeyChanges(DependencyObject startElement)
    {
        // Subscribe to the global SettingsKey change notifications
        SettingsManagement.SettingsKeyChanged += OnSettingsKeyChanged;
    }

    /// <summary>
    /// Handle SettingsKey property changes
    /// </summary>
    /// <param name="sender">The element that changed</param>
    /// <param name="e">Event arguments</param>
    private void OnSettingsKeyChanged(object? sender, SettingsKeyChangedEventArgs e)
    {
        if (TargetObject is not DependencyObject targetObject)
        {
            return;
        }

        // Resolve the actual dependency object if it's an attached object
        if (targetObject is IAttachedObject attachedObject)
        {
            targetObject = attachedObject.AssociatedObject;
            if (targetObject is null)
            {
                return;
            }
        }

        // Only update if the control is visible
        if (!IsControlVisible(targetObject))
        {
            return;
        }

        // Check if the changed element is in our visual tree hierarchy
        if (IsInVisualTreeHierarchy(targetObject, e.Element))
        {
            DebouncedUpdateValue();
        }
    }

    /// <summary>
    /// Check if the changed element affects our visual tree hierarchy
    /// </summary>
    /// <param name="targetObject">Our target object</param>
    /// <param name="changedElement">The element that changed</param>
    /// <returns>True if the change affects our hierarchy</returns>
    private bool IsInVisualTreeHierarchy(DependencyObject targetObject, DependencyObject changedElement)
    {
        if (targetObject is IAttachedObject attachedObject)
        {
            targetObject = attachedObject.AssociatedObject;
        }

        if (targetObject is null)
        {
            return false;
        }

        if (changedElement is IAttachedObject changedAttachedObject)
        {
            return Equals(changedAttachedObject.AssociatedObject, targetObject);
        }

        var startElement = IncludeSelf ? targetObject : VisualTreeHelper.GetParent(targetObject);
        var current = startElement;

        // Walk up our visual tree to see if the changed element is an ancestor
        while (current is not null)
        {
            if (current == changedElement)
            {
                return true;
            }

            current = current.GetVisualParent() ?? current.GetLogicalParent();
        }

        return false;
    }

    /// <summary>
    /// Clean up event subscriptions
    /// </summary>
    protected override void OnTargetObjectUnloaded()
    {
        // Stop and dispose the timer
        if (_updateTimer is not null)
        {
            _updateTimer.Stop();
            _updateTimer = null;
        }

        if (_isListening)
        {
            SettingsManagement.SettingsKeyChanged -= OnSettingsKeyChanged;
            _isListening = false;
        }

        if (_isVisibilityListening)
        {
            if (_targetFrameworkElement?.TryGetTarget(out var target) == true)
            {
                target.IsVisibleChanged -= OnIsVisibleChanged;
                target.Loaded -= OnFrameworkElementLoaded;

                // Unsubscribe from TabControl selection changes
                var current = target;
                while (current is not null)
                {
                    if (current is System.Windows.Controls.TabControl tabControl)
                    {
                        tabControl.SelectionChanged -= OnTabControlSelectionChanged;
                        break;
                    }
                    current = current.GetVisualParent() as FrameworkElement ?? current.GetLogicalParent() as FrameworkElement;
                }
            }

            _isVisibilityListening = false;
        }

        base.OnTargetObjectUnloaded();
    }
}
