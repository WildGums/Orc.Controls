namespace Orc.Automation
{
    using System;
    using System.IO;
    using System.Text;
    using Catel.Runtime.Serialization;

    public static class XmlSerializerHelper
    {
        public static string SerializeValue(object value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            var xmlSerializer = SerializationFactory.GetXmlSerializer();

            using var stream = new MemoryStream();
            xmlSerializer.Serialize(value, stream);
            var strResult = Encoding.Default.GetString(stream.ToArray());
            
            return strResult;
        }

        public static object DeserializeValue(string text, Type type)
        {
            var xmlSerializer = SerializationFactory.GetXmlSerializer();

            using var stream = text.ToStream();
            var result = xmlSerializer.Deserialize(type, stream);

            return result;
        }
    }
}
