namespace Orc.Controls.Tests
{
    using System;
    using System.Linq;
    using Automation;
    using NUnit.Framework;

    public class ColorLegendAssert
    {
        public static void AllCheckedState(ColorLegend target)
        {
            var isAllChecked = target.IsAllVisible;
            var model = target.Current;

            if (isAllChecked is null)
            {
                Assert.That(model.ItemsSource, 
                    Has.Some.Property(nameof(IColorLegendItem.IsChecked)).True
                    .And
                    .Some.Property(nameof(IColorLegendItem.IsChecked)).False);
            }
            
            switch (isAllChecked)
            {
                case true:
                    Assert.That(model.ItemsSource, Has.All.Property(nameof(IColorLegendItem.IsChecked)).True);
                    break;

                case false:
                    Assert.That(model.ItemsSource, Has.All.Property(nameof(IColorLegendItem.IsChecked)).False);
                    break;
            }
        }

        public static void ItemsCheckStateMatch(ColorLegend target)
        {
            var model = target.Current;

            var checkedSourceItems = model.FilteredItemsSource
                .Select(x => x.IsChecked);

            var checkedItems = target.Items
                .Select(x => x.IsChecked);

            CollectionAssert.AreEqual(checkedItems, checkedSourceItems);
        }

        public static void ItemsSelectedStateMatch(ColorLegend target)
        {
            var model = target.Current;

            var selectedSourceItems = (model.SelectedColorItems ?? Array.Empty<IColorLegendItem>())
                .Select(x => x.IsSelected);

            var selectedItems = target.Items.Where(x => x.IsSelected)
                .Select(x => x.IsSelected);

            CollectionAssert.AreEqual(selectedSourceItems, selectedItems);
        }

        public static void ClearSelectionState(ColorLegend target)
        {
            var model = target.Current;

            var countSelectedItems = model.SelectedColorItems?.Count() ?? 0;

            Assert.That(countSelectedItems > 0, Is.EqualTo(target.CanClearSelection));
        }
    }
}
