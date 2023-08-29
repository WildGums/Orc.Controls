namespace Orc.Controls.Tests;

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

        var viewModel = new FindReplaceViewModel(findReplaceSettings, findReplaceService);
        viewModel.FindAll.Execute(findAllSearchString);

        Assert.That(isExecuted, Is.True);
    }
}
