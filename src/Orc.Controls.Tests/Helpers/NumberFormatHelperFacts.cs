namespace Orc.Controls.Tests;

using NUnit.Framework;

public class NumberFormatHelperFacts
{
    [TestFixture]
    public class The_GetFormat_Method
    {
        [TestCase(1, "0")]
        [TestCase(2, "00")]
        [TestCase(3, "000")]
        [TestCase(5, "00000")]
        public void Returns_Correct_Zero_Padded_Format(int digits, string expected)
        {
            var result = NumberFormatHelper.GetFormat(digits);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Returns_Empty_String_For_Zero_Digits()
        {
            var result = NumberFormatHelper.GetFormat(0);

            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }
}
