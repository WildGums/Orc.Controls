namespace Orc.Controls.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Automation;
    using Catel.Linq;
    using NUnit.Framework;

    public class ColorLegendAssert
    {
        public static void AllCheckedState(ColorLegend target)
        {
            var isAllChecked = target.View.IsAllChecked;

            if (isAllChecked is null)
            {
                Assert.That(target.ItemsSource, 
                    Has.Some.Property(nameof(IColorLegendItem.IsChecked)).True
                    .And
                    .Some.Property(nameof(IColorLegendItem.IsChecked)).False);
            }
            
            switch (isAllChecked)
            {
                case true:
                    Assert.That(target.ItemsSource, Has.All.Property(nameof(IColorLegendItem.IsChecked)).True);
                    break;

                case false:
                    Assert.That(target.ItemsSource, Has.All.Property(nameof(IColorLegendItem.IsChecked)).False);
                    break;
            }
        }

        public static void ItemsCheckStateMatch(ColorLegend target)
        {
            var checkedSourceItems = target.FilteredItemsSource
                .Select(x => x.IsChecked);

            var checkedItems = target.View.Items
                .Select(x => x.IsChecked);

            CollectionAssert.AreEqual(checkedItems, checkedSourceItems);
        }

        public static void ItemsSelectedStateMatch(ColorLegend target)
        {
            var selectedSourceItems = (target.SelectedColorItems ?? Array.Empty<IColorLegendItem>())
                .Select(x => x.IsSelected);

            var selectedItems = target.View.Items.Where(x => x.IsSelected)
                .Select(x => x.IsSelected);

            CollectionAssert.AreEqual(selectedSourceItems, selectedItems);
        }

        public static void ClearSelectionState(ColorLegend target)
        {
            var countSelectedItems = target.SelectedColorItems?.Count() ?? 0;

            Assert.That(countSelectedItems > 0, Is.EqualTo(target.View.CanClearSelection));
        }
    }
}
