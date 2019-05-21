// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropdownArrowLocationConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel;
    using Catel.MVVM.Converters;

    /// <summary>
    /// The dropdown arrow location converter.
    /// </summary>
    public class DropdownArrowLocationConverter : ValueConverterBase
    {
        #region Methods
        /// <summary>
        /// Converts value <see cref="DropdownArrowLocation "/> values into <see cref="Dock"/> or <see cref="HorizontalAlignment"/>.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parementer</param>
        /// <returns>
        /// A converted value.
        /// </returns>
        protected override object Convert(object value, Type targetType, object parameter)
        {
            Argument.IsOfOneOfTheTypes(nameof(targetType), targetType, new[] { typeof(Dock), typeof(HorizontalAlignment) });

            object result = null;
            if (targetType == typeof(Dock))
            {
                result = value == null ? default(Dock) : Enum.Parse(targetType, ObjectToStringHelper.ToString(value), true);
            }
            else if (targetType == typeof(HorizontalAlignment))
            {
                if (value == null)
                {
                    result = default(HorizontalAlignment);
                }
                else
                {
                    var dropdownArrowLocation = (DropdownArrowLocation)value;
                    if (dropdownArrowLocation == DropdownArrowLocation.Top || dropdownArrowLocation == DropdownArrowLocation.Bottom)
                    {
                        result = HorizontalAlignment.Center;
                    }
                    else
                    {
                        result = Enum.Parse(targetType, ObjectToStringHelper.ToString(value), true);
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
