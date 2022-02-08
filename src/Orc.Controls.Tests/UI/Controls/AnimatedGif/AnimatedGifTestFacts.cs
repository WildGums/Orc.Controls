namespace Orc.Controls.Tests.UI
{
    using System.Windows.Media;
    using NUnit.Framework;
    using Orc.Automation;

    public class AnimatedGifTestFacts : StyledControlTestFacts<AnimatedGif>
    {
        [Target]
        public Automation.AnimatedGif Target { get; set; }

        [Test]
        public void CorrectlyInitialize()
        {
            var target = Target;

            var model = target.Current;

            model.GifSource = "C:\\Temp\\keys.gif";
        }
    }
}
