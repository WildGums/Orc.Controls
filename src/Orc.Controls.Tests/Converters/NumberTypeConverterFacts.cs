namespace Orc.Controls.Tests;

using System;
using System.Globalization;
using Converters;
using NUnit.Framework;

[TestFixture]
public class NumberTypeConverterFacts
{
    [TestFixture]
    public class The_CanConvertFrom_Method
    {
        [Test]
        public void Returns_True_For_String_Type()
        {
            var converter = new NumberTypeConverter();

            Assert.That(converter.CanConvertFrom(null, typeof(string)), Is.True);
        }

        [TestCase(typeof(int))]
        [TestCase(typeof(double))]
        [TestCase(typeof(float))]
        [TestCase(typeof(long))]
        public void Returns_True_For_Numeric_Types(Type type)
        {
            var converter = new NumberTypeConverter();

            Assert.That(converter.CanConvertFrom(null, type), Is.True);
        }

        [Test]
        public void Returns_False_For_Non_Numeric_Non_String_Type()
        {
            var converter = new NumberTypeConverter();

            Assert.That(converter.CanConvertFrom(null, typeof(DateTime)), Is.False);
        }
    }

    [TestFixture]
    public class The_ConvertFrom_Method
    {
        [Test]
        public void Returns_Number_From_String()
        {
            var converter = new NumberTypeConverter();

            var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, "42");

            Assert.That(result, Is.InstanceOf<Number>());
            Assert.That(((Number)result).DValue, Is.EqualTo(42d));
        }

        [Test]
        public void Returns_Empty_String_For_Whitespace_String()
        {
            var converter = new NumberTypeConverter();

            var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, "   ");

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Returns_Null_For_Null_Input()
        {
            var converter = new NumberTypeConverter();

            var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, null);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Returns_Number_From_Int_Value()
        {
            var converter = new NumberTypeConverter();

            var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, 100);

            Assert.That(result, Is.InstanceOf<Number>());
            Assert.That(((Number)result).DValue, Is.EqualTo(100d));
        }
    }

    [TestFixture]
    public class The_CanConvertTo_Method
    {
        [Test]
        public void Returns_True_For_String_Type()
        {
            var converter = new NumberTypeConverter();

            Assert.That(converter.CanConvertTo(null, typeof(string)), Is.True);
        }

        [TestCase(typeof(int))]
        [TestCase(typeof(double))]
        [TestCase(typeof(float))]
        public void Returns_True_For_Numeric_Types(Type type)
        {
            var converter = new NumberTypeConverter();

            Assert.That(converter.CanConvertTo(null, type), Is.True);
        }

        [Test]
        public void Returns_False_For_Non_Numeric_Non_String_Type()
        {
            var converter = new NumberTypeConverter();

            Assert.That(converter.CanConvertTo(null, typeof(DateTime)), Is.False);
        }
    }
}
