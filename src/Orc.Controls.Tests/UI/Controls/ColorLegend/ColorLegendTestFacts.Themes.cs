namespace Orc.Controls.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation.Tests;

    public partial class ColorLegendTestFacts
    {
        [Test]
        public void VerifyThemes()
        {
            var target = Target;
            var map = target.Map<ColorLegendMap>();

            ListBoxThemeAssert.VerifyTheme(map.ListPart);
            ButtonThemeAssert.VerifyTheme(map.UnselectAllButtonPart);
        }
    }
}
