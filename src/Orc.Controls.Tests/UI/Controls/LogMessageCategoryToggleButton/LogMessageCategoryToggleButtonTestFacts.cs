namespace Orc.Controls.Tests.UI
{
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(NumericUpDown))]
    [Category("UI Tests")]
    public class LogMessageCategoryToggleButtonTestFacts : StyledControlTestFacts<LogMessageCategoryToggleButton>
    {
        [Target]
        public Automation.LogMessageCategoryToggleButton Target { get; set; }

        [Test]
        public void VerifyApi()
        {
            var target = Target;
            var model = target.Current;
            
            //ConnectedPropertiesAssert.VerifyConnectedProperties(model, nameof(model.Category),
            //    target, nameof(target.Category), true,
            //    new ValueTuple<string, string>("Debug", "Debug"),
            //    new ValueTuple<string, string>("WrongData!", string.Empty),
            //    new ValueTuple<string, string>("Warning", "Warning"),
            //    new ValueTuple<string, string>("Error", "Error"));

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, nameof(target.IsToggled),
                model, nameof(model.IsChecked), true, false, true);

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, nameof(target.EntryCount),
                model, nameof(model.EntryCount), true, 1, 20);
        }

        [TestCase("Debug", "Debug")]
        [TestCase("Info", "Info")]
        [TestCase("Warning", "Warning")]
        [TestCase("Error", "Error")]
        [TestCase("Wrong!", "")]
        public void CorrectlySetCategory(string category, string expectedCategory)
        {
            var target = Target;
            var model = target.Current;

            model.Category = category;

            Wait.UntilResponsive();

            Assert.That(target.Category, Is.EqualTo(expectedCategory));

            //TODO:Test control theme
        }
    }
}
