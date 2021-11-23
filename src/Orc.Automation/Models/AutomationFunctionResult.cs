namespace Orc.Automation
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [KnownType(typeof(AutomationValue))]
    public class AutomationResultContainer
    {
        [NonSerialized]
        public static readonly AutomationResultContainer Empty = new();

        public static AutomationResultContainer FromStr(string resultText)
        {
            if (string.IsNullOrWhiteSpace(resultText))
            {
                return Empty;
            }

            return XmlSerializerHelper.DeserializeValue(resultText, typeof(AutomationResultContainer)) as AutomationResultContainer ?? Empty;
        }
        
        public string LastEventName { get; set; }
        public AutomationValue LastEventArgs { get; set; }

        public AutomationValue LastInvokedMethodResult { get; set; }

        public override string ToString()
        {
            return XmlSerializerHelper.SerializeValue(this);
        }
    }
}
