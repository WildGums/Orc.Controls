namespace Orc.Automation
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.Reflection;

    public static class TypeExtensions
    {
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
    }
}
