namespace Orc.Controls.Tests
{
    using System;
    using NUnit.Framework;
    using Orc.Automation;

    [TestFixture(TestOf = typeof(TimePicker))]
    [Category("UI Tests")]
    public class TimePickerFacts : StyledControlTestFacts<TimePicker>
    {
        [Target]
        public Automation.TimePicker Target { get; set; }

        [Test]
        public void CorrectlySetTime()
        {
            var target = Target;
            var model = target.Current;

            model.TimeValue = TimeSpan.FromHours(3);
        }
    }
}
