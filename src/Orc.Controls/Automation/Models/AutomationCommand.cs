namespace Orc.Controls.Automation
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [KnownType(typeof(AutomationSendData))]
    public class AutomationCommand
    {
        [NonSerialized]
        public static readonly AutomationCommand Empty = new ();

        public static AutomationCommand FromStr(string commandText)
        {
            if (string.IsNullOrWhiteSpace(commandText))
            {
                return Empty;
            }

            return XmlSerializerHelper.DeserializeValue(commandText, typeof(AutomationCommand)) as AutomationCommand ?? Empty;
        }

        public string CommandName { get; set; }
        public AutomationSendData Data { get; set; }

        public override string ToString()
        {
            return XmlSerializerHelper.SerializeValue(this);
        }
    }
}
