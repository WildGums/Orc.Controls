namespace Orc.Controls.Automation
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [KnownType(typeof(AutomationSendData))]
    public class AutomationCommandResult
    {
        [NonSerialized]
        public static readonly AutomationCommandResult Empty = new();

        public static AutomationCommandResult FromStr(string resultText)
        {
            if (string.IsNullOrWhiteSpace(resultText))
            {
                return Empty;
            }

            return XmlSerializerHelper.DeserializeValue(resultText, typeof(AutomationCommandResult)) as AutomationCommandResult ?? Empty;
        }

        public AutomationSendData Data { get; set; }

        public override string ToString()
        {
            return XmlSerializerHelper.SerializeValue(this);
        }
    }
}
