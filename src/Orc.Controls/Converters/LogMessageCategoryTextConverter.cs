namespace Orc.Controls.Converters
{
    using System;
    using System.Collections.Generic;
    using Catel.MVVM.Converters;

    internal class LogMessageCategoryTextConverter : ValueConverterBase<string>
    {
        private static readonly Dictionary<string, string?> PathCache = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        static LogMessageCategoryTextConverter()
        {
            PathCache["Debug"] = LanguageHelper.GetRequiredString("Controls_LogMessageCategoryToggleButton_Text_Debug");
            PathCache["Info"] = LanguageHelper.GetRequiredString("Controls_LogMessageCategoryToggleButton_Text_Info");
            PathCache["Warning"] = LanguageHelper.GetRequiredString("Controls_LogMessageCategoryToggleButton_Text_Warning");
            PathCache["Error"] = LanguageHelper.GetRequiredString("Controls_LogMessageCategoryToggleButton_Text_Error");
            PathCache["Clock"] = null;
        }

        protected override object? Convert(string? value, Type targetType, object? parameter)
        {
            return PathCache.TryGetValue(value, out var cachedvalue) ? cachedvalue : null;
        }
    }
}
