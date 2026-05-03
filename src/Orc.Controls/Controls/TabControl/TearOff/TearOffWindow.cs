namespace Orc.Controls;

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using Catel.IoC;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;

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

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
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
    /// Gets whether this window was closed due to docking back
    /// </summary>
    public bool WasDockedBack { get; set; }

    public bool IsClosing { get; private set; }

    public object? GetDetachedContent()
    {
        // Return the original content that we stored
        if (!(_windowContentGrid?.Children.Count > 0))
        {
            return _originalContent;
        }

        if (_windowContentGrid.Children[0] is not Border contentBorder)
        {
            return _originalContent;
        }

        var childContent = contentBorder.Child;
        if (childContent is null)
        {
            return _originalContent;
        }

        // Important: Remove from current parent before returning
        contentBorder.Child = null;

        if (_originalContent is ContentPresenter contentPresenter)
        {
            contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, childContent);
        }

        // Fallback to original content
        return _originalContent;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var appDataService = IoCContainer.ServiceProvider.GetRequiredService<IAppDataService>();
        appDataService.LoadWindowSize(this, _originalTabItem.Header?.ToString(), true);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        var appDataService = IoCContainer.ServiceProvider.GetRequiredService<IAppDataService>();
        appDataService.SaveWindowSize(this, _originalTabItem.Header?.ToString());
    }

    private void InitializeWindow(object? content, string title)
    {
        SetCurrentValue(TitleProperty, title);
        _originalContent = content;

        WindowStartupLocation = WindowStartupLocation.Manual;

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

    private void AddDockBackButton(object? originalContent)
    {
        _windowContentGrid = new();

        // Wrap original content
        var contentBorder = new Border();

        switch (originalContent)
        {
            // Handle different types of content properly
            case ContentPresenter contentPresenter:
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

                    break;
                }
            case UIElement element:
                contentBorder.Child = element;
                break;
            default:
                {
                    if (originalContent is not null)
                    {
                        // For non-UIElement content, wrap in a ContentPresenter
                        var presenter = new ContentPresenter
                        {
                            Content = originalContent
                        };
                        contentBorder.Child = presenter;
                    }

                    break;
                }
        }

        _windowContentGrid.Children.Add(contentBorder);

        SetCurrentValue(ContentProperty, _windowContentGrid);
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

        if (!IsClosing)
        {
            Close();
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        IsClosing = true;

        if (!WasDockedBack && AllowDockBack)
        {
            DockBack();
        }

        base.OnClosing(e);
    }
}
