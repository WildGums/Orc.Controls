namespace Orc.Controls.Tests.UI
{
    using System.Windows.Media;
    using NUnit.Framework;
    using Orc.Automation;

    [Explicit]
    [TestFixture(TestOf = typeof(AlignmentGrid))]
    [Category("UI Tests")]
    public class AlignmentGridTestFacts : StyledControlTestFacts<AlignmentGrid>
    {
        [Target]
        public Automation.AlignmentGrid Target { get; set; }

        [Test]
        public void CorrectlyInitialize()
        {
            var target = Target;

            var model = target.Current;

            model.HorizontalStep = 10d;
            model.VerticalStep = 10d;
            model.LineBrush = Brushes.Blue;
        }
    }
}
