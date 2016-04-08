// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorLegendItem.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows.Media;
    using System.Xml.Serialization;
    using Catel.Data;
    using Catel.Runtime.Serialization;

    /// <summary>
    /// The color legend item.
    /// </summary>
    [SerializerModifier(typeof(ColorLegendItemSerializerModifier))]
    public class ColorLegendItem : ModelBase, IColorLegendItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

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
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the additional data.
        /// </summary>
        public string AdditionalData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this model should behave as a lean and mean model.
        /// <para />
        /// A lean and mean model will not handle any validation code, nor will it raise any change notification events.
        /// </summary>
        /// <value><c>true</c> if this is a lean and mean model; otherwise, <c>false</c>.</value>
        public bool Silent
        {
            get { return LeanAndMeanModel; }
            set { LeanAndMeanModel = value; }
        }
    }
}