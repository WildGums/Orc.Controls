// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartHelper.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Helpers
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    public static class DateTimePartHelper
    {
        public static void CreateCombobox(Grid grid, DateTimePart dateTimePart)
        {
            var comboBox = new ComboBox();
            comboBox.ItemsSource = GetComboboxItemSource(dateTimePart);

            comboBox.Height = 22;
            comboBox.Name = "comboBox";

            Grid.SetRow(comboBox, 1);
            grid.Children.Add(comboBox);

        }

        public static ObservableCollection<string> GetComboboxItemSource(DateTimePart dateTimePart)
        {
            switch (dateTimePart)
            {
                case DateTimePart.Day:
                    return new ObservableCollection<string> { "1", "2" };
                case DateTimePart.Month:
                    return new ObservableCollection<string> { "3", "4" };
                case DateTimePart.Year:
                    return new ObservableCollection<string> { "5", "6" };
                case DateTimePart.Hour:
                    return new ObservableCollection<string> { "7", "8" };
                case DateTimePart.Minute:
                    return new ObservableCollection<string> { "9", "10" };
                case DateTimePart.Second:
                    return new ObservableCollection<string> { "11", "12" };
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}