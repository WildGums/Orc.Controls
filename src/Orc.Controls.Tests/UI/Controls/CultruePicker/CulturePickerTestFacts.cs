namespace Orc.Controls.Tests
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture]
    public partial class CulturePickerTestFacts : ControlUiTestFactsBase<CulturePicker>
    {
        [Target]
        public Automation.CulturePicker Target { get; set; }

        [TargetControlMap]
        public CulturePickerMap TargetMap { get; set; }


        [Test]
        public void VerifyInitialState()
        {
            var target = Target;
            var view = target.View;

            Assert.That(target.SelectedCulture?.Name, Is.EqualTo(view.SelectedCulture));

            var availableCultures = target.AvailableCultures;
            var cultureItems = view.Items;

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
            var view = target.View;

            var background = target.Background;

            view.SelectedCulture = cultureName;

            ConnectedPropertiesAssert.VerifyConnectedProperties(target, nameof(target.SelectedCulture), 
                view, nameof(view.SelectedCulture), false, new ValueTuple<object, object>(cultureInfo, cultureName));
        }
    }
}
