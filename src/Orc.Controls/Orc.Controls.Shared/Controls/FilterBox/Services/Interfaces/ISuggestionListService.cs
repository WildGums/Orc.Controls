// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISuggestionListService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    using System;
    using System.Collections.Generic;

    public interface ISuggestionListService
    {
        List<KeyValuePair<string, string>> GetSuggestionList(DateTime dateTime, DateTimePart editablePart);
    }
}