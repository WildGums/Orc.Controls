namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Automation;
    using Catel;

    public abstract class AutomationElementBase
    {
        private static readonly string NullKey = $"id = {null}: name = {null}: className = {null}: controlType = {null}";

        private readonly Dictionary<string, AutomationElement> _parts = new();

        protected AutomationElementBase(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            Element = element;
        }

        public AutomationElement Element { get; }

        public virtual AutomationElement GetPart(string id = null, string name = null, string className = null, ControlType controlType = null)
        {
            var key = $"id = {id}: name = {name}: className = {className}: controlType = {controlType}";

            if (string.Equals(key, NullKey))
            {
                var callingProperty = new StackTrace().GetFrame(1).GetMethod().Name;
                if (callingProperty.StartsWith("get_"))
                {
                    id = callingProperty.Replace("get_", string.Empty);

                    var firstTimeSearchElement = Element.FindFirstWithDelay(new OrCondition(new PropertyCondition(AutomationElement.NameProperty, id), new PropertyCondition(AutomationElement.AutomationIdProperty, id)));
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

            if (_parts.TryGetValue(key, out var part))
            {
                return part;
            }

            part = Element.Find(id: id, name: name, className: className, controlType: controlType, scope: TreeScope.Subtree, numberOfWaits: 3);
            _parts[key] = part ?? throw new Exception($"Can't find part with key {key}");

            return part;
        }
    }
}
