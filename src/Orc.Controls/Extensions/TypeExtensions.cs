// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;

    public static class TypeExtensions
    {
        #region Methods
        public static T[] GetEnumValues<T>()
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                return new T[0];
            }

            return GetEnumValues(typeof(T)).OfType<T>().ToArray();
        }

        public static Array GetEnumValues(this Type enumType)
        {
            Argument.IsNotNull(() => enumType);

            if (!enumType.IsEnum)
            {
                return Array.CreateInstance(enumType, 0);
            }

            var actualEnumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            var enumValues = Enum.GetValues(actualEnumType);

            return enumValues;
        }
        #endregion
    }
}
