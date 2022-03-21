namespace Orc.Controls.Tests.UI
{
    using System.Reflection;
    using NUnit.Framework;

    [RequiresThread(System.Threading.ApartmentState.STA)]
    [TestFixture]
    internal class NumericTextBoxFacts
    {
        [TestCase("", true)]
        [TestCase("-0", false)]
        [TestCase("-0.", false)]
        [TestCase("-2.5", true)]
        [TestCase("1.5", true)]
        public void DoesStringValueRequireUpdateMethodExpectedUpdate(string text, bool expectedUpdateFlag)
        {
            var numericTextBox = new NumericTextBox();
            var callTestMethod = numericTextBox.GetType().GetMethod("DoesStringValueRequireUpdate", BindingFlags.NonPublic | BindingFlags.Static);
            var actualUpdateFlag = (bool)callTestMethod.Invoke(numericTextBox, new object[] { text });

            Assert.AreEqual(expectedUpdateFlag, actualUpdateFlag);
        }
    }
}
