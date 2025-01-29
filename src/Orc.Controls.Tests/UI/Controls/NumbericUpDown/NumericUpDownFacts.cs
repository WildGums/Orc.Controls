namespace Orc.Controls.Tests.UI
{
    using System;
    using System.Windows.Automation;
    using System.Windows.Input;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(NumericUpDown))]
    [Category("UI Tests")]
    public class NumericUpDownFacts : StyledControlTestFacts<NumericUpDown>
    {
        [Target]
        public Orc.Controls.Automation.NumericUpDown Target { get; set; }

        [TestCase(10d, "10", 0)]
        [TestCase(123.4d, "123.4", 1)]
        [TestCase(-123.4d, "-123.40", 2)]
        [TestCase(-123.5d, "-124", 0)]
        public void VerifyValue(double value, string viewValue, int decimalPlaces)
        {
            var target = Target;
            var view = target.View;

            target.DecimalPlaces = decimalPlaces;

            ConnectedPropertiesAssert.VerifyConnectedProperties(target, nameof(target.Value), view, nameof(view.Value), true, 
                new ValueTuple<Number, string>(value, viewValue));
        }

        [TestCase(10d, 1.5d, 2, "11.50")]
        [TestCase(10d, -1.5d, 2, "8.50")]
        [TestCase(10d, 0d, 0, "10")]
        public void CorrectlyIncreaseValue(double initialValue, double delta, int decimalPlaces, string expectedViewValue)
        {
            var target = Target;
            var view = target.View;

            target.MinorDelta = delta;
            target.DecimalPlaces = decimalPlaces;

            view.Value = initialValue.ToString();

            view.IncreaseNumber();

            var expectedValue = initialValue + delta;

            Assert.That(target.Value, Is.EqualTo((Number)expectedValue));
            Assert.That(view.Value, Is.EqualTo(expectedViewValue));
        }

        [TestCase(10d, -1.5d, 2, "11.50")]
        [TestCase(10d, 1.5d, 2, "8.50")]
        [TestCase(10d, 0d, 0, "10")]
        public void CorrectlyDecreaseValue(double initialValue, double delta, int decimalPlaces, string expectedViewValue)
        {
            var target = Target;
            var view = target.View;

            target.MinorDelta = delta;
            target.DecimalPlaces = decimalPlaces;

            view.Value = initialValue.ToString();

            view.DecreaseNumber();

            var expectedValue = initialValue - delta;

            Assert.That(target.Value, Is.EqualTo((Number)expectedValue));
            Assert.That(view.Value, Is.EqualTo(expectedViewValue));
        }

        [TestCase("1000", 0, "1,000")]
        [TestCase("1000", 2, "1,000.00")]
        [TestCase("123456789", 0, "123,456,789")]
        [TestCase("123", 0, "123")]
        [TestCase("122123.00", 2, "122,123.00")]
        public void CorrectlyFormatThousandSeparator(string value, int decimalPlaces, string expectedValue)
        {
            var target = Target;
            var view = target.View;

            target.IsThousandSeparatorVisible = true;
            target.DecimalPlaces = decimalPlaces;

            view.Value = value;

            KeyboardInput.Type(Key.Tab);

            Assert.That(view.Value, Is.EqualTo(expectedValue));
        }

        [TestCase(10d, 10d, 0d, 18d, "10")]
        [TestCase(10d, 10d, 11d, double.MaxValue, "21")]
        [TestCase(0d, -1d, 0d, double.MaxValue, "0")]
        public void CorrectlyIncreaseValueWithinRanges(double initialValue, double delta, double min, double max, string expectedValue)
        {
            var target = Target;
            var view = target.View;

            target.MinorDelta = delta;
            target.MinValue = min;
            target.MaxValue = max;

            view.Value = initialValue.ToString();

            try
            {
                view.IncreaseNumber();
            }
            catch(ElementNotEnabledException)
            {
                //Do nothing
            }

            Assert.That(view.Value, Is.EqualTo(expectedValue));
        }

        [TestCase(10d, 10d, 2d, double.MaxValue, "10")]
        [TestCase(10d, 10d, 11d, double.MaxValue, "11")]
        [TestCase(0d, -1d, 0d, double.MaxValue, "1")]
        public void CorrectlyDecreaseValueWithinRanges(double initialValue, double delta, double min, double max, string expectedValue)
        {
            var target = Target;
            var view = target.View;

            target.MinorDelta = delta;
            target.MinValue = min;
            target.MaxValue = max;

            view.Value = initialValue.ToString();

            try
            {
                view.DecreaseNumber();
            }
            catch (ElementNotEnabledException)
            {
                //Do nothing
            }

            Assert.That(view.Value, Is.EqualTo(expectedValue));
        }
    }
}
