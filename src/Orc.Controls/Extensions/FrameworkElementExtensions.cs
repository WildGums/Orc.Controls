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
    using Catel;
    using Catel.IoC;

    public static class FrameworkElementExtensions
    {
        #region Constants
        private static readonly Dictionary<FrameworkElement, List<IControlTool>> ControlTools
            = new Dictionary<FrameworkElement, List<IControlTool>>();
        #endregion

        #region Methods
        public static IList<IControlTool> GetTools(this FrameworkElement frameworkElement)
        {
            Argument.IsNotNull(() => frameworkElement);

            return ControlTools.TryGetValue(frameworkElement, out var tools) ? tools : new List<IControlTool>();
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
            Argument.IsNotNull(() => frameworkElement);
            Argument.IsNotNull(() => toolType);

            var typeFactory = frameworkElement.GetTypeFactory();
            if (!(typeFactory.CreateInstanceWithParametersAndAutoCompletion(toolType) is IControlTool tool))
            {
                return null;
            }

            if (!ControlTools.TryGetValue(frameworkElement, out var tools))
            {
                tools = new List<IControlTool>();
                ControlTools[frameworkElement] = tools;
            }

            var existingTool = tools.FirstOrDefault(x => x.GetType() == toolType);
            if (existingTool != null)
            {
                return existingTool;
            }

            tools.Add(tool);
            tool.Attach(frameworkElement);

            return tool;
        }

        public static T AttachTool<T>(this FrameworkElement frameworkElement)
            where T : class, IControlTool
        {
            Argument.IsNotNull(() => frameworkElement);

            return frameworkElement.AttachTool(typeof(T)) as T;
        }

        public static bool DetachTool(this FrameworkElement frameworkElement, Type toolType)
        {
            Argument.IsNotNull(() => frameworkElement);

            if (!ControlTools.TryGetValue(frameworkElement, out var tools))
            {
                return false;
            }

            var tool = tools.FirstOrDefault(x => x.GetType() == toolType);
            if (tool == null)
            {
                return false;
            }

            tools.Remove(tool);
            return true;
        }
        #endregion
    }
}
