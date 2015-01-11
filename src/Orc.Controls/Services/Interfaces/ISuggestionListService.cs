namespace Orc.Controls.Services.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface ISuggestionListService
    {
        List<KeyValuePair<string, string>> GetSuggestionList(DateTime dateTime, DateTimePart editablePart);
    }
}