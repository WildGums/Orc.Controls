namespace Orc.Controls.Tests;

using NUnit.Framework;

[TestFixture]
public class DateTimeFormatInfoFacts
{
    [TestFixture]
    public class The_GetSeparator_Method
    {
        [TestCase(0, "-")]
        [TestCase(1, "/")]
        [TestCase(2, " ")]
        [TestCase(3, ":")]
        [TestCase(4, ".")]
        [TestCase(5, ",")]
        [TestCase(6, ";")]
        [TestCase(7, "|")]
        public void Returns_Correct_Separator_By_Position(int position, string separator)
        {
            var formatInfo = new DateTimeFormatInfo
            {
                Separator0 = "-",
                Separator1 = "/",
                Separator2 = " ",
                Separator3 = ":",
                Separator4 = ".",
                Separator5 = ",",
                Separator6 = ";",
                Separator7 = "|"
            };

            var result = formatInfo.GetSeparator(position);

            Assert.That(result, Is.EqualTo(separator));
        }

        [Test]
        public void Returns_Null_When_Separator_Not_Set()
        {
            var formatInfo = new DateTimeFormatInfo();

            var result = formatInfo.GetSeparator(0);

            Assert.That(result, Is.Null);
        }
    }

    [TestFixture]
    public class The_IsDateOnly_Property
    {
        [Test]
        public void Returns_True_When_No_Time_Fields_Set()
        {
            var formatInfo = new DateTimeFormatInfo
            {
                DayFormat = "dd",
                MonthFormat = "MM",
                YearFormat = "yyyy"
            };

            Assert.That(formatInfo.IsDateOnly, Is.True);
        }

        [Test]
        public void Returns_False_When_Hour_Is_Set()
        {
            var formatInfo = new DateTimeFormatInfo
            {
                DayFormat = "dd",
                MonthFormat = "MM",
                YearFormat = "yyyy",
                HourPosition = 3,
                HourFormat = "HH"
            };

            Assert.That(formatInfo.IsDateOnly, Is.False);
        }

        [Test]
        public void Returns_False_When_Minute_Is_Set()
        {
            var formatInfo = new DateTimeFormatInfo
            {
                MinutePosition = 4
            };

            Assert.That(formatInfo.IsDateOnly, Is.False);
        }
    }
}
