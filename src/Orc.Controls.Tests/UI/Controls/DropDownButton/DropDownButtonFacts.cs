namespace Orc.Controls.Tests
{
    using System;
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
            var model = target.Current;
            
            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, nameof(target.IsToggled), model, nameof(model.IsChecked), true, true, false);
        }

        [Test]
        public void CorrectlyShowMenu()
        {
            var target = Target;

            target.AddMenuItems(new List<string>
            {
                "One",
                "Two",
                "Three",
                "Four",
                "Five"
            });

            var menu = target.OpenDropDown();

            Assert.That(menu, Is.Not.Null);
        }
    }
}
