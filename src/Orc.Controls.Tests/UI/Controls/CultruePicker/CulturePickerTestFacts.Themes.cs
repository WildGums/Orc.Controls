namespace Orc.Controls.Tests.UI
{
    using System.Windows.Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Controls;
    using Orc.Automation.Tests.ThemingTests.ThemeAsserters;

    public partial class CulturePickerTestFacts
    {
        [Test]
        public void VerifyThemes() 
        {
            var target = Target;

            var background2 = target.Current.Background;

            var map = new CulturePickerMap(target.Element);
 
            var combobox = map.Combobox;

            var combobox1 = target.Element.Find(controlType: ControlType.ComboBox);
            var access = new AutomationElementAccessor(combobox1);
            var backgroundAccess = access.GetValue("Background");


            var comboboxControl = new ComboBox(combobox1);
            var background1 = comboboxControl.Current.Background;


            var background = combobox.Current.Background;

            ComboboxThemeAssert.VerifyTheme(combobox);
        }
    }
}
