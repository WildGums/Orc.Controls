namespace Orc.Automation
{
    using System;
    using System.Threading;
    using System.Windows.Automation;
    using Catel;

    public class CommandAutomationElement : AutomationElementBase
    {
        #region Fields
        private readonly InvokePattern _invokePattern;
        private readonly ValuePattern _valuePattern;
        #endregion

        #region Constructors
        public CommandAutomationElement(AutomationElement element)
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

            var automationResult = AutomationCommandResult.FromStr(result);

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

        protected object Execute(string commandName, string commandData, Type dataType, int delay = 200)
        {
            var command = new AutomationCommand
            {
                CommandName = commandName,
                Data = dataType is not null ? new AutomationSendData(dataType) { Data = commandData } : null
            };

            var result = Execute(command, delay);
            if (Equals(result, AutomationCommandResult.Empty))
            {
                return null;
            }

            var data = result.Data;
            var resultValue = data?.ExtractValue();

            return resultValue;
        }

        public object GetValue(string propertyName)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            var result = Execute(GetDependencyPropertyCommandCall.ConvertPropertyToCommandName(propertyName), null, null, 20);
            return result;
        }

        public void SetValue(string propertyName, object value)
        {
            Argument.IsNotNull(() => propertyName);
            Argument.IsNotNullOrEmpty(() => propertyName);

            var serializedValue = XmlSerializerHelper.SerializeValue(value);
            var result = Execute(SetDependencyPropertyCommandCall.ConvertPropertyToCommandName(propertyName), serializedValue, value?.GetType(), 20);
        }

        public object Execute(string methodName, object parameter)
        {
            var serializedValue = XmlSerializerHelper.SerializeValue(parameter);
            return Execute(methodName, serializedValue, parameter.GetType(), 20);
        }

        public object Execute(string methodName)
        {
            return Execute(methodName, null, null, 20);
        }

        private AutomationCommandResult Execute(AutomationCommand command, int delay)
        {
            var commandStr = command?.ToString();
            if (string.IsNullOrWhiteSpace(commandStr))
            {
                return null;
            }

            _valuePattern.SetValue(commandStr);

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

            return AutomationCommandResult.FromStr(result);
        }
        #endregion
    }
}
