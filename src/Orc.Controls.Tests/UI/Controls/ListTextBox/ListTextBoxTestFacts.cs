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
    public class ListTextBoxTestFacts : StyledControlTestFacts<ListTextBox>
    {
        [Target]
        public Automation.ListTextBox Target { get; set; }

        [TestCase("Test text")]
        [TestCase("")]
        public void CorrectlySetTextWithoutListOfValues(string inputText)
        {
            var target = Target;
            var model = target.Current;

            target.Text = inputText;

            Assert.That(model.Text, Is.EqualTo(target.Text));

            //Value should be null because no List of choices specified
            Assert.That(model.Value, Is.Null);
        }

        [Test(Description = "Value and text should be cleared after setting list of values, if existing text not in a list of values")]
        public void CorrectlySetListOfValues()
        {
            var target = Target;
            var model = target.Current;

            target.Text = "Some text not in a list";

            model.ListOfValues = new List<string> { "Text in a list", "Second text in a list", "Third text in a list" };

            Assert.That(model.Text, Is.Empty);
            Assert.That(model.Value, Is.Null);
        }

        [Test(Description = "Value and text shouldn't be cleared after setting list of values, if existing text in a list of values")]
        public void CorrectlySetListOfValues2()
        {
            var target = Target;
            var model = target.Current;

            const string textInList = "Some text in a list";

            target.Text = textInList;

            model.ListOfValues = new List<string> { textInList, "Second text in a list", "Third text in a list" };

            Assert.That(model.Text, Is.EqualTo(textInList));
            Assert.That(model.Value, Is.EqualTo(textInList));
        }

        [TestCaseSource(typeof(ListOfValuesWithDifferentFirstTwoLettersTestSource))]
        public void CorrectlyEnumerateUpListOfValues(List<string> listOfValues)
        {
            var target = Target;
            var model = target.Current;

            model.ListOfValues = listOfValues;

            target.SetFocus();

            Wait.UntilResponsive();

            foreach (var value in listOfValues)
            {
                KeyboardInput.PressRelease(Key.Up);

                Wait.UntilResponsive();

                Assert.That(model.Value, Is.EqualTo(model.Text));
                Assert.That(model.Text, Is.EqualTo(value));
            }
        }

        [TestCaseSource(typeof(ListOfValuesWithDifferentFirstTwoLettersTestSource))]
        public void CorrectlyEnumerateDownListOfValues(List<string> listOfValues)
        {
            var target = Target;
            var model = target.Current;

            model.ListOfValues = listOfValues;

            target.SetFocus();

            Wait.UntilResponsive();

            for (var index = listOfValues.Count - 1; index >= 0; index--)
            {
                var value = listOfValues[index];
                KeyboardInput.PressRelease(Key.Down);

                Wait.UntilResponsive();

                Assert.That(model.Value, Is.EqualTo(model.Text));
                Assert.That(model.Text, Is.EqualTo(value));
            }
        }

        [Test]
        public void CorrectlyPreventChoiceWhenValueDoesNotMatch()
        {
            var target = Target;
            var model = target.Current;

            model.ListOfValues = new List<string> { "ABC", "DEF", "GHI" };

            //Text should be typed by Keyboard
            target.SetFocus();
            KeyboardInput.Type("JKL");

            Assert.That(model.Value, Is.Null);
            Assert.That(model.Text, Is.Empty.Or.Null);
        }

        [TestCaseSource(typeof(ListOfValuesWithDifferentFirstTwoLettersTestSource))]
        public void CorrectlyStartTyping(List<string> listOfValues)
        {
            var target = Target;
            var model = target.Current;

            model.ListOfValues = listOfValues;

            target.SetFocus();

            Wait.UntilResponsive();

            foreach (var value in listOfValues)
            {
                var firstTwoLetters = value[..2];

                target.SetFocus();

                KeyboardInput.Type(firstTwoLetters);

                Wait.UntilResponsive();

                Assert.That(target.Text, Is.EqualTo(value));
            }
        }

        private class ListOfValuesWithDifferentFirstTwoLettersTestSource : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new List<string>{ "One", "Two", "Three", "Four", "Five"};
            }
        }
    }
}
