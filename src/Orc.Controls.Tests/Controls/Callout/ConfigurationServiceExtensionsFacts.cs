namespace Orc.Controls.Tests.Controls.Callout
{
    using System;
    using System.Threading.Tasks;
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
        public async Task SetCalloutLastShown_SetsDate_Async()
        {
            var callout = CreateCallout("Test");
            var configurationKey = callout.GetCalloutConfigurationKeyPrefix();
            var expectedConfigurationKey = $"{configurationKey}.LastShown";

            var expectedDate = DateTime.Now;

            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(x => x.SetValue(It.IsAny<ConfigurationContainer>(), It.IsAny<string>(), It.IsAny<DateTime?>()));

            await configurationServiceMock.Object.SetCalloutLastShownAsync(callout, expectedDate);

            configurationServiceMock.Verify(x => x.SetValue(ConfigurationContainer.Roaming, expectedConfigurationKey, expectedDate));
        }

        [Test]
        public async Task SetCalloutLastShown_ClearsDate_Async()
        {
            var callout = CreateCallout("Test");
            var configurationKey = callout.GetCalloutConfigurationKeyPrefix();
            var expectedConfigurationKey = $"{configurationKey}.LastShown";

            DateTime? expectedDate = null;

            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(x => x.SetValue(It.IsAny<ConfigurationContainer>(), It.IsAny<string>(), It.IsAny<DateTime?>()));

            await configurationServiceMock.Object.SetCalloutLastShownAsync(callout, expectedDate);

            configurationServiceMock.Verify(x => x.SetValue(ConfigurationContainer.Roaming, expectedConfigurationKey, expectedDate));
        }

        [Test]
        public async Task GetCalloutLastShown_Async()
        {
            var callout = CreateCallout("Test");
            var configurationKey = callout.GetCalloutConfigurationKeyPrefix();
            var expectedConfigurationKey = $"{configurationKey}.LastShown";

            DateTime? expectedDate = null;

            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(x => x.GetValue(It.IsAny<ConfigurationContainer>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
                .Returns(() => expectedDate);

            var lastShown = await configurationServiceMock.Object.GetCalloutLastShownAsync(callout);

            Assert.That(lastShown, Is.EqualTo(expectedDate));

            configurationServiceMock.Verify(x => x.GetValue(ConfigurationContainer.Roaming, expectedConfigurationKey, expectedDate));
        }

        [Test]
        public async Task MarkCalloutAsNotShown_Async()
        {
            var callout = CreateCallout("Test");
            var configurationKey = callout.GetCalloutConfigurationKeyPrefix();
            var expectedConfigurationKey = $"{configurationKey}.Shown";

            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(x => x.SetValue(It.IsAny<ConfigurationContainer>(), It.IsAny<string>(), It.IsAny<DateTime?>()));

            await configurationServiceMock.Object.MarkCalloutAsNotShownAsync(callout);

            configurationServiceMock.Verify(x => x.SetValue(ConfigurationContainer.Roaming, expectedConfigurationKey, false));
        }

        [Test]
        public async Task MarkCalloutAsShown_Async()
        {
            var callout = CreateCallout("Test");
            var configurationKey = callout.GetCalloutConfigurationKeyPrefix();
            var expectedConfigurationKey = $"{configurationKey}.Shown";

            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(x => x.SetValue(It.IsAny<ConfigurationContainer>(), It.IsAny<string>(), It.IsAny<DateTime?>()));

            await configurationServiceMock.Object.MarkCalloutAsShownAsync(callout);

            configurationServiceMock.Verify(x => x.SetValue(ConfigurationContainer.Roaming, expectedConfigurationKey, true));
        }
    }
}
