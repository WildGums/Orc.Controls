namespace Orc.Controls.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Automation;
    using Automation.Tests;
    using NUnit.Framework;

    [Explicit, TestFixture]
    public class CulturePickerTestFacts : ControlUiTestFactsBase<CulturePicker>
    {
        [Test]
        public void CorrectlyInitializeAvailableCultures()
        {
            GetRandomCultureInfo();
        }

        [Test]
        public void CorrectlySelectCulture()
        {
            var target = TestModel.TargetControl;

            if (!target.TryExpand())
            {
                Assert.Fail("Can't expand Culture picker");
            }

            var cultureInfo = GetRandomCultureInfo();
            if (!target.TrySelectItem(cultureInfo.Name, out var item))
            {
                Assert.Fail($"Can't select culture {cultureInfo.EnglishName}-{cultureInfo.Name}");
            }

            if (!target.TryGetPropertyValue(nameof(CulturePicker.SelectedCulture), out var result))
            {
                Assert.AreEqual(result, cultureInfo.Name);
            }
        }

        private CultureInfo GetRandomCultureInfo()
        {
            var target = TestModel.TargetControl;
            if (!target.TryExecute<List<string>>(nameof(CulturePickerAutomationPeer.GetAvailableCultures), out var availableCultures))
            {
                Assert.Fail("Can't get available cultures");
            }

            Assert.IsNotEmpty(availableCultures);

            var random = new Random();
            var index = random.Next(0, availableCultures.Count - 1);
            var cultureName = availableCultures[index];

            var culture = CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .FirstOrDefault(x => x.Name == cultureName);

            Assert.IsNotNull(culture);

            return culture;
        }
    }
}
