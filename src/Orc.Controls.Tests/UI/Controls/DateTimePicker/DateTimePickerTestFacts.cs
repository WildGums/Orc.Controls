namespace Orc.Controls.Tests
{
    using System;
    using FlaUI.Core.AutomationElements;
    using NUnit.Framework;
    using Orc.Automation;
    using DateTimePicker = Orc.Controls.DateTimePicker;

    [TestFixture]
    public partial class DateTimePickerTestFacts : StyledControlTestFacts<DateTimePicker>
    {
        [Target]
        public Automation.DateTimePicker Target { get; set; }


        [Test]
        public void VerifyInitialState()
        {
            var target = Target;
            var model = target.Current;
        }

        [Test]
        public void CorrectlySetTodayValue()
        {
            var target = Target;
            var model = target.Current;

            target.SetTodayValue();

            Assert.That(model.Value, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void CorrectlySetNowValue()
        {
            var target = Target;
            var model = target.Current;

            var begin = DateTime.Now;

            target.SetNowValue();

            var end = DateTime.Now;

            Assert.That(model.Value, Is.InRange(begin, end));
        }

        [Test]
        public void CorrectlySelectDateFromMenu()
        {
            var target = Target;
            var model = target.Current;

            target.OpenSelectDateDialog();
        }
    }
}
