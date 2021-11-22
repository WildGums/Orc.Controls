namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;
    using Catel;

    public class RunMethodAutomationElement : AutomationElementBase
    {
        #region Fields
        private readonly InvokePattern _invokePattern;
        private readonly ValuePattern _valuePattern;
        #endregion

        #region Constructors
        public RunMethodAutomationElement(AutomationElement element)
            : base(element)
        {
            _valuePattern = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            _invokePattern = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;

            Automation.AddAutomationEventHandler(InvokePattern.InvokedEvent, element, TreeScope.Element, OnEventInvoke);
        }
        #endregion

        #region Methods
        public event EventHandler<AutomationEventArgs> AutomationEvent;

        private void OnEventInvoke(object sender, System.Windows.Automation.AutomationEventArgs e)
        {
            var result = _valuePattern.Current.Value;

            var automationResult = AutomationMethodResult.FromStr(result);

            var eventName = automationResult.EventName;
            var eventData = automationResult.EventData?.ExtractValue();

            OnEvent(eventName, eventData);

            AutomationEvent?.Invoke(this, new AutomationEventArgs
            {
                EventName = eventName,
                Data = eventData
            });
        }

        protected virtual void OnEvent(string eventName, object eventData)
        {

        }

        public object GetValue(string propertyName)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            var result = Execute(GetDependencyPropertyMethodRun.ConvertPropertyToCommandName(propertyName), null, 20);
            return result;
        }

        public void SetValue(string propertyName, object value)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            var automationValues = AutomationHelper.ConvertToAutomationValuesList(value);
            var result = Execute(SetDependencyPropertyMethodRun.ConvertPropertyToCommandName(propertyName), automationValues, 20);
        }

        public object Execute(string methodName, params object[] parameters)
        {
            var automationValues = AutomationHelper.ConvertToAutomationValuesList(parameters);
            return Execute(methodName, automationValues, 20);
        }

        private object Execute(string methodName, AutomationValueList parameters, int delay = 200)
        {
            var method = new AutomationMethod
            {
                Name = methodName,
                Parameters = parameters
            };

            var result = Execute(method, delay);
            if (Equals(result, AutomationMethodResult.Empty))
            {
                return null;
            }

            var data = result.Data;
            var resultValue = data?.ExtractValue();

            return resultValue;
        }

        private AutomationMethodResult Execute(AutomationMethod method, int delay)
        {
            var methodStr = method?.ToString();
            if (string.IsNullOrWhiteSpace(methodStr))
            {
                return null;
            }

            _valuePattern.SetValue(methodStr);

            Thread.Sleep(delay);

            try
            {
                _invokePattern?.Invoke();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            Thread.Sleep(delay);

            var result = _valuePattern.Current.Value;

            return AutomationMethodResult.FromStr(result);
        }
        #endregion
    }
}
