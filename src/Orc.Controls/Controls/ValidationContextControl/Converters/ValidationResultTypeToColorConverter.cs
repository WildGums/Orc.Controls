// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultTypeToColorConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows.Media;
    using Catel.Data;
    using Catel.MVVM.Converters;

    public class ValidationResultTypeToColorConverter : ValueConverterBase
    {
        public Color DefaultColor { get; set; }

        public Color ErrorColor { get; set; }

        public Color WarningColor { get; set; }

        protected override object Convert(object value, Type targetType, object parameter)
        {
            var validationResultType = value as ValidationResultType?;
            if (validationResultType == null)
            {
                return new SolidColorBrush(DefaultColor);
            }

            var color = validationResultType.Value == ValidationResultType.Error ? ErrorColor : WarningColor;

            return new SolidColorBrush(color);
        }
    }
}