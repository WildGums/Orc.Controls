namespace Orc.Controls.Tests
{
    using System.Windows.Automation;
    using NUnit.Framework;
    using Orc.Automation;

    public partial class ColorLegendTestFacts
    {
        [TestCase(2)]
        [TestCase(0)]
        [TestCase(9)]
        [Description("IsChecked state of ColorItem should be in sync corresponding checkboxes.")]
        public void CorrectlyUncheckCheckItem(int itemIndex)
        {
            var target = Target;
            var map = TargetMap;

            var items = map.Items;

            var item = items[itemIndex];
            var itemCheckBox = item.CheckBox;

            //Check box should be there
            Assert.That(itemCheckBox, Is.Not.Null);

            //colorLegend.Items[itemIndex].Part<ColorItemMap>(x => x.CheckBox.TryGetToggleState);
            //item = colorLegend.Items[itemIndex]
            //item.IsChecked();
            //item.Description();

            //Should be able to retrieve state of toggle
            Assert.That(itemCheckBox.TryGetToggleState(out var toggleState));
            
            //State of Toggle and state of item should be the same
            var initialItemCheckedState = target.GetItemSourceItem(itemIndex).IsChecked;
            Assert.AreEqual(toggleState, initialItemCheckedState);

//USER:     Toggle checkbox
            Assert.That(itemCheckBox.TryToggle(out toggleState));

            //State of Toggle and state of item should be the same...
            var stateAfterToggleOnce = target.GetItemSourceItem(itemIndex).IsChecked;
            Assert.That(toggleState, Is.EqualTo(stateAfterToggleOnce));
            //...but different to previous state
            Assert.That(stateAfterToggleOnce, Is.Not.EqualTo(initialItemCheckedState));

//USER:     Toggle checkbox second time
            Assert.That(itemCheckBox.TryToggle(out toggleState));

            //State of Toggle and state of item should be the same...
            var stateAfterToggleTwice = target.GetItemSourceItem(itemIndex).IsChecked;
            Assert.AreEqual(toggleState, stateAfterToggleTwice);
            //...but different to previous state
            Assert.AreEqual(stateAfterToggleTwice, !stateAfterToggleOnce);
        }

        [Test]
        public void CorrectlyUncheckAllItems()
        {
            var map = TargetMap;

            //Get initial state of All toggle
            Assert.That(map.AllVisibleCheckBoxPart.TryGetToggleState(out var toggleState));

            AssertAllToggleStateCorrect(toggleState);

//USER:     Toggle All check box
            Assert.That(map.AllVisibleCheckBoxPart.TryToggle(out toggleState));

            AssertAllToggleStateCorrect(toggleState);

//USER:     Toggle All check box second time
            Assert.That(map.AllVisibleCheckBoxPart.TryToggle(out toggleState));

            AssertAllToggleStateCorrect(toggleState);
        }

        private void AssertAllToggleStateCorrect(bool? toggleState)
        {
            var target = Target;

            Assert.That(toggleState, Is.Not.Null);

            switch (toggleState)
            {
                case true:
                    Assert.That(target.ItemsSource, Has.Some.Property(nameof(IColorLegendItem.IsChecked)).True);
                    break;

                case false:
                    Assert.That(target.ItemsSource, Has.All.Property(nameof(IColorLegendItem.IsChecked)).False);
                    break;
            }
        }
    }
}
