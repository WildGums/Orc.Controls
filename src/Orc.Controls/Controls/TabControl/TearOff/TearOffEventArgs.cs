namespace Orc.Controls;

using System.Windows;

/// <summary>
/// Event arguments for tear-off events
/// </summary>
public class TearOffEventArgs : RoutedEventArgs
{
    public TearOffEventArgs(RoutedEvent routedEvent, TearOffTabItem tabItem) 
        : base(routedEvent)
    {
        TabItem = tabItem;
    }

    /// <summary>
    /// The tab item that was torn off or docked
    /// </summary>
    public TearOffTabItem TabItem { get; }
}
