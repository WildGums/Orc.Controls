// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderBar.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class HeaderBar : Control
    {
        public HeaderBar()
        {
            DefaultStyleKey = typeof(HeaderBar);
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), 
            typeof(HeaderBar), new PropertyMetadata(string.Empty));
    }
}