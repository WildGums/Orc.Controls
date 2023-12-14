namespace Orc.Controls;

/// <summary>
/// Available <see cref="LinkLabel"/> clickevent behaviors.
/// </summary>
public enum LinkLabelClickBehavior
{
    /// <summary>
    /// No explicit behavior defined, will use the set-click-event.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Opens the set url in the systems webbrowser.
    /// </summary>
    OpenUrlInBrowser = 1
}
