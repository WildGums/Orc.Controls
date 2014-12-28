// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartHelper.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public static class DateTimePartHelper
    {
        #region Methods
        public static void CreatePopup(Grid grid, DateTimePart dateTimePart, NumericTextBox textBox)
        {
            var popup = new Popup();
            var popupSource = new ListBox {ItemsSource = dateTimePart.GetPopupSource()};
            popup.Child = popupSource;

            popup.Name = "comboBox";
            popup.Width = textBox.ActualWidth + 10;
            popup.MaxHeight = 100;
            popup.PlacementTarget = textBox;
            popup.Placement = PlacementMode.Bottom;
            popup.VerticalOffset = 2;
            popup.IsOpen = true;
            popup.Visibility = Visibility.Visible;

            Grid.SetRow(popup, 1);
            grid.Children.Add(popup);
        }
        #endregion
    }
}