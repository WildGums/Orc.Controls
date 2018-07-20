// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PinnableItemTemplateSelector.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The pinnable item template selector.
    /// </summary>
    public class PinnableItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;
            if (element != null)
            {
                if (item is OrdinalToolTipItem)
                {
                    return element.FindResource("OrdinalToolTipItemTemplate") as DataTemplate;
                }

                if (item is LinkToolTipItem)
                {
                    return element.FindResource("LinkToolTipItemTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}