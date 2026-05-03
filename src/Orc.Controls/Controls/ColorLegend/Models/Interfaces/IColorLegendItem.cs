namespace Orc.Controls;

using System.ComponentModel;
using System.Windows.Media;

/// <summary>
/// The ColorProvider interface.
/// </summary>
public interface IColorLegendItem : INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets a value indicating whether color is visible or not
    /// </summary>
    bool IsChecked { get; set; }

    bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets the color.
    /// </summary>
    Color Color { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    /// Gets or sets the additional data.
    /// </summary>
    string? AdditionalData { get; set; }

    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    string? Id { get; set; }
}
