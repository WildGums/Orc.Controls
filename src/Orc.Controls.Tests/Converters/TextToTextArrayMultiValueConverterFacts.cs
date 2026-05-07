namespace Orc.Controls.Tests;

using System;
using System.Globalization;
using Converters;
using NUnit.Framework;

public class TextToTextArrayMultiValueConverterFacts
{
    [TestFixture]
    public class The_Convert_Method
    {
        [Test]
        public void Returns_Clone_Of_Input_Array()
        {
            var converter = new TextToTextArrayMultiValueConverter();
            var values = new object[] { "hello", "world" };

            var result = converter.Convert(values, typeof(object[]), null, CultureInfo.InvariantCulture);

            Assert.That(result, Is.Not.SameAs(values));
            Assert.That(result, Is.EquivalentTo(values));
        }

        [Test]
        public void Returns_Null_For_Null_Input()
        {
            var converter = new TextToTextArrayMultiValueConverter();

            var result = converter.Convert(null, typeof(object[]), null, CultureInfo.InvariantCulture);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Returns_Empty_Clone_For_Empty_Array()
        {
            var converter = new TextToTextArrayMultiValueConverter();
            var values = new object[] { };

            var result = converter.Convert(values, typeof(object[]), null, CultureInfo.InvariantCulture);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(values));
        }
    }

    [TestFixture]
    public class The_ConvertBack_Method
    {
        [Test]
        public void Throws_NotImplementedException()
        {
            var converter = new TextToTextArrayMultiValueConverter();

            Assert.Throws<NotImplementedException>(() =>
                converter.ConvertBack("hello", new[] { typeof(string) }, null, CultureInfo.InvariantCulture));
        }
    }
}
