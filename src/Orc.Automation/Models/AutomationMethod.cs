namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    public class AutomationValueList : List<AutomationValue>
    {
        public static AutomationValueList Create(params object[] parameters)
        {
            if (parameters is null || !parameters.Any())
            {
                return new AutomationValueList();
            }

            var automationValues = parameters.Select(x => AutomationValue.FromValue(x)).ToList();

            return new AutomationValueList(automationValues);
        }

        public AutomationValueList()
        {
            
        }

        public AutomationValueList(IEnumerable<AutomationValue> values)
            : base(values)
        {
            
        }
    }

    [Serializable]
    [KnownType(typeof(AutomationValue))]
    public class AutomationMethod
    {
        [NonSerialized]
        public static readonly AutomationMethod Empty = new ();

        public static AutomationMethod FromStr(string methodText)
        {
            if (string.IsNullOrWhiteSpace(methodText))
            {
                return Empty;
            }

            return XmlSerializerHelper.DeserializeValue(methodText, typeof(AutomationMethod)) as AutomationMethod ?? Empty;
        }

        public string Name { get; set; }
        public AutomationValueList Parameters { get; set; }

        public override string ToString()
        {
            return XmlSerializerHelper.SerializeValue(this);
        }
    }
}
