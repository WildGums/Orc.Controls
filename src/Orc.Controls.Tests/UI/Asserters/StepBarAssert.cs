namespace Orc.Controls.Tests
{
    using System;
    using NUnit.Framework;

    public static class StepBarAssert
    {
        public static void Selection(Automation.StepBar target, IStepBarItem selectedItem,
            Func<IStepBarItem, IStepBarItem, bool> itemComparer)
        {
            var model = target.Current;

            var items = model.Items;

            Assert.That(model.SelectedItem, Is.EqualTo(selectedItem)
                .Using<IStepBarItem>(itemComparer));

            Assert.That(target.SelectedItem?.Current.DataContext, Is.EqualTo(selectedItem)
                .Using<IStepBarItem>(itemComparer));

            var count = items.Count;

            var selectedIndex = 0;
            for (var i = 0; i < count; i++)
            {
                if (itemComparer.Invoke(items[i], selectedItem))
                {
                    selectedIndex = i;
                    break;
                }
            }

            var expectedStates = new StepBarItemStates[count];

            expectedStates[0] = StepBarItemStates.IsVisited;
            for (var i = 1; i < selectedIndex; i++)
            {
                expectedStates[i] = StepBarItemStates.IsBeforeCurrent;
            }

            expectedStates[selectedIndex] = StepBarItemStates.IsVisited | StepBarItemStates.IsCurrent;
            for (var i = selectedIndex + 1; i < count - 1; i++)
            {
                expectedStates[i] = StepBarItemStates.None;
            }

            expectedStates[count - 1] |= StepBarItemStates.IsLast;

            for (var i = 0; i < count; i++)
            {
                var expectedItem = expectedStates[i];
                var item = items[i];

                Assert.That(item.State.HasFlag(expectedItem));
            }
        }
    }
}
