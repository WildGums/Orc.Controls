namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.Reflection;

    public static class TypeExtensions
    {
        public static IReadOnlyDictionary<PropertyInfo, TAttribute> GetPropertiesDecoratedWith<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return type
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(TAttribute)))
                .ToDictionary(x => x, x => x.GetAttribute<TAttribute>());
        }

        public static Type FindGenericTypeImplementation<TBaseType>(this Type singleGenericTypeArgument, Assembly assembly = null)
        {
            Argument.IsNotNull(() => singleGenericTypeArgument);

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
                where genericArgs.Contains(singleGenericTypeArgument)
                select type;

            var projectorType = types.FirstOrDefault();
            if (projectorType is not null)
            {
                return projectorType;
            }

            var baseType = singleGenericTypeArgument.GetBaseTypeEx();
            return baseType is not null ? FindGenericTypeImplementation<TBaseType>(baseType) : null;
        }

        public static Type GetAnyElementType(this Type type)
        {
            // Type is Array
            // short-circuit if you expect lots of arrays 
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            // type is IEnumerable<T>;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return type.GetGenericArguments()[0];
            }

            // type implements/extends IEnumerable<T>;
            var enumType = type.GetInterfaces()
                .Where(t => t.IsGenericType &&
                            t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(t => t.GenericTypeArguments[0]).FirstOrDefault();
            return enumType ?? type;
        }
    }
}
