﻿namespace Orc.Controls.Tests.UI
{
    using System.Windows.Input;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(NumericTextBox))]
    [Category("UI Tests")]
    public class NumericTextBoxTestFacts : StyledControlTestFacts<NumericTextBox>
    {
        [Target]
        public Automation.NumericTextBox Target { get; set; }
            
        [Test]
        public void VerifyState()
        {
            var target = Target;
            var model = target.Current;

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, model, nameof(target.Text), true, "12", "12.3", "0");
        }

        [TestCase(12.4, "F1", "12.4")]
        [TestCase(12.4, "F2", "12.40")]
        [TestCase(null, "F0", "")]
        [TestCase(121, "F0", "121")]
        [TestCase(121.122, "F0", "121")]
        [TestCase(51, "P2", "5,100.00%")]
        public void CorrectlySetValue(double? value, string format, string expectedText)
        {
            var target = Target;
            var model = target.Current;

            model.Format = format;
            model.Value = value;

            Assert.That(model.Text, Is.EqualTo(expectedText));
            Assert.That(target.Text, Is.EqualTo(expectedText));
        }

        [TestCase("12.4", "F1", 12.4)]
        [TestCase("12.4", "F0", 12)]
        [TestCase("5,100.00%", "P2", 51)]
        [TestCase("", "F0", null)]
        public void CorrectlySetText(string text, string format, double? expectedValue)
        {
            var target = Target;
            var model = target.Current;

            model.Format = format;

            target.Text = text;

            //Move focus to apply value
            KeyboardInput.Press(Key.Tab);

            Assert.That(model.Value, Is.EqualTo(expectedValue));
        }

        [TestCase(1d, 1, 2d)]
        [TestCase(1d, 2, 3d)]
        [TestCase(12.4d, 1, 13.3d)]
        public void CorrectlyIncreaseValueByUpArrow(double initialValue, int count, double expectedValue)
        {
            var target = Target;
            var model = target.Current;

            model.IsChangeValueByUpDownKeyEnabled = true;

            model.Value = initialValue;

            KeyboardInputEx.SelectAll();

            Wait.UntilResponsive();

            for (var i = 0; i < count; i++)
            {
                KeyboardInput.Press(Key.Up);
                KeyboardInput.Release(Key.Up);

                Wait.UntilResponsive();
            }

            KeyboardInput.Press(Key.Tab);

            Wait.UntilResponsive();

            Assert.That(model.Value, Is.EqualTo(initialValue + count));
        }
    }
} 
