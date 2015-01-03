namespace Orc.Controls.Services.Interfaces
{
    using System;

    public interface ISuggestionListService
    {
        string[] GetSuggestionList(DateTime dateTime, DateTimePart editablePart);
    }
}