// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderBar.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class HeaderBar : Control
    {
        #region Constructors
        public HeaderBar()
        {
            DefaultStyleKey = typeof(HeaderBar);
        }
        #endregion

        #region Properties
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header),
            typeof(string), typeof(HeaderBar), new PropertyMetadata(string.Empty));
        #endregion
    }
}
