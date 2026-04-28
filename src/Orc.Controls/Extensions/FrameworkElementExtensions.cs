namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Catel.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;
using Tools;

public static class FrameworkElementExtensions
{
    private static readonly IControlToolManagerFactory ControlToolManagerFactory;

    private static readonly Point ZeroPoint = new(0, 0);

    static FrameworkElementExtensions()
    {
        ControlToolManagerFactory = IoCContainer.ServiceProvider.GetRequiredService<IControlToolManagerFactory>();
    }

    public static Rect GetScreenRect(this FrameworkElement frameworkElement)
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);

        return frameworkElement.IsVisible 
            ? new Rect(frameworkElement.PointToScreen(ZeroPoint), 
                frameworkElement.PointToScreen(new Point(frameworkElement.ActualWidth, frameworkElement.ActualHeight))) 
            : Rect.Empty;
    }

    public static IEditableControl? TryGetEditableControl(this FrameworkElement frameworkElement)
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);

        if (frameworkElement is IEditableControl editableControl)
        {
            return editableControl;
        }

        var behaviors = Interaction.GetBehaviors(frameworkElement);
        return behaviors.OfType<IEditableControl>().FirstOrDefault();
    }

    public static Point GetCenterPointInRoot(this FrameworkElement frameworkElement, FrameworkElement root)
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);
        ArgumentNullException.ThrowIfNull(root);

        var lowerThumbRelativePoint = frameworkElement.TransformToAncestor(root)
            .Transform(new Point(0, 0));

        return new Point(lowerThumbRelativePoint.X + frameworkElement.ActualWidth / 2, lowerThumbRelativePoint.Y + frameworkElement.ActualHeight / 2);
    }

    public static IControlToolManager GetControlToolManager(this FrameworkElement frameworkElement)
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);

        return ControlToolManagerFactory.GetOrCreateManager(frameworkElement);
    }

    public static IList<IControlTool> GetTools(this FrameworkElement frameworkElement)
    {
        return frameworkElement.GetControlToolManager().Tools;
    }

    public static TControlTool? GetTool<TControlTool>(this FrameworkElement frameworkElement)
        where TControlTool : IControlTool
    {
        return frameworkElement.GetControlToolManager().Tools.OfType<TControlTool>().FirstOrDefault();
    }

    public static bool CanAttach(this FrameworkElement frameworkElement, Type toolType)
    {
        var controlToolManager = frameworkElement.GetControlToolManager();
        return controlToolManager.CanAttachTool(toolType);
    }

    public static async Task AttachAndOpenToolAsync(this FrameworkElement frameworkElement, Type toolType, object? parameter = null)
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);
        ArgumentNullException.ThrowIfNull(toolType);

        if ((await frameworkElement.AttachToolAsync(toolType)) is not IControlTool tool)
        {
            return;
        }

        await tool.OpenAsync(parameter);
    }

    public static async Task AttachAndOpenToolAsync<T>(this FrameworkElement frameworkElement, object? parameter = null)
        where T : class, IControlTool
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);

        var tool = await frameworkElement.AttachToolAsync<T>();
        if (tool is null)
        {
            return;
        }

        await tool.OpenAsync(parameter);
    }

    public static Task<object?> AttachToolAsync(this FrameworkElement frameworkElement, Type toolType)
    {
        var controlToolManager = frameworkElement.GetControlToolManager();
        return controlToolManager.AttachToolAsync(toolType);
    }

    public static async Task<T?> AttachToolAsync<T>(this FrameworkElement frameworkElement)
        where T : class, IControlTool
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);

        return (await frameworkElement.AttachToolAsync(typeof(T))) as T;
    }

    public static Task<bool> DetachToolAsync(this FrameworkElement frameworkElement, Type toolType)
    {
        var controlToolManager = frameworkElement.GetControlToolManager();
        return controlToolManager.DetachToolAsync(toolType);
    }
}
