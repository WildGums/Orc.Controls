// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IColorProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.ComponentModel;
    using System.Windows.Media;

    /// <summary>
    /// The ColorProvider interface.
    /// </summary>
    public interface IColorProvider : INotifyPropertyChanged
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating whether color is visible or not
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the additional data.
        /// </summary>
        string AdditionalData { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        string Id { get; set; }

        #endregion
    }
}
