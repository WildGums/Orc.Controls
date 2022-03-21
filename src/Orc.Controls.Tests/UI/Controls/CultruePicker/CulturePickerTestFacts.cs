namespace Orc.Controls.Tests
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(CulturePicker))]
    [Category("UI Tests")]
    public partial class CulturePickerTestFacts : StyledControlTestFacts<CulturePicker>
    {
        [Target]
        public Automation.CulturePicker Target { get; set; }

        [Test]
        public void VerifyInitialState()
        {
            var target = Target;
            var model = target.Current;

            Assert.That(model.SelectedCulture?.Name, Is.EqualTo(target.SelectedCulture));

            var availableCultures = model.AvailableCultures;
            var cultureItems = target.Items;

            CollectionAssert.AreEquivalent(cultureItems, availableCultures.Select(x => x.Name));
        }

        private class CultureCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .Where(culture => !string.IsNullOrEmpty(culture.Name) && !string.IsNullOrEmpty(culture.Parent.Name))
                    .OrderBy(culture => culture.DisplayName).ToList();

                for (var i = 0; i < cultureInfos.Count; i+= 50)
                {
                    var cultureInfo = cultureInfos[i];
                    var cultureName = cultureInfo.EnglishName;

                    yield return new object[] { cultureInfo, cultureName };
                }
            }
        }

        [TestCaseSource(typeof(CultureCases))]
        public void CorrectlySelectCulture(CultureInfo cultureInfo, string cultureName)
        {
            var target = Target;
            var model = target.Current;

            target.SelectedCulture = cultureName;

            ConnectedPropertiesAssert.VerifyConnectedProperties(model, nameof(model.SelectedCulture),
                target, nameof(target.SelectedCulture), false, new ValueTuple<object, object>(cultureInfo, cultureName));
        }
    }
}
