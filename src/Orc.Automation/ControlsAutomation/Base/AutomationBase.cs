namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;
    using Catel;
    using Catel.Caching;

    public abstract class AutomationBase
    {
        private readonly CacheStorage<Type, AutomationBase> _maps = new();

        public AutomationBase(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            Element = element;
        }

        public AutomationElement Element { get; }
        public virtual By By => new (Element);
        protected AutomationFactory Factory { get; } = new();

        public T Map<T>()
            where T : AutomationBase
        {
            return (T)_maps.GetFromCacheOrFetch(typeof(T), () => Factory.Create<T>(this));
        }
    }
}
