namespace Orc.Controls.Tests;

using Catel.Data;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

[TestFixture]
public class ValidationNamesServiceFacts
{
    [Test]
    public void GetDisplayName_Uses_Localized_Prefix_Resources()
    {
        var serviceCollection = ServiceCollectionHelper.CreateServiceCollection();

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        var languageService = serviceProvider.GetRequiredService<ILanguageService>();
        var validationNamesService = new ValidationNamesService(languageService);

        var validationResultMock = new Mock<IValidationResult>();
        validationResultMock.SetupGet(x => x.Message).Returns("Invalid value");
        validationResultMock.SetupGet(x => x.ValidationResultType).Returns(ValidationResultType.Error);
        validationResultMock.SetupGet(x => x.Tag).Returns(new
        {
            Line = 3,
            ColumnName = "Amount",
            ColumnIndex = 2
        });

        var displayName = validationNamesService.GetDisplayName(validationResultMock.Object);

        Assert.That(displayName, Is.EqualTo("Row 3, Column 'Amount' (2) : Invalid value"));
    }
}
