namespace Orc.Controls.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using NUnit.Framework;
    using Orc.Automation;

    [TestFixture(TestOf = typeof(DateTimePicker))]
    [Category("UI Tests")]
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

            Target.SetTodayValue();

            DateTimePickerAssert.DateTime(target, DateTime.Today);
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

        [TestCaseSource(typeof(DatesTestSource))]
        public void CorrectlySelectDateFromCalendar(DateTime date)
        {
            var target = Target;

            var calendar = target.OpenCalendar();
            Assert.That(calendar, Is.Not.Null);

            calendar.SelectedDate = date;

            Wait.UntilResponsive();

            DateTimePickerAssert.DateTime(target, date.Date);
        }

        [TestCaseSource(typeof(DatesAndFormattedDatesTestSource))]
        [Apartment(ApartmentState.STA)]
        public void CorrectlyCopyDate(DateTime dateTime, string expectedDate)
        {
            var target = Target;
            var model = target.Current;

            model.Value = dateTime;

            Wait.UntilResponsive();

            target.CopyDate();

            Wait.UntilResponsive();

            var copiedText = Clipboard.GetText();

            Assert.That(copiedText, Is.EqualTo(expectedDate));
        }

        [TestCaseSource(typeof(DatesAndFormattedDatesTestSource))]
        [Apartment(ApartmentState.STA)]
        public void CorrectlyPasteDate(DateTime dateTime, string formattedDate)
        {
            var target = Target;

            Clipboard.SetText(formattedDate);

            Wait.UntilResponsive();

            target.PasteDate();

            Wait.UntilResponsive();

            DateTimePickerAssert.DateTime(target, dateTime);
        }

        [TestCaseSource(typeof(DatesAndFormattedDatesTestSource))]
        public void CorrectlyCopyPasteDate(DateTime dateTime, string formattedDate)
        {
            var target = Target;
            var model = target.Current;

            model.Value = dateTime;
            
            target.CopyDate();
            
            //override copied date
            model.Value = DateTime.Now;

            target.PasteDate();

            Wait.UntilResponsive();

            DateTimePickerAssert.DateTime(target, dateTime);
        }

        [TestCaseSource(typeof(DateTimeTestSource))]
        public void CorrectlySetDateTime(DateTime? dateTime)
        {
            var target = Target;

            target.Value = dateTime;

            DateTimePickerAssert.DateTime(target, dateTime);
        }

        [TestCaseSource(typeof(DateTimeTestSource))]
        public void CorrectlySelectDay(DateTime? dateTime)
        {
            var target = Target;

            var dateTimeValue = dateTime ?? DateTime.Today;

            target.Value = dateTime;

            var daysCount = DateTime.DaysInMonth(dateTimeValue.Year, dateTimeValue.Month);

            //Testing first, last and day in a middle of month
            var testDays = new List<int> { 1, daysCount, daysCount / 2};
            foreach (var testDay in testDays)
            {
                target.SelectDay(testDay);

                Wait.UntilResponsive();

                var expectedDate = new DateTime(dateTimeValue.Year, dateTimeValue.Month, testDay, dateTimeValue.Hour, dateTimeValue.Minute, dateTimeValue.Second);

                DateTimePickerAssert.DateTime(target, expectedDate);
            }
        }
        
        [TestCaseSource(typeof(DateTimeTestSource))]
        public void CorrectlySelectMonth(DateTime? dateTime)
        {
            var target = Target;

            var dateTimeValue = dateTime ?? DateTime.Today;

            target.Value = dateTime;

            var monthCount = 12;

            //Testing first, last and day in a middle of month
            var testMonths = new List<int> { 1, monthCount, monthCount / 2 };
            foreach (var testMonth in testMonths)
            {
                target.SelectMonth(testMonth);

                Wait.UntilResponsive();

                var day = Math.Min(DateTime.DaysInMonth(dateTimeValue.Year, testMonth), dateTimeValue.Day);

                var expectedDate = new DateTime(dateTimeValue.Year, testMonth, day, dateTimeValue.Hour, dateTimeValue.Minute, dateTimeValue.Second);

                DateTimePickerAssert.DateTime(target, expectedDate);
            }
        }

        [TestCase(null)]
        public void CorrectlySelectHour(DateTime? dateTime)
        {
            var target = Target;

            var dateTimeValue = dateTime ?? DateTime.Today;

            var hourCount = 24;

            //Testing first, last and day in a middle of month
            var testHours = new List<int> { 1, hourCount, hourCount / 2 };
            foreach (var testHour in testHours)
            {
                target.Value = dateTime;

                Wait.UntilResponsive();

                 target.SelectHour(testHour);

                Wait.UntilResponsive();

                var expectedDate = new DateTime(dateTimeValue.Year, dateTimeValue.Month, dateTimeValue.Day, testHour, dateTimeValue.Minute, dateTimeValue.Second);

                DateTimePickerAssert.DateTime(target, expectedDate);
            }
        }

        private class DatesAndFormattedDatesTestSource : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var currentCulture = CultureInfo.CurrentCulture;

                var date = new DateTime(1, 1, 1, 1, 1, 1);
                yield return new object[] { date, date.ToString(currentCulture) };

                date = new DateTime(1921, 2, 12, 2, 21, 1);
                yield return new object[] { date, date.ToString(currentCulture) };

                date = new DateTime(2032, 3, 31, 21, 32, 11);
                yield return new object[] { date, date.ToString(currentCulture) };
            }
        }

        private class DateTimeTestSource : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return DateTime.Now;
                yield return null;
                yield return new DateTime(1, 1, 1, 1, 1, 1);
                yield return new DateTime(1921, 2, 12, 21, 32, 59);
                yield return new DateTime(2032, 3, 31, 1, 2, 3);
            }
        }

        private class DatesTestSource : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return DateTime.Now;
                yield return new DateTime(1, 1, 1);
                yield return new DateTime(1921, 2, 12);
                yield return new DateTime(2032, 3, 31);
            }
        }
    }
}
