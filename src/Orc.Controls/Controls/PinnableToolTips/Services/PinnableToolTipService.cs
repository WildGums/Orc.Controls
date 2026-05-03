namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Catel.Windows;

/// <summary>
/// The pinnable toolTip service.
/// </summary>
public static class PinnableToolTipService
{
    /// <summary>
    /// The default initial show delay.
    /// </summary>
    private const int DefaultInitialShowDelay = 500;

    /// <summary>
    /// The default show duration.
    /// </summary>
    private const int DefaultShowDuration = 5000;

    /// <summary>
    /// The locker.
    /// </summary>
    private static readonly object Locker = new();

    /// <summary>
    /// The mouse position.
    /// </summary>
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    private static Point _mousePosition;

    /// <summary>
    /// The root visual.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private static FrameworkElement? _rootVisual;
#pragma warning restore IDE1006 // Naming Styles
    /// <summary>
    /// The initial show delay property.
    /// </summary>
    public static readonly DependencyProperty InitialShowDelayProperty = DependencyProperty.RegisterAttached("InitialShowDelay",
        typeof(int), typeof(PinnableToolTipService), new PropertyMetadata(DefaultInitialShowDelay));

    /// <summary>
    /// The placement property.
    /// </summary>
    public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement",
        typeof(PlacementMode), typeof(PinnableToolTipService), new PropertyMetadata(PlacementMode.Mouse, OnPlacementChanged));

    /// <summary>
    /// The placement target property.
    /// </summary>
    public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.RegisterAttached("PlacementTarget",
        typeof(UIElement), typeof(PinnableToolTipService), new PropertyMetadata(OnPlacementTargetChanged));

    /// <summary>
    /// The show duration property.
    /// </summary>
    public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.RegisterAttached("ShowDuration",
        typeof(int), typeof(PinnableToolTipService), new PropertyMetadata(DefaultShowDuration));

    /// <summary>
    /// The toolTip property.
    /// </summary>
    public static readonly DependencyProperty ToolTipProperty = DependencyProperty.RegisterAttached("ToolTip",
        typeof(object), typeof(PinnableToolTipService), new PropertyMetadata(OnToolTipChanged));

    /// <summary>
    /// The toolTip owner property.
    /// </summary>
    public static readonly DependencyProperty IsToolTipOwnerProperty = DependencyProperty.RegisterAttached("IsToolTipOwner",
        typeof(bool), typeof(PinnableToolTipService), new PropertyMetadata(OnIsToolTipOwnerChanged));

    /// <summary>
    /// The elements and tool tips.
    /// </summary>
    private static readonly Dictionary<UIElement, PinnableToolTip> ElementsAndToolTips = new();

    /// <summary>
    ///     Gets the mouse position.
    /// </summary>
    internal static Point MousePosition
    {
        get
        {
            lock (Locker)
            {
                return _mousePosition;
            }
        }

        private set
        {
            lock (Locker)
            {
                _mousePosition = value;
            }
        }
    }

    /// <summary>
    ///     Gets the root visual.
    /// </summary>
    internal static FrameworkElement? RootVisual
    {
        get
        {
            lock (Locker)
            {
                return _rootVisual;
            }
        }

        private set
        {
            lock (Locker)
            {
                _rootVisual = value;
            }
        }
    }

    /// <summary>
    /// The get initial show delay.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="int" />.</returns>
    public static int GetInitialShowDelay(DependencyObject element)
    {
        return (int)element.GetValue(InitialShowDelayProperty);
    }

    public static PlacementMode GetPlacement(DependencyObject element)
    {
#pragma warning disable WPF0042 // Set mutable dependency properties using SetCurrentValue.
        return (PlacementMode?)element.GetValue(PlacementProperty) ?? PlacementMode.Mouse;
#pragma warning restore WPF0042 // Set mutable dependency properties using SetCurrentValue.
    }

    /// <summary>
    /// The get placement target.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="UIElement" />.</returns>
    public static UIElement? GetPlacementTarget(DependencyObject element)
    {
        return (UIElement?)element.GetValue(PlacementTargetProperty);
    }

    /// <summary>
    /// The get show duration.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="int" />.</returns>
    public static int GetShowDuration(DependencyObject element)
    {
        return (int)element.GetValue(ShowDurationProperty);
    }

    /// <summary>
    /// The get toolTip.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="object" />.</returns>
    public static object? GetToolTip(DependencyObject element)
    {
        return element.GetValue(ToolTipProperty);
    }

    /// <summary>
    /// The get is toolTip owner.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="object" />.</returns>
    public static bool GetIsToolTipOwner(DependencyObject element)
    {
        return (bool)element.GetValue(IsToolTipOwnerProperty);
    }

    /// <summary>
    /// The set initial show delay.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="value">The value.</param>
    public static void SetInitialShowDelay(DependencyObject element, int value)
    {
        element.SetValue(InitialShowDelayProperty, value);
    }

    /// <summary>
    /// The set placement.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="value">The value.</param>
    public static void SetPlacement(DependencyObject element, PlacementMode value)
    {
        element.SetValue(PlacementProperty, value);
    }

    /// <summary>
    /// The set placement target.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="value">The value.</param>
    public static void SetPlacementTarget(DependencyObject element, UIElement? value)
    {
        element.SetValue(PlacementTargetProperty, value);
    }

    /// <summary>
    /// The set show duration.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="value">The value.</param>
    public static void SetShowDuration(DependencyObject element, int value)
    {
        element.SetValue(ShowDurationProperty, value);
    }

    /// <summary>
    /// The set toolTip.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="value">The value.</param>
    public static void SetToolTip(DependencyObject element, object? value)
    {
        element.SetValue(ToolTipProperty, value);
    }

    /// <summary>
    /// The set is toolTip owner.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="value">The value.</param>
    public static void SetIsToolTipOwner(DependencyObject element, bool value)
    {
        element.SetValue(IsToolTipOwnerProperty, value);
    }

    /// <summary>
    /// The convert to toolTip.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <returns>The <see cref="PinnableToolTip" />.</returns>
    private static PinnableToolTip ConvertToToolTip(object p)
    {
        return p as PinnableToolTip ?? new PinnableToolTip { Content = p };
    }

    /// <summary>
    /// The framework element unloaded.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private static void FrameworkElementUnloaded(object? sender, RoutedEventArgs e)
    {
        if (sender is not UIElement element)
        {
            return;
        }

        PinnableToolTip? toolTip = null;

        lock (Locker)
        {
            if (ElementsAndToolTips.ContainsKey(element))
            {
                toolTip = ElementsAndToolTips[element];
            }
        }

        toolTip?.Hide();
    }

    /// <summary>
    /// The on control enabled changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private static void OnControlEnabledChanged(object? sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not UIElement element)
        {
            return;
        }

        if ((bool)e.NewValue)
        {
            return;
        }

        PinnableToolTip? toolTip = null;

        lock (Locker)
        {
            if (ElementsAndToolTips.ContainsKey(element))
            {
                toolTip = ElementsAndToolTips[element];
            }
        }

        toolTip?.StopTimer();
    }

    /// <summary>
    /// The on element mouse enter.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private static void OnElementMouseEnter(object? sender, MouseEventArgs e)
    {
        if (sender is not FrameworkElement element)
        {
            return;
        }

        PinnableToolTip? toolTip;

        lock (Locker)
        {
            if (!ElementsAndToolTips.TryGetValue(element, out toolTip))
            {
                return;
            }

            MousePosition = e.GetPosition(null);
            SetRootVisual(element);
        }

        if ((toolTip.Content is null && toolTip.ContentTemplate is null)
            || toolTip.IsTimerEnabled 
            || toolTip.IsOpen)
        {
            return;
        }

        var initialShowDelay = GetInitialShowDelay(element);
        var showDuration = GetShowDuration(element);

        toolTip.Owner = element;
        toolTip.SetupTimer(initialShowDelay, showDuration);
        toolTip.StartTimer();
    }

    /// <summary>
    /// The on element mouse leave.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private static void OnElementMouseLeave(object? sender, MouseEventArgs e)
    {
        if (sender is not UIElement currentElement)
        {
            return;
        }

        PinnableToolTip? toolTip;

        lock (Locker)
        {
            if (!ElementsAndToolTips.TryGetValue(currentElement, out toolTip))
            {
                return;
            }
        }

        RootVisual = null;

        if (!toolTip.IsOpen)
        {
            toolTip.StopTimer();
        }
    }

    /// <summary>
    /// The on placement property changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    private static void OnPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var element = (UIElement)d;

        PinnableToolTip? toolTip = null;
        lock (Locker)
        {
            if (ElementsAndToolTips.ContainsKey(element))
            {
                toolTip = ElementsAndToolTips[element];
            }
        }

        if (toolTip is not null && toolTip.IsOpen)
        {
            toolTip.PerformPlacement();
        }
    }

    /// <summary>
    /// The on placement target property changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    private static void OnPlacementTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var element = (UIElement)d;

        PinnableToolTip? toolTip = null;
        lock (Locker)
        {
            if (ElementsAndToolTips.ContainsKey(element))
            {
                toolTip = ElementsAndToolTips[element];
            }
        }

        if (toolTip is not null && toolTip.IsOpen)
        {
            toolTip.PerformPlacement();
        }
    }

    /// <summary>
    /// The on root visual mouse left button down.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private static void OnRootVisualMouseLeftButtonDown(object? sender, MouseButtonEventArgs e)
    {
        lock (Locker)
        {
            foreach (var toolTip in ElementsAndToolTips.Values.Where(toolTip => !toolTip.IsPinned))
            {
                if (toolTip.FindLogicalAncestor(x => ReferenceEquals(x, e.OriginalSource)) is not null)
                {
                    continue;
                }

                toolTip.Hide();
            }
        }
    }

    /// <summary>
    /// The on root visual mouse move.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private static void OnRootVisualMouseMove(object? sender, MouseEventArgs e)
    {
        MousePosition = e.GetPosition(null);
    }

    /// <summary>
    /// The on is toolTip owner property changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    private static void OnIsToolTipOwnerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not UIElement element)
        {
            return;
        }

        if (e.OldValue is not null)
        {
            UnregisterToolTip(element);
        }

        if ((e.NewValue is not null) && ((bool)e.NewValue))
        {
            RegisterToolTip(element, null);
        }
    }

    /// <summary>
    /// The on toolTip property changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    private static void OnToolTipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not UIElement element)
        {
            return;
        }

        if (e.OldValue is not null)
        {
            UnregisterToolTip(element);
        }

        if (e.NewValue is not null)
        {
            RegisterToolTip(element, e.NewValue);
        }
    }

    /// <summary>
    /// The register toolTip.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="p">The p.</param>
    private static void RegisterToolTip(UIElement owner, object? p)
    {
        if (p is null)
        {
            return;
        }

        lock (Locker)
        {
            if (ElementsAndToolTips.ContainsKey(owner))
            {
                return;
            }
        }

        var toolTip = p as PinnableToolTip ?? ConvertToToolTip(p);
        toolTip.Owner = owner;

        if (owner is FrameworkElement element)
        {
            element.Unloaded += FrameworkElementUnloaded;
        }

        owner.MouseEnter += OnElementMouseEnter;
        owner.MouseLeave += OnElementMouseLeave;

        if (owner is Control control)
        {
            control.IsEnabledChanged += OnControlEnabledChanged;
        }

        lock (Locker)
        {
            ElementsAndToolTips[owner] = toolTip;
        }
    }

    /// <summary>
    ///     The set root visual.
    /// </summary>
    /// <param name="frameworkElement"></param>
    private static void SetRootVisual(FrameworkElement frameworkElement)
    {
        lock (Locker)
        {
            if (Application.Current is null)
            {
                return;
            }

            var existingRootVisual = RootVisual;
            if (existingRootVisual is not null)
            {
                existingRootVisual.MouseMove -= OnRootVisualMouseMove;
            }

            var rootVisual = frameworkElement.GetVisualRoot() as FrameworkElement
                             ?? Application.Current.MainWindow?.Content as FrameworkElement
                             ?? Application.Current.MainWindow;

            RootVisual = rootVisual;

            if (rootVisual is null)
            {
                return;
            }

            rootVisual.MouseMove += OnRootVisualMouseMove;
            rootVisual.AddHandler(
                UIElement.MouseLeftButtonDownEvent,
                new MouseButtonEventHandler(OnRootVisualMouseLeftButtonDown),
                true);
        }
    }

    /// <summary>
    /// The unregister toolTip.
    /// </summary>
    /// <param name="owner">The owner.</param>
    private static void UnregisterToolTip(UIElement owner)
    {
        try
        {
            lock (Locker)
            {
                if (!ElementsAndToolTips.TryGetValue(owner, out var toolTip))
                {
                    return;
                }

                if (owner is FrameworkElement element)
                {
                    element.Unloaded -= FrameworkElementUnloaded;
                    toolTip.SetValue(FrameworkElement.DataContextProperty, null);
                }

                owner.MouseEnter -= OnElementMouseEnter;
                owner.MouseLeave -= OnElementMouseLeave;

                if (owner is Control control)
                {
                    control.IsEnabledChanged -= OnControlEnabledChanged;
                }

                toolTip.Hide();
                toolTip.Owner = null;

                ElementsAndToolTips.Remove(owner);
            }
        }
        catch (Exception)
        {
            // Ignore
        }
    }
}
