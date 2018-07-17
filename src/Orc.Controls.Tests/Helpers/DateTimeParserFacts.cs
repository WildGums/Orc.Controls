// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeParserFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.Tests;
    using NUnit.Framework;

    public class DateTimeParserFacts
    {
        [TestFixture]
        public class TheParseMethod
        {
            private static readonly object[][] ParseDateTestCases = new object[][]
            {
                // Years
                new object []
                {
                    "9-02-12",
                    "y-MM-dd",
                    new DateTime(2009, 2, 12, 0, 0, 0)
                },
                new object []
                {
                    "09-02-12",
                    "yy-MM-dd",
                    new DateTime(2009, 2, 12, 0, 0, 0)
                },
                new object []
                {
                    "917-02-12",
                    "yyy-MM-dd",
                    new DateTime(917, 2, 12, 0, 0, 0),
                },
                new object []
                {
                    "2017-02-12",
                    "yyy-MM-dd",
                    new DateTime(2017, 2, 12, 0, 0, 0)
                },
                new object []
                {
                    "2017-02-12",
                    "yyyy-MM-dd",
                    new DateTime(2017, 2, 12, 0, 0, 0)
                },
                new object []
                {
                    "02017-02-12",
                    "yyyyy-MM-dd",
                    new DateTime(2017, 2, 12, 0, 0, 0)
                },

                // Months
                new object []
                {
                    "2017-2-12",
                    "yyyy-M-dd",
                    new DateTime(2017, 2, 12, 0, 0, 0)
                },
                new object []
                {
                    "2017-02-12",
                    "yyyy-MM-dd",
                    new DateTime(2017, 2, 12, 0, 0, 0)
                },

                // Days
                new object []
                {
                    "2017-02-1",
                    "yyyy-MM-d",
                    new DateTime(2017, 2, 1, 0, 0, 0)
                },
                new object []
                {
                    "2017-02-01",
                    "yyyy-MM-dd",
                    new DateTime(2017, 2, 1, 0, 0, 0)
                },

                // Accept bad formats
                new object []
                {
                    "2017-02-01",
                    "-yyyy-MM-dd",
                    new DateTime(2017, 2, 1, 0, 0, 0)
                },
                new object []
                {
                    "2017-02-01",
                    "-yyyy-MM-dd-",
                    new DateTime(2017, 2, 1, 0, 0, 0)
                },
                new object []
                {
                    "2017-02-01",
                    "yyyy-MM-dd-",
                    new DateTime(2017, 2, 1, 0, 0, 0)
                },
            };

            private static readonly object[][] ParseDateTimeTestCases = new object[][]
            {
                // Years
                new object []
                {
                    "9-02-12 09:17:31",
                    "y-MM-dd HH:mm:ss",
                    new DateTime(2009, 2, 12, 9, 17, 31)
                },
                new object []
                {
                    "09-02-12 09:17:31",
                    "yy-MM-dd HH:mm:ss",
                    new DateTime(2009, 2, 12, 9, 17, 31)
                },
                new object []
                {
                    "917-02-12 09:17:31",
                    "yyy-MM-dd HH:mm:ss",
                    new DateTime(917, 2, 12, 9, 17, 31)
                },
                new object []
                {
                    "2017-02-12 09:17:31",
                    "yyy-MM-dd HH:mm:ss",
                    new DateTime(2017, 2, 12, 9, 17, 31)
                },
                new object []
                {
                    "2017-02-12 09:17:31",
                    "yyyy-MM-dd HH:mm:ss",
                    new DateTime(2017, 2, 12, 9, 17, 31)
                },
                new object []
                {
                    "02017-02-12 09:17:31",
                    "yyyyy-MM-dd HH:mm:ss",
                    new DateTime(2017, 2, 12, 9, 17, 31)
                },

                // Months
                new object []
                {
                    "2017-2-12 09:17:31",
                    "yyyy-M-dd HH:mm:ss",
                    new DateTime(2017, 2, 12, 9, 17, 31)
                },
                new object []
                {
                    "2017-02-12 09:17:31",
                    "yyyy-MM-dd HH:mm:ss",
                    new DateTime(2017, 2, 12, 9, 17, 31)
                },

                // Days
                new object []
                {
                    "2017-02-1 09:17:31",
                    "yyyy-MM-d HH:mm:ss",
                    new DateTime(2017, 2, 1, 9, 17, 31)
                },
                new object []
                {
                    "2017-02-01 09:17:31",
                    "yyyy-MM-dd HH:mm:ss",
                    new DateTime(2017, 2, 1, 9, 17, 31)
                },

                // Hours
                new object []
                {
                    "2017-02-01 9:17:31",
                    "yyyy-MM-dd H:mm:ss",
                    new DateTime(2017, 2, 1, 9, 17, 31)
                },
                new object []
                {
                    "2017-02-01 09:17:31",
                    "yyyy-MM-dd HH:mm:ss",
                    new DateTime(2017, 2, 1, 9, 17, 31)
                },
                new object []
                {
                    "2017-02-01 9:17:31 AM",
                    "yyyy-MM-dd h:mm:ss tt",
                    new DateTime(2017, 2, 1, 9, 17, 31)
                },
                new object []
                {
                    "2017-02-01 09:17:31 AM",
                    "yyyy-MM-dd hh:mm:ss tt",
                    new DateTime(2017, 2, 1, 9, 17, 31)
                },
                new object []
                {
                    "2017-02-01 2:17:31 PM",
                    "yyyy-MM-dd h:mm:ss tt",
                    new DateTime(2017, 2, 1, 14, 17, 31)
                },
                new object []
                {
                    "2017-02-01 02:17:31 PM",
                    "yyyy-MM-dd hh:mm:ss tt",
                    new DateTime(2017, 2, 1, 14, 17, 31)
                },

                // More tolerant with hours
                new object []
                {
                    "2017-02-01 13:17:31 PM",
                    "yyyy-MM-dd hh:mm:ss tt",
                    new DateTime(2017, 2, 1, 13, 17, 31)
                },
                new object []
                {
                    "2017-02-01 16:17:31",
                    "yyyy-MM-dd hh:mm:ss tt",
                    new DateTime(2017, 2, 1, 16, 17, 31)
                },
                new object []
                {
                    "2017-02-01 08:17:31 PM",
                    "yyyy-MM-dd hh:mm:ss",
                    new DateTime(2017, 2, 1, 20, 17, 31)
                },
                new object []
                {
                    "2017-02-01 08:17:31 AM",
                    "yyyy-MM-dd hh:mm:ss",
                    new DateTime(2017, 2, 1, 08, 17, 31)
                },
                new object []
                {
                    "2017-02-01 08:17:31 am",
                    "yyyy-MM-dd hh:mm:ss",
                    new DateTime(2017, 2, 1, 08, 17, 31)
                },


                // Minutes
                new object []
                {
                    "2017-02-1 09:9:31",
                    "yyyy-MM-d HH:m:ss",
                    new DateTime(2017, 2, 1, 9, 9, 31)
                },
                new object []
                {
                    "2017-02-01 09:09:31",
                    "yyyy-MM-dd HH:mm:ss",
                    new DateTime(2017, 2, 1, 9, 9, 31)
                },

                // Seconds
                new object []
                {
                    "2017-02-1 09:09:6",
                    "yyyy-MM-d HH:mm:s",
                    new DateTime(2017, 2, 1, 9, 9, 6)
                },
                new object []
                {
                    "2017-02-01 09:09:06",
                    "yyyy-MM-dd HH:mm:ss",
                    new DateTime(2017, 2, 1, 9, 9, 6)
                },

                // AM/PM
                new object []
                {
                    "2017-02-1 09:09:06 A",
                    "yyyy-MM-d hh:mm:ss t",
                    new DateTime(2017, 2, 1, 9, 9, 6)
                },
                new object []
                {
                    "2017-02-1 09:09:06 AM",
                    "yyyy-MM-d hh:mm:ss tt",
                    new DateTime(2017, 2, 1, 9, 9, 6)
                },
                new object []
                {
                    "2017-02-1 09:09:06 a",
                    "yyyy-MM-d hh:mm:ss t",
                    new DateTime(2017, 2, 1, 9, 9, 6)
                },
                new object []
                {
                    "2017-02-1 09:09:06 am",
                    "yyyy-MM-d hh:mm:ss tt",
                    new DateTime(2017, 2, 1, 9, 9, 6)
                },
                new object []
                {
                    "2017-02-01 02:09:06 P",
                    "yyyy-MM-dd hh:mm:ss t",
                    new DateTime(2017, 2, 1, 14, 9, 6)
                },
                new object []
                {
                    "2017-02-01 02:09:06 PM",
                    "yyyy-MM-dd hh:mm:ss tt",
                    new DateTime(2017, 2, 1, 14, 9, 6)
                },
                new object []
                {
                    "2017-02-01 02:09:06 p",
                    "yyyy-MM-dd hh:mm:ss t",
                    new DateTime(2017, 2, 1, 14, 9, 6)
                },
                new object []
                {
                    "2017-02-01 02:09:06 pm",
                    "yyyy-MM-dd hh:mm:ss tt",
                    new DateTime(2017, 2, 1, 14, 9, 6)
                },

                // Accept bad formats
                new object []
                {
                    "2017-02-01 02:09:06 pm",
                    "yyyy-MM-dd hh:mm:ss tt-",
                    new DateTime(2017, 2, 1, 14, 9, 6)
                },
                new object []
                {
                    "2017-02-01 02:09:06 pm",
                    "-yyyy-MM-dd hh:mm:ss tt-",
                    new DateTime(2017, 2, 1, 14, 9, 6)
                },
                new object []
                {
                    "2017-02-01 02:09:06 pm",
                    "-yyyy-MM-dd hh:mm:ss tt",
                    new DateTime(2017, 2, 1, 14, 9, 6)
                },
            };

            [Test]
            [TestCaseSource("parseDateTestCases")]
            public void CorrectlyParseDate(string input, string format, DateTime expectedResult)
            {
                var result = DateTimeParser.Parse(input, format, true);

                Assert.That(result, Is.EqualTo(expectedResult));
            }

            [Test]
            [TestCaseSource("parseDateTimeTestCases")]
            public void CorrectlyParseDateTime(string input, string format, DateTime expectedResult)
            {
                var result = DateTimeParser.Parse(input, format, false);

                Assert.That(result, Is.EqualTo(expectedResult));
            }

            [Test]
            [TestCase("2017-02-01", "y-MM-dd", true, "Invalid year value. Year must contain 1 or 2 digits")]
            [TestCase("2017-02-01 23:40:01", "y-MM-dd HH:mm:ss", false, "Invalid year value. Year must contain 1 or 2 digits")]
            [TestCase("2017-02-01", "yy-MM-dd", true, "Invalid year value. Year must contain 2 digits")]
            [TestCase("2017-02-01 23:40:01", "yy-MM-dd HH:mm:ss", false, "Invalid year value. Year must contain 2 digits")]
            [TestCase("17-02-01", "yyy-MM-dd", true, "Invalid year value. Year must contain 3 or 4 or 5 digits")]
            [TestCase("17-02-01 23:40:01", "yyy-MM-dd HH:mm:ss", false, "Invalid year value. Year must contain 3 or 4 or 5 digits")]
            [TestCase("17-02-01", "yyyy-MM-dd", true, "Invalid year value. Year must contain 4 or 5 digits")]
            [TestCase("17-02-01 23:40:01", "yyyy-MM-dd HH:mm:ss", false, "Invalid year value. Year must contain 4 or 5 digits")]
            [TestCase("17-02-01", "yyyyy-MM-dd", true, "Invalid year value. Year must contain 5 digits")]
            [TestCase("17-02-01 23:40:01", "yyyyy-MM-dd HH:mm:ss", false, "Invalid year value. Year must contain 5 digits")]
            public void ThrowsFormatExceptionWhenYearPartValueIsIncorrect(string input, string format, bool isDateOnly, string expectedMessage)
            {
                ExceptionTester.CallMethodAndExpectException<FormatException>(() => DateTimeParser.Parse(input, format, isDateOnly), x =>
                {
                    return string.Equals(x.Message, expectedMessage);
                });
            }

            [TestCase("2017-002-01", "yyyy-M-dd", true, "Invalid month value. Month must contain 1 or 2 digits")]
            [TestCase("2017-002-01 23:40:01", "yyyy-M-dd HH:mm:ss", false, "Invalid month value. Month must contain 1 or 2 digits")]
            [TestCase("2017-2-01", "yyyy-MM-dd", true, "Invalid month value. Month must contain 2 digits")]
            [TestCase("2017-2-01 23:40:01", "yyyy-MM-dd HH:mm:ss", false, "Invalid month value. Month must contain 2 digits")]
            public void ThrowsFormatExceptionWhenMonthPartValueIsIncorrect(string input, string format, bool isDateOnly, string expectedMessage)
            {
                ExceptionTester.CallMethodAndExpectException<FormatException>(() => DateTimeParser.Parse(input, format, isDateOnly), x =>
                {
                    return string.Equals(x.Message, expectedMessage);
                });
            }

            [TestCase("2017-02-001", "yyyy-MM-d", true, "Invalid day value. Day must contain 1 or 2 digits")]
            [TestCase("2017-02-001 23:40:01", "yyyy-MM-d HH:mm:ss", false, "Invalid day value. Day must contain 1 or 2 digits")]
            [TestCase("2017-02-1", "yyyy-MM-dd", true, "Invalid day value. Day must contain 2 digits")]
            [TestCase("2017-02-1 23:40:01", "yyyy-MM-dd HH:mm:ss", false, "Invalid day value. Day must contain 2 digits")]
            public void ThrowsFormatExceptionWhenDayPartValueIsIncorrect(string input, string format, bool isDateOnly, string expectedMessage)
            {
                ExceptionTester.CallMethodAndExpectException<FormatException>(() => DateTimeParser.Parse(input, format, isDateOnly), x =>
                {
                    return string.Equals(x.Message, expectedMessage);
                });
            }

            [TestCase("2017-02-01 001:40:01", "yyyy-MM-dd H:mm:ss", "Invalid hour value. Hour must contain 1 or 2 digits")]
            [TestCase("2017-02-01 1:40:01", "yyyy-MM-dd HH:mm:ss", "Invalid hour value. Hour must contain 2 digits")]
            [TestCase("2017-02-01 001:40:01", "yyyy-MM-dd h:mm:ss", "Invalid hour value. Hour must contain 1 or 2 digits")]
            [TestCase("2017-02-01 1:40:01", "yyyy-MM-dd hh:mm:ss", "Invalid hour value. Hour must contain 2 digits")]
            public void ThrowsFormatExceptionWhenHourPartValueIsIncorrect(string input, string format, string expectedMessage)
            {
                ExceptionTester.CallMethodAndExpectException<FormatException>(() => DateTimeParser.Parse(input, format, false), x =>
                {
                    return string.Equals(x.Message, expectedMessage);
                });
            }

            [TestCase("2017-02-01 13:004:01", "yyyy-MM-dd HH:m:ss", "Invalid minute value. Minute must contain 1 or 2 digits")]
            [TestCase("2017-02-01 13:4:01", "yyyy-MM-dd HH:mm:ss", "Invalid minute value. Minute must contain 2 digits")]
            public void ThrowsFormatExceptionWhenMinutePartValueIsIncorrect(string input, string format, string expectedMessage)
            {
                ExceptionTester.CallMethodAndExpectException<FormatException>(() => DateTimeParser.Parse(input, format, false), x =>
                {
                    return string.Equals(x.Message, expectedMessage);
                });
            }

            [TestCase("2017-02-01 13:04:001", "yyyy-MM-dd HH:mm:s", "Invalid second value. Second must contain 1 or 2 digits")]
            [TestCase("2017-02-01 13:04:1", "yyyy-MM-dd HH:mm:ss", "Invalid second value. Second must contain 2 digits")]
            public void ThrowsFormatExceptionWhenSecondPartValueIsIncorrect(string input, string format, string expectedMessage)
            {
                ExceptionTester.CallMethodAndExpectException<FormatException>(() => DateTimeParser.Parse(input, format, false), x =>
                {
                    return string.Equals(x.Message, expectedMessage);
                });
            }

            [TestCase("2017-02-01 01:04:01 An", "yyyy-MM-dd hh:mm:ss tt", "Invalid AM/PM designator value")]
            [TestCase("2017-02-01 01:04:01 Pn", "yyyy-MM-dd hh:mm:ss tt", "Invalid AM/PM designator value")]
            [TestCase("2017-02-01 01:04:01 b", "yyyy-MM-dd hh:mm:ss t", "Invalid AM/PM designator value")]
            [TestCase("2017-02-01 01:04:01 r", "yyyy-MM-dd hh:mm:ss t", "Invalid AM/PM designator value")]
            public void ThrowsFormatExceptionWhenAmPmPartValueIsIncorrect(string input, string format, string expectedMessage)
            {
                ExceptionTester.CallMethodAndExpectException<FormatException>(() => DateTimeParser.Parse(input, format, false), x =>
                {
                    return string.Equals(x.Message, expectedMessage);
                });
            }

            [TestCase("2017/02-01 01:04:01 AM", "yyyy-MM-dd hh:mm:ss tt", false)]
            [TestCase("2017/02-01", "yyyy-MM-dd", true)]
            [TestCase("2017-02/01 01:04:01 AM", "yyyy-MM-dd hh:mm:ss tt", false)]
            [TestCase("2017-02/01", "yyyy-MM-dd", true)]
            [TestCase("2017-02-01/01:04:01 AM", "yyyy-MM-dd hh:mm:ss tt", false)]
            [TestCase("2017-02-01 01/04:01 AM", "yyyy-MM-dd hh:mm:ss tt", false)]
            [TestCase("2017-02-01 01:04/01 AM", "yyyy-MM-dd hh:mm:ss tt", false)]
            [TestCase("2017-02-01 01:04:01/AM", "yyyy-MM-dd hh:mm:ss tt", false)]
            public void ThrowsFormatExceptionWhenSeparatorIsInvalid(string input, string format, bool isDateOnly)
            {
                ExceptionTester.CallMethodAndExpectException<FormatException>(() => DateTimeParser.Parse(input, format, isDateOnly), x =>
                {
                    return string.Equals(x.Message, "Invalid value. Value does not match format");
                });
            }

            [TestCase("10931-02-01 01:04:01 AM", "yyyyy-MM-dd hh:mm:ss tt", false)]
            [TestCase("10931-02-01", "yyyyy-MM-dd", true)]
            [TestCase("2017-13-01 01:04:01 AM", "yyyy-MM-dd hh:mm:ss tt", false)]
            [TestCase("2017-13-01", "yyyy-MM-dd", true)]
            [TestCase("2017-02-32 01:04:01 AM", "yyyy-MM-dd hh:mm:ss tt", false)]
            [TestCase("2017-02-32", "yyyy-MM-dd", true)]
            [TestCase("2017-02-01 25:04:01", "yyyy-MM-dd HH:mm:ss", false)]
            [TestCase("2017-02-01 11:61:01 AM", "yyyy-MM-dd hh:mm:ss tt", false)]
            [TestCase("2017-02-01 11:04:61 AM", "yyyy-MM-dd hh:mm:ss tt", false)]
            public void ThrowsFormatExceptionWhenItsNotPossibleToBuildNewDateTimeObject(string input, string format, bool isDateOnly)
            {
                ExceptionTester.CallMethodAndExpectException<FormatException>(() => DateTimeParser.Parse(input, format, isDateOnly), x =>
                {
                    return string.Equals(x.Message, "Invalid value. It's not possible to build new DateTime object");
                });
            }
        }
    }
}
