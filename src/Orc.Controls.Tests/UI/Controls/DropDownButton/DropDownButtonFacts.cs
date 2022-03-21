namespace Orc.Controls.Tests
{
    using System.Windows;
    using System.Windows.Controls;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;


    [TestFixture(TestOf = typeof(DropDownButton))]
    [Category("UI Tests")]
    public class DropDownButtonFacts : StyledControlTestFacts<DropDownButton>
    {
        [Target]
        public Automation.DropDownButton Target { get; set; }

        [Test]
        public void VerifyProperties()
        {
            var target = Target;
            var model = target.Current;
            
            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, nameof(target.IsToggled), model, nameof(model.IsChecked), true, true, false);
        }

        [Test]
        public void CorrectlyShowMenu()
        {
            var target = Target;

            target.Execute<AddMenuItemsAutomationMethodRun>();

            Wait.UntilResponsive();

            var menu = target.OpenDropDown();

            Assert.That(menu, Is.Not.Null);
            Assert.That(menu.Items.Count, Is.EqualTo(5));
        }
    }

    public class AddMenuItemsAutomationMethodRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.FromValue(10);

            if (owner is not DropDownButton dropDown)
            {
                return false;
            }

            var contextMenu = new ContextMenu();

            var items = contextMenu.Items;

            items.Add("One");
            items.Add("Two");
            items.Add("Three");
            items.Add("Four");
            items.Add("Five");

            dropDown.SetCurrentValue(DropDownButton.DropDownProperty, contextMenu);

            return true;
        }
    }
}
