namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;
    using Catel;
    using Catel.IoC;

    public static partial class AutomationElementExtensions
    {
        public static TTemplate CreateControlMap<TTemplate>(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return (TTemplate)element.CreateControlMap(typeof(TTemplate));
        }

        public static object CreateControlMap(this AutomationElement element, Type controlMapType)
        {
            Argument.IsNotNull(() => element);

            var template = element.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(controlMapType);

            InitializeControlMap(element, template);

            return template;
        }

        public static void InitializeControlMap(this AutomationElement element, object controlMap)
        {
            Argument.IsNotNull(() => element);
            Argument.IsNotNull(() => controlMap);

            TargetAttribute.ResolveTargetProperty(element, controlMap);
            //ControlPartAttribute.ResolvePartProperties(element, controlMap);
            TargetControlMapAttribute.Initialize(element, controlMap);
        }
    }
}
