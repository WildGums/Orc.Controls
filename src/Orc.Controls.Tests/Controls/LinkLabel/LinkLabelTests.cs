namespace Orc.Controls.Tests;

using System;
using NUnit.Framework;

[TestFixture]
public class LinkLabelTests
{
    [TestCase("catelproject.com", UriKind.Relative, "https://catelproject.com/")]
    [TestCase("myapp://myaction", UriKind.Absolute, "myapp://myaction/")]
    [TestCase("С:\\my\\long\\path\\to\\exe\\file\\exe.exe", UriKind.Relative, "С:\\my\\long\\path\\to\\exe\\file\\exe.exe")]
    public void BuildDestinationUrl_Returns_Correct_Url(string uriPath, UriKind kind, string expectedUrl)
    {
        var uri = new Uri(uriPath, kind);
        var result = LinkLabel.BuildDestinationUrl(uri);
        Assert.That(result, Is.EqualTo(expectedUrl));
    }
}
