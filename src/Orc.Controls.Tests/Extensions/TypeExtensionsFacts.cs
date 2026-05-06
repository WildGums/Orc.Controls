namespace Orc.Controls.Tests;

using System;
using NUnit.Framework;

[TestFixture]
public class TypeExtensionsFacts
{
    [TestFixture]
    public class TheIsFloatingPointTypeMethod
    {
        [TestCase(typeof(float))]
        [TestCase(typeof(double))]
        [TestCase(typeof(decimal))]
        public void Returns_True_For_Floating_Point_Types(Type type)
        {
            Assert.That(type.IsFloatingPointType(), Is.True);
        }

        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        [TestCase(typeof(byte))]
        [TestCase(typeof(string))]
        public void Returns_False_For_Non_Floating_Point_Types(Type type)
        {
            Assert.That(type.IsFloatingPointType(), Is.False);
        }
    }

    [TestFixture]
    public class TheTryGetNumberRangeMethod
    {
        [TestCase(typeof(int))]
        [TestCase(typeof(double))]
        [TestCase(typeof(byte))]
        [TestCase(typeof(long))]
        public void Returns_Range_For_Numeric_Types(Type type)
        {
            var range = type.TryGetNumberRange();

            Assert.That(range, Is.Not.Null);
        }

        [TestCase(typeof(string))]
        [TestCase(typeof(object))]
        [TestCase(typeof(DateTime))]
        public void Returns_Null_For_Non_Numeric_Types(Type type)
        {
            var range = type.TryGetNumberRange();

            Assert.That(range, Is.Null);
        }

        [Test]
        public void Returns_Correct_Range_For_Int()
        {
            var range = typeof(int).TryGetNumberRange();

            Assert.That(range, Is.Not.Null);
            Assert.That(range.Value.Min, Is.EqualTo((double)int.MinValue));
            Assert.That(range.Value.Max, Is.EqualTo((double)int.MaxValue));
        }
    }
}
