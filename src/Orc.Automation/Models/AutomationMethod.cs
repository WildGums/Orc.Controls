namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public class AutomationValueList : List<AutomationValue>
    {
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
