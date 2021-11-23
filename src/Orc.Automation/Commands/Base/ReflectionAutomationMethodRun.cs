namespace Orc.Automation
{
    using System;
    using System.Linq;
    using System.Windows;
    using Catel;

    public class ReflectionAutomationMethodRun : NamedAutomationMethodRun
    {
        private readonly RunMethodAutomationPeerBase _peer;

        public ReflectionAutomationMethodRun(RunMethodAutomationPeerBase peer, string methodName)
        {
            Argument.IsNotNull(() => peer);

            _peer = peer;
            Name = methodName;
        }

        public override string Name { get; }

        public override bool TryInvoke(FrameworkElement owner, AutomationMethod automationMethod, out AutomationValue result)
        {
            var type = _peer.GetType();

            result = null;

            var method = type.GetMethod(Name);
            if (method is null)
            {
                return false;
            }

            var automationInputParameters = automationMethod.Parameters?
                    .Select(x => x?.ExtractValue())
                    .ToArray() 
                ?? Array.Empty<object>();

            var methodResult = method.Invoke(_peer, automationInputParameters);

            result = AutomationValue.FromValue(methodResult);

            return true;
        }
    }
}
