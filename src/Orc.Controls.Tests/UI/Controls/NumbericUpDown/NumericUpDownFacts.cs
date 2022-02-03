namespace Orc.Controls.Tests.UI
{
    using System.Windows;
    using System.Windows.Media;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(NumericUpDown))]
    [Category("UI Tests")]
    public class NumericUpDownFacts : ControlUiTestFactsBase<NumericUpDown>
    {
        [Target]
        public Automation.NumericUpDown Target { get; set; }

        [Test]
        public void CorrectlyTest()
        {
            var target = Target;

            var number = target.Value;

            var minValue = target.MinValue;

            target.VerticalAlignment = VerticalAlignment.Center;
            target.HorizontalAlignment = HorizontalAlignment.Center;
            target.Height = 26d;

            target.Background = Brushes.Blue;
        }
    }
}
