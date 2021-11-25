namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Automation;
    using Catel;

    public class AutomationControl
    {
        private static readonly string NullKey = $"id = {null}: name = {null}: className = {null}: controlType = {null}";

        private readonly Dictionary<string, AutomationElement> _parts = new();

        protected AutomationControl(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            Element = element;
        }

        public AutomationElement Element { get; }

        public virtual AutomationElement GetPart(string id = null, string name = null, string className = null, ControlType controlType = null, bool isCached = true)
        {
            var key = $"id = {id}: name = {name}: className = {className}: controlType = {controlType}";

            if (string.Equals(key, NullKey))
            {
                var callingProperty = new StackTrace().GetFrame(1).GetMethod().Name;
                if (callingProperty.StartsWith("get_"))
                {
                    id = callingProperty.Replace("get_", string.Empty);

                    var firstTimeSearchElement = Element.Find(new OrCondition(new PropertyCondition(AutomationElement.NameProperty, id), new PropertyCondition(AutomationElement.AutomationIdProperty, id)));
                    if (firstTimeSearchElement is null)
                    {
                        throw new Exception($"Can't find part with Id = {id} or Name = {id}");
                    }

                    var namePropertyValue = firstTimeSearchElement.GetCurrentPropertyValue(AutomationElement.NameProperty);
                    var idPropertyValue = firstTimeSearchElement.GetCurrentPropertyValue(AutomationElement.AutomationIdProperty);
                    if (Equals(id, namePropertyValue) && !Equals(id, idPropertyValue))
                    {
                        name = id;
                        id = null;
                    }

                    key = $"id = {id}: name = {name}: className = {className}: controlType = {controlType}";
                }
            }

            if (isCached)
            {
                if (_parts.TryGetValue(key, out var cachedPart))
                {
                    return cachedPart;
                }
            }

            var part = Element.Find(id: id, name: name, className: className, controlType: controlType, scope: TreeScope.Subtree, numberOfWaits: 3);
          
            if (isCached)
            {
                _parts[key] = part ?? throw new Exception($"Can't find part with key {key}");
            }

            return part;
        }

        public static implicit operator AutomationElement(AutomationControl element)
        {
            return element?.Element;
        }
    }
}
