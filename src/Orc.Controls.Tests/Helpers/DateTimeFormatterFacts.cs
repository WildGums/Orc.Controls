// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormatterFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.Tests;
    using NUnit.Framework;

    public class DateTimeFormatterFacts
    {
        [TestFixture]
        public class TheFormatMethod
        {
            private static readonly object[][] FormatDateTestCases = new object[][]
            {
                // Years
                new object []
                {
                    new DateTime(2009, 2, 12, 0, 0, 0),
                    "y-MM-dd",
                    "9-02-12"
                },
                new object []
                {
                    new DateTime(2009, 2, 12, 0, 0, 0),
                    "yy-MM-dd",
                    "09-02-12"
                },
                new object []
                {
                    new DateTime(917, 2, 12, 0, 0, 0),
                    "yyy-MM-dd",
                    "917-02-12"
                },
                new object []
                {
                    new DateTime(2017, 2, 12, 0, 0, 0),
                    "yyy-MM-dd",
                    "2017-02-12"
                },
                new object []
                {
                    new DateTime(2017, 2, 12, 0, 0, 0),
                    "yyyy-MM-dd",
                    "2017-02-12"
                },
                new object []
                {
                    new DateTime(2017, 2, 12, 0, 0, 0),
                    "yyyyy-MM-dd",
                    "02017-02-12"
                },

                // Months
                new object []
                {
                    new DateTime(2017, 2, 12, 0, 0, 0),
                    "yyyy-M-dd",
                    "2017-2-12"
                },
                new object []
                {
                    new DateTime(2017, 2, 12, 0, 0, 0),
                    "yyyy-MM-dd",
                    "2017-02-12"
                },

                // Days
                new object []
                {
                    new DateTime(2017, 2, 1, 0, 0, 0),
                    "yyyy-MM-d",
                    "2017-02-1"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 0, 0, 0),
                    "yyyy-MM-dd",
                    "2017-02-01"
                },
            };

            private static readonly object[][] FormatDateTimeTestCases = new object[][]
            {
                // Years
                new object []
                {
                    new DateTime(2009, 2, 12, 9, 17, 31),
                    "y-MM-dd HH:mm:ss",
                    "9-02-12 09:17:31"
                },
                new object []
                {
                    new DateTime(2009, 2, 12, 9, 17, 31),
                    "yy-MM-dd HH:mm:ss",
                    "09-02-12 09:17:31"
                },
                new object []
                {
                    new DateTime(917, 2, 12, 9, 17, 31),
                    "yyy-MM-dd HH:mm:ss",
                    "917-02-12 09:17:31"
                },
                new object []
                {
                    new DateTime(2017, 2, 12, 9, 17, 31),
                    "yyy-MM-dd HH:mm:ss",
                    "2017-02-12 09:17:31"
                },
                new object []
                {
                    new DateTime(2017, 2, 12, 9, 17, 31),
                    "yyyy-MM-dd HH:mm:ss",
                    "2017-02-12 09:17:31"
                },
                new object []
                {
                    new DateTime(2017, 2, 12, 9, 17, 31),
                    "yyyyy-MM-dd HH:mm:ss",
                    "02017-02-12 09:17:31"
                },

                // Months
                new object []
                {
                    new DateTime(2017, 2, 12, 9, 17, 31),
                    "yyyy-M-dd HH:mm:ss",
                    "2017-2-12 09:17:31"
                },
                new object []
                {
                    new DateTime(2017, 2, 12, 9, 17, 31),
                    "yyyy-MM-dd HH:mm:ss",
                    "2017-02-12 09:17:31"
                },

                // Days
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 17, 31),
                    "yyyy-MM-d HH:mm:ss",
                    "2017-02-1 09:17:31"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 17, 31),
                    "yyyy-MM-dd HH:mm:ss",
                    "2017-02-01 09:17:31"
                },

                // Hours
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 17, 31),
                    "yyyy-MM-dd H:mm:ss",
                    "2017-02-01 9:17:31"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 17, 31),
                    "yyyy-MM-dd HH:mm:ss",
                    "2017-02-01 09:17:31"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 17, 31),
                    "yyyy-MM-dd h:mm:ss tt",
                    "2017-02-01 9:17:31 AM"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 17, 31),
                    "yyyy-MM-dd hh:mm:ss tt",
                    "2017-02-01 09:17:31 AM"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 14, 17, 31),
                    "yyyy-MM-dd h:mm:ss tt",
                    "2017-02-01 2:17:31 PM"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 14, 17, 31),
                    "yyyy-MM-dd hh:mm:ss tt",
                    "2017-02-01 02:17:31 PM"
                },

                // Minutes
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 9, 31),
                    "yyyy-MM-d HH:m:ss",
                    "2017-02-1 09:9:31"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 9, 31),
                    "yyyy-MM-dd HH:mm:ss",
                    "2017-02-01 09:09:31"
                },

                // Seconds
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 9, 6),
                    "yyyy-MM-d HH:mm:s",
                    "2017-02-1 09:09:6"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 9, 6),
                    "yyyy-MM-dd HH:mm:ss",
                    "2017-02-01 09:09:06"
                },

                // AM/PM
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 9, 6),
                    "yyyy-MM-d hh:mm:ss t",
                    "2017-02-1 09:09:06 A"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 9, 9, 6),
                    "yyyy-MM-d hh:mm:ss tt",
                    "2017-02-1 09:09:06 AM"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 14, 9, 6),
                    "yyyy-MM-dd hh:mm:ss t",
                    "2017-02-01 02:09:06 P"
                },
                new object []
                {
                    new DateTime(2017, 2, 1, 14, 9, 6),
                    "yyyy-MM-dd hh:mm:ss tt",
                    "2017-02-01 02:09:06 PM"
                },
            };

            [Test]
            [TestCaseSource("FormatDateTestCases")]
            public void CorrectlyFormatDate(DateTime dateTime, string format, string expectedResult)
            {
                var result = DateTimeFormatter.Format(dateTime, format, true);

                Assert.That(result, Is.EqualTo(expectedResult));
            }

            [Test]
            [TestCaseSource("FormatDateTimeTestCases")]
            public void CorrectlyFormatDateTime(DateTime dateTime, string format, string expectedResult)
            {
                var result = DateTimeFormatter.Format(dateTime, format, false);

                Assert.That(result, Is.EqualTo(expectedResult));
            }
        }
    }
}
