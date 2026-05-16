namespace Orc.Controls.ViewModels;

using System;
using System.Media;
using Catel.MVVM;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Services;

public partial class FindReplaceViewModel : FeaturedViewModelBase
{
    private readonly IFindReplaceService _findReplaceService;
    private readonly ILanguageService _languageService;

    public FindReplaceViewModel(FindReplaceSettings findReplaceSettings, IServiceProvider serviceProvider,
        IFindReplaceService findReplaceService)
        : this(findReplaceSettings, serviceProvider, findReplaceService, serviceProvider.GetRequiredService<ILanguageService>())
    {
    }

    public FindReplaceViewModel(FindReplaceSettings findReplaceSettings, IServiceProvider serviceProvider,
        IFindReplaceService findReplaceService, ILanguageService languageService)
        : base(serviceProvider)
    {
        _findReplaceService = findReplaceService;
        _languageService = languageService;

        FindNext = new Command<string?>(serviceProvider, OnFindNext);
        FindAll = new Command<string?>(serviceProvider, OnFindAll);
        Replace = new Command<object?>(serviceProvider, OnReplace);
        ReplaceAll = new Command<object?>(serviceProvider, OnReplaceAll);

        FindReplaceSettings = findReplaceSettings;

        var initialText = _findReplaceService.GetInitialFindText();

        TextToFind = initialText;
        TextToFindForReplace = initialText;
    }

    public override string Title => _languageService.GetRequiredString("Controls_FindReplaceView_Title");

    [Model]
    public FindReplaceSettings FindReplaceSettings { get; }

    public string TextToFind { get; set; }
    public string TextToFindForReplace { get; set; }
    public Command<string?> FindNext { get; }
    public Command<object?> Replace { get; }
    public Command<object?> ReplaceAll { get; }
    public Command<string?> FindAll { get; set; }

    private void OnReplaceAll(object? parameter)
    {
        if (parameter is null)
        {
            return;
        }

        var values = (object[])parameter;
        var textToFind = values[0] as string ?? string.Empty;
        var replacementText = values[1] as string ?? string.Empty;

        _findReplaceService.ReplaceAll(textToFind, replacementText, FindReplaceSettings);
    }

    private void OnReplace(object? parameter)
    {
        if (parameter is null)
        {
            return;
        }

        var values = (object[])parameter;
        var textToFind = values[0] as string ?? string.Empty;
        var replacementText = values[1] as string ?? string.Empty;

        if (!_findReplaceService.Replace(textToFind, replacementText, FindReplaceSettings))
        {
            SystemSounds.Beep.Play();
        }
    }

    private void OnFindNext(string? text)
    {
        var textToFind = text ?? string.Empty;

        if (!_findReplaceService.FindNext(textToFind, FindReplaceSettings))
        {
            SystemSounds.Beep.Play();
        }
    }

    private void OnFindAll(string? text)
    {
        var textToFind = text ?? string.Empty;

        _findReplaceService.FindAll(textToFind, FindReplaceSettings);
    }
}
