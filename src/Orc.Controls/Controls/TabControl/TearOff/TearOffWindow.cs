namespace Orc.Controls;

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using Cursors = System.Windows.Input.Cursors;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

/// <summary>
/// Window that hosts a torn-off tab content
/// </summary>
public class TearOffWindow : Window
{
    /// <summary>
    /// Dependency property for dock back drop zones
    /// </summary>
    public static readonly DependencyProperty AllowDockBackProperty =
        DependencyProperty.Register(nameof(AllowDockBack), typeof(bool), typeof(TearOffWindow),
            new(true));

    private readonly TearOffTabItem _originalTabItem;
    private bool _isDockingBack;
    private object? _originalContent;
    private Grid? _windowContentGrid;

    public TearOffWindow(TearOffTabItem originalTabItem, object? content, string title)
    {
        _originalTabItem = originalTabItem ?? throw new ArgumentNullException(nameof(originalTabItem));
        _originalContent = content;

        InitializeWindow(content, title);
        SetupDragAndDrop();
    }

    /// <summary>
    /// Gets or sets whether this window allows docking back
    /// </summary>
    public bool AllowDockBack
    {
        get => (bool)GetValue(AllowDockBackProperty);
        set => SetValue(AllowDockBackProperty, value);
    }

    /// <summary>
    /// Gets the original content that was in the tab
    /// </summary>
    public object? OriginalContent => _originalContent;

    /// <summary>
    /// Gets whether this window was closed due to docking back
    /// </summary>
    public bool WasDockedBack { get; set; }

    /// <summary>
    /// Gets the detached content for docking back
    /// </summary>
    public object? GetDetachedContent()
    {
        if (!(_windowContentGrid?.Children.Count > 0))
        {
            return _originalContent;
        }

        if (_windowContentGrid.Children[0] is not Border { Child: UIElement childContent } border)
        {
            return _originalContent;
        }

        border.Child = null;
        return childContent;
    }

    private void InitializeWindow(object? content, string title)
    {
        SetCurrentValue(TitleProperty, title);
        _originalContent = content;

        SetCurrentValue(WidthProperty, (double)600);
        SetCurrentValue(HeightProperty, (double)400);
        WindowStartupLocation = WindowStartupLocation.Manual;

        // Position near mouse cursor
        var mousePos = GetMousePosition();
        SetCurrentValue(LeftProperty, mousePos.X - Width / 2);
        SetCurrentValue(TopProperty, mousePos.Y - 50);

        // Ensure window is on screen
        EnsureWindowOnScreen();

        // Style the window
        SetCurrentValue(WindowStyleProperty, WindowStyle.SingleBorderWindow);
        SetCurrentValue(ResizeModeProperty, ResizeMode.CanResize);
        SetCurrentValue(ShowInTaskbarProperty, true);

        // Add dock back button to window
        if (AllowDockBack)
        {
            AddDockBackButton(content);
        }
        else
        {
            SetCurrentValue(ContentProperty, content);
        }
    }

    //private void AddDockBackButton(object? originalContent)
    //{
    //    _windowContentGrid = new();

    //    var dockButton = new Button
    //    {
    //        Content = "Dock Back",
    //        Width = 80,
    //        Height = 25,
    //        HorizontalAlignment = HorizontalAlignment.Right,
    //        VerticalAlignment = VerticalAlignment.Top,
    //        Margin = new(0, 5, 5, 0)
    //    };

    //    dockButton.Click += OnDockBackButtonClick;

    //    // Wrap original content
    //    var contentBorder = new Border
    //    {
    //        Margin = new(5, 35, 5, 5)
    //    };

    //    // Set the content directly to the border without detaching
    //    // since it should already be detached by the calling code
    //    if (originalContent is UIElement element)
    //    {
    //        contentBorder.Child = element;
    //    }
    //    else if (originalContent is not null)
    //    {
    //        // For non-UIElement content, wrap in a ContentPresenter
    //        var presenter = new ContentPresenter
    //        {
    //            Content = originalContent
    //        };
    //        contentBorder.Child = presenter;
    //    }

    //    _windowContentGrid.Children.Add(contentBorder);
    //    _windowContentGrid.Children.Add(dockButton);

    //    SetCurrentValue(ContentProperty, _windowContentGrid);
    //}

    private void AddDockBackButton(object? originalContent)
    {
        _windowContentGrid = new();

        var dockButton = new Button
        {
            Content = "Dock Back",
            Width = 80,
            Height = 25,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new(0, 5, 5, 0)
        };

        dockButton.Click += OnDockBackButtonClick;

        // Wrap original content
        var contentBorder = new Border
        {
            Margin = new(5, 35, 5, 5)
        };

        // Handle different types of content properly
        if (originalContent is ContentPresenter contentPresenter)
        {
            // If it's a ContentPresenter, we need to get its actual content
            var actualContent = contentPresenter.Content;

            if (actualContent is UIElement actualElement)
            {
                // Remove the actual content from the ContentPresenter first
                contentPresenter.Content = null;
                contentBorder.Child = actualElement;
            }
            else if (actualContent is not null)
            {
                // For non-UIElement content, create a new ContentPresenter
                var newPresenter = new ContentPresenter
                {
                    Content = actualContent,
                    ContentTemplate = contentPresenter.ContentTemplate,
                    ContentTemplateSelector = contentPresenter.ContentTemplateSelector,
                    ContentStringFormat = contentPresenter.ContentStringFormat
                };
                contentBorder.Child = newPresenter;
            }
            else
            {
                // If ContentPresenter has no content, check if it has a bound content
                // This handles cases where content is databound
                var newPresenter = new ContentPresenter();

                // Copy relevant properties from the original ContentPresenter
                if (contentPresenter.ContentTemplate is not null)
                {
                    newPresenter.ContentTemplate = contentPresenter.ContentTemplate;
                }

                if (contentPresenter.ContentTemplateSelector is not null)
                {
                    newPresenter.ContentTemplateSelector = contentPresenter.ContentTemplateSelector;
                }

                if (contentPresenter.ContentStringFormat is not null)
                {
                    newPresenter.ContentStringFormat = contentPresenter.ContentStringFormat;
                }

                // Try to copy bindings if they exist
                var contentBinding = BindingOperations.GetBinding(contentPresenter, ContentPresenter.ContentProperty);
                if (contentBinding is not null)
                {
                    newPresenter.SetBinding(ContentPresenter.ContentProperty, contentBinding);
                }
                else
                {
                    // As last resort, set the ContentPresenter itself (though this might not display properly)
                    newPresenter.Content = contentPresenter;
                }

                contentBorder.Child = newPresenter;
            }
        }
        else if (originalContent is UIElement element)
        {
            contentBorder.Child = element;
        }
        else if (originalContent is not null)
        {
            // For non-UIElement content, wrap in a ContentPresenter
            var presenter = new ContentPresenter
            {
                Content = originalContent
            };
            contentBorder.Child = presenter;
        }

        _windowContentGrid.Children.Add(contentBorder);
        _windowContentGrid.Children.Add(dockButton);

        SetCurrentValue(ContentProperty, _windowContentGrid);
    }

    private void OnDockBackButtonClick(object sender, RoutedEventArgs e)
    {
        DockBack();
    }

    private void SetupDragAndDrop()
    {
        MouseLeftButtonDown += OnMouseLeftButtonDown;
        MouseMove += OnMouseMove;
        MouseLeftButtonUp += OnMouseLeftButtonUp;
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (AllowDockBack && e.ClickCount == 1)
        {
            DragMove();
        }
        else if (e.ClickCount == 2)
        {
            // Double-click to dock back
            DockBack();
        }
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (AllowDockBack && e.LeftButton == MouseButtonState.Pressed && !_isDockingBack)
        {
            // Check if we're over a valid drop target (TabControl)
            CheckForDockTarget();
        }
    }

    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_isDockingBack)
        {
            return;
        }

        if (AllowDockBack)
        {
            // Check if we should dock back
            var dropTarget = GetDropTarget();
            if (dropTarget is not null)
            {
                DockBack();
            }
        }
    }

    private void CheckForDockTarget()
    {
        var dropTarget = GetDropTarget();
        if (dropTarget is not null)
        {
            // Visual feedback could be added here
            SetCurrentValue(CursorProperty, Cursors.Hand);
        }
        else
        {
            SetCurrentValue(CursorProperty, Cursors.Arrow);
        }
    }

    private TabControl? GetDropTarget()
    {
        var mousePos = GetMousePosition();
        var elementUnderMouse = GetElementUnderMouse(mousePos);

        // Walk up the visual tree to find a TabControl
        var current = elementUnderMouse;
        while (current is not null)
        {
            if (current is TabControl tabControl)
            {
                return tabControl;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }

    private DependencyObject? GetElementUnderMouse(Point screenPoint)
    {
        // Convert screen coordinates to find element
        foreach (Window window in Application.Current.Windows)
        {
            if (window == this)
            {
                continue;
            }

            try
            {
                var windowPoint = window.PointFromScreen(screenPoint);
                if (windowPoint.X >= 0 && windowPoint.Y >= 0 &&
                    windowPoint.X <= window.ActualWidth && windowPoint.Y <= window.ActualHeight)
                {
                    return window.InputHitTest(windowPoint) as DependencyObject;
                }
            }
            catch
            {
                // Ignore hit test failures
            }
        }

        return null;
    }

    private Point GetMousePosition()
    {
        var point = Mouse.GetPosition(Application.Current.MainWindow);
        return Application.Current.MainWindow?.PointToScreen(point) ?? new Point(0, 0);
    }

    private void EnsureWindowOnScreen()
    {
        var screen = Screen.FromPoint(
            new((int)Left, (int)Top));

        if (Left < screen.WorkingArea.Left)
        {
            SetCurrentValue(LeftProperty, (double)screen.WorkingArea.Left);
        }

        if (Top < screen.WorkingArea.Top)
        {
            SetCurrentValue(TopProperty, (double)screen.WorkingArea.Top);
        }

        if (Left + Width > screen.WorkingArea.Right)
        {
            SetCurrentValue(LeftProperty, screen.WorkingArea.Right - Width);
        }

        if (Top + Height > screen.WorkingArea.Bottom)
        {
            SetCurrentValue(TopProperty, screen.WorkingArea.Bottom - Height);
        }
    }

    /// <summary>
    /// Docks this window back to its original tab
    /// </summary>
    public void DockBack()
    {
        if (_isDockingBack)
        {
            return;
        }

        _isDockingBack = true;
        WasDockedBack = true;

        // Get the content back from the wrapper
        var contentToRestore = GetDetachedContent();

        _originalTabItem.DockBack(contentToRestore);
        Close();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (!WasDockedBack && AllowDockBack)
        {
            var result = MessageBox.Show(
                "Do you want to dock this tab back to the main window?",
                "Close Tab",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    DockBack();
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }

        base.OnClosing(e);
    }
}
