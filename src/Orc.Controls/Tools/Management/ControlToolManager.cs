// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlToolManager.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Catel;
    using Catel.IoC;

    public class ControlToolManager : IControlToolManager
    {
        #region Fields
        private readonly FrameworkElement _frameworkElement;
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public ControlToolManager(FrameworkElement frameworkElement, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => typeFactory);

            _frameworkElement = frameworkElement;
            _typeFactory = typeFactory;
        }
        #endregion

        #region Properties
        public IList<IControlTool> Tools { get; } = new List<IControlTool>();
        #endregion

        #region IControlToolManager Members
        public bool CanAttachTool(Type toolType)
        {
            if (toolType is null)
            {
                return false;
            }

            var tools = Tools;
            var existingTool = tools.FirstOrDefault(x => x.GetType() == toolType);
            if (existingTool != null)
            {
                return false;
            }

            if (!(_typeFactory.CreateInstanceWithParametersAndAutoCompletion(toolType) is IControlTool tool))
            {
                return false;
            }

            return tool.CanAttach(_frameworkElement);
        }

        public object AttachTool(Type toolType)
        {
            Argument.IsNotNull(() => toolType);

            var tools = Tools;
            var existingTool = tools.FirstOrDefault(x => x.GetType() == toolType);
            if (existingTool != null)
            {
                return existingTool;
            }

            if (!(_typeFactory.CreateInstanceWithParametersAndAutoCompletion(toolType) is IControlTool tool))
            {
                return null;
            }

            if (!tools.Any())
            {
                _frameworkElement.Unloaded += OnFrameworkElementUnloaded;
            }

            tools.Add(tool);
            tool.Attach(_frameworkElement);

            tool.Opening += OnToolOpening;
            tool.Opened += OnToolClosed;
            tool.Closed += OnToolClosed;

            ToolAttached?.Invoke(this, new ToolManagementEventArgs(tool));

            return tool;
        }

        public bool DetachTool(Type toolType)
        {
            var tools = Tools;
            var tool = tools.FirstOrDefault(x => x.GetType() == toolType);
            if (tool == null)
            {
                return false;
            }

            tool.Opening -= OnToolOpening;
            tool.Opened -= OnToolClosed;
            tool.Closed -= OnToolClosed;

            tool.Close();
            tool.Detach();

            tools.Remove(tool);

            ToolDetached?.Invoke(this, new ToolManagementEventArgs(tool));

            return true;
        }
        #endregion

        #region Methods
        private void OnToolClosed(object sender, EventArgs e)
        {
            ToolClosed?.Invoke(this, new ToolManagementEventArgs(sender as IControlTool));
        }

        private void OnToolOpening(object sender, EventArgs e)
        {
            ToolOpening?.Invoke(this, new ToolManagementEventArgs(sender as IControlTool));
        }

        private void OnToolOpened(object sender, EventArgs e)
        {
            ToolOpened?.Invoke(this, new ToolManagementEventArgs(sender as IControlTool));
        }

        private void OnFrameworkElementUnloaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement frameworkElement))
            {
                return;
            }

            foreach (var tool in Tools)
            {
                tool.Opening -= OnToolOpening;
                tool.Opened -= OnToolClosed;
                tool.Closed -= OnToolClosed;

                tool.Close();
                tool.Detach();
            }

            frameworkElement.Unloaded -= OnFrameworkElementUnloaded;
        }
        #endregion

        public event EventHandler<ToolManagementEventArgs> ToolAttached;
        public event EventHandler<ToolManagementEventArgs> ToolDetached;
        public event EventHandler<ToolManagementEventArgs> ToolOpening;
        public event EventHandler<ToolManagementEventArgs> ToolOpened;
        public event EventHandler<ToolManagementEventArgs> ToolClosed;
    }
}
