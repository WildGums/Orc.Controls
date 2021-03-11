// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkElementExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Microsoft.Xaml.Behaviors;
    using Orc.Controls.Controls.StepBar.Models;
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
        public static IEditableControl TryGetEditableControl(this FrameworkElement frameworkElement)
        {
            Argument.IsNotNull(() => frameworkElement);

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
            Argument.IsNotNull(() => frameworkElement);
            Argument.IsNotNull(() => root);

            var lowerThumbRelativePoint = frameworkElement.TransformToAncestor(root)
                .Transform(new Point(0, 0));

            return new Point(lowerThumbRelativePoint.X + frameworkElement.ActualWidth / 2, lowerThumbRelativePoint.Y + frameworkElement.ActualHeight / 2);
        }

        public static IControlToolManager GetControlToolManager(this FrameworkElement frameworkElement)
        {
            Argument.IsNotNull(() => frameworkElement);

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
            Argument.IsNotNull(() => frameworkElement);
            Argument.IsNotNull(() => toolType);

            var tool = frameworkElement.AttachTool(toolType) as IControlTool;
            tool?.Open(parameter);
        }

        public static void AttachAndOpenTool<T>(this FrameworkElement frameworkElement, object parameter = null)
            where T : class, IControlTool
        {
            Argument.IsNotNull(() => frameworkElement);

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
            Argument.IsNotNull(() => frameworkElement);

            return frameworkElement.AttachTool(typeof(T)) as T;
        }

        public static bool DetachTool(this FrameworkElement frameworkElement, Type toolType)
        {
            var controlToolManager = frameworkElement.GetControlToolManager();
            return controlToolManager.DetachTool(toolType);
        }

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static SolidColorBrush GetAccentColorBrush(this FrameworkElement frameworkElement, bool isSelected = true)
        {
            Argument.IsNotNull(() => frameworkElement);

            var resourceName = isSelected ? ThemingKeys.AccentColorBrush : ThemingKeys.AccentColorBrush40;

            var brush = frameworkElement.TryFindResource(resourceName) as SolidColorBrush;
            if (brush is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Theming is not yet initialized, make sure to initialize a theme via ThemeManager first");
            }

            return brush;
        }
        #endregion
    }
}
