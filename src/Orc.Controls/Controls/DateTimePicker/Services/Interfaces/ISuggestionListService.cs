namespace Orc.Controls;

using System;
using System.Collections.Generic;

public interface ISuggestionListService
{
    IReadOnlyList<KeyValuePair<string, string>> GetSuggestionList(DateTime dateTime, DateTimePart editablePart, DateTimeFormatInfo dateTimeFormatInfo);
}
