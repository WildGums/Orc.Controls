// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindableRun.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



#if NET

namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Documents;

    /// <summary>
    /// Bindable run class.
    /// </summary>
    public class BindableRun : Run
    {
        #region Methods
        /// <summary>
        /// Invoked when the BoundText dependency property has changed.
        /// </summary>
        /// <param name="sender">The object that contains the dependency property.</param>
        /// <param name="e">The event data.</param>
        private static void OnBoundTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var typedSender = sender as BindableRun;
            typedSender?.SetCurrentValue(TextProperty, e.NewValue as string);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Wrapper for the BoundText dependency property.
        /// </summary>
        public string BoundText
        {
            get { return (string) GetValue(BoundTextProperty); }
            set { SetValue(BoundTextProperty, value); }
        }

        /// <summary>
        /// DependencyProperty definition as the backing store for BoundText
        /// </summary>
        public static readonly DependencyProperty BoundTextProperty = DependencyProperty.Register(nameof(BoundText), typeof (string),
            typeof (BindableRun), new UIPropertyMetadata(string.Empty, OnBoundTextChanged));
        #endregion
    }
}
#endif
