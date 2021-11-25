namespace Orc.Controls.Tests
{
    using System;
    using FlaUI.Core;
    using FlaUI.Core.AutomationElements;
    using FlaUI.Core.Conditions;
    using FlaUI.Core.Definitions;
    using FlaUI.UIA3;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;
    using AutomationElementExtensions = FlaUI.Core.AutomationElements.AutomationElementExtensions;
    using Calendar = System.Windows.Controls.Calendar;


    [TestFixture, Explicit]
    public class WpfControlTestFacts : ControlUiTestFactsBase<Calendar>
    {
        [Target]
        public CalendarAutomationElement Target { get; set; }

        [Test]
        public void CorrectlyRun()
        {
            using (var automation = new UIA3Automation())
            {
                Target.Element.GetClickablePoint();

                var conveted = automation.FromPoint(Target.Element.GetClickablePoint().ToDrawingPoint());

                var calendar1 = AutomationElementExtensions.AsCalendar(conveted);

                calendar1.SelectDate(DateTime.Today + TimeSpan.FromDays(42));

                var slider = AutomationElementExtensions.AsToggleButton(conveted);

                var dateTimePicker = AutomationElementExtensions.AsDateTimePicker(conveted);
                var checkBox = AutomationElementExtensions.AsCheckBox(conveted);
            }
        }
    }
}
