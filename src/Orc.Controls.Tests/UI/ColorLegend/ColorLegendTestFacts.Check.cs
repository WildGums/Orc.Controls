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
            var view = View;

            var itemControls = view.Items;
            var itemControl = itemControls[itemIndex];
            //Item control is there
            Assert.That(itemControl, Is.Not.Null);

            var itemSource = target[itemIndex];
            //Item source is there
            Assert.That(itemSource, Is.Not.Null);

            //Verify that source and control match each other -> they should be in one check state
            Assert.That(itemControl.IsChecked, Is.EqualTo(itemSource.IsChecked));

/*Simulate:   Changing the state from control*/
            itemControl.IsChecked = !itemSource.IsChecked;
            //Verify that source and control match each other -> they should be in one check state
            Assert.That(itemControl.IsChecked, Is.EqualTo(target[itemIndex].IsChecked));

            //Changing the state from data
            var isChecked = target[itemIndex].IsChecked;
            target.SetItemCheckState(itemIndex, !isChecked);

            Assert.That(itemControl.IsChecked, Is.EqualTo(target[itemIndex].IsChecked));
        }

        [Test]
        public void CorrectlyUncheckAllItems()
        {
            var view = View;

            //Checking initial state of color legend
           ColorLegendAssert.AllCheckedState(Target);

//Simulate: 
            //target.Simulate.TrySetCheckAllState(true);



            //Get initial state of All toggle
//            Assert.That(map.i.TryGetToggleState(out var toggleState));

//            AssertAllToggleStateCorrect(toggleState);

////Simulate: Toggle All
//            Assert.That(map.AllVisibleCheckBoxPart.TryToggle(out toggleState));

//            AssertAllToggleStateCorrect(toggleState);

////Simulate: Toggle All second time
//            Assert.That(map.AllVisibleCheckBoxPart.TryToggle(out toggleState));

//            AssertAllToggleStateCorrect(toggleState);
        }
    }
}
