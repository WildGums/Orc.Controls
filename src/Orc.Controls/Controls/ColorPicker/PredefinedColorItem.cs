// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredefinedColorItem.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
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
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColorItem"/> class.
        /// </summary>
        public PredefinedColorItem()
        {
            DefaultStyleKey = typeof(PredefinedColorItem);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColorItem" /> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="text">The text.</param>
        public PredefinedColorItem(Color color, string text)
            : this()
        {
            Color = color;
            Text = text;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// The color property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.RegisterAttached(
            nameof(Color), typeof(Color), typeof(PredefinedColorItem), new PropertyMetadata(Colors.White));

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            nameof(Text), typeof(string), typeof(PredefinedColorItem), new PropertyMetadata(string.Empty));
        #endregion
    }
}
