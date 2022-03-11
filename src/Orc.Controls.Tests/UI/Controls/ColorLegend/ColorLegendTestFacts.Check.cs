namespace Orc.Controls.Tests
{
    using NUnit.Framework;

    public partial class ColorLegendTestFacts
    {
        [TestCase(2)]
        [TestCase(0)]
        [TestCase(9)]
        [Description("Each item check state match underlying itemsource state.")]
        public void CorrectlyUncheckCheckItem(int itemIndex)
        {
            var target = Target;
            var model = target.Current;

            var itemControls = target.Items;
            var itemControl = itemControls[itemIndex];
            //Item control is there
            Assert.That(itemControl, Is.Not.Null);

            var itemSource = model[itemIndex];
            //Item source is there
            Assert.That(itemSource, Is.Not.Null);

            //Verify that source and control match each other -> they should be in one check state
            Assert.That(itemControl.IsChecked, Is.EqualTo(itemSource.IsChecked));

/*Simulate:   Changing the state from control*/
            itemControl.IsChecked = !itemSource.IsChecked;
            //Verify that source and control match each other -> they should be in one check state
            Assert.That(itemControl.IsChecked, Is.EqualTo(model[itemIndex].IsChecked));

            //Changing the state from data
            var isChecked = model[itemIndex].IsChecked;
            target.SetItemCheckState(itemIndex, !isChecked);

            Assert.That(itemControl.IsChecked, Is.EqualTo(model[itemIndex].IsChecked));
        }

        [Test]
        public void CorrectlyUncheckAllItems()
        { 
            //Checking initial state of color legend
           ColorLegendAssert.AllCheckedState(Target);
        }
    }
}
