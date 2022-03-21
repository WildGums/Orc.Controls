namespace Orc.Controls.Tests.UI
{
    using System;
    using System.Collections;
    using NUnit.Framework;
    using Orc.Automation;

    [Explicit]
    [TestFixture(TestOf = typeof(TimePicker))]
    [Category("UI Tests")]
    public class TimeSpanPickerFacts : StyledControlTestFacts<TimeSpanPicker>
    {
        [Target]
        public Automation.TimeSpanPicker Target { get; set; }

        [Test]
        public void CorrectlySetReadonlyState()
        {
            var target = Target;
            var model = target.Current;

            var timeSpan = TimeSpan.FromHours(10);
            target.Value = timeSpan;

            model.CanEdit = false;

            var timeSpanAfterEdit = TimeSpan.FromHours(20);
            target.Value = timeSpanAfterEdit;

            TimeSpanPickerAssert.Time(target, timeSpan);
        }

        [TestCaseSource(nameof(GetTimeSpanSource))]
        public void CorrectlySetTime(TimeSpan? timeSpan)
        {
            var target = Target;

            target.Value = timeSpan;

            TimeSpanPickerAssert.Time(target, timeSpan);
        }

        [TestCase(null)]
        [TestCase(21.2)]
        public void CorrectlySetDays(double? days)
        {
            var target = Target;

            target.TotalDays = days;

            TimeSpanPickerAssert.Time(target, days is null ? null : TimeSpan.FromDays(days.Value));
        }

        [TestCase(null)]
        [TestCase(24)]
        [TestCase(51.1)]
        [TestCase(2.21)]
        public void CorrectlySetHours(double? hours)
        {
            var target = Target;

            target.TotalHours = hours;

            TimeSpanPickerAssert.Time(target, hours is null ? null : TimeSpan.FromHours(hours.Value));
        }

        [TestCase(null)]
        [TestCase(1.2)]
        [TestCase(21.2)]
        [TestCase(60)]
        [TestCase(125)]
        [TestCase(1440)]
        [TestCase(2440)]
        public void CorrectlySetMinutes(double? minutes)
        {
            var target = Target;

            target.TotalMinutes = minutes;

            TimeSpanPickerAssert.Time(target, minutes is null ? null : TimeSpan.FromMinutes(minutes.Value));
        }

        [TestCase(null)]
        [TestCase(1)]
        [TestCase(21)]
        [TestCase(60)]
        [TestCase(3600)]
        [TestCase(86400)]
        [TestCase(100000)]
        public void CorrectlySetSeconds(double? seconds)
        {
            var target = Target;

            target.TotalSeconds = seconds;

            TimeSpanPickerAssert.Time(target, seconds is null ? null : TimeSpan.FromSeconds(seconds.Value));
        }

        [TestCaseSource(nameof(GetTimeSpanSource))]
        public void CorrectlySetTimeApi(TimeSpan? timeSpan)
        {
            var target = Target;
            var model = target.Current;

            model.Value = timeSpan;

            TimeSpanPickerAssert.Time(target, timeSpan);
        }

        private static IEnumerable GetTimeSpanSource()
        {
            yield return null;

            yield return TimeSpan.FromDays(5);
            yield return TimeSpan.FromHours(10);
            yield return TimeSpan.FromMinutes(10);
            yield return TimeSpan.FromSeconds(10);

            yield return new TimeSpan(0, 5, 10, 10);
            yield return new TimeSpan(5, 5, 10, 10);
        }
    }
}
