namespace Orc.Automation
{
    using System.Collections.Generic;
    using Catel.Data;

    public abstract class AutomationScenarioBase : ModelBase
    {
        private readonly Dictionary<string, object> _cachedProperties = new();
        private readonly Dictionary<string, IControlPartSearchInfo> _cachedSearchInfo = new();

        protected override T GetValueFromPropertyBag<T>(string propertyName)
        {
            if (!_cachedSearchInfo.TryGetValue(propertyName, out var searchInfo))
            {
                searchInfo = GetSearchInfo(propertyName);
                if (searchInfo is null)
                {
                    return base.GetValueFromPropertyBag<T>(propertyName);
                }

                _cachedSearchInfo[propertyName] = searchInfo;
            }

            if (!searchInfo.IsTransient)
            {
                if (_cachedProperties.TryGetValue(propertyName, out var propertyValue))
                {
                    return (T)propertyValue;
                }
            }

            var part = GetPart<T>(propertyName, searchInfo);
            if (!searchInfo.IsTransient)
            {
                _cachedProperties[propertyName] = part;
            }

            return part;
        }

        protected abstract IControlPartSearchInfo GetSearchInfo(string propertyName);
        protected abstract TAutomationElement GetPart<TAutomationElement>(string propertyName, IControlPartSearchInfo searchInfo);
    }
}
