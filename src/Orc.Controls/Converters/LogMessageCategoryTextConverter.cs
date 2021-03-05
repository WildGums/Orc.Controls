// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryTextConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2021 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Converters
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Catel.MVVM.Converters;

    internal class LogMessageCategoryTextConverter : ValueConverterBase<string>
    {
        #region Constants
        private static readonly Dictionary<string, string> PathCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        #endregion

        #region Constructors
        static LogMessageCategoryTextConverter()
        {
            PathCache["Debug"] = LanguageHelper.GetString("Controls_LogMessageCategoryToggleButton_Text_Debug");
            PathCache["Info"] = LanguageHelper.GetString("Controls_LogMessageCategoryToggleButton_Text_Info");
            PathCache["Warning"] = LanguageHelper.GetString("Controls_LogMessageCategoryToggleButton_Text_Warning");
            PathCache["Error"] = LanguageHelper.GetString("Controls_LogMessageCategoryToggleButton_Text_Error");
            PathCache["Clock"] = null;
        }
        #endregion

        #region Methods
        protected override object Convert(string value, Type targetType, object parameter)
        {
            return PathCache.TryGetValue(value, out var cachedvalue) ? cachedvalue : null;
        }
        #endregion
    }
}
