namespace Orc.Controls.Tests
{
    using System;
    using NUnit.Framework;

    public static class DateTimePickerAssert
    {
        public static void DateTime(Automation.DateTimePicker target, DateTime? expectedDate)
        {
            var model = target.Current;

            expectedDate = TruncateMilliseconds(expectedDate);

            Assert.That(model.Value, Is.EqualTo(expectedDate));
            Assert.That(target.Value, Is.EqualTo(expectedDate));
        }

        private static DateTime? TruncateMilliseconds(DateTime? date)
        {
            if (date is null)
            {
                return null;
            }

            var dateValue = date.Value;

            return new DateTime(dateValue.Year, dateValue.Month, dateValue.Day, dateValue.Hour, dateValue.Minute, dateValue.Second, dateValue.Kind);
        }
    }
}
