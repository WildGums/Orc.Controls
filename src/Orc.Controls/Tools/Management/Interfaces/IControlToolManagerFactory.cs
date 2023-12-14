namespace Orc.Controls.Tools;

using System.Windows;

public interface IControlToolManagerFactory
{
    IControlToolManager GetOrCreateManager(FrameworkElement frameworkElement);
}