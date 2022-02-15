namespace Orc.Controls.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Input;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(NumericUpDown))]
    [Category("UI Tests")]
    public class ListTextBoxTestFacts : ControlUiTestFactsBase<ListTextBox>
    {
        [Target]
        public Automation.ListTextBox Target { get; set; }

        [TestCaseSource(typeof(ListOfValuesTestSource))]
        public void CorrectlyChooseValue(List<string> listOfValues)
        {
            var target = Target;
            var model = target.Current;

            model.ListOfValues = listOfValues;

            target.SetFocus();

            Wait.UntilResponsive();

            KeyboardInput.PressRelease(Key.Up);

            Assert.That(model.Value, Is.EqualTo(model.Text));
            Assert.That(model.Text, Is.EqualTo(listOfValues[0]));
        }

        [TestCaseSource(typeof(ListOfValuesTestSource))]
        public void CorrectlyStartTyping(List<string> listOfValues)
        {
            var target = Target;
            var model = target.Current;

            model.ListOfValues = listOfValues;

            target.SetFocus();

            Wait.UntilResponsive();

            foreach (var value in listOfValues)
            {
                var firstLetters = value[..(value.Length / 2)];

                target.SetFocus();

                KeyboardInput.Type(firstLetters);

                Wait.UntilResponsive();

                Assert.That(target.Text, Is.EqualTo(value));
            }
        }

        private class ListOfValuesTestSource : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new List<string>{ "One", "Two", "Three", "Four", "Five"};
            }
        }
    }
}
