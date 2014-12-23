// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartHelper.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows.Controls;

    public static class DateTimePartHelper
    {
        public static void CreateCombobox(Grid grid, DateTimePart dateTimePart)
        {
            var comboBox = new ComboBox();
            comboBox.ItemsSource = dateTimePart.GetComboboxItemSource();

            comboBox.Height = 22;
            comboBox.Name = "comboBox";

            Grid.SetRow(comboBox, 1);
            grid.Children.Add(comboBox);
        }
    }
}