namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Automation;
    using Automation;
    using Catel;
    using Catel.Caching;
    using Catel.Data;
    using Catel.Reflection;

    public abstract class AutomationModel : ModelBase
    {
        private readonly CacheStorage<string, SearchContext> _cachedPartSearchContexts = new();
        private readonly Dictionary<string, object> _cachedProperties = new();

        public AutomationModel(AutomationElement target)
        {
            Argument.IsNotNull(() => target);

            Element = target;
        }

        public AutomationModel(AutomationControl targetControl)
        {
            Argument.IsNotNull(() => targetControl);

            Element = targetControl.Element;
        }

        public virtual AutomationElement Element { get; }

        public By By => new By(Element);

        protected override T GetValueFromPropertyBag<T>(string propertyName)
        {
            if (TryGetControlPart<T>(propertyName, out var controlPart))
            {
                return controlPart;
            }

            //if (TryGetControlMap(propertyName, out var controlMap))
            //{
            //    return controlMap;
            //}

            //Manage ordinary properties
            return base.GetValueFromPropertyBag<T>(propertyName);
        }

        protected override void SetValueToPropertyBag<TValue>(string propertyName, TValue value)
        {
            //Manage ordinary properties
            base.SetValueToPropertyBag(propertyName, value);
        }

        //protected virtual bool TryGetControlMap<T>(string propertyName, out T controlMap)
        //{
        //    controlMap = default;

        //    if (_cachedProperties.TryGetValue(propertyName, out var controlMapObj))
        //    {
        //        controlMap = (T)controlMapObj;

        //        return true;
        //    }


        //}

        protected virtual bool TryGetControlPart<T>(string propertyName, out T controlPart)
        {
            controlPart = default;

            if (_cachedProperties.TryGetValue(propertyName, out var controlPartObj))
            {
                controlPart = (T)controlPartObj;

                return true;
            }

            var searchContext = _cachedPartSearchContexts.GetFromCacheOrFetch(propertyName, () => GetSearchContext(propertyName));
            if (searchContext is null)
            {
                return false;
            }

            controlPart = GetPart<T>(propertyName, searchContext);
            if (searchContext.IsCached)
            {
                _cachedProperties[propertyName] = controlPart;
            }

            return true;
        }

        protected virtual SearchContext GetSearchContext(string propertyName)
        {
            var type = GetType();

            var property = type.GetRuntimeProperties().FirstOrDefault(x => Equals(x.Name, propertyName));
            var controlPartAttribute = property?.GetAttribute<ControlPartAttribute>();
            if (controlPartAttribute is null)
            {
                return null;
            }

            var searchContext = controlPartAttribute.GetSearchContext();
            if (searchContext.IsEmpty)
            {
                searchContext.Id = propertyName;
            }

            return searchContext;
        }

        protected virtual TAutomationElement GetPart<TAutomationElement>(string propertyName, SearchContext searchContext)
        {
            return Element.FindOneOrMany<TAutomationElement>(searchContext);
        }

        public static implicit operator AutomationElement(AutomationModel model)
        {
            return model?.Element;
        }
    }
}
