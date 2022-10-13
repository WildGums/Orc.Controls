namespace Orc.Controls
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM;
    using Catel.Windows;
    using Catel.Windows.Markup;

    public class OpenToolCommandExtension : UpdatableMarkupExtension
    {
        private readonly Type _frameworkElementType;
        private readonly Type _toolType;
        protected Command<object> Command { get; }

        public OpenToolCommandExtension(Type toolType, Type frameworkElementType)
        {
            ArgumentNullException.ThrowIfNull(toolType);
            ArgumentNullException.ThrowIfNull(frameworkElementType);

            _toolType = toolType;
            _frameworkElementType = frameworkElementType;
            Command = new Command<object>(OnOpenTool, CanExecute);
        }

        protected override object ProvideDynamicValue(IServiceProvider serviceProvider)
        {
            Command.RaiseCanExecuteChanged();
            return Command;
        }

        private bool CanExecute(object parameter)
        {
            var attachmentTarget = GetAttachmentTarget();
            if (attachmentTarget is null)
            {
                return false;
            }

            var tool = attachmentTarget.GetTools().FirstOrDefault(x => x.GetType() == _toolType);
            if (tool is not null)
            {
                return tool.IsEnabled;
            }

            return attachmentTarget.CanAttach(_toolType);
        }

        private void OnOpenTool(object parameter)
        {
            GetAttachmentTarget(parameter)?.AttachAndOpenTool(_toolType, parameter);
        }

        protected virtual FrameworkElement GetAttachmentTarget(object? parameter = null)
        {
            if (TargetObject is not FrameworkElement targetObject)
            {
                return null;
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
}
