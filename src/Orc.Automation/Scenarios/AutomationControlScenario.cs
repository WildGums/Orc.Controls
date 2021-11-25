namespace Orc.Automation
{
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Automation;
    using Catel;
    using Catel.Reflection;

    public class AutomationControlScenario : AutomationScenarioBase
    {
        private readonly AutomationControl _targetControl;
        private readonly AutomationElement _targetElement;

        public AutomationControlScenario(AutomationControl target)
        {
            Argument.IsNotNull(() => target);

            _targetControl = target;
            _targetElement = target.Element;
        }

        public AutomationControlScenario(AutomationElement target)
        {
            Argument.IsNotNull(() => target);

            _targetElement = target;
        }

        protected override IControlPartSearchInfo GetSearchInfo(string propertyName)
        {
            var type = GetType();

            var property = type.GetRuntimeProperties().FirstOrDefault(x => Equals(x.Name, propertyName));
            return property.GetAttribute<ControlPartAttribute>();
        }
        protected override TAutomationElement GetPart<TAutomationElement>(string propertyName, IControlPartSearchInfo searchInfo)
        {
            object propertyValue;
            var property = GetType().GetRuntimeProperties().FirstOrDefault(x => Equals(x.Name, propertyName));
            if (property is null)
            {
                return default;
            }

            var propertyType = property.PropertyType;
            if (typeof(IEnumerable).IsAssignableFrom(propertyType))
            {
                propertyValue = _targetElement.FindAll(id: searchInfo.AutomationId, name: searchInfo.Name, className: searchInfo.ClassName, controlType: searchInfo.ControlType);
            }
            else
            {
                propertyValue = !searchInfo.IsTransient && _targetControl is not null
                    ? _targetControl.GetPart(id: searchInfo.AutomationId, name: searchInfo.Name, className: searchInfo.ClassName, controlType: searchInfo.ControlType)
                    :_targetElement.Find(id: searchInfo.AutomationId, name: searchInfo.Name, className: searchInfo.ClassName, controlType: searchInfo.ControlType);
            }

            propertyValue = (TAutomationElement)AutomationHelper.WrapAutomationObject(property.PropertyType, propertyValue);

            return (TAutomationElement)propertyValue;
        }
    }
}
