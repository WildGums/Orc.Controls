// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredefinedColorItem.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// The predefined color item.
    /// </summary>
    public class PredefinedColorItem : Control
    {
        #region Public Methods and Operators
        /// <summary>
        /// The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        #endregion

        #region Static Fields
        /// <summary>
        /// The color property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.RegisterAttached(
            "Color", typeof (Color), typeof (PredefinedColorItem), new PropertyMetadata(Colors.White));

        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text", typeof (string), typeof (PredefinedColorItem), new PropertyMetadata(string.Empty));
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColorItem"/> class.
        /// </summary>
        public PredefinedColorItem()
        {
            this.DefaultStyleKey = typeof (PredefinedColorItem);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColorItem"/> class.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        public PredefinedColorItem(Color color, string text)
            : this()
        {
            this.Color = color;
            this.Text = text;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color
        {
            get { return (Color) this.GetValue(ColorProperty); }

            set { this.SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return (string) this.GetValue(TextProperty); }

            set { this.SetValue(TextProperty, value); }
        }
        #endregion
    }
}