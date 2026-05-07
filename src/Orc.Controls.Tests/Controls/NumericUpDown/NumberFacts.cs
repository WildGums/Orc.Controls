namespace Orc.Controls.Tests;

using NUnit.Framework;

[TestFixture]
public class NumberFacts
{
    [TestFixture]
    public class The_Constructor
    {
        [Test]
        public void Creates_Valid_Number_From_Int()
        {
            var number = new Number(42);

            Assert.That(number.IsValid, Is.True);
            Assert.That(number.DValue, Is.EqualTo(42d));
        }

        [Test]
        public void Creates_Valid_Number_From_Double()
        {
            var number = new Number(3.14);

            Assert.That(number.IsValid, Is.True);
            Assert.That(number.DValue, Is.EqualTo(3.14d).Within(1e-10));
        }

        [Test]
        public void Creates_Invalid_Number_For_Non_Numeric_Type()
        {
            var number = new Number("hello", typeof(string));

            Assert.That(number.IsValid, Is.False);
        }

        [Test]
        public void Sets_MinValue_And_MaxValue_For_Byte()
        {
            var number = new Number((byte)100);

            Assert.That(number.MinValue, Is.EqualTo(byte.MinValue));
            Assert.That(number.MaxValue, Is.EqualTo(byte.MaxValue));
        }
    }

    [TestFixture]
    public class The_ComparisonOperators
    {
        [Test]
        public void Greater_Than_Returns_True_When_Left_Is_Larger()
        {
            Number left = 10.0;
            Number right = 5.0;

            Assert.That(left > right, Is.True);
        }

        [Test]
        public void Less_Than_Returns_True_When_Left_Is_Smaller()
        {
            Number left = 3.0;
            Number right = 7.0;

            Assert.That(left < right, Is.True);
        }
    }

    [TestFixture]
    public class The_ArithmeticOperators
    {
        [Test]
        public void Addition_With_Double_Returns_Correct_Result()
        {
            Number number = 10.0;

            var result = number + 5.0;

            Assert.That(result.DValue, Is.EqualTo(15d));
        }

        [Test]
        public void Subtraction_With_Double_Returns_Correct_Result()
        {
            Number number = 10.0;

            var result = number - 3.0;

            Assert.That(result.DValue, Is.EqualTo(7d));
        }
    }

    [TestFixture]
    public class The_ImplicitConversions
    {
        [Test]
        public void Implicit_Cast_From_Int_Creates_Number()
        {
            Number number = 42;

            Assert.That(number.DValue, Is.EqualTo(42d));
        }

        [Test]
        public void Implicit_Cast_To_Double_Returns_DValue()
        {
            Number number = 99.0;

            double value = number;

            Assert.That(value, Is.EqualTo(99d));
        }
    }
}
