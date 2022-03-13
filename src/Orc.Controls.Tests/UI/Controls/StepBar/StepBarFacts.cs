namespace Orc.Controls.Tests
{
    using NUnit.Framework;
    using Orc.Automation;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;using Catel.Data;
    using Catel.MVVM;
    using Orc.Automation.Tests;
    using FrameworkElement = System.Windows.FrameworkElement;

    [TestFixture(TestOf = typeof(StepBar))]
    [Category("UI Tests")]
    public class StepBarFacts : StyledControlTestFacts<StepBar>
    {
        [Target]
        public Automation.StepBar Target { get; set; }

        [Test]
        public void CorrectlyRespondToClick()
        {
            var target = Target;
            var model = target.Current;

            model.Items = CreateTestStepBarItems(10);

            var item = target.Items[2];

            item.Execute<SpecifyCommandMethodRun>();

            //Catch command executed
            EventAssert.Raised(target, "AutomationMessageSent", () => item.Invoke());
        }

        [Test]
        public void CorrectlyInitialize()
        {
            var target = Target;
            var model = target.Current;

            var itemList = CreateTestStepBarItems(10);
            model.Items = itemList;

            var items = target.Items;

            Assert.That(itemList, Is.EquivalentTo(items.Select(x => x.Current.DataContext))
                .Using<TestStepBarItem>(CompareItems));
        }

        [TestCase(20, 17)]
        [TestCase(20, 0)]
        [TestCase(20, 19)]
        public void CorrectlySelectItem(int count, int selectedIndex)
        {
            var target = Target;
            var model = target.Current;

            var testItems = CreateTestStepBarItems(count);
            
            var selectedItem = testItems[selectedIndex];
            model.Items = testItems;

            model.SelectedItem = selectedItem;

            StepBarAssert.Selection(target, selectedItem, CompareItems);
        }

        private static bool CompareItems(IStepBarItem x, IStepBarItem y)
        {
            return string.Equals(x?.Title, y?.Title)
                   && string.Equals(x?.Description, y?.Description)
                   && Equals(x?.Number, y?.Number);
        }

        private static List<IStepBarItem> CreateTestStepBarItems(int count)
        {
            var items = new List<IStepBarItem>();

            for (var i = 1; i <= count; i++)
            {
                items.Add(new TestStepBarItem
                {
                    Title = $"Title{i}",
                    Number = i,
                    Description = $"Description{i}",
                });
            }

            return items;
        }
    }

    public class TestStepBarItem : ModelBase, IStepBarItem
    {
        private ICommand _command;

        public string Title { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public StepBarItemStates State { get; set; }

        public ICommand Command
        {
            get => _command;
            set
            {
                if (Equals(_command, value))
                {
                    return;
                }

                _command = value;
                RaisePropertyChanged(nameof(Command));
            }
        }
    }

    public class SpecifyCommandMethodRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.FromValue(true);

            if (owner is not StepBarItem stepBarItem)
            {
                return false;
            }

            if (stepBarItem.DataContext is not TestStepBarItem itemModel)
            {
                return false;
            }

            itemModel.Command = new Command<TestStepBarItem>(x =>
                stepBarItem.RaiseEvent(new AutomationMessageSentEventArgs(AutomationRoutedEvents.AutomationMessageSentEvent)));

            return true;
        }
    }
}
