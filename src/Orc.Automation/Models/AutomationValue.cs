namespace Orc.Automation
{
    using System;

    [Serializable]
    public class AutomationValue
    {
        public static AutomationValue NotSetValue = new () { Data = "This value is not set" };

        [NonSerialized]
        private readonly Type _dataType;

        public static AutomationValue FromValue(object value, Type valueType = null)
        {
            if (value is null)
            {
                return null;
            }

            var dataSourceXml = XmlSerializerHelper.SerializeValue(value);

            return new AutomationValue(valueType ?? value.GetType())
            {
                Data = dataSourceXml
            };
        }

        protected AutomationValue()
        {

        }

        private AutomationValue(Type dataType)
        {
            _dataType = dataType;
            DataTypeFullName = _dataType?.FullName;
        }

        public string DataTypeFullName { get; set; }

        public string Data { get; set; }

        public object ExtractValue()
        {
            var type = _dataType ?? Catel.Reflection.TypeCache.GetType(DataTypeFullName);
            return XmlSerializerHelper.DeserializeValue(Data, type);
        }
    }
}
