// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryBorderBrushConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2021 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Catel.MVVM.Converters;

    internal class LogMessageCategoryBorderBrushConverter : ValueConverterBase<string>
    {
        #region Constants
        public static readonly Dictionary<string, SolidColorBrush> BrushCache = new Dictionary<string, SolidColorBrush>(StringComparer.OrdinalIgnoreCase);
        #endregion

        #region Constructors
        static LogMessageCategoryBorderBrushConverter()
        {
            BrushCache["Debug"] = new SolidColorBrush(Colors.DarkGray);
            BrushCache["Info"] = new SolidColorBrush(Colors.RoyalBlue);
            BrushCache["Warning"] = new SolidColorBrush(Colors.DarkOrange);
            BrushCache["Error"] = new SolidColorBrush(Colors.Red);
            BrushCache["Clock"] = new SolidColorBrush(Colors.Gray);
        }
        #endregion

        #region Methods
        protected override object Convert(string value, Type targetType, object parameter)
        {
            return BrushCache.TryGetValue(value, out var brush) ? brush : Brushes.Black;
        }
        #endregion
    }
}
