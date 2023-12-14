namespace Orc.Controls.Tests.UI
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
            var model = target.Current;

            //USER:     Input search string
            model.Filter = searchString;

            // Checking control API

            //ItemSource collection shouldn't change
            Assert.That(OriginalItemSource, Is.EqualTo(model.ItemsSource).Using(new TestColorItemComparer()));

            //Filtered Items and their ids should be filtered
            Assert.That(expectedFilteredItems, Is.EqualTo(model.FilteredItemsSource).Using(new TestColorItemComparer()));
            Assert.That(expectedFilteredIds, Is.EqualTo(model.FilteredItemsIds).AsCollection);

//Checking visual state

            //List part items should have expected items
            var allItemDescriptions = target.Items.Select(x => x.Description).ToList();

            Assert.That(expectedFilteredItems.Select(x => x.Description), Is.EqualTo(allItemDescriptions).AsCollection);
        }
    }
}
