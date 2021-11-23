namespace Orc.Controls.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    public class CulturePickerMap
    {
        [Target]
        public AutomationElement Target { get; set; }

        public List<AutomationElement> Items
        {
            get
            {
                var target = Target;

                target.TryExpand();

                Thread.Sleep(200);

                var items = target.FindAll(controlType: ControlType.ListItem);

                return items;
            }
        }
    }

    [TestFixture]
    public class CulturePickerTestFacts : ControlUiTestFactsBase<CulturePicker>
    {
        [Target]
        public AutomationElement Target { get; set; }

        [TargetControlMap]
        public CulturePickerMap TargetMap { get; set; }


        [Test]
        public void CorrectlyInitializeItems()
        {
            var target = Target;

            var items = TargetMap.Items;
        }

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

            Assert.That(target.TryExecute<CultureInfo>(nameof(CulturePickerAutomationPeer.GetAvailableCultures), out var culture));

            //Assert.That(availableCultures, Is.Not.Empty);

            //var random = new Random();
            //var index = random.Next(0, availableCultures.Count - 1);
            //var culture = availableCultures[index];

            Assert.That(culture, Is.Not.Null);

            return culture;
        }
    }
}
