namespace Orc.Controls.Tests
{
    using System;
    using System.Collections;
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

        [TestCaseSource(typeof(DatesTestSource))]
        public void CorrectlySelectDateFromCalendar(DateTime date)
        {
            var target = Target;
            var model = target.Current;

            target.SelectDateFromCalendar(date);

            Wait.UntilResponsive();

            Assert.That(model.Value, Is.EqualTo(date.Date));
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
            var model = target.Current;

            Clipboard.SetText(formattedDate);

            Wait.UntilResponsive();

            target.PasteDate();

            Wait.UntilResponsive();

            Assert.That(dateTime, Is.EqualTo(model.Value));
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

            Assert.That(model.Value, Is.EqualTo(dateTime));
        }

        private class DatesAndFormattedDatesTestSource : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var date = new DateTime(1, 1, 1, 1, 1, 1);
                yield return new object[] { date, date.ToString(CultureInfo.CurrentCulture) };

                date = new DateTime(1921, 2, 12, 2, 21, 1);
                yield return new object[] { date, date.ToString(CultureInfo.CurrentCulture) };

                date = new DateTime(2032, 3, 31, 21, 32, 11);
                yield return new object[] { date, date.ToString(CultureInfo.CurrentCulture) };
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
