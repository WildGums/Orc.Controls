namespace Orc.Controls;

using System;
using System.Runtime.Serialization;
using System.Windows.Media;
using Catel.Data;

/// <summary>
/// The color legend item.
/// </summary>
[Serializable]
[KnownType(typeof(Color))]
public class ColorLegendItem : ModelBase, IColorLegendItem
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the property value.
    /// </summary>
    public bool IsChecked { get; set; }

    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets the property value.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the additional data.
    /// </summary>
    public string? AdditionalData { get; set; }
}
