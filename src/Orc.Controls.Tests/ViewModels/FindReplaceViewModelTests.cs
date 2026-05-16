namespace Orc.Controls.Tests;

using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Services;
using ViewModels;

public class FindReplaceViewModelTests
{
    [Test]
    public void Find_All_Should_Execute_FindReplaceService_With_Correct_Parameters()
    {
        var findReplaceSettings = new FindReplaceSettings();
        const string findAllSearchString = "searchStr";

        var serviceCollection = ServiceCollectionHelper.CreateServiceCollection();

        using var serviceProvider = serviceCollection.BuildServiceProvider();
        var languageService = serviceProvider.GetRequiredService<ILanguageService>();

        var isExecuted = false;

        var findReplaceServiceMock = new Mock<IFindReplaceService>();
        findReplaceServiceMock.Setup(x => x.FindAll(It.IsAny<string>(), It.IsAny<FindReplaceSettings>()))
            .Callback<string, FindReplaceSettings>((x, y) =>
            {
                Assert.That(x, Is.EqualTo(findAllSearchString));
                Assert.That(y, Is.EqualTo(findReplaceSettings));

                isExecuted = true;
            });
        var findReplaceService = findReplaceServiceMock.Object;

        var viewModel = new FindReplaceViewModel(findReplaceSettings, serviceProvider, findReplaceService, languageService);
        viewModel.FindAll.Execute(findAllSearchString);

        Assert.That(isExecuted, Is.True);
    }

    [Test]
    public void Title_Should_Be_Loaded_From_Language_Service()
    {
        var findReplaceSettings = new FindReplaceSettings();

        var serviceCollection = ServiceCollectionHelper.CreateServiceCollection();

        using var serviceProvider = serviceCollection.BuildServiceProvider();
        var languageService = serviceProvider.GetRequiredService<ILanguageService>();

        var findReplaceServiceMock = new Mock<IFindReplaceService>();
        var findReplaceService = findReplaceServiceMock.Object;

        var viewModel = new FindReplaceViewModel(findReplaceSettings, serviceProvider, findReplaceService, languageService);

        Assert.That(viewModel.Title, Is.EqualTo("Find and Replace"));
    }
}
