// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryPathConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2021 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using Catel.MVVM.Converters;

    internal class LogMessageCategoryPathConverter : ValueConverterBase<string>
    {
        #region Fields
        private static readonly Dictionary<string, Geometry> PathCache = new Dictionary<string, Geometry>(StringComparer.OrdinalIgnoreCase);
        #endregion

        #region Constructors
        static LogMessageCategoryPathConverter()
        {
            var application = Application.Current;

            PathCache["Debug"] = application.TryFindResource("LogDebugGeometry") as Geometry;
            PathCache["Info"] = application.TryFindResource("LogInfoGeometry") as Geometry;
            PathCache["Warning"] = application.TryFindResource("LogWarningGeometry") as Geometry;
            PathCache["Error"] = application.TryFindResource("LogErrorGeometry") as Geometry;
            PathCache["Clock"] = application.TryFindResource("LogClockGeometry") as Geometry;
        }
        #endregion

        #region Methods
        protected override object Convert(string value, Type targetType, object parameter)
        {
            return PathCache.TryGetValue(value, out var geometry) ? geometry : null;
        }
        #endregion
    }
}
