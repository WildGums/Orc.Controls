namespace Orc.Controls.Tests
{
    using System;
    using System.Threading;
    using NUnit.Framework;

    public partial class ColorLegendTestFacts
    {
        private bool _isSelectedItemsEventRaised;

        [TestCase()]
        public void CorrectlySelectItem()
        {
            var map = TargetMap;

            _isSelectedItemsEventRaised = false;

            Target.SelectionChanged += OnColorLegendSelectionChanged;

            const int itemIndex = 2;
            Assert.That(map.Items[itemIndex].TrySelect());

            Thread.Sleep(200);

            Assert.IsTrue(_isSelectedItemsEventRaised);
        }

        private void OnColorLegendSelectionChanged(object sender, EventArgs e)
        {
            _isSelectedItemsEventRaised = true;

            Target.SelectionChanged -= OnColorLegendSelectionChanged;
        }
    }
}
