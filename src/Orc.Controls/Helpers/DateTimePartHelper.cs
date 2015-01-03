// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartHelper.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using Catel.IoC;
    using Services.Interfaces;

    public static class DateTimePartHelper
    {
        #region Methods
        public static Popup CreatePopup(DateTime dateTime, Grid grid, DateTimePart dateTimePart, NumericTextBox textBox)
        {
            var popup = new Popup();
            var popupSource = CreatePopupSource(dateTime, dateTimePart);
            popup.Child = popupSource;

            popup.Name = "comboBox";
            popup.MinWidth = textBox.ActualWidth + 25;
            popup.MaxHeight = 100;
            popup.PlacementTarget = textBox;
            popup.Placement = PlacementMode.Bottom;
            popup.VerticalOffset = 2;
            popup.IsOpen = true;
            popup.Visibility = Visibility.Visible;

            Grid.SetRow(popup, 1);
            grid.Children.Add(popup);

            return popup;
        }

        private static ListBox CreatePopupSource(DateTime dateTime, DateTimePart dateTimePart)
        {
            var serviceLocator = ServiceLocator.Default;
            var suggestionListService = serviceLocator.ResolveType<ISuggestionListService>();
            var source = suggestionListService.GetSuggestionList(dateTime, dateTimePart);

            return new ListBox(){ItemsSource = source};
        }
        #endregion
    }
}