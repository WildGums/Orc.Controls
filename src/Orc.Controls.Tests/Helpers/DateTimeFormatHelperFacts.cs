﻿namespace Orc.Controls
{
    using System;
    using NUnit.Framework;

    public class DateTimeFormatHelperFacts
    {
        [TestFixture]
        public class TheSplitMethod
        {
            private static readonly object[][] SplitTestCases = new object[][]
            {
                new object []
                {
                    "yyyy-MM-dd",
                    new char[] { 'y', 'M', 'd', 'H', 'h', 'm', 's', 't' },
                    new string [] { "yyyy", "-", "MM", "-", "dd" }
                },
                new object []
                {
                    "dd/MM/yyyy",
                    new char[] { 'y', 'M', 'd', 'H', 'h', 'm', 's', 't' },
                    new string [] { "dd", "/", "MM", "/", "yyyy" }
                },
                new object []
                {
                    "yyyy-MM-dd HH:mm:ss",
                    new char[] { 'y', 'M', 'd', 'H', 'h', 'm', 's', 't' },
                    new string [] { "yyyy", "-", "MM", "-", "dd", " ", "HH", ":", "mm", ":", "ss" }
                },
                new object []
                {
                    "yyyy/MM/dd tt hh:mm:ss",
                    new char[] { 'y', 'M', 'd', 'H', 'h', 'm', 's', 't' },
                    new string [] { "yyyy", "/", "MM", "/", "dd", " ", "tt", " ", "hh", ":", "mm", ":", "ss" }
                },
                new object []
                {
                    "yyyy/MMMM/dddd tt hh:mm:ss",
                    new char[] { 'y', 'M', 'd', 'H', 'h', 'm', 's', 't' },
                    new string [] { "yyyy", "/", "MMMM", "/", "dddd", " ", "tt", " ", "hh", ":", "mm", ":", "ss" }
                },
            };

            [Test]
            [TestCaseSource(nameof(SplitTestCases))]
            public void CorrectlySplitsDateToParts(string format, char[] formatCharacters, string[] expectedResult)
            {
                var result = DateTimeFormatHelper.Split(format, formatCharacters);

                Assert.That(result, Is.EquivalentTo(expectedResult));
            }
        }

        [TestFixture]
        public class TheGetDateTimeFormatInfoMethod
        {
            private static readonly object[][] GetDateTimeFormatInfoForDateFormatTestCases = new object[][]
            {
                new object []
                {
                    "yyyy-MM-dd",
                    new DateTimeFormatInfo()
                    {
                        Separator0 = null,
                        YearPosition = 0,
                        YearFormat = "yyyy",
                        Separator1 = "-",
                        MonthPosition = 1,
                        MonthFormat = "MM",
                        Separator2 = "-",
                        DayPosition = 2,
                        DayFormat = "dd",
                        Separator3 = null,
                        IsYearShortFormat = false
                    }
                },
                new object []
                {
                    "yyyy-MMM-dddd",
                    new DateTimeFormatInfo()
                    {
                        Separator0 = null,
                        YearPosition = 0,
                        YearFormat = "yyyy",
                        Separator1 = "-",
                        MonthPosition = 1,
                        MonthFormat = "MMM",
                        Separator2 = "-",
                        DayPosition = 2,
                        DayFormat = "dddd",
                        Separator3 = null,
                        IsYearShortFormat = false
                    }
                },
                new object []
                {
                    "dd/MM/yy",
                    new DateTimeFormatInfo()
                    {
                        Separator0 = null,
                        DayPosition = 0,
                        DayFormat = "dd",
                        Separator1 = "/",
                        MonthPosition = 1,
                        MonthFormat = "MM",
                        Separator2 = "/",
                        YearPosition = 2,
                        YearFormat = "yy",
                        Separator3 = null,
                        IsYearShortFormat = true
                    }
                }
            };

            private static readonly object[][] GetDateTimeFormatInfoForDateTimeFormatTestCases = new object[][]
            {
                new object []
                {
                    "yyyy-MM-dd hh:mm:ss tt",
                    new DateTimeFormatInfo()
                    {
                        Separator0 = null,
                        YearPosition = 0,
                        YearFormat = "yyyy",
                        Separator1 = "-",
                        MonthPosition = 1,
                        MonthFormat = "MM",
                        Separator2 = "-",
                        DayPosition = 2,
                        DayFormat = "dd",
                        Separator3 = " ",
                        HourPosition = 3,
                        HourFormat = "hh",
                        Separator4 = ":",
                        MinutePosition = 4,
                        MinuteFormat = "mm",
                        Separator5 = ":",
                        SecondPosition = 5,
                        SecondFormat = "ss",
                        Separator6 = " ",
                        AmPmPosition = 6,
                        AmPmFormat = "tt",
                        Separator7 = null,
                        IsYearShortFormat = false,
                        IsHour12Format = true,
                        IsAmPmShortFormat = false
                    }
                },
                new object []
                {
                    "MM/dd/yyyy HH:mm:ss",
                    new DateTimeFormatInfo()
                    {
                        Separator0 = null,
                        MonthPosition = 0,
                        MonthFormat = "MM",
                        Separator1 = "/",
                        DayPosition = 1,
                        DayFormat = "dd",
                        Separator2 = "/",
                        YearPosition = 2,
                        YearFormat = "yyyy",
                        Separator3 = " ",
                        HourPosition = 3,
                        HourFormat = "HH",
                        Separator4 = ":",
                        MinutePosition = 4,
                        MinuteFormat = "mm",
                        Separator5 = ":",
                        SecondPosition = 5,
                        SecondFormat = "ss",
                        Separator6 = null,
                        AmPmPosition = null,
                        AmPmFormat = null,
                        Separator7 = null,
                        IsYearShortFormat = false,
                        IsHour12Format = false,
                        IsAmPmShortFormat = false
                    }
                },
                new object []
                {
                    "dd/MM/yy t hh:mm:ss",
                    new DateTimeFormatInfo()
                    {
                        Separator0 = null,
                        DayPosition = 0,
                        DayFormat = "dd",
                        Separator1 = "/",
                        MonthPosition = 1,
                        MonthFormat = "MM",
                        Separator2 = "/",
                        YearPosition = 2,
                        YearFormat = "yy",
                        Separator3 = " ",
                        AmPmPosition = 3,
                        AmPmFormat = "t",
                        Separator4 = " ",
                        HourPosition = 4,
                        HourFormat = "hh",
                        Separator5 = ":",
                        MinutePosition = 5,
                        MinuteFormat = "mm",
                        Separator6 = ":",
                        SecondPosition = 6,
                        SecondFormat = "ss",
                        Separator7 = null,
                        IsYearShortFormat = true,
                        IsHour12Format = true,
                        IsAmPmShortFormat = true
                    }
                }
            };

            [Test]
            [TestCaseSource(nameof(GetDateTimeFormatInfoForDateFormatTestCases))]
            public void ReturnsCorrectDateFormatInfoForDateFormat(string format, DateTimeFormatInfo expectedResult)
            {
                var result = DateTimeFormatHelper.GetDateTimeFormatInfo(format, true);

                Assert.That(AreEqual(result, expectedResult), Is.True);
            }

            [TestCase("yyyy-yyyy-MM")]
            public void ThrowsFormatExceptionForDateFormatWhenYearPartExistsMoreThanOnce(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, true));
            }

            [TestCase("yyyy-MM-MM")]
            public void ThrowsFormatExceptionForDateFormatWhenMonthPartExistsMoreThanOnce(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, true));
            }

            [TestCase("yyyy-dd-dd")]
            public void ThrowsFormatExceptionForDateFormatWhenDayPartExistsMoreThanOnce(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, true));
            }

            [TestCase("yyyyyy-MM-dd")]
            public void ThrowsFormatExceptionForDateFormatWhenYearPartFormatIsIncorrect(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, true));
            }

            [TestCase("yyyy-MMMMM-dd")]
            public void ThrowsFormatExceptionForDateFormatWhenMonthPartFormatIsIncorrect(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, true));
            }

            [TestCase("yyyy-MM-ddddd")]
            public void ThrowsFormatExceptionForDateFormatWhenDayPartFormatIsIncorrect(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, true));
            }

            [TestCase("yyyy-MM")]
            [TestCase("MM-dd")]
            [TestCase("yyyy-dd")]
            public void ThrowsFormatExceptionForDateFormatWhenMissingYearOrMonthOrDayPart(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, true));
            }

            [Test]
            [TestCaseSource(nameof(GetDateTimeFormatInfoForDateTimeFormatTestCases))]
            public void ReturnsCorrectDateFormatInfoForDateTimeFormat(string format, DateTimeFormatInfo expectedResult)
            {
                var result = DateTimeFormatHelper.GetDateTimeFormatInfo(format, false);

                Assert.That(AreEqual(result, expectedResult), Is.True);
            }

            [TestCase("yyyy-yyyy-MM HH:mm:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenYearPartExistsMoreThanOnce(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-MM HH:mm:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenMonthPartExistsMoreThanOnce(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-dd-dd HH:mm:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenDayPartExistsMoreThanOnce(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-dd HH:HH:ss")]
            [TestCase("yyyy-MM-dd hh:hh:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenHourPartExistsMoreThanOnce(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-dd HH:mm:mm")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenMinutePartExistsMoreThanOnce(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-dd HH:ss:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenSecondPartExistsMoreThanOnce(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-dd HH:tt:tt")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenAmPmPartExistsMoreThanOnce(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-dd HH:hh:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenHour12PartAndHour24PartExistsBoth(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyyyy-MM-dd HH:mm:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenYearPartFormatIsIncorrect(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MMMMM-dd HH:mm:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenMonthPartFormatIsIncorrect(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-ddddd HH:mm:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenDayPartFormatIsIncorrect(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-dd HHH:mm:ss")]
            [TestCase("yyyy-MM-dd hhh:mm:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenHourPartFormatIsIncorrect(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-dd HH:mmm:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenMinutePartFormatIsIncorrect(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-dd HH:mm:sss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenSecondPartFormatIsIncorrect(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-dd HH:mm:ss ttt")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenAmPmPartFormatIsIncorrect(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            [TestCase("yyyy-MM-dd HH:mm:ss")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenDateFormatExpected(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, true));
            }

            [TestCase("yyyy-MM HH:mm:ss")]
            [TestCase("MM-dd HH:mm:ss")]
            [TestCase("yyyy-dd HH:mm:ss")]
            //[TestCase("yyyy-MM-dd mm:ss")]
            //[TestCase("yyyy-MM-dd HH:ss")]
            //[TestCase("yyyy-MM-dd HH:mm")]
            public void ThrowsFormatExceptionForDateTimeFormatWhenMissingYearOrMonthOrDayOrHourOrMinuteOrSecondPart(string format)
            {
                Assert.Throws<FormatException>(() => DateTimeFormatHelper.GetDateTimeFormatInfo(format, false));
            }

            private bool AreEqual(DateTimeFormatInfo info1, DateTimeFormatInfo info2)
            {
                return (
                    object.Equals(info1.AmPmFormat, info2.AmPmFormat) &&
                    object.Equals(info1.AmPmPosition, info2.AmPmPosition) &&
                    object.Equals(info1.DayFormat, info2.DayFormat) &&
                    object.Equals(info1.DayPosition, info2.DayPosition) &&
                    object.Equals(info1.HourFormat, info2.HourFormat) &&
                    object.Equals(info1.HourPosition, info2.HourPosition) &&
                    object.Equals(info1.IsAmPmShortFormat, info2.IsAmPmShortFormat) &&
                    object.Equals(info1.IsHour12Format, info2.IsHour12Format) &&
                    object.Equals(info1.IsYearShortFormat, info2.IsYearShortFormat) &&
                    object.Equals(info1.MinuteFormat, info2.MinuteFormat) &&
                    object.Equals(info1.MinutePosition, info2.MinutePosition) &&
                    object.Equals(info1.MonthFormat, info2.MonthFormat) &&
                    object.Equals(info1.MonthPosition, info2.MonthPosition) &&
                    object.Equals(info1.SecondFormat, info2.SecondFormat) &&
                    object.Equals(info1.SecondPosition, info2.SecondPosition) &&
                    object.Equals(info1.Separator0, info2.Separator0) &&
                    object.Equals(info1.Separator1, info2.Separator1) &&
                    object.Equals(info1.Separator2, info2.Separator2) &&
                    object.Equals(info1.Separator3, info2.Separator3) &&
                    object.Equals(info1.Separator4, info2.Separator4) &&
                    object.Equals(info1.Separator5, info2.Separator5) &&
                    object.Equals(info1.Separator6, info2.Separator6) &&
                    object.Equals(info1.Separator7, info2.Separator7) &&
                    object.Equals(info1.YearFormat, info2.YearFormat) &&
                    object.Equals(info1.YearPosition, info2.YearPosition)
                );
            }
        }
    }
}
