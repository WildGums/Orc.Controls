namespace Orc.Controls.Tests.UI
{
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(RangeSlider))]
    [Category("UI Tests")]
    public class RangeSliderTestFacts : StyledControlTestFacts<RangeSlider>
    {
        [Target]
        public Automation.RangeSlider Target { get; set; }
            
        [Test]
        public void VerifyApi()
        {
            var target = Target;
            var model = target.Current;

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, model, nameof(target.Minimum), true,
                -100d, 200, 0.1, 0);

            model.Minimum = -1000;

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, model, nameof(target.Maximum), true,
                -100d, 200, 0.1, 0);

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, model, nameof(target.LowerValue), true, 
                0.2d, 0.1, 1, 0);

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, model, nameof(target.UpperValue), true,
                0.2d, 0.1, 1, 0);
        }


        [TestCase(0, 100, 50, 20, 20, 20, Description = "upper < lower")]
        [TestCase(0, 100, 50, 60, 50, 60, Description = "upper > lower")]
        public void CorrectlySetUpperValue(double min, double max,
            double lowerValue, double upperValue,
            double expectedLowerValue, double expectedUpperValue)
        {
            var target = Target;
            var model = target.Current;

            //Pre-install values
            model.Minimum = min;
            model.Maximum = max;
            model.LowerValue = lowerValue;

            //Setting lower value thought user-interface
            target.UpperValue = upperValue;

            Assert.That(target.LowerValue, Is.EqualTo(expectedLowerValue));
            Assert.That(target.UpperValue, Is.EqualTo(expectedUpperValue));
        }

        [TestCase(0, 100, 50, 20, 50, 50, Description = "lower > upper")]
        [TestCase(0, 100, 50, 60, 50, 60, Description = "lower < upper")]
        public void CorrectlySetLowerValue(double min, double max,
            double lowerValue, double upperValue,
            double expectedLowerValue, double expectedUpperValue)
        {
            var target = Target;
            var model = target.Current;

            //Pre-install values
            model.Minimum = min;
            model.Maximum = max;
            model.UpperValue = upperValue;

            //Setting lower value thought user-interface
            target.LowerValue = lowerValue;

            Assert.That(target.LowerValue, Is.EqualTo(expectedLowerValue));
            Assert.That(target.UpperValue, Is.EqualTo(expectedUpperValue));
        }
    }
} 
