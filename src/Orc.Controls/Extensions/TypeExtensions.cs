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
    using System.Reflection;
    using Catel;
    using Catel.Reflection;
    using static System.Convert;

    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, (double Min, double Max)> MinMaxNumberValues = new ()
        {
            { typeof(byte), (byte.MinValue, byte.MaxValue) },
            { typeof(sbyte), (sbyte.MinValue, sbyte.MaxValue) },
            { typeof(short), (short.MinValue, short.MaxValue) },
            { typeof(ushort), (ushort.MinValue, ushort.MaxValue) },
            { typeof(int), (int.MinValue, int.MaxValue) },
            { typeof(uint), (uint.MinValue, uint.MaxValue) },
            { typeof(long), (long.MinValue, long.MaxValue) },
            { typeof(ulong), (ulong.MinValue, ulong.MaxValue) },
            { typeof(float), (float.MinValue, float.MaxValue) },
            { typeof(double), (double.MinValue, double.MaxValue) },
            { typeof(decimal), ( (double)decimal.MinValue, (double)decimal.MaxValue) },
        };

        private static readonly HashSet<Type> FloatingPointTypes = new()
        {
            typeof(decimal),
            typeof(double),
            typeof(float)
        };

        #region Methods
        public static bool IsFloatingPointType(this Type type)
        {
            return FloatingPointTypes.Contains(type);
        }

        public static bool IsInNumberRange(this Type type, double checkedValue)
        {
            if (!MinMaxNumberValues.TryGetValue(type, out var range))
            {
                return false;
            }

            return checkedValue <= range.Min && checkedValue >= range.Max;
        }

        public static object ChangeTypeSafe(this Type convertToType, double dValue)
        {
            var range = convertToType.TryGetNumberRange();
            if (range is null)
            {
                return Activator.CreateInstance(convertToType);
            }

            var (min, max) = range.Value;
            if (dValue < min)
            {
                return ChangeType(min, convertToType);
            }

            if (dValue > max)
            {
                return ChangeType(max, convertToType);
            }

            return ChangeType(dValue, convertToType);
        }

        public static (double Min, double Max)? TryGetNumberRange(this Type type)
        {
            if (MinMaxNumberValues.TryGetValue(type, out var range))
            {
                return range;
            }

            return null;
        }

        public static T[] GetEnumValues<T>()
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                return Array.Empty<T>();
            }

            return GetEnumValues(typeof(T)).OfType<T>().ToArray();
        }

        public static Array GetEnumValues(this Type enumType)
        {
            ArgumentNullException.ThrowIfNull(enumType);

            if (!enumType.IsEnum)
            {
                return Array.CreateInstance(enumType, 0);
            }

            var actualEnumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            var enumValues = Enum.GetValues(actualEnumType);

            return enumValues;
        }

        public static Type FindGenericTypeImplementation<TBaseType>(this Type singleGenericTypeArgument, Assembly assembly = null)
        {
            ArgumentNullException.ThrowIfNull(singleGenericTypeArgument);

#if NET|| NETCORE
            var types = from type in (assembly ?? Assembly.GetExecutingAssembly()).GetTypesEx()
                where !type.IsAbstract && typeof(TBaseType).IsAssignableFromEx(type)
                let baseTypeLocal = type.BaseType
#elif NETFX_CORE
            var types = from type in AppDomain.CurrentDomain.GetLoadedAssemblies().SelectMany(x => x.GetTypesEx())
                        where !type.GetTypeInfo().IsAbstract && typeof(TBaseType).IsAssignableFromEx(type)
                        let baseTypeLocal = type.GetTypeInfo().BaseType
#endif
                let genericArgs = baseTypeLocal.GetGenericArguments()
                where genericArgs.Length == 1 && genericArgs.First() == singleGenericTypeArgument
                select type;

            var projectorType = types.FirstOrDefault();
            if (projectorType is not null)
            {
                return projectorType;
            }

            var baseType = singleGenericTypeArgument.GetBaseTypeEx();
            return baseType is not null ? FindGenericTypeImplementation<TBaseType>(baseType) : null;
        }
        #endregion
    }
}
