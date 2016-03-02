// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmPmLongToAmPmShortConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    internal class AmPmLongToAmPmShortConverter : ValueConverterBase
    {
        #region Constructors
        public AmPmLongToAmPmShortConverter()
        {

        }
        #endregion

        #region Properties
        public bool IsEnabled { get; set; }
        #endregion

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (IsEnabled && value != null)
            {
                if(string.Equals(value, "AM"))
                {
                    return "A";
                }

                if(string.Equals(value, "PM"))
                {
                    return "P";
                }
            }
            return value;
        }
        #endregion
    }
}
