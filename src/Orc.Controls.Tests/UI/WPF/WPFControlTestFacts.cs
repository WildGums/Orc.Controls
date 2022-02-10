namespace Orc.Controls.Tests
{
    using System;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;
    using Calendar = System.Windows.Controls.Calendar;


    [TestFixture, Explicit]
    [Category("UI Tests")]
    public class WpfControlTestFacts : ControlUiTestFactsBase<Calendar>
    {
        [Target]
        public Orc.Automation.Controls.Calendar Target { get; set; }

        [Test]
        public void CorrectlyRun()
        {
            var target = Target;

            target.SelectedDate = new DateTime(2121, 12, 20);
        }
    }
}
