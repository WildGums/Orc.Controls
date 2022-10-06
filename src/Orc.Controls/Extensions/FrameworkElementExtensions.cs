namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Microsoft.Xaml.Behaviors;
    using Tools;

    public static class FrameworkElementExtensions
    {
        #region Constants
        private static readonly IControlToolManagerFactory ControlToolManagerFactory;
        #endregion

        #region Constructors
        static FrameworkElementExtensions()
        {
            ControlToolManagerFactory = ServiceLocator.Default.ResolveType<IControlToolManagerFactory>();
        }
        #endregion

        #region Methods
        public static Rect GetScreenRect(this FrameworkElement frameworkElement)
        {
            ArgumentNullException.ThrowIfNull(frameworkElement);

            if (!frameworkElement.IsVisible)
            {
                return Rect.Empty;
            }

            return new Rect(frameworkElement.PointToScreen(new Point(0, 0)), frameworkElement.PointToScreen(new Point(frameworkElement.ActualWidth, frameworkElement.ActualHeight)));
        }

        public static IEditableControl TryGetEditableControl(this FrameworkElement frameworkElement)
        {
            ArgumentNullException.ThrowIfNull(frameworkElement);

            if (frameworkElement is IEditableControl editableControl)
            {
                return editableControl;
            }

            var behaviors = Interaction.GetBehaviors(frameworkElement);
            editableControl = behaviors.OfType<IEditableControl>().FirstOrDefault();

            return editableControl;
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

        public static bool CanAttach(this FrameworkElement frameworkElement, Type toolType)
        {
            var controlToolManager = frameworkElement.GetControlToolManager();
            return controlToolManager.CanAttachTool(toolType);
        }

        public static void AttachAndOpenTool(this FrameworkElement frameworkElement, Type toolType, object parameter = null)
        {
            ArgumentNullException.ThrowIfNull(frameworkElement);
            ArgumentNullException.ThrowIfNull(toolType);

            var tool = frameworkElement.AttachTool(toolType) as IControlTool;
            tool?.Open(parameter);
        }

        public static void AttachAndOpenTool<T>(this FrameworkElement frameworkElement, object parameter = null)
            where T : class, IControlTool
        {
            ArgumentNullException.ThrowIfNull(frameworkElement);

            frameworkElement?.AttachTool<T>()?.Open(parameter);
        }

        public static object AttachTool(this FrameworkElement frameworkElement, Type toolType)
        {
            var controlToolManager = frameworkElement.GetControlToolManager();
            return controlToolManager.AttachTool(toolType);
        }

        public static T AttachTool<T>(this FrameworkElement frameworkElement)
            where T : class, IControlTool
        {
            ArgumentNullException.ThrowIfNull(frameworkElement);

            return frameworkElement.AttachTool(typeof(T)) as T;
        }

        public static bool DetachTool(this FrameworkElement frameworkElement, Type toolType)
        {
            var controlToolManager = frameworkElement.GetControlToolManager();
            return controlToolManager.DetachTool(toolType);
        }
        #endregion
    }
}
