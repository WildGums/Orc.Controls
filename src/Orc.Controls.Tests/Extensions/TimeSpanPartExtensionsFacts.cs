namespace Orc.Controls.Tests;

using System;
using NUnit.Framework;

[TestFixture]
public class TimeSpanPartExtensionsFacts
{
    [TestFixture]
    public class The_CreateTimeSpan_Method
    {
        [Test]
        public void Creates_TimeSpan_From_Days()
        {
            var result = TimeSpanPart.Days.CreateTimeSpan(3);

            Assert.That(result, Is.EqualTo(TimeSpan.FromDays(3)));
        }

        [Test]
        public void Creates_TimeSpan_From_Hours()
        {
            var result = TimeSpanPart.Hours.CreateTimeSpan(5);

            Assert.That(result, Is.EqualTo(TimeSpan.FromHours(5)));
        }

        [Test]
        public void Creates_TimeSpan_From_Minutes()
        {
            var result = TimeSpanPart.Minutes.CreateTimeSpan(30);

            Assert.That(result, Is.EqualTo(TimeSpan.FromMinutes(30)));
        }

        [Test]
        public void Creates_TimeSpan_From_Seconds()
        {
            var result = TimeSpanPart.Seconds.CreateTimeSpan(45);

            Assert.That(result, Is.EqualTo(TimeSpan.FromSeconds(45)));
        }
    }

    [TestFixture]
    public class The_GetTimeSpanPartValue_Method
    {
        [Test]
        public void Returns_TotalDays_For_Days_Part()
        {
            var timeSpan = TimeSpan.FromDays(2.5);

            var result = timeSpan.GetTimeSpanPartValue(TimeSpanPart.Days);

            Assert.That(result, Is.EqualTo(timeSpan.TotalDays));
        }

        [Test]
        public void Returns_TotalHours_For_Hours_Part()
        {
            var timeSpan = TimeSpan.FromHours(3);

            var result = timeSpan.GetTimeSpanPartValue(TimeSpanPart.Hours);

            Assert.That(result, Is.EqualTo(timeSpan.TotalHours));
        }
    }

    [TestFixture]
    public class The_GetTimeSpanPartName_Method
    {
        [TestCase(TimeSpanPart.Days, "days")]
        [TestCase(TimeSpanPart.Hours, "hours")]
        [TestCase(TimeSpanPart.Minutes, "minutes")]
        [TestCase(TimeSpanPart.Seconds, "seconds")]
        public void Returns_Correct_Name(TimeSpanPart part, string expectedName)
        {
            var result = part.GetTimeSpanPartName();

            Assert.That(result, Is.EqualTo(expectedName));
        }
    }
}
