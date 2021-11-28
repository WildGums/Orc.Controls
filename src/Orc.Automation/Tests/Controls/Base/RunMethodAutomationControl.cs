namespace Orc.Automation
{
    using System;
    using System.Threading;
    using System.Windows.Automation;
    using Catel;

    public class RunMethodAutomationControl : AutomationControl
    {
        #region Fields
        private readonly InvokePattern _invokePattern;
        private readonly ValuePattern _valuePattern;
        #endregion

        #region Constructors
        public RunMethodAutomationControl(AutomationElement element)
            : base(element)
        {
            _valuePattern = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            _invokePattern = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;

            System.Windows.Automation.Automation.AddAutomationEventHandler(InvokePattern.InvokedEvent, element, TreeScope.Element, OnEventInvoke);
        }
        #endregion

        #region Methods
        public event EventHandler<AutomationEventArgs> AutomationEvent;

        private void OnEventInvoke(object sender, System.Windows.Automation.AutomationEventArgs e)
        {
            var result = _valuePattern.Current.Value;

            var automationResult = AutomationResultContainer.FromStr(result);

            var eventName = automationResult.LastEventName;
            var eventData = automationResult.LastEventArgs?.ExtractValue();

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

        public object GetApiPropertyValue(string propertyName)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            var result = Execute(GetDependencyPropertyMethodRun.ConvertPropertyToCommandName(propertyName), null, 20);
            return result;
        }

        public void SetApiPropertyValue(string propertyName, object value)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            var automationValues = AutomationValueList.Create(value);
            var result = Execute(SetDependencyPropertyMethodRun.ConvertPropertyToCommandName(propertyName), automationValues, 20);
        }

        public object Execute(string methodName, params object[] parameters)
        {
            var automationValues = AutomationValueList.Create(parameters);
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
            var resultValue = result?.ExtractValue();

            return resultValue;
        }

        private AutomationValue Execute(AutomationMethod method, int delay)
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
            var automationResultContainer = AutomationResultContainer.FromStr(result);

            return automationResultContainer.LastInvokedMethodResult;
        }
        #endregion
    }
}
