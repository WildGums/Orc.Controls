namespace Orc.Controls.Tests.Controls.Callout
{
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CalloutExtensionsFacts
    {
        [TestCase(null, null, "Callouts.Unnamed.Default")]
        [TestCase("MyName", null, "Callouts.MyName.Default")]
        [TestCase("MyName", "1.0.0", "Callouts.MyName.1.0.0")]
        public void GetCalloutConfigurationKeyPrefix(string name, string version, string expected)
        {
            var callout = new Mock<ICallout>();
            callout.Setup(x => x.Name).Returns(name);
            callout.Setup(x => x.Version).Returns(version);

            var actual = ICalloutExtensions.GetCalloutConfigurationKeyPrefix(callout.Object);

            Assert.AreEqual(expected, actual);
        }
    }
}
