namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Catel.IoC;

    public class AutomationHelper
    {
        private static readonly Dictionary<Type, IResultToAutomationCommandResultConverter> Converters = new();

        public static AutomationValueList ConvertToAutomationValuesList(params object[] parameters)
        {
            if (parameters is null || !parameters.Any())
            {
                return new AutomationValueList();
            }

            var automationValues = parameters.Select(x =>
            {
                var parameterXml = XmlSerializerHelper.SerializeValue(x);

                return new AutomationValue(x.GetType())
                {
                    Data = parameterXml
                };
            }).ToList();

            return new AutomationValueList(automationValues);
        }

        public static AutomationValue ConvertToAutomationValue(object value)
        {
            if (value is null)
            {
                return null;
            }

            var serializedValue = XmlSerializerHelper.SerializeValue(value);

            return new AutomationValue(value.GetType())
            {
                Data = serializedValue
            };
        }

        public static AutomationMethodResult ConvertToSerializableResult(object result)
        {
            if (result is null)
            {
                return null;
            }

            var converterType = result.GetType().FindGenericTypeImplementation<IResultToAutomationCommandResultConverter>(Assembly.GetExecutingAssembly());
            if (converterType is null)
            {
                return new AutomationMethodResult { Data = AutomationValue.FromValue(result) };
            }

            if (!Converters.TryGetValue(converterType, out var converter))
            {
                converter = result.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(converterType) as IResultToAutomationCommandResultConverter;
            }
                    
            if (converter is not null)
            {
                Converters[converterType] = converter;

                return converter.Convert(result);
            }

            return new AutomationMethodResult
            {
                Data = AutomationValue.FromValue(result)
            };
        }
    }
}
