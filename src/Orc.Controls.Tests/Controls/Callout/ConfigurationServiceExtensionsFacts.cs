namespace Orc.Controls.Tests.Controls.Callout
{
    using System;
    using Catel.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ConfigurationServiceExtensionsFacts
    {
        private ICallout CreateCallout(string name, string version = "1.0")
        {
            var callout = new Mock<ICallout>();
            callout.Setup(x => x.Name).Returns(name);
            callout.Setup(x => x.Version).Returns(version);

            return callout.Object;
        }

        [Test]
        public void SetCalloutLastShown_SetsDate()
        {
            var callout = CreateCallout("Test");
            var configurationKey = callout.GetCalloutConfigurationKeyPrefix();
            var expectedConfigurationKey = $"{configurationKey}.LastShown";

            var expectedDate = DateTime.Now;

            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(x => x.SetValue(It.IsAny<ConfigurationContainer>(), It.IsAny<string>(), It.IsAny<DateTime?>()));

            configurationServiceMock.Object.SetCalloutLastShown(callout, expectedDate);

            configurationServiceMock.Verify(x => x.SetValue(ConfigurationContainer.Roaming, expectedConfigurationKey, expectedDate));
        }

        [Test]
        public void SetCalloutLastShown_ClearsDate()
        {
            var callout = CreateCallout("Test");
            var configurationKey = callout.GetCalloutConfigurationKeyPrefix();
            var expectedConfigurationKey = $"{configurationKey}.LastShown";

            DateTime? expectedDate = null;

            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(x => x.SetValue(It.IsAny<ConfigurationContainer>(), It.IsAny<string>(), It.IsAny<DateTime?>()));

            configurationServiceMock.Object.SetCalloutLastShown(callout, expectedDate);

            configurationServiceMock.Verify(x => x.SetValue(ConfigurationContainer.Roaming, expectedConfigurationKey, expectedDate));
        }

        [Test]
        public void GetCalloutLastShown()
        {
            var callout = CreateCallout("Test");
            var configurationKey = callout.GetCalloutConfigurationKeyPrefix();
            var expectedConfigurationKey = $"{configurationKey}.LastShown";

            DateTime? expectedDate = null;

            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(x => x.GetValue(It.IsAny<ConfigurationContainer>(), It.IsAny<string>(), It.IsAny<DateTime?>())).Returns(expectedDate);

            var lastShown = configurationServiceMock.Object.GetCalloutLastShown(callout);

            Assert.AreEqual(expectedDate, lastShown);

            configurationServiceMock.Verify(x => x.GetValue(ConfigurationContainer.Roaming, expectedConfigurationKey, expectedDate));
        }

        [Test]
        public void MarkCalloutAsNotShown()
        {
            var callout = CreateCallout("Test");
            var configurationKey = callout.GetCalloutConfigurationKeyPrefix();
            var expectedConfigurationKey = $"{configurationKey}.Shown";

            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(x => x.SetValue(It.IsAny<ConfigurationContainer>(), It.IsAny<string>(), It.IsAny<DateTime?>()));

            configurationServiceMock.Object.MarkCalloutAsNotShown(callout);

            configurationServiceMock.Verify(x => x.SetValue(ConfigurationContainer.Roaming, expectedConfigurationKey, false));
        }

        [Test]
        public void MarkCalloutAsShown()
        {
            var callout = CreateCallout("Test");
            var configurationKey = callout.GetCalloutConfigurationKeyPrefix();
            var expectedConfigurationKey = $"{configurationKey}.Shown";

            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(x => x.SetValue(It.IsAny<ConfigurationContainer>(), It.IsAny<string>(), It.IsAny<DateTime?>()));

            configurationServiceMock.Object.MarkCalloutAsShown(callout);

            configurationServiceMock.Verify(x => x.SetValue(ConfigurationContainer.Roaming, expectedConfigurationKey, true));
        }
    }
}
