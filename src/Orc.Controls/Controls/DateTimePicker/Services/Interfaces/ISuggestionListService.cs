namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;

    public interface ISuggestionListService
    {
        #region Methods
        List<KeyValuePair<string, string>> GetSuggestionList(DateTime dateTime, DateTimePart editablePart, DateTimeFormatInfo dateTimeFormatInfo);
        #endregion
    }
}
