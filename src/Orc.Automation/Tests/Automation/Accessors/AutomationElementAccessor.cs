namespace Orc.Automation
{
    using System;
    using System.Threading;
    using System.Windows.Automation;
    using Catel;

    public class AutomationElementAccessor
    {
        private AutomationElement _accessElement;
        private AutomationElement _element;
        private InvokePattern _invokePattern;
        private ValuePattern _valuePattern;

        public AutomationElementAccessor(AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            InitializeAccessElement(element);
        }

        private void InitializeAccessElement(AutomationElement element)
        {
            _element = element;
            _accessElement = element;

            _valuePattern = _accessElement.TryGetPattern<ValuePattern>();
            if (_valuePattern is null)
            {
                _accessElement = _accessElement.Find(className: typeof(AutomationInformer).FullName, scope:TreeScope.Parent);
                _valuePattern = _accessElement?.TryGetPattern<ValuePattern>();
            }

            _invokePattern = _accessElement?.TryGetPattern<InvokePattern>();

            if (_invokePattern is not null && _valuePattern is not null)
            {
                System.Windows.Automation.Automation.AddAutomationEventHandler(InvokePattern.InvokedEvent, _accessElement, TreeScope.Element, OnEventInvoke);
            }
        }

        private void EnsureAccessElement()
        {
            if (_accessElement is null)
            {
                throw new AutomationException("Can't access element API...this element doesn't implement Run method and there is no AutomationInformer present");
            }
        }

        private void OnEventInvoke(object sender, System.Windows.Automation.AutomationEventArgs e)
        {
            EnsureAccessElement();

            var result = _valuePattern.Current.Value;

            var automationResult = AutomationResultContainer.FromStr(result);

            var eventName = automationResult.LastEventName;
            var eventData = automationResult.LastEventArgs?.ExtractValue();

            AutomationEvent?.Invoke(this, new AutomationEventArgs
            {
                EventName = eventName,
                Data = eventData
            });
        }
        
        public object GetValue(string propertyName)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            if (!Equals(_accessElement, _element))
            {
                var id = _element.Current.AutomationId;

                return Execute(nameof(AutomationInformerPeer.GetTargetPropertyValue), propertyName, id);
            }

            var result = Execute(GetDependencyPropertyMethodRun.ConvertPropertyToCommandName(propertyName), null, 20);
            return result;
        }

        public void SetValue(string propertyName, object value)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            if (!Equals(_accessElement, _element))
            {
                var id = _element.Current.AutomationId;

                Execute(nameof(AutomationInformerPeer.SetTargetPropertyValue), propertyName, id, value);

                return;
            }

            var automationValues = AutomationValueList.Create(value);
            var result = Execute(SetDependencyPropertyMethodRun.ConvertPropertyToCommandName(propertyName), automationValues, 20);
        }

        public object TryFindResource(string resourceKey)
        {
            return Execute(nameof(TryFindResource), resourceKey);
        }

        public void AttachBehavior(Type behaviorType)
        {
            Execute(AttachBehaviorMethodRun.AttachBehaviorMethodPrefix, behaviorType);
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
            EnsureAccessElement();

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

        public event EventHandler<AutomationEventArgs> AutomationEvent;
    }
}
