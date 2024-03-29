﻿namespace Orc.Controls.Tests.UI
{
    using NUnit.Framework;
    using Orc.Automation;

    [Explicit]
    [TestFixture(TestOf = typeof(AnimatedGif))]
    [Category("UI Tests")]
    public class AnimatedGifTestFacts : StyledControlTestFacts<AnimatedGif>
    {
        [Target]
        public Automation.AnimatedGif Target { get; set; }

        [Test]
        public void CorrectlyInitialize()
        {
            var target = Target;

            var model = target.Current;

            //model.GifSource = "C:\\Temp\\keys.gif";
        }
    }
}
