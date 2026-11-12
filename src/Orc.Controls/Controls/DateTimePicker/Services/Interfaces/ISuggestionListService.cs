namespace Orc.Controls;

using System;
using System.Collections.Generic;

public interface ISuggestionListService
{
    IReadOnlyList<KeyValuePair<string, string>> GetSuggestionList(DateTimeOffset dateTime, 
        DateTimePart editablePart, DateTimeFormatInfo dateTimeFormatInfo);
}
