namespace Orc.Controls.Tools;

using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

public class ControlToolManagerFactory : IControlToolManagerFactory
{
    private readonly Dictionary<FrameworkElement, IControlToolManager> _controlToolManagers = new();

    private readonly IServiceProvider _serviceProvider;

    public ControlToolManagerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IControlToolManager GetOrCreateManager(FrameworkElement frameworkElement)
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);

        if (!_controlToolManagers.TryGetValue(frameworkElement, out var manager))
        {
            manager = ActivatorUtilities.CreateInstance<ControlToolManager>(_serviceProvider, frameworkElement);
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
