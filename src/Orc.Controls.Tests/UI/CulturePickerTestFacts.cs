namespace Orc.Controls.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture]
    public class CulturePickerTestFacts : ControlUiTestFactsBase<CulturePicker>
    {
        [Target]
        public AutomationElement Target { get; set; }

        [Test]
        public void CorrectlyInitializeAvailableCultures()
        {
            GetRandomCultureInfo();
        }

        [Test]
        public void CorrectlySelectCulture()
        {
            var target = Target;

            Assert.That(target.TryExpand());

            var cultureInfo = GetRandomCultureInfo();

            Assert.That(target.TrySelectItem(cultureInfo.Name, out _));
            Assert.That(target.TryGetPropertyValue(nameof(CulturePicker.SelectedCulture), out _));
        }

        private CultureInfo GetRandomCultureInfo()
        {
            var target = Target;

            Assert.That(target.TryExecute<List<string>>(nameof(CulturePickerAutomationPeer.GetAvailableCultures), out var availableCultures));

            Assert.That(availableCultures, Is.Not.Empty);

            var random = new Random();
            var index = random.Next(0, availableCultures.Count - 1);
            var cultureName = availableCultures[index];

            var culture = CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .FirstOrDefault(x => x.Name == cultureName);

            Assert.That(culture, Is.Not.Null);

            return culture;
        }
    }
}
