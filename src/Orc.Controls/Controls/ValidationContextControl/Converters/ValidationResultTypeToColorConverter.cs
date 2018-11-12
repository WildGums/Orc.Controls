// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultTypeToColorConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
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
        #region Properties
        public Color DefaultColor { get; set; }
        public Color ErrorColor { get; set; }
        public Color WarningColor { get; set; }
        #endregion

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (!(value is ValidationResultType validationResultType))
            {
                return new SolidColorBrush(DefaultColor);
            }

            var color = validationResultType == ValidationResultType.Error ? ErrorColor : WarningColor;

            return new SolidColorBrush(color);

        }
        #endregion
    }
}
