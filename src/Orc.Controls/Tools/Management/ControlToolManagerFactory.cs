namespace Orc.Controls.Tools;

using System;
using System.Collections.Generic;
using System.Windows;
using Catel.IoC;

public class ControlToolManagerFactory : IControlToolManagerFactory
{
    private readonly Dictionary<FrameworkElement, IControlToolManager> _controlToolManagers = new();

    private readonly ITypeFactory _typeFactory;

    public ControlToolManagerFactory(ITypeFactory typeFactory)
    {
        ArgumentNullException.ThrowIfNull(typeFactory);

        _typeFactory = typeFactory;
    }

    public IControlToolManager GetOrCreateManager(FrameworkElement frameworkElement)
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);

        if (!_controlToolManagers.TryGetValue(frameworkElement, out var manager))
        {
            manager = _typeFactory.CreateRequiredInstanceWithParametersAndAutoCompletion<ControlToolManager>(frameworkElement);
            frameworkElement.Unloaded += OnFrameworkElementUnloaded;
        }

        _controlToolManagers[frameworkElement] = manager;
        return manager;
    }

    private void OnFrameworkElementUnloaded(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement frameworkElement)
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
}
