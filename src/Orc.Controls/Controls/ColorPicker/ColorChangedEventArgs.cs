namespace Orc.Controls;

using System;
using System.Windows.Media;

/// <summary>
/// The color changed event args.
/// </summary>
public class ColorChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorChangedEventArgs" /> class.
    /// </summary>
    /// <param name="newColor">The new color.</param>
    /// <param name="oldColor">The old color.</param>
    public ColorChangedEventArgs(Color newColor, Color oldColor)
    {
        NewColor = newColor;
        OldColor = oldColor;
    }

    /// <summary>
    /// Gets or sets the new color.
    /// </summary>
    public Color NewColor { get; }

    /// <summary>
    /// Gets or sets the old color.
    /// </summary>
    public Color OldColor { get; }
}
