namespace Orc.Controls.Tests.UI
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
            var current = target.Current;

            var result = $"{333.2324000:##.#######}";
            var result1 = $"{333.2324000:D2}";

            target.SelectedDate = new DateTime(2121, 12, 20);
        }
    }
}
