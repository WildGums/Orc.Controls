namespace Orc.Controls.Converters;

using System;
using System.Collections.Generic;
using Catel;
using Catel.MVVM.Converters;
using Catel.Services;

internal class LogMessageCategoryTextConverter : ValueConverterBase<string>
{
    private static readonly Dictionary<string, string?> PathCache = new(StringComparer.OrdinalIgnoreCase);

    private readonly ILanguageService _languageService;

    public LogMessageCategoryTextConverter(ILanguageService languageService)
    {
        _languageService = languageService;

        if (PathCache.Count == 0)
        {
            PathCache["Debug"] = _languageService.GetRequiredString("Controls_LogMessageCategoryToggleButton_Text_Debug");
            PathCache["Information"] = _languageService.GetRequiredString("Controls_LogMessageCategoryToggleButton_Text_Info");
            PathCache["Warning"] = _languageService.GetRequiredString("Controls_LogMessageCategoryToggleButton_Text_Warning");
            PathCache["Error"] = _languageService.GetRequiredString("Controls_LogMessageCategoryToggleButton_Text_Error");
            PathCache["Critical"] = _languageService.GetRequiredString("Controls_LogMessageCategoryToggleButton_Text_Error");
            PathCache["Clock"] = null;
        }
    }

    protected override object? Convert(string? value, Type targetType, object? parameter)
    {
        return PathCache.TryGetValue(value ?? string.Empty, out var cachedValue) ? cachedValue : null;
    }
}
