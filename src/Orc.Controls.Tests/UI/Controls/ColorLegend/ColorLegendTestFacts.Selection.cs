namespace Orc.Controls.Tests
{
    using System;
    using System.Threading;
    using NUnit.Framework;

    public partial class ColorLegendTestFacts
    {
        private bool _isSelectedItemsEventRaised;

        [Test]
        public void CorrectlySelectItem()
        {
            var view = View;

            _isSelectedItemsEventRaised = false;

            Target.SelectionChanged += OnColorLegendSelectionChanged;

            const int itemIndex = 2;
            view.Items[itemIndex].Select();

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
