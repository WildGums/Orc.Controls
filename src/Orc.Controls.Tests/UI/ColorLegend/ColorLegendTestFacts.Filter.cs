namespace Orc.Controls.Tests
{
    using System.Collections;
    using System.Linq;
    using NUnit.Framework;

    public partial class ColorLegendTestFacts
    {
        private class FilterItemsTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[] { "1", new[] { OriginalItemSource[0], OriginalItemSource[9] }, new[] { "1", "10" } };
                yield return new object[] { "/\"Item \\d\"/", OriginalItemSource.TakeLast(1).ToArray(), Enumerable.Range(1, 9).Select(x => x.ToString()).ToArray() };
            }
        }

        [TestCaseSource(typeof(FilterItemsTestCases))]
        public void CorrectlyFilterItems(string searchString, IColorLegendItem[] expectedFilteredItems, string[] expectedFilteredIds)
        {
            var target = Target;
            var view = View;

            //USER:     Input search string
            view.SetFilter(searchString);

// Checking control API

            //ItemSource collection shouldn't change
            CollectionAssert.AreEqual(target.ItemsSource, OriginalItemSource, new TestColorItemComparer());

            //Filtered Items and their ids should be filtered
            CollectionAssert.AreEqual(target.FilteredItemsSource, expectedFilteredItems, new TestColorItemComparer());
            CollectionAssert.AreEqual(target.FilteredItemsIds, expectedFilteredIds);

//Checking visual state

            //List part items should have expected items
            var allItemDescriptions = view.Items.Select(x => x.Description).ToList();

            CollectionAssert.AreEqual(allItemDescriptions, expectedFilteredItems.Select(x => x.Description));
        }
    }
}
