namespace Orc.Controls.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class StringExtensionsFacts
    {
        [TestCase("1")]
        [TestCase("/Item //d/")]
        [TestCase("\"Item //d\"")]
        public void CorrectlyGetRegexStringFromSearchPattern(string pattern)
        {
            var regExPattern = StringExtensions.GetRegexStringFromSearchPattern(pattern);
        }
    }
}
