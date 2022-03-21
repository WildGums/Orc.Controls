namespace Orc.Controls.Tests
{
    using System;
    using NUnit.Framework;

    public static class TimeSpanPickerAssert
    {
        public static void Time(Automation.TimeSpanPicker target, TimeSpan? expectedValue)
        {
            var model = target.Current;

            var correctedExpectedValue = expectedValue ?? TimeSpan.FromMilliseconds(0);

            Assert.That(model.Value ?? TimeSpan.FromMilliseconds(0), Is.EqualTo(correctedExpectedValue));
            Assert.That(target.Value ?? TimeSpan.FromMilliseconds(0), Is.EqualTo(correctedExpectedValue));

            Assert.That(target.TotalDays, Is.EqualTo(correctedExpectedValue.TotalDays));
            Assert.That(target.TotalHours, Is.EqualTo(correctedExpectedValue.TotalHours));
            Assert.That(target.TotalMinutes, Is.EqualTo(correctedExpectedValue.TotalMinutes));
            Assert.That(target.TotalSeconds, Is.EqualTo(correctedExpectedValue.TotalSeconds));
        }
    }
}
