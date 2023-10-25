namespace Orc.Controls.Tests.UI
{
    using System.Windows.Media;
    using NUnit.Framework;
    using Orc.Automation;

    [Explicit]
    [TestFixture(TestOf = typeof(FontImageControl))]
    [Category("UI Tests")]
    public class FontImageTestFacts : StyledControlTestFacts<FontImageControl>
    {
        [Target]
        public Automation.FontImageControl Target { get; set; }

        [Test]
        public void CorrectlyInitialize()
        {
            var target = Target;

            var model = target.Current;

            model.Foreground = Brushes.Red;
            model.FontFamily = "Segoe UI";
            model.ItemName = "WOW";
        }
    }
}
