namespace Orc.Controls.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    public class DropDownButtonFacts : StyledControlTestFacts<Orc.Controls.DropDownButton>
    {
        [Target]
        public Automation.DropDownButton Target { get; set; }

        [Test]
        public void VerifyProperties()
        {
            var target = Target;
            var view = target.View;

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, nameof(target.IsChecked), view, nameof(view.IsToggled), true, true, false);
        }

        [Test]
        public void CorrectlyShowMenu()
        {
            var target = Target;
            var view = target.View;
            
            target.AddMenuItems(new List<string>
            {
                "One", 
                "Two", 
                "Three", 
                "Four", 
                "Five"
            });

            var menu = view.OpenDropDown();

            Assert.That(menu, Is.Not.Null);
        }
    }
}
