namespace Orc.Automation
{
    using System.Windows;
    using Catel;

    public class RunMethodAutomationCommandCall : NamedAutomationCommandCallBase
    {
        private readonly CommandAutomationPeerBase _peer;

        public RunMethodAutomationCommandCall(CommandAutomationPeerBase peer, string methodName)
        {
            Argument.IsNotNull(() => peer);

            _peer = peer;
            Name = methodName;
        }

        public override string Name { get; }
        public override bool TryInvoke(FrameworkElement owner, AutomationCommand command, out AutomationCommandResult result)
        {
            var type = _peer.GetType();

            result = null;

            var method = type.GetMethod(Name);
            if (method is null)
            {
                return false;
            }

            var parameters = method.GetParameters();

            var inputParameter = command.Data?.ExtractValue();
            var inputParameters = parameters.Length != 1 || inputParameter is null ? null : new[] { inputParameter };

            var methodResult = method.Invoke(_peer, inputParameters);
            
            result = AutomationHelper.ConvertToSerializableResult(methodResult);

            return true;
        }
    }
}
