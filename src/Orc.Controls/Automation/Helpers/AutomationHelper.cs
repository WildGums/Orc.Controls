namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.IoC;
    using Catel.Reflection;

    public class AutomationHelper
    {
        private static readonly Dictionary<Type, IResultToAutomationCommandResultConverter> Converters = new();

        public static AutomationCommandResult ConvertToSerializableResult(object result)
        {
            if (result is null)
            {
                return null;
            }

            var converterType = result.GetType().FindGenericTypeImplementation<IResultToAutomationCommandResultConverter>(Assembly.GetExecutingAssembly());
            if (converterType is null)
            {
                return new AutomationCommandResult { Data = AutomationSendData.FromValue(result) };
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

            return new AutomationCommandResult
            {
                Data = AutomationSendData.FromValue(result)
            };
        }
    }
}
