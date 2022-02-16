namespace Orc.Controls.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using NUnit.Framework;
    using Orc.Automation;

    [TestFixture(TestOf = typeof(Expander))]
    [Category("UI Tests")]
    public class FilterBoxTestFacts : StyledControlTestFacts<FilterBox>
    {
        [Target]
        public Automation.FilterBox Target { get; set; }
        
        [TestCaseSource(typeof(FilterSourceTestCases))]
        public void CorrectlyBuildSuggestionList(TestFilterSourceList filterSourceList)
        {
            var target = Target;
            var model = target.Current;

            model.FilterSource = filterSourceList;
            model.PropertyName = nameof(TestFilterSource.Prop1);

            var suggestionList = target.OpenSuggestionList();

            CollectionAssert.AreEquivalent(suggestionList, filterSourceList.Select(x => x.Prop1));
        }

        [TestCaseSource(typeof(FilterSourceTestCases))]
        public void CorrectlyFilterSuggestionList(TestFilterSourceList filterSourceList)
        {
            var target = Target;
            var model = target.Current;

            model.FilterSource = filterSourceList;
            model.PropertyName = nameof(TestFilterSource.Prop1);

            const string inputText = "o";

            target.Text = inputText;

            var filteredList = filterSourceList.Where(x => x.Prop1.Contains(inputText))
                .ToList();

            var suggestionList = target.OpenSuggestionList();

            CollectionAssert.AreEquivalent(suggestionList, filteredList.Select(x => x.Prop1));
        }

        [TestCaseSource(typeof(FilterSourceTestCases))]
        public void CorrectlyBuildSuggestionListAfterClearingContents(TestFilterSourceList filterSourceList)
        {
            var target = Target;
            var model = target.Current;

            model.FilterSource = filterSourceList;
            model.PropertyName = nameof(TestFilterSource.Prop1);

            target.Text = "Test value";

            Wait.UntilResponsive();

            target.Clear();

            Wait.UntilResponsive();

            var suggestionList = target.OpenSuggestionList();

            CollectionAssert.AreEquivalent(suggestionList, filterSourceList.Select(x => x.Prop1));
        }

        [Test]
        public void CorrectlyClearContents()
        {
            var target = Target;
            var model = target.Current;

            const string text = "Test value";

            target.Text = text;

            target.Clear();

            Assert.That(target.Text, Is.Empty.Or.Null);
            Assert.That(model.Text, Is.Empty.Or.Null);
        }

        [TestCaseSource(typeof(FilterSourceTestCases))]
        public void CorrectlySelectItemFromSuggestionList(TestFilterSourceList filterSourceList)
        {
            var target = Target;
            var model = target.Current;

            model.FilterSource = filterSourceList;
            model.PropertyName = nameof(TestFilterSource.Prop1);

            var item = filterSourceList[filterSourceList.Count / 2].Prop1;

            target.SelectItemFromSuggestionList(item);

            Assert.That(item, Is.EqualTo(target.Text));
            Assert.That(item, Is.EqualTo(model.Text));
        }

        [TestCase("TestWatermark", "TestWatermark")]
        [TestCase("", "")]
        [TestCase(null, "")]
        public void CorrectlySetWatermark(string watermark, string expectedWatermark)
        {
            var target = Target;
            var model = target.Current;

            model.Watermark = watermark;

            KeyboardInput.PressRelease(Key.Tab);

            Assert.That(target.Watermark, Is.EqualTo(expectedWatermark));
        }

        private class FilterSourceTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new TestFilterSourceList
                {
                    new () { Prop1 = "AbC" },
                    new () { Prop1 = "dEF" },
                    new () { Prop1 = "ghI" },
                    new () { Prop1 = "JKl" },
                    new () { Prop1 = "mno" },
                    new () { Prop1 = "ooo" },
                };
            }
        }
    }

    public class TestFilterSource
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
    }

    public class TestFilterSourceList : List<TestFilterSource>
    {

    }
}
