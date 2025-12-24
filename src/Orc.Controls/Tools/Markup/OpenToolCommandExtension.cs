namespace Orc.Controls;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Catel.MVVM;
using Catel.Windows;
using Catel.Windows.Markup;

public class OpenToolCommandExtension : UpdatableMarkupExtension
{
    private readonly Type _frameworkElementType;
    private readonly Type _toolType;
    protected TaskCommand<object> Command { get; }

    public OpenToolCommandExtension(Type toolType, Type frameworkElementType)
    {
        ArgumentNullException.ThrowIfNull(toolType);
        ArgumentNullException.ThrowIfNull(frameworkElementType);

        _toolType = toolType;
        _frameworkElementType = frameworkElementType;
        Command = new TaskCommand<object>(ServiceProvider, OnOpenToolAsync, CanExecute);
    }

    protected override object ProvideDynamicValue(IServiceProvider? serviceProvider)
    {
        Command.RaiseCanExecuteChanged();
        return Command;
    }

    private bool CanExecute(object? parameter)
    {
        var attachmentTarget = GetAttachmentTarget();
        if (attachmentTarget is null)
        {
            return false;
        }

        var tool = attachmentTarget.GetTools().FirstOrDefault(x => x.GetType() == _toolType);
        return attachmentTarget.CanAttach(_toolType) || tool?.IsEnabled == true;
    }

    private async Task OnOpenToolAsync(object? parameter)
    {
        var tool = GetAttachmentTarget(parameter);
        if (tool is null)
        {
            return;
        }

        await tool.AttachAndOpenToolAsync(_toolType, parameter);
    }

    protected virtual FrameworkElement? GetAttachmentTarget(object? parameter = null)
    {
        if (TargetObject is not FrameworkElement targetObject)
        {
            return null;
        }

        if (targetObject is ICommandSource commandSource)
        {
            if (commandSource.CommandTarget is FrameworkElement commandTarget
                && commandTarget.GetType() == _frameworkElementType)
            {
                return commandTarget;
            }
        }
        
        var contextMenu = targetObject.FindLogicalAncestorByType<ContextMenu>()
                          ?? targetObject.FindLogicalOrVisualAncestor(x => x.GetType() == typeof(ContextMenu)) as ContextMenu;

        if (contextMenu?.PlacementTarget is not FrameworkElement placementTarget)
        {
            return null;
        }

        if (placementTarget.GetType() == _frameworkElementType)
        {
            return placementTarget;
        }

        return placementTarget.FindLogicalOrVisualAncestor(x => x.GetType() == _frameworkElementType) as FrameworkElement;
    }
}
