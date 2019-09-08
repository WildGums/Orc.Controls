// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlToolManagerFactory.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Tools
{
    using System.Collections.Generic;
    using System.Windows;
    using Catel;
    using Catel.IoC;

    public class ControlToolManagerFactory : IControlToolManagerFactory
    {
        #region Fields
        private readonly Dictionary<FrameworkElement, IControlToolManager> _controlToolManagers
            = new Dictionary<FrameworkElement, IControlToolManager>();

        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public ControlToolManagerFactory(ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => typeFactory);

            _typeFactory = typeFactory;
        }
        #endregion

        #region IControlToolManagerFactory Members
        public IControlToolManager GetOrCreateManager(FrameworkElement frameworkElement)
        {
            Argument.IsNotNull(() => frameworkElement);

            if (!_controlToolManagers.TryGetValue(frameworkElement, out var manager))
            {
                manager = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<ControlToolManager>(frameworkElement);
                frameworkElement.Unloaded += OnFrameworkElementUnloaded;
            }

            _controlToolManagers[frameworkElement] = manager;
            return manager;
        }
        #endregion

        #region Methods
        private void OnFrameworkElementUnloaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement frameworkElement))
            {
                return;
            }

            if (!_controlToolManagers.ContainsKey(frameworkElement))
            {
                return;
            }

            _controlToolManagers.Remove(frameworkElement);
            frameworkElement.Unloaded -= OnFrameworkElementUnloaded;
        }
        #endregion
    }
}
