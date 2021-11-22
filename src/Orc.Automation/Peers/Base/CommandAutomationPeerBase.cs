namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using Catel;
    using Catel.IoC;
    using Catel.Reflection;

    public abstract class CommandAutomationPeerBase : FrameworkElementAutomationPeer, IValueProvider, IInvokeProvider
    {
        #region Fields
        private readonly FrameworkElement _owner;

        private string _currentCommand;
        private readonly AutomationCommandResult _result = new ();

        private IList<IAutomationCommandCall> _automationCommandCalls;
        #endregion

        #region Constructors
        public CommandAutomationPeerBase(FrameworkElement owner)
            : base(owner)
        {
            Argument.IsNotNull(() => owner);

            _owner = owner;

            _automationCommandCalls = Array.Empty<IAutomationCommandCall>();
            Initialize();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            _automationCommandCalls = GetAvailableCommandCalls();
        }

        protected virtual IList<IAutomationCommandCall> GetAvailableCommandCalls()
        {
            var calls = GetType().GetMethods().Where(x => x.IsDecoratedWithAttribute(typeof(CommandRunMethodAttribute)))
                .Select(x => (IAutomationCommandCall) new RunMethodAutomationCommandCall(this, x.Name))
                .ToList();

            calls.Add(new GetDependencyPropertyCommandCall());
            calls.Add(new SetDependencyPropertyCommandCall());
            calls.Add(new RaiseMouseDownEventCall());

            return calls;
        }

        protected override string GetClassNameCore()
        {
            return _owner.GetType().FullName;
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }

        protected override bool IsContentElementCore()
        {
            return true;
        }

        protected override bool IsControlElementCore()
        {
            return true;
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Value)
            {
                return this;
            }

            if (patternInterface == PatternInterface.Invoke)
            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }
        #endregion

        #region IValueProvider
        public virtual void SetValue(string value)
        {
            _currentCommand = value;
        }

        public virtual bool IsReadOnly => false;
        public virtual string Value => _result?.ToString();
        #endregion

        #region InvokeProvider
        public virtual void Invoke()
        {
            var commandStr = _currentCommand;
            if (string.IsNullOrWhiteSpace(commandStr))
            {
                return;
            }

            _currentCommand = string.Empty;

            var command = AutomationCommand.FromStr(commandStr);
            if (command is null)
            {
                _result.Data = null;

                return;
            }

            var commandCall = _automationCommandCalls.FirstOrDefault(x => x.IsMatch(_owner, command));
            if (commandCall is null)
            {
                _result.Data = null;

                return;
            }

            if (!commandCall.TryInvoke(_owner, command, out var commandResult))
            {
                _result.Data = null;

                return;
            }

            _result.Data = commandResult?.Data;
        }

        protected void RaiseEvent(string eventName, object args)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                return;
            }

            _result.EventName = eventName;
            _result.EventData = AutomationSendData.FromValue(args);

            RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
        }
        #endregion
    }
}
