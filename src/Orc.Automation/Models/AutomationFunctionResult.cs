namespace Orc.Automation
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [KnownType(typeof(AutomationValue))]
    public class AutomationMethodResult
    {
        [NonSerialized]
        public static readonly AutomationMethodResult Empty = new();

        public static AutomationMethodResult FromStr(string resultText)
        {
            if (string.IsNullOrWhiteSpace(resultText))
            {
                return Empty;
            }

            return XmlSerializerHelper.DeserializeValue(resultText, typeof(AutomationMethodResult)) as AutomationMethodResult ?? Empty;
        }

        public AutomationValue Data { get; set; }

        public string EventName { get; set; }
        public AutomationValue EventData { get; set; }

        public override string ToString()
        {
            return XmlSerializerHelper.SerializeValue(this);
        }
    }
}
